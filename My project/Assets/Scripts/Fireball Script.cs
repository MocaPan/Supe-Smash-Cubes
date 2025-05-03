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

    void Update()
    {
        rb.linearVelocity = Direction * speed;
    }
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
        
    }
    public void DestroyFireBall()
    {
        Destroy(gameObject);

    }


}

