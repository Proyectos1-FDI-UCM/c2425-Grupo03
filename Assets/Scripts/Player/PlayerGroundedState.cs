//---------------------------------------------------------
// Estado del jugador cuando esta en el suelo
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;

//---------------------------------------------------------
// Estado del jugador cuando esta en el suelo
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerGroundedState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField][Min(0)] float _jumpBufferTime;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    Rigidbody2D _rigidbody; //El rigidbody del jugador
    PlayerStateMachine _ctx; //el contexto para acceder a parametros globales del playerstatemachine
    float _jumpBuffer; //tiempo en el que el jugador puede saltar sin llegar al suelo
    float _moveDir; //para detectar si el jugador esta en movimiento

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
    /// </summary>
    private void Start()
    {
        // Asigna la referencia a _ctx y _rigidbody
        _ctx = GetCTX<PlayerStateMachine>();
        _rigidbody = _ctx.Rigidbody;
        //Si el jugador mantiene pulsado el salto, solo lo detecta 1 vez.
        _ctx.PlayerInput.Jump.started += (InputAction.CallbackContext context) => _jumpBuffer = _jumpBufferTime;
    }
    /// <summary>
    /// Metodo que actualiza todo el rato
    /// </summary>
    private void Update()
    {
        if ( _jumpBuffer > 0)
        {
            _jumpBuffer-=Time.deltaTime;// Va restando al tiempo de jumpBuffer segun el tiempo. 
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
    /// Determina si el subestado es Move o Idle dependiendo de si esta en movimiento el jugador
    /// </summary>
    public override void EnterState()
    {
        if (_moveDir != 0)//si movimiento no es nulo
        {
            SetSubState(Ctx.GetStateByType<PlayerMoveState>());
        }
        else
        {
            SetSubState(Ctx.GetStateByType<PlayerIdleState>());
        }
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _ctx.Animator.SetBool("IsIdle", false);
        _ctx.Animator.SetBool("IsRunning", false);
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
        _moveDir = GetCTX<PlayerStateMachine>().PlayerInput.Move.ReadValue<float>(); //_moveDir será 0 si no esta moviendo el jugador
        if (_ctx.PlayerInput.Move.ReadValue<float>() != 0 )
        {
            _ctx.Animator.SetBool("IsRunning", true);
            _ctx.Animator.SetBool("IsIdle", false);
        }
        else
        {
            _ctx.Animator.SetBool("IsIdle", true);
            _ctx.Animator.SetBool("IsRunning", false);
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_jumpBuffer > 0) //si jumpBuffer es mayor que 0, para a jumpState
        {
            ChangeState(Ctx.GetStateByType<PlayerJumpState>());
        }
        else if (_rigidbody.velocity.y < 0) //si esta cayendo el jugador, pasa a Falling
        {
            PlayerFallingState fallingState = Ctx.GetStateByType<PlayerFallingState>();
            ChangeState(fallingState);
            fallingState.ResetCoyoteTime();
        }
        else if (_ctx.PlayerInput.Dash.IsPressed()) //detecta si presionas al Dash
        {
            PlayerDashState dashState = _ctx.GetStateByType<PlayerDashState>();
            if(Time.time > dashState.NextAvailableDashTime) ChangeState(dashState);
        }
        else if (_ctx.PlayerInput.Attack.IsPressed())
        {
            PlayerAttackState attackState = _ctx.GetStateByType<PlayerAttackState>();
            if (Time.time > attackState.NextAttackTime) ChangeState(attackState);
        }
    }
    #endregion   

} // class PlayerGroundedState 
// namespace
