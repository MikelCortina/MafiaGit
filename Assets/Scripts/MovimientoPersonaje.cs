using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float defaultSpeed;  // Velocidad de movimiento
    public float jumpForce;     // Fuerza de salto
    private Rigidbody2D rb;     // Referencia al Rigidbody2D del personaje
    public float vidaTotal = 1000f;
    public float vidaActual = 1000f;
    public PlayerShooting shooting;

    private bool facingRight = true; // Indica si el personaje está mirando a la derecha

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtenemos el Rigidbody2D del personaje
        rb.freezeRotation = true;        // Congelamos la rotación para evitar que el jugador gire al colisionar
    }

    void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");

        if (!shooting.isWaitingToShoot)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }

        // Reducir velocidad al recargar
        speed = shooting.isReloading ? 1f : defaultSpeed;

        // Verifica la dirección y voltea si es necesario
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Verificamos si el jugador presiona la tecla de salto y está en el suelo
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBullet bullet = collision.GetComponent<EnemyBullet>();
        if (collision.CompareTag("BulletEnemy"))
        {
            vidaActual -= bullet.damage;
            Debug.Log("Has recibido un total de daño de " + bullet.damage);
            Debug.Log("Tienes un total de vida de " + vidaActual);

            Destroy(collision.gameObject);
        }
        if (vidaActual <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Has muerto");
        }
    }

    // Método para verificar si está en el suelo
    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f).collider != null;
    }

    // Método para voltear al jugador
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f); // Gira el personaje 180° en el eje Y
    }
}

