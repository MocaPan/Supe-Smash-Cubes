using UnityEngine;
using CustomPhysics2D;

public class FireballScript : MonoBehaviour
{
    public float speed;
    private MyRigidbody2D rb;
    private Vector2 Direction;

    void Start()
    {
        rb = GetComponent<MyRigidbody2D>();
    }

    void OnMyCollisionEnter(MyCollider2D other)
    {
        DestroyFireBall();
    }

    void Update()
    {
        rb.linearVelocity = Direction * speed;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction.normalized;

        // Girar la bola según la dirección
        Vector3 s = transform.localScale;
        s.x = (Direction.x < 0) ? -Mathf.Abs(s.x) : Mathf.Abs(s.x);
        transform.localScale = s;
    }

    public void DestroyFireBall()
    {
        Destroy(gameObject);
    }
}
