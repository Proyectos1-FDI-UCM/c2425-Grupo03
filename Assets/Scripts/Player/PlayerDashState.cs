//---------------------------------------------------------
// Estado que permite hacer un dash al jugador.
// Adrian Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// Clase que extiende BaseState para hacer el estado Dash del jugador.
/// </summary>
public class PlayerDashState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.

    [Header("Dash properties")]
    /// <summary>
    /// Distancia que se mueve el jugador al hacer un dash
    /// </summary>
    [Tooltip("Distance moved while dashing in units.")]
    [SerializeField][Min(0)] float _distance;

    /// <summary>
    /// Cuanto tarda en recorrer la distancia indicada
    /// </summary>
    [Tooltip("Duration of the dash in seconds.")]
    [SerializeField][Range(0.1f, 1)] float _duration;

    /// <summary>
    /// Cuanto tarda en poder volver a hacer un dash
    /// </summary>
    [Tooltip("Time needed after dashing to dash again. Starts counting after dash began.")]
    [SerializeField][Min(0)] float _rechargeTime;

    /// <summary>
    /// El trigger a desactivar mientras hace el dash para que los enemigos no hagan daño al jugador
    /// </summary>
    [Tooltip("Trigger deactivated while dashing.")]
    [SerializeField] BoxCollider2D _playerTrigger;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    /// <summary>
    /// Tiempo en el que debe terminar el dash
    /// </summary>
    float _finishDashingTime;

    /// <summary>
    /// Tiempo en el que se podrá volver a hacer un dash
    /// </summary>
    float _nextAvailableDashTime = -1;

    /// <summary>
    /// El Rigidbody del jugador.
    /// </summary>
    Rigidbody2D _rb;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Coge el Rigidbody del contexto para tener la referencia.
    /// </summary>
    void Start()
    {
        _rb = GetCTX<PlayerStateMachine>().Rigidbody;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// Pone los valores que hacen el dash y establece el tiempo para finalizar el dash.
    /// Si no se puede hacer un dash cambia a otro estado.
    /// </summary>
    public override void EnterState()
    {
        if (Time.time > _nextAvailableDashTime)
        {
            _rb.velocity = new Vector2(_distance * (short)GetCTX<PlayerStateMachine>().LookingDirection / _duration
                                        , 0);
            _rb.gravityScale = 0;
            _finishDashingTime = Time.time + _duration;
            _nextAvailableDashTime = Time.time + _rechargeTime;
            _playerTrigger.enabled = false;
        }
        else
        {
            ChangeState(Ctx.GetStateByType<PlayerFallingState>());
        }
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = GetCTX<PlayerStateMachine>().GravityScale;
        _playerTrigger.enabled = true;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí

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
        if (Time.time > _finishDashingTime) ChangeState(Ctx.GetStateByType<PlayerFallingState>());
    }

    #endregion   

} // class PlayerDashState 
// namespace
