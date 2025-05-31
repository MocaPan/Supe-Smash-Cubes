using UnityEngine;
using CustomPhysics2D;

[RequireComponent(typeof(MyRectangleCollider2D))]
public class KillZone : MonoBehaviour
{
    [Tooltip("Sonido al matar en la zona (opcional)")]
    public AudioSource deathSound;

    void OnMyTriggerEnter(MyCollider2D other)
{
    if (!other.CompareTag("Player")) return;
    var health = other.GetComponent<PlayerHealth>();
    if (health == null) return;

    if (deathSound != null) deathSound.Play();

    health.InstantKill();
}
}
