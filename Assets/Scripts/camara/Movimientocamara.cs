using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Referencia al Transform del jugador
    public float smoothSpeed = 0.125f; // Velocidad de suavizado de la cámara
    public Vector3 offset;          // Desplazamiento de la cámara respecto al jugador

    void LateUpdate()
    {
        // Solo seguimos al jugador en el eje X (manteniendo las posiciones Y y Z fijas)
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);

        // Interpolamos suavemente hacia la posición deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplicamos la posición suavizada a la cámara
        transform.position = smoothedPosition;
    }
}
