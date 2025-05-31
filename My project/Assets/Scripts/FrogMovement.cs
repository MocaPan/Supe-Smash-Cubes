using UnityEngine;
using UnityEngine.InputSystem;
using CustomPhysics2D;

public class FrogMovement : MonoBehaviour
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

    public bool hasDoubleJump = false;
    private bool doubleJumpUsed = false;
    public bool hasStrongShot = false;
    private float strongShotEndTime = 0f;
    public float shotForceMultiplier = 1f;
    public AudioSource powerUpSound;

    private PlayerControls controls;
    private Vector2 moveInput;
    private bool jumpQueued = false;
    private bool shootQueued = false;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Gameplay.Jump.performed += ctx => jumpQueued = true;
        controls.Gameplay.Shoot.performed += ctx => shootQueued = true;

        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Gameplay.Disable();
    }

    void Start()
    {
        rb = GetComponent<MyRigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Horizontal = moveInput.x * speed;

        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(1, 1, 1);

        Animator.SetBool("Running", Horizontal != 0.0f);

        if (jumpQueued)
        {
            jumpQueued = false;
            if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            {
                rb.AddForce(Vector2.up * jumpForce);
                doubleJumpUsed = false;
            }
            else if (hasDoubleJump && !doubleJumpUsed)
            {
                rb.AddForce(Vector2.up * jumpForce * 2f);
                doubleJumpUsed = true;
                hasDoubleJump = false;
                if (powerUpSound != null) powerUpSound.Play();
            }
        }

        if (shootQueued && Time.time > LastShoot + shootDelay)
        {
            Shoot();
            LastShoot = Time.time;
        }
        shootQueued = false;

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
        GameObject FireBall = Instantiate(FireballPrefab, spawnPos, Quaternion.identity);
        FireBall.GetComponent<FireballScript>().SetDirection(dir, shotForceMultiplier);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(Horizontal, rb.linearVelocity.y);
    }

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
