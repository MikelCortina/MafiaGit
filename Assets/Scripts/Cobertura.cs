using UnityEngine;

public class Cobertura : MonoBehaviour
{
    public float radioDeCobertura = 1f; // Radio para la detecci�n
    public bool coberturaEstado = false; // Estado de cobertura
    public GameObject player; // Referencia al jugador
    private Renderer objectRenderer; // Para manejar el color del objeto
    public float energia = 100f;
    public float energiaMaxima = 100f;
    public float duracionCoberturaPorEnergia = 0.5f;
    

    public bool CoberturaEstado => coberturaEstado;
    void Start()
    {
        // Obtener el Renderer del objeto en el que est� el script
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No se encontr� un Renderer en el objeto.");
        }

        // Verificar que se haya asignado un jugador
        if (player == null)
        {
            Debug.LogError("No se asign� un jugador al script.");
        }
    }

    void Update()
    {
        // Detecta si hay un objeto en el rango de cobertura
        Collider2D cobertura = Physics2D.OverlapCircle(transform.position, radioDeCobertura, LayerMask.GetMask("Cobertura"));
        
        if (cobertura != null)
        {
            if (Input.GetKeyDown(KeyCode.C) && energia>0)
            {
                coberturaEstado = true;
                Debug.Log("Estoy cubierto");
               
            }
        }
        else
        {
            coberturaEstado = false;
        }
        if (CoberturaEstado==true)
        {
            energia -= Time.deltaTime * 10;
            if (energia <= 0)
            {
                coberturaEstado = false;
            }
        }
        if (CoberturaEstado == false && energia<energiaMaxima)
        {
            energia += Time.deltaTime * 10;
            if (energia <= 0)
            {
                coberturaEstado = false;
            }
        }

        // Eliminar cobertura si el jugador se mueve
        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null && Mathf.Abs(playerRb.linearVelocity.x) > 0.3f)
            {
                coberturaEstado = false;
            }
        }

        // Cambiar el color dependiendo del estado de cobertura
        if (coberturaEstado)
        {
            CambiarEscala(new Vector3(0.5f, 0.5f, 1f));
            CambiarColor(Color.blue); // Cambia el color a azul si est� cubierto
        }
        else
        {
            CambiarEscala(new Vector3(0.5f, 1f, 1f));
            CambiarColor(Color.white); // Cambia el color a blanco si no est� cubierto
        }
    }

    // M�todo para cambiar el color del objeto
    void CambiarColor(Color nuevoColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = nuevoColor;
        }
    }
    void CambiarEscala(Vector3 nuevaEscala)
    {
        transform.localScale = nuevaEscala;
    }

}

