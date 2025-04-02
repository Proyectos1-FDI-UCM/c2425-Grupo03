//---------------------------------------------------------
// Simple estado para la caída del enemigo
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado que cambia a la animación de caer hasta que la velocidad sea 0
/// </summary>
public class EnemyFallingState : BaseState
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Booleana que determina si el enemigo toca el suelo o no.
    /// </summary>
    bool _isGrounded;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si el enemigo ha entrado en contacto con un objeto en la capa del suelo. ( 1 << 7 ) es la capa del suelo.
        if (collision.gameObject.layer == 7)
        {
            Debug.Log("is gound");
            _isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Detecta si el enemigo ha dejado de estar en contacto con un objeto en la capa del suelo. ( 1 << 7 ) es la capa del suelo.
        if (collision.gameObject.layer == 7)
        {
            _isGrounded = false;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        // Cambia a la animación a caer (WIP)
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        // Quita la animación de caer (WIP)
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Metodo llamado cada frame cuando este es el estado activo de la maquina de estados.
    /// </summary>
    protected override void UpdateState()
    {
        
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        
        // Cuando el enemigo vuelva a tocar el suelo vuelve a estar inactivo
        if (_isGrounded)
        {
            // Cambiamos al estado cuyo nombre sea IdleState
            Ctx.ChangeState(Ctx.GetStateByName("IdleState"));
        }
    }

    #endregion   

} // class EnemyFallingState 
// namespace
