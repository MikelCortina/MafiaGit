using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab del proyectil
    public GameObject bulletPrefabEffect;
    public Transform player;
    public GameObject player1;
    public float bulletSpeed = 20f; // Velocidad del proyectil
    public Transform firePoint;
    private bool disparando;
    public bool isWaitingToShoot;
    private SpriteRenderer objectRenderer;
    private Transform posEnemyTarget;
    public float cargadorRecamara = 2f;
    public float capacidadCargador = 2f;
    public bool isReloading;
    private float reloadTime = 4f;

    public Image barraRecarga; // La imagen de la barra
    public Transform barraRecargaPosicion; // Posición encima del jugador

    void Start()
    {
        // Asigna el SpriteRenderer del objeto actual
        objectRenderer = player1.GetComponent<SpriteRenderer>();

        // Ocultar la barra al inicio
        if (barraRecarga != null)
        {
            barraRecarga.fillAmount = 0; // Vacía la barra
            barraRecarga.gameObject.SetActive(false); // Oculta la barra
        }
    }
    void Update()
    {
        // Si el jugador presiona el botón izquierdo del ratón, disparamos
        if (Input.GetMouseButtonDown(0) && IsMouseOverEnemy() && !isReloading)
        {
            if (CanShoot() && cargadorRecamara > 0)
            {
                cargadorRecamara -= 1;
                Debug.Log("Has disparado, te quedan " + cargadorRecamara + " balas");
                StartCoroutine(CasteoDisparo());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(ReloadWeapon());
        }

        // Si el jugador está moviéndose, no puede disparar
        if (isWaitingToShoot)
        {
            CambiarColor(Color.black);
        }
        else
        {
            CambiarColor(Color.clear);
        }

        // Actualizar la posición de la barra de recarga (opcional si se mueve el jugador)
        if (barraRecarga != null && barraRecargaPosicion != null)
        {
            barraRecarga.transform.position = barraRecargaPosicion.position;
        }
    }
    IEnumerator ReloadWeapon()
    {
        isReloading = true;
        Debug.Log("Recargando...");

        // Mostrar la barra de recarga
        if (barraRecarga != null)
        {
            barraRecarga.gameObject.SetActive(true);
        }

        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < reloadTime)
        {
            tiempoTranscurrido += Time.deltaTime;
            if (barraRecarga != null)
            {
                barraRecarga.fillAmount = tiempoTranscurrido / reloadTime; // Llenar progresivamente
            }
            yield return null; // Esperar un frame
        }

        // Una vez terminado el tiempo de recarga
        cargadorRecamara = capacidadCargador;
        Debug.Log("Arma recargada");

        // Ocultar la barra de recarga
        if (barraRecarga != null)
        {
            barraRecarga.fillAmount = 0; // Vacía la barra
            barraRecarga.gameObject.SetActive(false); // Oculta la barra
        }

        isReloading = false;
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
    private void Shoot()
    {

        float distanceToEnemy = Vector2.Distance(posEnemyTarget.position, transform.position);
        Debug.Log($"Disparando al enemigo a una distancia de {distanceToEnemy} unidades.");

        GameObject bulletInstance = Instantiate(bulletPrefab, posEnemyTarget.position, Quaternion.identity);
        Bullet bullet = bulletInstance.GetComponent<Bullet>();
        if (distanceToEnemy > 10f)
        {
            bullet.damage = 33f;
        }
        else if (distanceToEnemy > 7.5f && distanceToEnemy <= 10f)
        {
            bullet.damage = 66f;
        }
        else if (distanceToEnemy <= 7.5f)
        {
            bullet.damage = 100f;
        }
    }
    private IEnumerator CasteoDisparo()
    {
        isWaitingToShoot = true;
        Debug.Log("eSTAS ESPERANDO PARA IDPRARA");
        if (CanShoot())
        {
            yield return new WaitForSeconds(0.5f);
            Shoot();
            ShootEffect();
            isWaitingToShoot=false;
        }

    }


    // Función que verifica si el ratón está encima de un enemigo
    bool CanShoot()
    {
        Vector2 direction = (posEnemyTarget.position - transform.position).normalized;


        LayerMask layerMask = LayerMask.GetMask("Obstaculo");
        float distance = (posEnemyTarget.position - transform.position).magnitude;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);

        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstaculo"))
        {
            Debug.Log("Obstáculo detectado: " + hit.collider.name);
            return false;
        }
        return true;
    }

    bool IsMouseOverEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Creamos el rayo desde el ratón
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); // Raycast en 2D

        // Verificamos si el rayo ha golpeado un objeto con la etiqueta "Enemigo"
        if (hit.collider != null && hit.collider.CompareTag("Enemigo"))
        {
            posEnemyTarget = hit.collider.transform;
            return true; // Si es un enemigo, retornamos true
        }
        if (hit.collider != null && hit.collider.CompareTag("Interruptor"))
        {
            posEnemyTarget = hit.collider.transform;
            return true; // Si es un enemigo, retornamos true
        }

        return false; // Si no es un enemigo, retornamos false
    }
    void CambiarColor(Color nuevoColor)
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = nuevoColor;
        }
    }

}
