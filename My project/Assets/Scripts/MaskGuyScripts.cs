using UnityEngine;
using CustomPhysics2D;

public class Player2Script : MonoBehaviour
{
    private MyRigidbody2D rb;
    public AudioSource ShootSound;
    private Animator Animator;
    public GameObject FireballPrefab;
    private float Horizontal;
    public float speed;
    public float jumpForce;
    private float LastShoot;
    public float shootDelay;

    private void Start()
    {
        rb = GetComponent<MyRigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento con flechas ? / ?
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Horizontal = -1.0f * speed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
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

        // Salto con ? (opcional)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        // Disparo con ñ (KeyCode.Semicolon en teclado español)
        if (Input.GetKey(KeyCode.Semicolon) && Time.time > LastShoot + shootDelay)
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
        ShootSound.Play();
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
            transform.position + direction * 0.2f,
            Quaternion.identity
        );
        FireBall.GetComponent<FireballScript>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(Horizontal, rb.linearVelocity.y);
    }
}
