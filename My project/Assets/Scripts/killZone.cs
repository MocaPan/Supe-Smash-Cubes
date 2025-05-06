using UnityEngine;
using CustomPhysics2D;

[RequireComponent(typeof(MyRectangleCollider2D))]
public class KillZone : MonoBehaviour
{
    [Tooltip("Sonido al matar en la zona (opcional)")]
    public AudioSource deathSound;

    void OnMyTriggerEnter(MyCollider2D other)
    {
        // Solo nos importa el jugador
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Intentamos obtener el componente PlayerHealth
        var health = other.GetComponent<PlayerHealth>();
        if (health == null)
        {
            return;
        }

        // Reproducir sonido (si est� asignado)
        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Infligir da�o igual a la vida m�xima para vaciar todos los corazones
        health.TakeDamage(health.maxHealth);
    }
}
