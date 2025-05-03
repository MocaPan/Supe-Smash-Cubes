using UnityEngine;
using CustomPhysics2D;

public class FrogMovement : MonoBehaviour
{
    private MyRigidbody2D rb;      // Tu Rigidbody2D “casero”
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

        // Salto con W
        if (Input.GetKeyDown(KeyCode.W) && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            rb.AddForce(Vector2.up * jumpForce);

        // Disparo con F
        if (Input.GetKey(KeyCode.F) && Time.time > LastShoot + shootDelay)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        Vector2 dir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        Vector3 spawnPos = transform.position + (Vector3)dir * 0.2f;

        GameObject FireBall = Instantiate(
            FireballPrefab,
            spawnPos,
            Quaternion.identity
        );
        FireBall.GetComponent<FireballScript>().SetDirection(dir);
    }

    private void FixedUpdate()
    {
        // Asigna la velocidad horizontal en tu físico propio
        rb.linearVelocity = new Vector2(Horizontal, rb.linearVelocity.y);
    }
}
