using UnityEngine;
using UnityEngine.UI;
using CustomPhysics2D;

public class PlayerHealth : MonoBehaviour
{
    [Header("Score Enemico")]
    [Tooltip("Referencia de score del enemigo")]
    [SerializeField] private Score playerScore;

    [Header("Configuración de Vida")]
    [Tooltip("Puntos de vida máximos")]
    public int maxHealth = 4;

    [Header("Respawn")]
    [Tooltip("Segundos de espera antes de reaparecer")]
    public float respawnDelay = 1f;
    [Tooltip("Puntos de respawn, asigna en orden")]
    public Transform[] respawnPoints;
    [Tooltip("Índice de este jugador en el array")]
    private int playerIndex = 0;
    public AudioSource hitSound; // Sonido de daño (opcional)

    [Header("Sonido y Animación")]
    [Tooltip("AudioSource que reproduce el sonido de muerte")]
    public AudioSource deathSound;
    [Tooltip("Animator con el trigger de muerte")]
    public Animator animator;
    [Tooltip("Nombre del trigger de muerte en el Animator")]
    public string deathTrigger = "Die";

    [Header("UI de Corazones (llenos y vacíos)")]
    [Tooltip("Imágenes de corazones vacíos (fondo)")]
    public Image[] heartsEmpty;   // tamaño = maxHealth
    [Tooltip("Imágenes de corazones llenos (encima)")]
    public Image[] heartsFull;    // tamaño = maxHealth

    private int currentHealth;
    private bool isDead;
    private float respawnTimer;

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Validación array de respawn
        if (respawnPoints == null || respawnPoints.Length == 0)
        {
            Debug.LogWarning(name + ": no hay puntos de respawn asignados");
        }

        // Inicializa UI
        UpdateHeartsUI();
    }

    void Update()
    {
        if (!isDead)
        {
            return;
        }

        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 0f)
        {
            ChooseRandomRespawnPoint();
            Respawn();
        }
    }

    public void TakeDamage(int amount)
    {
        if (hitSound != null)
        {
            hitSound.Play();
        }

        if (isDead)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHeartsUI();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        respawnTimer = respawnDelay;

        if (deathSound != null)
        {
            deathSound.Play();
        }

        if (animator != null)
        {
            animator.SetTrigger(deathTrigger);
        }

        if (playerScore != null)
            playerScore.AddScore(1);
    }

    // Llamado desde un Animation Event al final del clip "Death"
    public void OnDeathAnimationComplete()
    {
        respawnTimer = respawnDelay;
    }

    private void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHeartsUI();

        if (respawnPoints != null
            && playerIndex >= 0
            && playerIndex < respawnPoints.Length)
        {
            transform.position = respawnPoints[playerIndex].position;
        }

        if (animator != null)
        {
            animator.ResetTrigger(deathTrigger);
        }
    }

    private void ChooseRandomRespawnPoint()
    {
        if (respawnPoints != null
            && respawnPoints.Length > 0)
        {
            playerIndex = Random.Range(0, respawnPoints.Length);
        }
    }

    private void UpdateHeartsUI()
    {
        if (heartsEmpty == null
            || heartsFull == null)
        {
            return;
        }

        int count = Mathf.Min(heartsEmpty.Length, heartsFull.Length);

        for (int i = 0; i < count; i++)
        {
            // El corazón vacío siempre está activo
            heartsEmpty[i].enabled = true;

            // Si el índice está por debajo de la vida actual, mostramos el corazón lleno
            // en caso contrario lo ocultamos, dejando ver el vacío de fondo
            heartsFull[i].enabled = (i < currentHealth);
        }
    }

    void OnMyCollisionEnter(MyCollider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
    }

    void OnMyTriggerEnter(MyCollider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            TakeDamage(currentHealth);
        }
    }
}
