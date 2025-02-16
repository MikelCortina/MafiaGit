using UnityEngine;
using UnityEngine.UIElements;

public class EnemyFSM : MonoBehaviour
{
    //Campo de vision, detecciones 
    public float visionRange = 10f;
    public float visionAngle = 60f;
    public LayerMask playerLayer;
    public Transform player;
    public float alertTime = 10f; // Tiempo de espera en alerta
    public float attackTime = 6f; // Tiempo de espera en ataque
    private float coberturaTimer = 0f;
    public float coberturaTime = 3f;


    public float circleRadius = 5f; // Radio del c�rculo
    public Vector2 circleCenter;   // Centro del c�rculo
    public LayerMask bulletLayer;

    //Estados
    public enum EnemyState { Idle, Alert, Attack }
    public EnemyState currentState = EnemyState.Idle;
    public bool atacando = false;
    public GameObject alertaSprite; // Referencia al sprite de alerta


    private bool playerInSight = false;
    private bool playerInVisionRange = false;
    private float alertTimer = 0f;
    private float attackTimer = 0f;
    private float enemyHealth = 100f;


    //Coberturas

    public float radioDeCobertura = 1f;
    public bool coberturaEstado = false;
    private Renderer objectRenderer;


    //Disparo
    public float shootInterval = 4f;
    private bool coberturaPlayer;
    public GameObject bulletPrefab;
    private float lastShot = 0;
    public Transform firePoint;
    public GameObject bulletPrefabEffect;
    public float bulletSpeed;

    private void Start()
    {
        if (alertaSprite != null)
        {
            alertaSprite.SetActive(false); // Oculta el sprite de alerta al inicio
        }
    }
    void Update()
    {
        if (alertaSprite != null)
        {
            alertaSprite.transform.position = transform.position + new Vector3(0, 1.5f, 0); // Ajusta la posici�n encima del enemigo
        }

        // Detecci�n y l�gica de estado
        DetectPlayer();
        DetectEnemy();

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Alert:
                HandleAlertState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
        }
    }

    void DetectEnemy()
    {
        circleCenter = transform.position;

        // Detectar colisiones dentro del c�rculo
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(circleCenter, circleRadius, bulletLayer);

        // Procesar los objetos detectados
        foreach (Collider2D collider in hitColliders)
        {
            Debug.Log($"Detectado: {collider.name}");
            visionRange = 15f;
            HandleAttackState();
        }


    }

    void DetectPlayer()
    {
        // Calcular direcci�n y �ngulo hacia el jugador
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector2.Angle(-transform.right, directionToPlayer);
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        playerInSight = angle < visionAngle / 2;
        playerInVisionRange = playerInSight && distanceToPlayer < visionRange;

        // Cambiar de estado seg�n la detecci�n
        if (playerInVisionRange)
        {
            if (currentState == EnemyState.Idle)
            {
                currentState = EnemyState.Alert;
                alertTimer = 0f; // Reiniciar temporizador al entrar en alerta
            }
        }
        else if (!playerInVisionRange && currentState == EnemyState.Alert)
        {
            // Si el jugador no est� visible y estamos en alerta, iniciar temporizador
            alertTimer += Time.deltaTime;
            if (alertTimer >= alertTime)
            {
                currentState = EnemyState.Idle;
            }
        }
    }


    void HandleIdleState()
    {

        if (alertaSprite != null)
        {
            alertaSprite.SetActive(false); // Desactiva el sprite de alerta
        }
        alertTimer = 0f;

    }
    void HandleAlertState()
    {
        if (alertaSprite != null)
        {
            alertaSprite.SetActive(true); // Activa el sprite de alerta
        }

        if (playerInVisionRange)
        {
            // Incrementar temporizadores solo si el jugador est� en rango
            attackTimer += Time.deltaTime;
            coberturaTimer += Time.deltaTime;

            // Transici�n al estado de ataque
            if (attackTimer >= attackTime)
            {
                currentState = EnemyState.Attack;
                attackTimer = 0f; // Reiniciar el temporizador solo despu�s de la transici�n
                coberturaTimer = 0f;
            }

            // Comportamiento de cobertura
            if (coberturaTimer >= coberturaTime)
            {
                Cobertura();
            }
        }
        else
        {
            // Si el jugador no est� visible y estamos en alerta, iniciar temporizador
            alertTimer += Time.deltaTime;
            if (alertTimer >= alertTime)
            {
                currentState = EnemyState.Idle;
            }
        }
    }





    void HandleAttackState()
    {
        if (alertaSprite != null)
        {
            alertaSprite.SetActive(false); // Desactiva el sprite de alerta
        }
        //Debug.Log("Estado: Ataque");
        FueraCobertura();
        FireBullet();
        atacando = true;

        // Simular l�gica de ataque (puedes agregar tu propia implementaci�n aqu�)

        if (!playerInVisionRange)
        {
            // Si el jugador ya no es visible, volver al estado de alerta
            currentState = EnemyState.Alert;
        }
        else if (enemyHealth <= 0)
        {
            // Si el enemigo muere, cambiar a estado Idle o manejar muerte
            currentState = EnemyState.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState = EnemyState.Alert;
        Bullet bullet = collision.GetComponent<Bullet>();
        if (collision.CompareTag("Bullet"))
        {
            if (!coberturaEstado)
            {
                enemyHealth -= bullet.damage;
                Debug.Log("El enemigo tiene " + enemyHealth + " de salud.");
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("El enemigo est� cubierto.");
                Destroy(collision.gameObject);
            }

            if (enemyHealth > 0)
            {
                currentState = EnemyState.Attack;
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("El enemigo ha muerto.");
            }
        }
    }

    private void Cobertura()
    {
        coberturaEstado = true;
        CambiarEscala(new Vector3(0.5f, 0.5f, 1f));
        Debug.Log("Entra en cobertura");
        
    }
    private void FueraCobertura()
    {
        coberturaEstado = false;
        CambiarEscala(new Vector3(0.5f, 1f, 1f));
        Debug.Log("Sale de cobertura");
    }

    void CambiarEscala(Vector3 nuevaEscala)
    {
        transform.localScale = nuevaEscala;
    }
    private void FireBullet()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector2.Angle(-transform.right, directionToPlayer);
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Cobertura coberturaComponente = player.GetComponent<Cobertura>();
        lastShot += Time.deltaTime;
       
        //Debug de la cobertura del aliado
        if (coberturaComponente != null)
        {
            coberturaPlayer = coberturaComponente.coberturaEstado;
        }
        else
        {
            coberturaPlayer = false;
            Debug.LogWarning("El jugador no tiene el componente Cobertura.");
        }


        if(lastShot>=shootInterval)
        {
            Debug.Log($"Te est�n disparando a una distancia de {distanceToPlayer} unidades.");

            // Crear la bala
            GameObject bulletInstance = Instantiate(bulletPrefab, player.position, Quaternion.identity);
            EnemyBullet bullet = bulletInstance.GetComponent<EnemyBullet>();
            ShootEffect();
            // Determinar el da�o en funci�n de la distancia
            if (distanceToPlayer > 7.5f)
            {
                bullet.damage = coberturaPlayer ? 0f : 11f;
            }
            else if (distanceToPlayer > 5f && distanceToPlayer <= 7.5f)
            {
                bullet.damage = coberturaPlayer ? 0f : 33f;
            }
            else if (distanceToPlayer <= 5f)
            {
                bullet.damage = coberturaPlayer ? 0f : 66f;
            }
            lastShot = 0;
        }
       
    }
    void ShootEffect()
    {
        // Calculamos la direcci�n del disparo (desde el firePoint hacia el jugador)
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Instanciamos el proyectil en el punto de disparo
        GameObject bullet = Instantiate(bulletPrefabEffect, firePoint.position, Quaternion.identity);

        // Asignamos la velocidad al proyectil
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed; // Aplica velocidad en la direcci�n del jugador
        }
        // Ajustamos la rotaci�n del proyectil para que apunte en la direcci�n correcta
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 left = transform.position + (Quaternion.Euler(0, 0, visionAngle / 2) * -transform.right) * visionRange;
        Vector3 right = transform.position + (Quaternion.Euler(0, 0, -visionAngle / 2) * -transform.right) * visionRange;
        Gizmos.DrawLine(transform.position, left);
        Gizmos.DrawLine(transform.position, right);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -transform.right * visionRange);
        Gizmos.color = coberturaEstado ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeCobertura);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);


    }
}
