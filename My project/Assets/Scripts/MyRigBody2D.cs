using UnityEngine;
using UnityEngine.Tilemaps;
using System;  // Para Math.Abs

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
            if (!isKinematic)
                velocity.y += Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;

            frameMovement = velocity * Time.fixedDeltaTime;
            if (frameMovement != Vector2.zero)
                transform.position += (Vector3)frameMovement;

            MyCollider2D[] all = MyCollider2D.AllColliders;
            int count = MyCollider2D.ColliderCount;

            for (int i = 0; i < count; i++)
            {
                MyCollider2D other = all[i];
                if (other == null || other == myCollider) continue;

                bool collided = false;

                if (myCollider is MyRectangleCollider2D rA && other is MyRectangleCollider2D rB)
                {
                    Vector2 aPos = (Vector2)rA.transform.position;
                    Vector2 bPos = (Vector2)rB.transform.position;
                    Vector2 aHalf = rA.HalfSize;
                    Vector2 bHalf = rB.HalfSize;

                    if (MyCollider2D.CheckRectRect(aPos, aHalf, bPos, bHalf))
                    {
                        collided = true;
                        if (!rA.isTrigger && !rB.isTrigger)
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
                else if (myCollider is MyCapsuleCollider2D cA && other is MyCapsuleCollider2D cB)
                {
                    Vector2 posA = (Vector2)cA.transform.position;
                    Vector2 posB = (Vector2)cB.transform.position;
                    float radA = cA.radius;
                    float radB = cB.radius;

                    if (MyCollider2D.CheckCircleCircle(posA, radA, posB, radB))
                    {
                        collided = true;
                        if (!cA.isTrigger && !cB.isTrigger)
                        {
                            Vector2 diff = posA - posB;
                            float dist = diff.magnitude;
                            if (dist < Mathf.Epsilon) { diff = Vector2.right; dist = 1e-6f; }
                            float overlap = (radA + radB) - dist;
                            Vector2 push = diff.normalized;
                            transform.position += (Vector3)(push * overlap);
                            if (Vector2.Dot(velocity, push) < 0)
                                velocity -= Vector2.Dot(velocity, push) * push;
                        }
                    }
                }
                else if ((myCollider is MyCapsuleCollider2D && other is MyRectangleCollider2D) ||
                         (myCollider is MyRectangleCollider2D && other is MyCapsuleCollider2D))
                {
                    MyCapsuleCollider2D cap = myCollider is MyCapsuleCollider2D ? (MyCapsuleCollider2D)myCollider : (MyCapsuleCollider2D)other;
                    MyRectangleCollider2D rect = myCollider is MyRectangleCollider2D ? (MyRectangleCollider2D)myCollider : (MyRectangleCollider2D)other;
                    Vector2 cPos = (Vector2)cap.transform.position;
                    Vector2 rPos = (Vector2)rect.transform.position;
                    Vector2 rHalf = rect.HalfSize;
                    float rad = cap.radius;

                    if (MyCollider2D.CheckCircleRect(cPos, rad, rPos, rHalf))
                    {
                        collided = true;
                        if (!cap.isTrigger && !rect.isTrigger)
                        {
                            Vector2 diff = cPos - rPos;
                            float overlapX = (rHalf.x + rad) - Mathf.Abs(diff.x);
                            float overlapY = (rHalf.y + rad) - Mathf.Abs(diff.y);

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
                else if (other is MyTilemapCollider2D tmCol)
                {
                    Vector2 dynCenter = (Vector2)myCollider.transform.position;
                    float minX, minY, maxX, maxY;

                    if (myCollider is MyRectangleCollider2D rect)
                    {
                        Vector2 half = rect.HalfSize;
                        minX = dynCenter.x - half.x;
                        maxX = dynCenter.x + half.x;
                        minY = dynCenter.y - half.y;
                        maxY = dynCenter.y + half.y;
                    }
                    else
                    {
                        MyCapsuleCollider2D cap = (MyCapsuleCollider2D)myCollider;
                        float rad = cap.radius;
                        float halfW = rad;
                        float halfH = (cap.height <= 2 * rad) ? rad : cap.height / 2f;
                        minX = dynCenter.x - halfW;
                        maxX = dynCenter.x + halfW;
                        minY = dynCenter.y - halfH;
                        maxY = dynCenter.y + halfH;
                    }

                    Tilemap tm = tmCol.GetComponent<Tilemap>();
                    Vector3Int minCell = tm.WorldToCell(new Vector3(minX, minY, 0));
                    Vector3Int maxCell = tm.WorldToCell(new Vector3(maxX, maxY, 0));

                    for (int cx = minCell.x; cx <= maxCell.x; cx++)
                        for (int cy = minCell.y; cy <= maxCell.y; cy++)
                        {
                            Vector3Int cellPos = new Vector3Int(cx, cy, 0);
                            if (!tm.HasTile(cellPos)) continue;
                            Vector3 worldCenter = tm.CellToWorld(cellPos) + (Vector3)tm.cellSize / 2;
                            Vector2 blockHalf = tm.cellSize / 2;

                            if (myCollider is MyRectangleCollider2D dynrect)
                            {
                                if (MyCollider2D.CheckRectRect((Vector2)dynrect.transform.position, dynrect.HalfSize, (Vector2)worldCenter, blockHalf))
                                {
                                    collided = true;
                                    if (!dynrect.isTrigger)
                                    {
                                        Vector2 diff = (Vector2)dynrect.transform.position - (Vector2)worldCenter;
                                        float overlapX = dynrect.HalfSize.x + blockHalf.x - Mathf.Abs(diff.x);
                                        float overlapY = dynrect.HalfSize.y + blockHalf.y - Mathf.Abs(diff.y);

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
                            else if (myCollider is MyCapsuleCollider2D cap)
                            {
                                float rad = cap.radius;
                                if (MyCollider2D.CheckCircleRect((Vector2)cap.transform.position, rad, (Vector2)worldCenter, blockHalf))
                                {
                                    collided = true;
                                    if (!cap.isTrigger)
                                    {
                                        Vector2 diff = (Vector2)cap.transform.position - (Vector2)worldCenter;
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
    }
}

