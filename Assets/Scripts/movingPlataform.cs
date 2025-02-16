using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 2f;  // Distancia que la plataforma sube y baja
    public float moveSpeed = 2f;     // Velocidad de movimiento

    private Vector3 startPosition;
    private bool isMoving = false;
    private bool isMovingUp = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform; // Hacer al jugador hijo de la plataforma
            if (!isMoving)
            {
                StartCoroutine(MovePlatform());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null; // Eliminar la relación de hijo cuando el jugador salga de la plataforma
        }
    }

    private IEnumerator MovePlatform()
    {
        isMoving = true;
        Vector3 targetPosition;

        if (isMovingUp)
        {
            targetPosition = startPosition + new Vector3(0, moveDistance, 0);
        }
        else
        {
            targetPosition = startPosition;
        }

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // Asegurarse de que la posición final es exacta
        isMovingUp = !isMovingUp;
        isMoving = false;
    }
}