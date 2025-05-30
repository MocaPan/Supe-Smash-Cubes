using UnityEngine;
using UnityEngine.UI;
using CustomPhysics2D;


public class PlayerHealth : MonoBehaviour
{
    [Header("Score Enemico")]
    [Tooltip("Referencia de score del enemigo")]
    [SerializeField] private Score playerScore;

    [Header("Configuraci�n de Vida")]
    [Tooltip("Puntos de vida m�ximos")]
    public int maxHealth = 4;

    [Header("Respawn")]
    [Tooltip("Segundos de espera antes de reaparecer")]
    public float respawnDelay = 1f;
    [Tooltip("Puntos de respawn, asigna en orden")]
    public Transform[] respawnPoints;
    [Tooltip("�ndice de este jugador en el array")]
    private int playerIndex = 0;
    public AudioSource hitSound; // Sonido de da�o (opcional)

    [Header("Sonido y Animaci�n")]
    [Tooltip("AudioSource que reproduce el sonido de muerte")]
    public AudioSource deathSound;
    [Tooltip("Animator con el trigger de muerte")]
    public Animator animator;
    [Tooltip("Nombre del trigger de muerte en el Animator")]
    public string deathTrigger = "Die";

    [Header("UI de Corazones (llenos y vac�os)")]
    [Tooltip("Im�genes de corazones vac�os (fondo)")]
    public Image[] heartsEmpty;   // tama�o = maxHealth
    [Tooltip("Im�genes de corazones llenos (encima)")]
    public Image[] heartsFull;    // tama�o = maxHealth

    private int currentHealth;
    private bool isDead;
    private float respawnTimer;
    public bool isShielded = false;
    private float shieldEndTime = 0f;
    public AudioSource shieldActivateSound; // Optional

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Validaci�n array de respawn
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
        // Handle shield expiration
        if (isShielded && Time.time > shieldEndTime)
        {
            isShielded = false;
            // Optional: visual/audio feedback
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
        
        if (isShielded)
        {
                // Ignore damage while shielded
            return;
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
            // El coraz�n vac�o siempre est� activo
            heartsEmpty[i].enabled = true;

            // Si el �ndice est� por debajo de la vida actual, mostramos el coraz�n lleno
            // en caso contrario lo ocultamos, dejando ver el vac�o de fondo
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
