using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float vidaEnemigo = 99f;
    public EnemyFSM enemy;
    public Image barraVida; // Referencia a la barra de vida
    public Transform barraVidaPosicion; // Posición donde se mostrará la barra de vida

    private Coroutine barraVidaCoroutine; // Para controlar el tiempo de aparición de la barra

    void Start()
    {
        // Ocultar la barra de vida al inicio
        if (barraVida != null)
        {
            barraVida.fillAmount = 1; // Barra llena
            barraVida.gameObject.SetActive(false); // Ocultar barra
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (collision.CompareTag("Bullet"))
        {
            if (!enemy.coberturaEstado)
            {
                // Reducir la vida del enemigo
                float vidaAnterior = vidaEnemigo;
                vidaEnemigo -= bullet.damage;
                Debug.Log("El enemigo tiene " + vidaEnemigo);

                // Mostrar la barra de vida
                MostrarBarraVida();

                // Actualizar la barra de vida
                if (barraVida != null)
                {
                    barraVida.fillAmount = vidaEnemigo / 99f; // Actualizar proporcionalmente
                }

                // Destruir la bala
                Destroy(collision.gameObject);

                // Verificar si el enemigo muere
                if (vidaEnemigo <= 0)
                {
                    Destroy(enemy.alertaSprite);
                    Destroy(gameObject);
                    Debug.Log("El enemigo ha muerto");
                }
            }

            else
            {
                Debug.Log("El enemigo está cubierto!");
                Destroy(collision.gameObject);
            }
        }
        if (collision.CompareTag("Destructor"))
        {

            Destroy(enemy.alertaSprite);
            Destroy(gameObject);
            Debug.Log("El enemigo ha muerto");
        }
    }
    private void MostrarBarraVida()
    {
        if (barraVida != null)
        {
            // Mostrar la barra de vida
            barraVida.gameObject.SetActive(true);

            // Cancelar cualquier coroutine previo para reiniciar el temporizador
            if (barraVidaCoroutine != null)
            {
                StopCoroutine(barraVidaCoroutine);
            }

            // Iniciar el temporizador para ocultar la barra
            barraVidaCoroutine = StartCoroutine(OcultarBarraVida());
        }
    }

    private IEnumerator OcultarBarraVida()
    {
        yield return new WaitForSeconds(5f); // Esperar 5 segundos

        // Ocultar la barra de vida
        if (barraVida != null)
        {
            barraVida.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Actualizar la posición de la barra de vida (opcional si el enemigo se mueve)
        if (barraVida != null && barraVidaPosicion != null)
        {
            barraVida.transform.position = barraVidaPosicion.position;
        }
    }
}

