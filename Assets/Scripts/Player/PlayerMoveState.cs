//---------------------------------------------------------
// Estado que permite avanzar al jugador en el escenario.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerMoveState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    
    
    [Header("Movement Properties")]
    /// <summary>
    /// Velocidad con la cual se mueve el jugador.
    /// </summary>
    [Tooltip("The player's constant speed in units per second.")]
    [SerializeField][Min(0)] float _speed;
    [SerializeField] private ParallaxEffect ParallaxEffect;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// El Rigidbody del jugador.
    /// </summary>
    Rigidbody2D _rb;
    SpriteRenderer _sprite;
    /// <summary>
    /// La dirección del movimiento del jugador.
    /// </summary>
    private float _moveDir;

    /// <summary>
    /// El estado de ataque del jugador.
    /// </summary>
    PlayerAttackState _attackState;
    AudioSource _audioSource;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    public void Start()
    {
        _rb = GetCTX<PlayerStateMachine>().Rigidbody;
        _sprite = GetCTX<PlayerStateMachine>().SpriteRenderer;
        _attackState = Ctx.GetStateByType<PlayerAttackState>();
        _audioSource = GetComponent<AudioSource>();
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
        _attackState.ResetAttackCombo();
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _rb.velocity *= Vector2.up;
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
        //_moveDir = GetCTX<PlayerStateMachine>().PlayerInput.Move.ReadValue<float>();
        _moveDir = InputManager.Instance.MoveDirection;
        if (_moveDir < 0)
        {
            GetCTX<PlayerStateMachine>().LookingDirection = PlayerStateMachine.PlayerLookingDirection.Left;
            _sprite.flipX = true;
        }
        else if(_moveDir > 0) 
        {
            GetCTX<PlayerStateMachine>().LookingDirection = PlayerStateMachine.PlayerLookingDirection.Right;
            _sprite.flipX = false;
        }

        _rb.velocity = new Vector2(_moveDir * _speed, _rb.velocity.y);
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (InputManager.Instance.MoveDirection == 0)
        {
            Ctx.ChangeState(Ctx.GetStateByType<PlayerIdleState>());
        }
    }

    #endregion   

} // class PlayerMoveState 
// namespace
