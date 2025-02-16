using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Salir() 
    { 
        Application.Quit();
    }
    public void Opciones()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void DentroOpciones()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
