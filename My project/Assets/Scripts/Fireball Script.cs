using UnityEngine;
using CustomPhysics2D;

public class FireballScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float pushDistance = 1f; // <-- tamaño del empuje, ajusta según tu grid
    public float forceMultiplier = 1f;

    private MyRigidbody2D rb;
    private Vector2 direction;
    private bool hasHit;

    void Start()
    {
        rb = GetComponent<MyRigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = direction * speed;
    }

    public void SetDirection(Vector2 dir, float multiplier = 1f)
    {
        direction = dir.normalized;
        forceMultiplier = multiplier;

        // Flip sprite para la dirección
        Vector3 s = transform.localScale;
        s.x = Mathf.Sign(direction.x) * Mathf.Abs(s.x);
        transform.localScale = s;
    }

    void OnMyCollisionEnter(MyCollider2D other)
    {
        var maskGuy = other.GetComponent<MaskGuy>();
        var frog = other.GetComponent<FrogMovement>();
        var playerHealth = other.GetComponent<PlayerHealth>();
        // ADD THIS CHECK:
        if (playerHealth != null && playerHealth.isShielded)
        {
        // (Optional) play shield-blocked sound or effect here!
            DestroyFireball();
            return;
        }

        if ((maskGuy != null || frog != null) && !hasHit)
        {
            // Mover exactamente un espacio en la dirección del disparo
            Vector2 pushDir = direction.normalized;
            Transform playerTransform = other.transform;
            Vector3 targetPos = playerTransform.position + (Vector3)(pushDir * pushDistance * forceMultiplier / 3);
            playerTransform.position = targetPos;
            

            hasHit = true;
        }
        DestroyFireball();
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
