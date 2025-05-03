using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 Direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
