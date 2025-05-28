using UnityEngine;
using CustomPhysics2D;

public class FireballScript : MonoBehaviour
{
    private Vector2 direction;
    public float speed = 5f;
    public float basePushForce = 5f;
    public float strongPushMultiplier = 2.5f;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Vector3 s = transform.localScale;
        s.x = Mathf.Sign(direction.x) * Mathf.Abs(s.x);
        transform.localScale = s;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ("Enemy".Equals(other.tag)) // Re-ordered operands for better performance
        {
            float pushForce = basePushForce;
            if (PowerUpManager.Instance != null && PowerUpManager.Instance.HasPowerUp(PowerUpType.StrongShot))
                pushForce *= strongPushMultiplier;

            // Assume enemy has a method to be pushed
            var enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
                enemyRb.AddForce(direction * pushForce, ForceMode2D.Impulse);

            Destroy(gameObject);
        }
    }
}