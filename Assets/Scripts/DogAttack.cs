using Unity.VisualScripting;
using UnityEngine;

public class DogAtack : MonoBehaviour
{
    public GameObject perroPrefab;
    public GameObject bulletPrefab;
    public Transform player;
    public Transform dog;
    public float bulletSpeed = 20f;
    private float lastShotTime = -Mathf.Infinity;
    public float cooldown = 10f;

    void Update()
    {
        // Si el jugador presiona el bot�n izquierdo del rat�n, disparamos
        if (Input.GetMouseButtonDown(1) && !(Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f)) // 0 es el bot�n izquierdo del rat�n
        {
            if (IsMouseOverEnemy() && !IsRayBlockedByObstacle() && (Time.time - lastShotTime>=cooldown)) // Verificamos si el ratón está sobre un enemigo y si el rayo no está bloqueado por un obstáculo
            {
                Shoot();
            }
        }
        if (Input.GetMouseButtonDown(1) && !(Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f)) // 0 es el bot�n izquierdo del rat�n
        {
            if (IsMouseOverObstacle() && !IsRayBlockedByObstacle() && (Time.time - lastShotTime >= cooldown)) // Verificamos si el ratón está sobre un enemigo y si el rayo no está bloqueado por un obstáculo
            {
                Shoot();
            }
        }
        if (Input.GetMouseButtonDown(1) && !IsMouseOverEnemy())
        {
            Debug.Log("No se puede disparar porque el ratón no está sobre un enemigo.");
        }

        if (Input.GetMouseButtonDown(1) && (Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f))
        {
            Debug.Log("No se puede mandar al perro mientras te mueves");
        }
    }

    void Shoot()
    {
        // Obtenemos la posici�n del rat�n en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Aseguramos que el eje Z est� en 0 para 2D

        // Instanciamos el proyectil en la posici�n del rat�n
        GameObject bullet = Instantiate(bulletPrefab, mousePosition, Quaternion.identity);

        // Calculamos la direcci�n del disparo (por ejemplo, hacia un punto espec�fico)
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Asignamos la velocidad al proyectil
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed; // Aplica velocidad en la direcci�n del disparo
        }

        // Ajustamos la rotaci�n del proyectil para que apunte en la direcci�n correcta
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    bool IsMouseOverEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Creamos el rayo desde el ratón
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); // Raycast en 2D

        // Verificamos si el rayo ha golpeado un objeto con la etiqueta "Enemigo"
        if (hit.collider != null && hit.collider.CompareTag("Enemigo"))
        {
            return true; // Si es un enemigo, retornamos true
        }
        if (hit.collider != null && hit.collider.CompareTag("Enemigo"))
        {
            return true; // Si es un enemigo, retornamos true
        }

        return false; // Si no es un enemigo, retornamos false
    }
    bool IsMouseOverObstacle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Creamos el rayo desde el ratón
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); // Raycast en 2D

        // Verificamos si el rayo ha golpeado un objeto con la etiqueta "Enemigo"
        if (hit.collider != null && hit.collider.CompareTag("Interruptor"))
        {
            return true; // Si es un enemigo, retornamos true
        }

        return false; // Si no es un enemigo, retornamos false
    }
    bool IsRayBlockedByObstacle()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        LayerMask layerMask = LayerMask.GetMask("Obstaculo", "Enemigo");

        // Realizamos un Raycast desde el jugador hacia el ratón, con un alcance largo (Mathf.Infinity)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, layerMask);

        // Verificamos si el rayo ha golpeado un objeto con la etiqueta "Obstaculo"
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstaculo"))
        {
            Debug.Log("Obstáculo detectado: " + hit.collider.name);
            return true; // Si hay un obstáculo, retornamos true
        }

        return false; // Si no hay obstáculos, retornamos false
     
    }
}
