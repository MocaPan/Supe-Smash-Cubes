// MyRigidbody2D.cs (actualizado)
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomPhysics2D
{
    public class MyRigidbody2D : MonoBehaviour
    {
        public Vector2 velocity;
        public float gravityScale = 1f;
        public bool isKinematic = false;

        public Vector2 linearVelocity
        {
            get => velocity;
            set => velocity = value;
        }

        public void AddForce(Vector2 force)
        {
            velocity += force;
        }

        private MyCollider2D myCollider;
        private Vector2 frameMovement;

        void Start()
        {
            myCollider = GetComponent<MyCollider2D>();
        }

        void FixedUpdate()
        {
            // Aplicar gravedad si no es kinematic
            if (!isKinematic)
                velocity.y += Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;

            // Movimiento
            frameMovement = velocity * Time.fixedDeltaTime;
            if (frameMovement != Vector2.zero)
                transform.position += (Vector3)frameMovement;

            // Colisiones personalizadas
            var all = MyCollider2D.AllColliders;
            int count = MyCollider2D.ColliderCount;

            for (int i = 0; i < count; i++)
            {
                var other = all[i];
                if (other == null || other == myCollider)
                    continue;

                bool collided = false;

                // Rect vs Rect
                var rectA = myCollider as MyRectangleCollider2D;
                var rectB = other as MyRectangleCollider2D;
                if (rectA != null && rectB != null)
                {
                    Vector2 aPos = rectA.transform.position;
                    Vector2 bPos = rectB.transform.position;
                    Vector2 aHalf = rectA.HalfSize;
                    Vector2 bHalf = rectB.HalfSize;

                    if (MyCollider2D.CheckRectRect(aPos, aHalf, bPos, bHalf))
                    {
                        collided = true;
                        if (!rectA.isTrigger && !rectB.isTrigger)
                        {
                            Vector2 diff = aPos - bPos;
                            float overlapX = aHalf.x + bHalf.x - Mathf.Abs(diff.x);
                            float overlapY = aHalf.y + bHalf.y - Mathf.Abs(diff.y);

                            if (overlapX < overlapY)
                            {
                                float dir = diff.x >= 0 ? 1 : -1;
                                transform.position += new Vector3(dir * overlapX, 0, 0);
                                velocity.x = 0;
                            }
                            else
                            {
                                float dir = diff.y >= 0 ? 1 : -1;
                                transform.position += new Vector3(0, dir * overlapY, 0);
                                velocity.y = 0;
                            }
                        }
                    }
                }
                // Circle vs Circle
                else if (myCollider is MyCapsuleCollider2D capA && other is MyCapsuleCollider2D capB)
                {
                    Vector2 posA = capA.transform.position;
                    Vector2 posB = capB.transform.position;
                    float rA = capA.radius;
                    float rB = capB.radius;

                    if (MyCollider2D.CheckCircleCircle(posA, rA, posB, rB))
                    {
                        collided = true;
                        if (!capA.isTrigger && !capB.isTrigger)
                        {
                            Vector2 diff = posA - posB;
                            float dist = diff.magnitude;
                            if (dist < Mathf.Epsilon) { diff = Vector2.right; dist = 1e-6f; }
                            float overlap = (rA + rB) - dist;
                            Vector2 push = diff.normalized;
                            transform.position += (Vector3)(push * overlap);
                            if (Vector2.Dot(velocity, push) < 0)
                                velocity -= Vector2.Dot(velocity, push) * push;
                        }
                    }
                }
                // Capsule vs Rect or Rect vs Capsule
                else
                {
                    var cap = myCollider as MyCapsuleCollider2D;
                    var rect = myCollider as MyRectangleCollider2D;
                    var otherCap = other as MyCapsuleCollider2D;
                    var otherRect = other as MyRectangleCollider2D;

                    if (cap != null && otherRect != null)
                    {
                        ProcessCapsuleRect(cap, otherRect, ref collided);
                    }
                    else if (rect != null && otherCap != null)
                    {
                        ProcessCapsuleRect(otherCap, rect, ref collided);
                    }
                    // Tilemap vs Dynamic
                    else if (other is MyTilemapCollider2D tmCol)
                    {
                        ProcessTilemapCollision(other as MyTilemapCollider2D, ref collided);
                    }
                }

                // Eventos
                if (collided)
                {
                    if (myCollider.isTrigger || other.isTrigger)
                    {
                        SendMessage("OnMyTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
                        other.SendMessage("OnMyTriggerEnter", myCollider, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        SendMessage("OnMyCollisionEnter", other, SendMessageOptions.DontRequireReceiver);
                        other.SendMessage("OnMyCollisionEnter", myCollider, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        // Procesamiento de colisión cápsula-rectángulo
        private void ProcessCapsuleRect(MyCapsuleCollider2D cap, MyRectangleCollider2D rect, ref bool collided)
        {
            Vector2 cPos = cap.transform.position;
            Vector2 rPos = rect.transform.position;
            Vector2 half = rect.HalfSize;
            float rad = cap.radius;

            if (MyCollider2D.CheckCircleRect(cPos, rad, rPos, half))
            {
                collided = true;
                if (!cap.isTrigger && !rect.isTrigger)
                {
                    Vector2 diff = cPos - rPos;
                    float overlapX = half.x + rad - Mathf.Abs(diff.x);
                    float overlapY = half.y + rad - Mathf.Abs(diff.y);

                    if (overlapX < overlapY)
                    {
                        float dir = diff.x >= 0 ? 1 : -1;
                        transform.position += new Vector3(dir * overlapX, 0, 0);
                        velocity.x = 0;
                    }
                    else
                    {
                        float dir = diff.y >= 0 ? 1 : -1;
                        transform.position += new Vector3(0, dir * overlapY, 0);
                        velocity.y = 0;
                    }
                }
            }
        }

        // Procesamiento de colisión con Tilemap
        private void ProcessTilemapCollision(MyTilemapCollider2D tmCol, ref bool collided)
        {
            Vector2 dynCenter;
            float minX, minY, maxX, maxY;

            if (myCollider is MyRectangleCollider2D rect)
            {
                dynCenter = rect.transform.position;
                Vector2 half = rect.HalfSize;
                minX = dynCenter.x - half.x;
                maxX = dynCenter.x + half.x;
                minY = dynCenter.y - half.y;
                maxY = dynCenter.y + half.y;
            }
            else if (myCollider is MyCapsuleCollider2D cap)
            {
                dynCenter = cap.transform.position;
                float rad = cap.radius;
                float halfW = rad;
                float halfH = cap.height <= 2 * rad ? rad : cap.height / 2f;
                minX = dynCenter.x - halfW;
                maxX = dynCenter.x + halfW;
                minY = dynCenter.y - halfH;
                maxY = dynCenter.y + halfH;
            }
            else return;

            Tilemap tm = tmCol.GetComponent<Tilemap>();
            Vector3Int minCell = tm.WorldToCell(new Vector3(minX, minY, 0));
            Vector3Int maxCell = tm.WorldToCell(new Vector3(maxX, maxY, 0));

            for (int cx = minCell.x; cx <= maxCell.x; cx++)
                for (int cy = minCell.y; cy <= maxCell.y; cy++)
                {
                    Vector3Int cellPos = new Vector3Int(cx, cy, 0);
                    if (!tm.HasTile(cellPos)) continue;
                    Vector3 worldCenter = tm.CellToWorld(cellPos) + (Vector3)tm.cellSize / 2f;
                    Vector2 blockHalf = tm.cellSize / 2f;

                    if (myCollider is MyRectangleCollider2D dynRect)
                    {
                        if (MyCollider2D.CheckRectRect((Vector2)dynRect.transform.position, dynRect.HalfSize, (Vector2)worldCenter, blockHalf))
                        {
                            collided = true;
                            if (!dynRect.isTrigger)
                            {
                                Vector2 diff = (Vector2)dynRect.transform.position - (Vector2)worldCenter;
                                float overlapX = dynRect.HalfSize.x + blockHalf.x - Mathf.Abs(diff.x);
                                float overlapY = dynRect.HalfSize.y + blockHalf.y - Mathf.Abs(diff.y);

                                if (overlapX < overlapY)
                                {
                                    float dir = diff.x >= 0 ? 1 : -1;
                                    transform.position += new Vector3(dir * overlapX, 0, 0);
                                    velocity.x = 0;
                                }
                                else
                                {
                                    float dir = diff.y >= 0 ? 1 : -1;
                                    transform.position += new Vector3(0, dir * overlapY, 0);
                                    velocity.y = 0;
                                }
                            }
                        }
                    }
                    else if (myCollider is MyCapsuleCollider2D dynCap)
                    {
                        float rad = dynCap.radius;
                        if (MyCollider2D.CheckCircleRect((Vector2)dynCap.transform.position, rad, (Vector2)worldCenter, blockHalf))
                        {
                            collided = true;
                            if (!dynCap.isTrigger)
                            {
                                Vector2 diff = (Vector2)dynCap.transform.position - (Vector2)worldCenter;
                                float overlapX = blockHalf.x + rad - Mathf.Abs(diff.x);
                                float overlapY = blockHalf.y + rad - Mathf.Abs(diff.y);

                                if (overlapX < overlapY)
                                {
                                    float dir = diff.x >= 0 ? 1 : -1;
                                    transform.position += new Vector3(dir * overlapX, 0, 0);
                                    velocity.x = 0;
                                }
                                else
                                {
                                    float dir = diff.y >= 0 ? 1 : -1;
                                    transform.position += new Vector3(0, dir * overlapY, 0);
                                    velocity.y = 0;
                                }
                            }
                        }
                    }
                }
        }
    }
}
