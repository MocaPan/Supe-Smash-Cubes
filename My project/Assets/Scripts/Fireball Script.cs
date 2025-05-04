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
       // if (other.CompareTag("Player")) 
       // { 
       //     other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        //}
        //else if (other.CompareTag("Wall"))
        //{
          //  DestroyFireBall();
        //}
            DestroyFireBall();
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

