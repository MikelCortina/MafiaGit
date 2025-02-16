using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    public Image alertImage;  // Imagen para el estado de alerta
    public Image attackImage; // Imagen para el estado de ataque

    private EnemyFSM fsm;     // Referencia al script EnemyFSM
    private EnemyFSM.EnemyState lastState; // Último estado conocido de la FSM

    private void Start()
    {
        // Inicializar HUD y obtener referencia a EnemyFSM
        InitializeHUD();
        fsm = GetComponent<EnemyFSM>();
        if (fsm == null)
        {
            Debug.LogError("EnemyFSM no está asignado en EnemyHUD.");
            return;
        }

        // Configurar el HUD según el estado inicial de la FSM
        UpdateHUD(fsm.currentState);
        lastState = fsm.currentState;
    }

    private void Update()
    {
        if (fsm == null) return;

        // Detectar cambio de estado en la FSM
        if (fsm.currentState != lastState)
        {
            UpdateHUD(fsm.currentState);
            lastState = fsm.currentState; // Actualizar el último estado conocido
        }
    }

    /// <summary>
    /// Inicializa el HUD desactivando todas las imágenes.
    /// </summary>
    public void InitializeHUD()
    {
        if (alertImage != null) alertImage.gameObject.SetActive(false);
        if (attackImage != null) attackImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Actualiza el HUD en función del estado del enemigo.
    /// </summary>
    /// <param name="state">Estado actual del enemigo.</param>
    public void UpdateHUD(EnemyFSM.EnemyState state)
    {
        // Activar/desactivar imágenes según el estado
        if (alertImage != null) alertImage.gameObject.SetActive(state == EnemyFSM.EnemyState.Alert);
        if (attackImage != null) attackImage.gameObject.SetActive(state == EnemyFSM.EnemyState.Attack);
    }
}
