using UnityEngine;
using CustomPhysics2D;

public class FireballScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;

    private MyRigidbody2D rb;
    private Vector2 direction;
    private bool hasHit;

    // ----- STRONG SHOT FORCE MULTIPLIER -----
    private float forceMultiplier = 1f;

    void Start()
    {
        rb = GetComponent<MyRigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = direction * speed;
    }

    // Pass force multiplier for power-up
    public void SetDirection(Vector2 dir, float multiplier = 1f)
    {
        direction = dir.normalized;
        forceMultiplier = multiplier;

        // Opcional: voltear sprite en X según la dirección
        Vector3 s = transform.localScale;
        s.x = Mathf.Sign(direction.x) * Mathf.Abs(s.x);
        transform.localScale = s;
    }

    void OnMyCollisionEnter(MyCollider2D other)
    {
        // Push player/enemy with extra force if applicable
        var hitRb = other.GetComponent<MyRigidbody2D>();
        if (hitRb != null && !hasHit)
        {
            float baseForce = 10f; // Ajusta según el diseño del juego
            Vector2 push = direction * baseForce * forceMultiplier;
            hitRb.AddForce(push);

            hasHit = true;
        }

        DestroyFireball();
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
