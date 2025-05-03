// FrogMovement.cs
using UnityEngine;

public class FrogMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator Animator;
    public GameObject FireballPrefab;
    private float Horizontal;
    public float speed;
    public float jumpForce;
    private float LastShoot;
    public float shootDelay;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento con A/D
        if (Input.GetKey(KeyCode.A))
        {
            Horizontal = -1.0f * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Horizontal = 1.0f * speed;
        }
        else
        {
            Horizontal = 0.0f;
        }

        // Voltear sprite
        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Horizontal > 0.0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Animación de correr
        Animator.SetBool("Running", Horizontal != 0.0f);

        // Salto con W
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        // Disparo con F
        if (Input.GetKey(KeyCode.F) && Time.time > LastShoot + shootDelay)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Jump()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x < 0)
        {
            direction = new Vector2(-1, 0);
        }
        else
        {
            direction = new Vector2(1, 0);
        }

        GameObject FireBall = Instantiate(
            FireballPrefab,
            transform.position + direction * 0.15f,
            Quaternion.identity
        );
        FireBall.GetComponent<FireballScript>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(Horizontal, rb.linearVelocity.y);
    }
}
