using UnityEngine;

public class Ladder : MonoBehaviour
{
    public float climbSpeed = 5f; // Velocidad al subir/bajar
    private bool isClimbing = false; // Indica si el jugador est� escalando
    private Rigidbody2D playerRb; // Referencia al Rigidbody del jugador
    private PlayerController playerController; // Referencia opcional al controlador del jugador (si aplica)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprueba si el objeto que entra es el jugador
        if (collision.CompareTag("Player"))
        {
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerController = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Comprueba si el jugador ha salido de la escalera
        if (collision.CompareTag("Player"))
        {
            ExitLadder(); // Llama a la funci�n para restaurar el estado normal
        }
    }

    private void Update()
    {
        // Si el jugador est� en la escalera
        if (playerRb != null)
        {
            // Comienza a escalar al pulsar W
            if (Input.GetKeyDown(KeyCode.W))
            {
                isClimbing = true;
                playerRb.gravityScale = 0f; // Desactiva la gravedad
                playerRb.linearVelocity = Vector2.zero; // Detiene cualquier movimiento actual
                DisableHorizontalMovement(); // Desactiva el movimiento horizontal
            }

            // Si el jugador est� escalando
            if (isClimbing)
            {
                // Movimiento hacia arriba o hacia abajo
                if (Input.GetKey(KeyCode.W))
                {
                    playerRb.linearVelocity = new Vector2(0f, climbSpeed); // Subir
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    playerRb.linearVelocity = new Vector2(0f, -climbSpeed); // Bajar
                }
                else
                {
                    playerRb.linearVelocity = new Vector2(0f, 0f); // Detenerse si no se presionan teclas
                }

                // Salir de la escalera al soltar W o bajar con S
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.S) && transform.position.y < playerRb.transform.position.y)
                {
                    ExitLadder();
                }
            }
        }
    }

    private void DisableHorizontalMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = false; // Desactiva el controlador del jugador si aplica
        }
    }

    private void EnableHorizontalMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = true; // Reactiva el controlador del jugador si aplica
        }
    }

    private void ExitLadder()
    {
        isClimbing = false;
        if (playerRb != null)
        {
            playerRb.gravityScale = 1f; // Restaura la gravedad normal
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f); // Detener cualquier movimiento vertical
        }
        EnableHorizontalMovement(); // Reactivar el movimiento horizontal
    }
}
