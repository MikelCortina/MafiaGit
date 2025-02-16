using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float visionRange = 10f;  // Rango máximo del campo de visión
    public float visionAngle = 60f;  // Ángulo del campo de visión (en grados)
    public LayerMask playerLayer;  // Capa del jugador (para filtrar las detecciones)
    public Transform player; // Referencia al jugador

    private bool playerInSight = false; // Para saber si el jugador está dentro del campo de visión

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        // Obtener la dirección hacia el jugador desde el enemigo
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // El enemigo mira hacia la izquierda, por lo tanto invertimos la dirección en la que está mirando
        Vector2 enemyLookDirection = -transform.right; // Esto hace que el enemigo mire a la izquierda

        // Comprobar si el jugador está dentro del ángulo de visión
        float angle = Vector2.Angle(enemyLookDirection, directionToPlayer);  // Comparar el ángulo de visión del enemigo hacia la izquierda

        // Si el ángulo está dentro del rango de visión y la distancia es correcta, realizar el raycast
        if (angle < visionAngle / 2)
        {
            // Raycast hacia el jugador para ver si está dentro del campo de visión
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, visionRange, playerLayer);  // Lanzamos el raycast hacia la izquierda

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                if (!playerInSight) // Si el jugador entra en el campo de visión
                {
                    Debug.Log("Jugador detectado dentro del campo de visión a la izquierda");
                    playerInSight = true; // Marcamos que el jugador está dentro del campo de visión
                }
            }
            else
            {
                if (playerInSight) // Si el jugador sale del campo de visión
                {
                    Debug.Log("Jugador salió del campo de visión");
                    playerInSight = false; // Marcamos que el jugador salió del campo de visión
                }
            }
        }
        else
        {
            if (playerInSight) // Si el jugador sale del campo de visión
            {
                Debug.Log("Jugador salió del campo de visión");
                playerInSight = false; // Marcamos que el jugador salió del campo de visión
            }
        }

       
    }
    private void OnDrawGizmos()
    {
        // Dibujar el campo de visión en forma de cono
        Gizmos.color = Color.red;

        // Dibujar un cono de visión desde la posición del enemigo
        Vector3 left = transform.position + (Quaternion.Euler(0, 0, visionAngle / 2) * -transform.right) * visionRange;
        Vector3 right = transform.position + (Quaternion.Euler(0, 0, -visionAngle / 2) * -transform.right) * visionRange;

        Gizmos.DrawLine(transform.position, left);
        Gizmos.DrawLine(transform.position, right);

        // Dibujar el raycast hacia la izquierda
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -transform.right * visionRange);
    }
}
