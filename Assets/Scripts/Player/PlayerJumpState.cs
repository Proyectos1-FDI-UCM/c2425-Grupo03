//---------------------------------------------------------
// Estado de salto del jugador
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerJumpState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    /// <summary>
    /// Altura maxima que puede saltar el jugador
    /// </summary>
    [SerializeField] float _maxHeight;
    /// <summary>
    /// El sonido de salto del jugador
    /// </summary>
    [SerializeField] AudioClip _jumpSound;
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
    ///  //para detectar si el jugador esta en movimiento
    /// </summary>
    float _moveDir;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Metodo llamado al instanciar el script
    ///         // Asigna la referencia a _ctx y _rigidbody
    /// </summary>
    private void Start()
    {

        _ctx = GetCTX<PlayerStateMachine>();
        _rigidbody = _ctx.Rigidbody;
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
    /// Determina si el subestado es Move o Idle dependiendo de si esta en movimiento el jugador
    /// // le aplica una fuerza hacia arriba para que salte el jugador
    /// </summary>
    public override void EnterState()
    {
      
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Sqrt(-2 * _maxHeight * Physics2D.gravity.y * _ctx.GravityScale)); 
        if (_moveDir != 0)//si movimiento no es nulo
        {
            SetSubState(Ctx.GetStateByType<PlayerMoveState>());
        }
        else
        {
            SetSubState(Ctx.GetStateByType<PlayerIdleState>());
        }
        _ctx.Animator.SetBool("IsJumping", true);
        SoundManager.Instance.PlaySFX(_jumpSound, transform, 1);
    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _ctx.Animator.SetBool("IsJumping", false);
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
        _moveDir = InputManager.Instance.MoveDirection;//_moveDir será 0 si no esta moviendo el jugador
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_rigidbody.velocity.y <= 0) //detecta si esta cayendo el jugador, pasa su estado a falling
        {
            Ctx.ChangeState(Ctx.GetStateByType<PlayerFallingState>());
        }
        else if (InputManager.Instance.DashIsPressed()) // detecta si el jugador presiona al dash.
        {
            PlayerDashState dashState = _ctx.GetStateByType<PlayerDashState>();
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
        else if (InputManager.Instance.superDashIsPressed() && _ctx.GetComponent<PlayerCharge>().SuperDash.isCharged && !Ctx.GetStateByType<PlayerSuperDashState>().IsLocked)
        {
            PlayerSuperDashState playerSuperDashState = _ctx.GetStateByType<PlayerSuperDashState>();
            Ctx.ChangeState(playerSuperDashState);
        }
    }

    #endregion   

} // class PlayerJumpState 
// namespace
