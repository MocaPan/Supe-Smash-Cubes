using UnityEngine;
using CustomPhysics2D;

[RequireComponent(typeof(MyRectangleCollider2D))]
public class KillZone : MonoBehaviour
{
    public AudioSource deathSound;

    void OnMyTriggerEnter(MyCollider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        var health = other.GetComponent<PlayerHealth>();
        if (health == null || health.IsDead)
            return;

        deathSound?.Play();
        // Inflige daño igual a toda la vida para forzar la muerte
        health.TakeDamage(health.maxHealth);
    }
}
