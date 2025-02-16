using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 10000f;  // Duración de vida del proyectil
    public float speed = 20f;    // Velocidad del proyectil
    private Rigidbody2D rb;
    public float damage;// Referencia al Rigidbody2D

    void Start()
    {
        // Obtén el componente Rigidbody2D de la bala
        rb = GetComponent<Rigidbody2D>();

        // Si el Rigidbody no tiene una velocidad inicial, dale una
        // Esto solo se usará si la velocidad no se estableció en el script de disparo
        // Destruye la bala después de un tiempo determinado
        Destroy(gameObject, lifeTime);
    }

  
}
