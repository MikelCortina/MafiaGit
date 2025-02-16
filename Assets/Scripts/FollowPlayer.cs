using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayer2D : MonoBehaviour
{
    public Transform player;  // Objeto del jugador
    public float speed = 5f;  // Velocidad base a la que el objeto sigue al jugador
    public float maxSpeed = 10f; // Velocidad máxima
    public float acceleration = 0.5f; // Aceleración del objeto
    public float offset = 1f; // Desplazamiento a la izquierda del jugador
    public float safeDistance = 0.5f; // Distancia de seguridad para frenar

    private Rigidbody2D rb;
    private float currentVelocity = 0f; // Usamos un float para almacenar la velocidad actual
    public Animator animacion;

    public AttackEnemyOnClick attackScript;

    void Start()
    {
        // Obtener el Rigidbody2D del objeto
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1; // Asegurar que el objeto use la gravedad
    }

    void FixedUpdate()
    {
        animacion.SetFloat("movimiento", currentVelocity);
        // Solo sigue al jugador si el booleano 'seguiraJugador' está activo
        if (attackScript != null && attackScript.seguiraJugador)
        {
            if (player != null)
            {
                // Calcula la posición objetivo con un desplazamiento a la izquierda
                Vector2 targetPosition = new Vector2(player.position.x - offset, transform.position.y);

                // Calcula la distancia actual al jugador
                float distanceToPlayer = Vector2.Distance(transform.position, targetPosition);

                // Verifica si el objeto está dentro del rango de seguridad
                if (distanceToPlayer > safeDistance)
                {
                    // Mueve el objeto hacia la posición objetivo con la velocidad base
                    Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
                    rb.MovePosition(newPosition);
                    currentVelocity = 0f; // Resetea la velocidad si está fuera del rango de seguridad
                }
                else
                {
                    // Si está dentro de la distancia de seguridad, solo aceleramos si el jugador se mueve
                    if (Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f) // Si el jugador se mueve
                    {
                        // Acelera en la dirección del jugador
                        if (player.position.x > transform.position.x) // Si el jugador se mueve hacia la derecha
                        {
                            currentVelocity = Mathf.Min(currentVelocity + acceleration * Time.fixedDeltaTime, maxSpeed);
                        }
                        else if (player.position.x < transform.position.x) // Si el jugador se mueve hacia la izquierda
                        {
                            currentVelocity = Mathf.Max(currentVelocity - acceleration * Time.fixedDeltaTime, -maxSpeed);
                        }
                        rb.linearVelocity = new Vector2(currentVelocity, rb.linearVelocity.y); // Aplica la velocidad con aceleración
                    }
                    else
                    {
                        // Si el jugador no se mueve, resetea la aceleración y la velocidad
                        currentVelocity = 0f; // Reinicia la velocidad
                        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Detener el movimiento horizontal
                    }
                }

                // Obtener y mostrar la magnitud de la velocidad (float) en consola
                float speedMagnitude = rb.linearVelocity.magnitude; // Esto te da el valor de la velocidad como un float
                Debug.Log("Velocidad actual: " + speedMagnitude);  // Imprime la velocidad escalar (float)
            }
        }
    }
}
