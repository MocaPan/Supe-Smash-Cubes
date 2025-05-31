using UnityEngine;
using CustomPhysics2D;

public class MaskGuy : MonoBehaviour
{
    private MyRigidbody2D rb;
    private Animator Animator;
    public AudioSource ShootSound;
    public GameObject FireballPrefab;
    private float Horizontal;
    public float speed;
    public float jumpForce;
    private float LastShoot;
    public float shootDelay;

    // ----- POWER-UP FIELDS -----
    public bool hasDoubleJump = false;
    private bool doubleJumpUsed = false;

    public bool hasStrongShot = false;
    private float strongShotEndTime = 0f;
    public float shotForceMultiplier = 1f; // default is 1

    public AudioSource powerUpSound; // Optional: assign in Inspector for feedback

    void Start()
    {
        rb = GetComponent<MyRigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento con A/D
        if (Input.GetKey(KeyCode.A))
            Horizontal = -1.0f * speed;
        else if (Input.GetKey(KeyCode.D))
            Horizontal = 1.0f * speed;
        else
            Horizontal = 0.0f;

        // Voltear sprite
        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(1, 1, 1);

        // Animación de correr
        Animator.SetBool("Running", Horizontal != 0.0f);

        // ----- DOUBLE JUMP LOGIC -----
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            {
                rb.AddForce(Vector2.up * jumpForce);
                doubleJumpUsed = false; // Reset double jump on ground
            }
            else if (hasDoubleJump && !doubleJumpUsed)
            {
                rb.AddForce(Vector2.up * jumpForce * 2f); // Double jump is 2x force
                doubleJumpUsed = true;
                hasDoubleJump = false; // Power-up is consumed
                if (powerUpSound != null) powerUpSound.Play();
            }
        }

        // Disparo con F
        if (Input.GetKey(KeyCode.F) && Time.time > LastShoot + shootDelay)
        {
            Shoot();
            LastShoot = Time.time;
        }

        // ----- STRONG SHOT TIMER -----
        if (hasStrongShot && Time.time > strongShotEndTime)
        {
            hasStrongShot = false;
            shotForceMultiplier = 1f;
        }
    }

    private void Shoot()
    {
        if (ShootSound != null) ShootSound.Play();
        Vector2 dir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        Vector3 spawnPos = transform.position + (Vector3)dir * 0.28f;

        GameObject FireBall = Instantiate(
            FireballPrefab,
            spawnPos,
            Quaternion.identity
        );
        // Pass the force multiplier to the Fireball
        FireBall.GetComponent<FireballScript>().SetDirection(dir, shotForceMultiplier);
    }

    private void FixedUpdate()
    {
        // Asigna la velocidad horizontal en tu físico propio
        rb.linearVelocity = new Vector2(Horizontal, rb.linearVelocity.y);
    }

    // --- POWER-UP ACTIVATION HELPERS ---
    public void ActivateDoubleJump()
    {
        hasDoubleJump = true;
        doubleJumpUsed = false;
    }

    public void ActivateStrongShot(float duration, float multiplier)
    {
        hasStrongShot = true;
        shotForceMultiplier = multiplier;
        strongShotEndTime = Time.time + duration;
        if (powerUpSound != null) powerUpSound.Play();
    }
}
