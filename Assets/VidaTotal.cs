using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaTotal : MonoBehaviour
{
    public PlayerMovement vidaScript; // Referencia al script Cobertura
    public Image barraDeVidaRelleno;

    void Update()
    {
        if (vidaScript != null)
        {
            // Asegurarte de que la barra represente la proporción actual de energía
            barraDeVidaRelleno.fillAmount = vidaScript.vidaActual/ vidaScript.vidaTotal;
        }
    }

    // Método opcional para modificar la energía desde el script Cobertura
    public void CambiarEnergia(float cantidad)
    {
        if (vidaScript != null)
        {
            vidaScript.vidaTotal += cantidad;
        }
    }
}
