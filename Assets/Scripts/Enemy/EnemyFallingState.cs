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
[RequireComponent (typeof(IsGroundedCheck))]
public class EnemyFallingState : BaseState
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Componente que mira si la entidad toca el suelo o no
    /// </summary>
    IsGroundedCheck _isGroundedCheck;

    /// <summary>
    /// Si esta en el suelo
    /// </summary>
    bool _isGrounded => _isGroundedCheck.IsGrounded();
    Animator _animator;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        //Coge el componente que mira si la entidad toca el suelo
        _isGroundedCheck = GetComponent<IsGroundedCheck>();
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
        _animator = Ctx.GetComponent<Animator>();

        //Ponemos la animación correspondiente a aparecer
        _animator?.SetBool("IsFalling", true);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _animator?.SetBool("IsFalling", false);
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
