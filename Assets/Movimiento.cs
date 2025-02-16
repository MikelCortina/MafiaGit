using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    public Transform playerr;  // Referencia al jugador
    public float smoothTime = 0.3f; // Tiempo de suavizado
    private Vector3 velocity = Vector3.zero; // Velocidad de amortiguación

    void LateUpdate()
    {
        if (playerr != null)
        {
            // Posición objetivo de la cámara (manteniendo la misma Z)
            Vector3 targetPosition = new Vector3(playerr.position.x, playerr.position.y, transform.position.z);

            // Movimiento suave de la cámara con SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
