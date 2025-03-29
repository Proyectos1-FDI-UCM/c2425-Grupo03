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
    /// <summary>
    /// El tiempo de margen que le da al jugador para presionar a la tecla de saltar y efectuar el salto antes de caer al suelo
    /// </summary>
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
    /// <summary>
    /// //El rigidbody del jugador
    /// </summary>
    Rigidbody2D _rigidbody;
    /// <summary>
    /// //el contexto para acceder a parametros globales del playerstatemachine
    /// </summary>
    PlayerStateMachine _ctx;
    /// <summary>
    ///  //tiempo en el que el jugador puede saltar sin llegar al suelo
    /// </summary>
    float _jumpBuffer;
    /// <summary>
    /// //para detectar si el jugador esta en movimiento
    /// </summary>
    float _moveDir; 
    /// <summary>
    /// El audioSource que reprocude los pasos
    /// </summary>
    AudioSource _audioSource;

    /// <summary>
    /// Si esta en el suelo
    /// </summary>
    bool _isGrounded;
    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Metodo llamado al instanciar el script
    /// // Asigna la referencia a _ctx y _rigidbody
    /// </summary>
    private void Start()
    {
        
        _ctx = GetCTX<PlayerStateMachine>();
        //Coge el rigidbody del jugador
        _rigidbody = _ctx.Rigidbody;
        //Si el jugador mantiene pulsado el salto, solo lo detecta 1 vez.
        _ctx.PlayerInput.Jump.started += (InputAction.CallbackContext context) => _jumpBuffer = _jumpBufferTime;
        //Coge le audio source para hacer sonar los pasos
        _audioSource = _ctx.PlayerAudio;
    }
    /// <summary>
    /// Metodo que actualiza todo el rato
    /// // Va restando al tiempo de jumpBuffer segun el tiempo. 
    /// </summary>
    private void Update()
    {
        if ( _jumpBuffer > 0)
        {
            _jumpBuffer-=Time.deltaTime;
        }
    }
    /// <summary>
    /// // El trigger debe solo tocar la layer del suelo.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer ("Ground"))
        {
            _isGrounded = true;
        }

    }
    /// <summary>
    /// El trigger debe solo tocar la layer del suelo.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
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
        _audioSource?.Stop();
        Ctx.Animator?.SetBool("IsIdle", false);
        Ctx.Animator?.SetBool("IsRunning", false);
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
        //_moveDir será 0 si no esta moviendo el jugador
        _moveDir = GetCTX<PlayerStateMachine>().PlayerInput.Move.ReadValue<float>(); 
        if (_ctx != null && _ctx.PlayerInput.Move.ReadValue<float>() != 0 )
        {
            Ctx.Animator.SetBool("IsRunning", true);
            Ctx.Animator.SetBool("IsIdle", false);
            PlayerAttackState attackState = Ctx.GetStateByType<PlayerAttackState>();
            attackState?.ResetAttackCombo();
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }

        }
        else
        {
            Ctx.Animator.SetBool("IsIdle", true);
            Ctx.Animator.SetBool("IsRunning", false);
            _audioSource?.Stop();
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_ctx == null) return;

        //si jumpBuffer es mayor que 0, pasa a jumpState
        if (_jumpBuffer > 0) 
        {
            Ctx.ChangeState(Ctx.GetStateByType<PlayerJumpState>());
            _jumpBuffer = 0;

        }
        else if (_jumpBuffer < 0)
        {
            _jumpBuffer = 0;
        }
        //si esta cayendo el jugador, pasa a Falling
        else if (!_isGrounded) 
        {
            PlayerFallingState fallingState = Ctx.GetStateByType<PlayerFallingState>();
            Ctx.ChangeState(fallingState);
            fallingState.ResetCoyoteTime();
        }
        //detecta si presionas al Dash
        else if (_ctx.PlayerInput.Dash.IsPressed()) 
        {
            PlayerDashState dashState = _ctx.GetStateByType<PlayerDashState>();
            if (Time.time > dashState.NextAvailableDashTime)
            {
                Ctx.ChangeState(dashState);
            }
        }
        else if (_ctx.PlayerInput.Attack.triggered)
        {
            PlayerAttackState attackState = _ctx.GetStateByType<PlayerAttackState>();
            if (Time.time > attackState.NextAttackTime)
            {
                Ctx.ChangeState(attackState);
            }
        }
        else if (_ctx.PlayerInput.ManoDeLasSombras.IsPressed() && _ctx.GetComponent<PlayerCharge>().ManoDeLasSombras.isCharged  && !Ctx.GetStateByType<PlayerManoDeLasSombrasState>().IsLocked)
        {
            PlayerManoDeLasSombrasState playerManoDeLasSombras = _ctx.GetStateByType<PlayerManoDeLasSombrasState>();
            Ctx.ChangeState(playerManoDeLasSombras);
        }
        else if (_ctx.PlayerInput.SuperDash.triggered && _ctx.GetComponent<PlayerCharge>().SuperDash.isCharged && !Ctx.GetStateByType<PlayerSuperDashState>().IsLocked)
        {
            PlayerSuperDashState playerSuperDashState = _ctx.GetStateByType<PlayerSuperDashState>();
            Ctx.ChangeState(playerSuperDashState);
        }
    }
    #endregion   

} // class PlayerGroundedState 
// namespace
