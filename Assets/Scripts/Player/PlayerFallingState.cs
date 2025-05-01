//---------------------------------------------------------
// El estado de caer del jugador
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// El estado de caer del jugador
/// </summary>
[RequireComponent (typeof(IsGroundedCheck))]
public class PlayerFallingState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    /// <summary>
    /// //tiempo en el que el jugador puede saltar aunque este en el aire despues de caer de una plataforma
    /// </summary>
    [SerializeField][Min(0)] float _maxCoyoteTime;
    /// <summary>
    /// //velocidad maxima del jugador para caer
    /// </summary>
    [SerializeField] float _maxSpeed; 

    /// <summary>
    /// Sonido que hace el jugador al caer al suelo
    /// </summary>
    [SerializeField] AudioClip _landSound;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    /// <summary>
    /// //El rigidbody del jugador
    /// </summary>
    Rigidbody2D _rigidbody;
    /// <summary>
    /// //el contexto para acceder a parametros globales del playerstatemachine
    /// </summary>
    PlayerStateMachine _ctx;
    /// <summary>
    /// //parametro para saber si el jugador ha caido de una plataforma y si puede seguir saltando
    /// </summary>
    float _coyoteTime;
    /// <summary>
    /// //para detectar si el jugador esta en movimiento
    /// </summary>
    float _moveDir;

    /// <summary>
    /// Componente que mira si la entidad toca el suelo o no
    /// </summary>
    IsGroundedCheck _isGroundedCheck;

    /// <summary>
    /// Si esta en el suelo
    /// </summary>
    bool _isGrounded => _isGroundedCheck.IsGrounded();

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Metodo llamado al instanciar el script
    /// Asigna la referencia a _ctx y _rigidbody
    /// </summary>
    private void Start()
    {

        _ctx = GetCTX<PlayerStateMachine>();
        _rigidbody = Ctx?.Rigidbody;
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
    /// Determina si el subestado es Move o Idle dependiendo de si esta en movimiento el jugador
    /// </summary>
    public override void EnterState()
    {
        if (_moveDir != 0) //si movimiento no es nulo
        {
            SetSubState(Ctx.GetStateByType<PlayerMoveState>());
        }
        else
        {
            SetSubState(Ctx.GetStateByType<PlayerIdleState>());
        }
        Ctx?.Animator.SetBool("IsFalling", true);

    }
    /// <summary>
    /// //resetea el coyote time al maximo fijado desde el editor de unity
    /// </summary>
    public void ResetCoyoteTime()
    {
        _coyoteTime = _maxCoyoteTime; 
    }
    /// <summary>
    /// cuando sale del estado falling, pone el coyoteTime a 0
    /// </summary>
    public override void ExitState()
    {
        _coyoteTime = 0;
        Ctx?.Animator.SetBool("IsFalling",false);
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
    /// _moveDir será 0 si no esta moviendo el jugador y va restando el coyoteTime si no es 0
    /// </summary>
    protected override void UpdateState()
    {
        _moveDir = InputManager.Instance.MoveDirection;

        if (_coyoteTime > 0) 
        {
            _coyoteTime -= Time.deltaTime;
        }
        else if (_coyoteTime < 0) _coyoteTime = 0;

        
    }
    /// <summary>
    /// Aplica la velocidad al rigidbody si no ha llegado a la velocidad maxima
    /// </summary>
    protected override void FixedUpdateState()
    {
        if (_rigidbody.velocity.y < _maxSpeed) 
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _maxSpeed);
        }
    }
    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Ctx != null)
        {
            if (_isGrounded) //detecta si esta colisionando con el suelo para pasar al estado Grounded
            {
                SoundManager.Instance.PlaySFX(_landSound, transform, 1);
                Ctx.ChangeState(Ctx.GetStateByType<PlayerGroundedState>());
            }
            else if (_coyoteTime > 0 && InputManager.Instance.jumpWasPressedThisFrame()) // detecta si el jugador a dado a saltar o si el coyotetime es mayor que 0 para pasar la estado Jump
            {
                Ctx.ChangeState(Ctx.GetStateByType<PlayerJumpState>());
            }
            else if (InputManager.Instance.DashIsPressed()) // detecta si el jugador ha presionado al dash
            {
                PlayerDashState dashState = Ctx.GetStateByType<PlayerDashState>();
                if (Time.time > dashState.NextAvailableDashTime)
                {
                    Ctx.ChangeState(dashState);
                }
            }
            else if (InputManager.Instance.attackTriggered() && !_ctx.AttackedOnAir)
            {
                PlayerAirAttackState airAttackState = _ctx.GetStateByType<PlayerAirAttackState>();
                Ctx.ChangeState(airAttackState);
            }
        }
    }

    #endregion   

} // class PlayerFallingState 
// namespace
