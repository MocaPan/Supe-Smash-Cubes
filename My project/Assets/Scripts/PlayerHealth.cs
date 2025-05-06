using UnityEngine;
using CustomPhysics2D;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int      maxHealth    = 3;
    public float    respawnDelay = 1f;
    public Transform respawnPoint;

    [Header("Sonido y Animación")]
    public AudioSource deathSound;
    public Animator     animator;
    public string       deathTrigger = "Die";

    private int   currentHealth;
    private bool  isDead;
    private float respawnTimer;

    /// <summary>
    /// Permite a otros scripts saber si este jugador está «muerto»
    /// </summary>
    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
            return;

        respawnTimer -= Time.deltaTime;
        if (respawnTimer <= 0f)
            Respawn();
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead       = true;
        respawnTimer = respawnDelay;

        if (deathSound != null)
            deathSound.Play();

        if (animator != null)
            animator.SetTrigger(deathTrigger);
        else
            // sin animación, reaparecer de inmediato
            respawnTimer = 0f;
    }

    // Debe llamarse desde un Animation Event al final del clip "Death"
    public void OnDeathAnimationComplete()
    {
        respawnTimer = respawnDelay;
    }

    private void Respawn()
    {
        isDead        = false;
        currentHealth = maxHealth;

        transform.position = respawnPoint.position;

        if (animator != null)
            animator.ResetTrigger(deathTrigger);
    }
}
