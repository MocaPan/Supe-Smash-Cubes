using UnityEngine;
using CustomPhysics2D;

public class FireballScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;

    private MyRigidbody2D rb;
    private Vector2 direction;
    private bool hasHit;

    // STRONG SHOT FORCE MULTIPLIER
    private float forceMultiplier = 1f;

    // Inspector: Set this for the "feel" of the push
    public float basePushForce = 10f;

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

        // Flip sprite for direction
        Vector3 s = transform.localScale;
        s.x = Mathf.Sign(direction.x) * Mathf.Abs(s.x);
        transform.localScale = s;
    }

    // Use custom physics event!
    void OnMyCollisionEnter(MyCollider2D other)
    {
        // Only push MaskGuy or FrogMovement (no damage)
        var maskGuy = other.GetComponent<MaskGuy>();
        var frog = other.GetComponent<FrogMovement>();
        if ((maskGuy != null || frog != null) && !hasHit)
        {
            var hitRb = other.GetComponent<MyRigidbody2D>();
            if (hitRb != null)
            {
                // Push using base force * multiplier, instant impulse
                Vector2 push = direction.normalized * basePushForce * forceMultiplier;
                hitRb.AddForce(push);

                // Optionally, play a sound or effect here

                hasHit = true;
            }
        }
        DestroyFireball();
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
