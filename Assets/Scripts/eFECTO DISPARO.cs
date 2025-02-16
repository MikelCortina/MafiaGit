using UnityEngine;


public class eFECTODISPARO : MonoBehaviour
{
    public GameObject bulletPrefabEffect;    // Prefab del proyectil
    public Transform player;
    public Transform firePoint;        // Punto de origen del disparo
    public float bulletSpeed = 20f; 
    private PlayerShooting disparo;// Velocidad del proyectil

    void Update()
    {
        // Si el jugador presiona el bot�n izquierdo del rat�n, disparamos
        if (Input.GetMouseButtonDown(0) && !(Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f)) // 0 es el bot�n izquierdo del rat�n
        {
            if (IsMouseOverEnemy()) // Verificamos si el ratón está sobre un enemigo
            {
                ShootEffect();
            }
        }
        if (Input.GetMouseButtonDown(0) && (Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f))
        {
            Debug.Log("No se puede disparar mientras te mueves");
        }
    }

    void ShootEffect()
    {
 
        // Obtenemos la posici�n del rat�n en el mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculamos la direcci�n del disparo (desde el firePoint hacia el rat�n)
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // Instanciamos el proyectil en el punto de disparo
        GameObject bullet = Instantiate(bulletPrefabEffect, firePoint.position, Quaternion.identity);

        // Asignamos la velocidad al proyectil
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed; // Aplica velocidad en la direcci�n del cursor
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

        return false; // Si no es un enemigo, retornamos false
    }

}