using UnityEngine;
using UnityEngine.UI;

public class BarraDeEnergia : MonoBehaviour
{
    public Cobertura coberturaScript; // Referencia al script Cobertura
    public Image barraDeEnergiaRelleno;

    void Update()
    {
        if (coberturaScript != null)
        {
            // Asegurarte de que la barra represente la proporción actual de energía
            barraDeEnergiaRelleno.fillAmount = coberturaScript.energia / coberturaScript.energiaMaxima;
        }
    }

    // Método opcional para modificar la energía desde el script Cobertura
    public void CambiarEnergia(float cantidad)
    {
        if (coberturaScript != null)
        {
            coberturaScript.energia += cantidad;
        }
    }
}
