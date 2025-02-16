using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Collider2D playerCollider;

    void Start()
    {
        // Obtener el colisionador del jugador
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // Cambia la tecla según tus controles
        {
            IgnorePlatformCollision(true);
        }

        // Si el jugador suelta la tecla hacia abajo, restaurar la colisión
        if (Input.GetKeyUp(KeyCode.S))
        {
            IgnorePlatformCollision(false);
        }
    }

   public void IgnorePlatformCollision(bool ignore)
    {
        // Obtener todos los colisionadores del suelo con la etiqueta "Platform"
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");

        foreach (GameObject platform in platforms)
        {
            Collider2D platformCollider = platform.GetComponent<Collider2D>();

            if (platformCollider != null)
            {
                // Ignorar colisiones entre el jugador y las plataformas según el estado
                Physics2D.IgnoreCollision(playerCollider, platformCollider, ignore);
            }
        }
    }
}
