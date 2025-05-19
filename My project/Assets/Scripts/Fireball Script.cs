using UnityEngine;
using CustomPhysics2D;

public class FireballScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;

    private MyRigidbody2D rb;
    private Vector2 direction;
    private bool hasHit;    // ? flag para un solo impacto

    void Start()
    {
        rb = GetComponent<MyRigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = direction * speed;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Opcional: voltear sprite en X según la dirección
        Vector3 s = transform.localScale;
        s.x = Mathf.Sign(direction.x) * Mathf.Abs(s.x);
        transform.localScale = s;
    }

    void OnMyCollisionEnter(MyCollider2D other)
    {

        DestroyFireball();
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
