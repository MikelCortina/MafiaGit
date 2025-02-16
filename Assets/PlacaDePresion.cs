using UnityEngine;
using System.Collections;

public class PlacaPresion : MonoBehaviour
{
    public GameObject elevator;      // Referencia al objeto del ascensor
    public float moveDistance = 5f;  // Distancia que el ascensor se moverá
    public float moveSpeed = 2f;     // Velocidad de movimiento del ascensor

    private Vector3 startPosition;   // Posición inicial del ascensor
    private Vector3 targetPosition;  // Posición objetivo del ascensor
    private Coroutine currentCoroutine;

    void Start()
    {
        if (elevator != null)
        {
            startPosition = elevator.transform.position;
            targetPosition = startPosition + new Vector3(0, moveDistance, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            // Detenemos cualquier coroutine previa para evitar conflictos
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveElevator(targetPosition));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Dog"))
        {
            // Detenemos cualquier coroutine previa para evitar conflictos
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(MoveElevator(startPosition));
        }
    }

    private IEnumerator MoveElevator(Vector3 destination)
    {
        while (Vector3.Distance(elevator.transform.position, destination) > 0.01f)
        {
            elevator.transform.position = Vector3.MoveTowards(elevator.transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        elevator.transform.position = destination; // Asegurarse de que la posición final es exacta
    }
}
