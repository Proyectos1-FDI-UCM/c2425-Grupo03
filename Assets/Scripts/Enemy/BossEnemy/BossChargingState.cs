//---------------------------------------------------------
// El estado de carga (corriendo contra el jugador) del jefe final
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// El estado de carga (corriendo contra el jugador) del jefe final
/// </summary>
public class BossChargingState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Movement Speed")]
    /// <summary>
    /// Velocidad del jefe al iniciar la carga contra el jugador
    /// </summary>
    [SerializeField]
    [Min(0f)]
    [Tooltip("Speed at the beginning of the charge")]
    float _launchSpeed;

    /// <summary>
    /// Aceleración de la velocidad
    /// </summary>
    [SerializeField]
    [Min(0f)]
    [Tooltip("Deceleration after passing below the player")]
    float _speedDeceleration;

    [Header("Damage")]
    /// <summary>
    /// Daño causado al jugador al entrar en contacto con el jefe
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _damage;

    /// <summary>
    /// el sonido de girar
    /// </summary>
    [SerializeField]AudioClip _spinnerSound;
    /// <summary>
    /// el sonido de impactar
    /// </summary>
    [SerializeField] AudioClip _hitWall;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Referencia por cómodidad al contexto
    /// </summary>
    BossStateMachine _ctx;

    /// <summary>
    /// Flag para determinar si ha chocado con un muro
    /// </summary>
    bool _hasHitWall;

    /// <summary>
    /// Flag para determinar si ha chocado contra el jugador
    /// </summary>
    bool _hasHitPlayer;

    /// <summary>
    /// Flag para determinar si ha pasado por debajo del jugador
    /// </summary>
    bool _hasMissedPlayer;

    /// <summary>
    /// Audiosource que reproduce los pasos
    /// </summary>
    AudioSource _audioSource;

   
    #endregion


    // ---- MÉTODOS DE MONOBEHABIOUR ----
    #region Métodos de Monobehaviour
    /// <summary>
    /// Trigger que detecta solo al jugador
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerStateMachine>() != null)
        {
            _hasHitPlayer = true;
        }
        
    }
    /// <summary>
    /// Collider que detecta solo las paredes
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _hasHitWall = true;

            // Mueve el objeto en la dirección contraria un poquito para que pueda volver a hacer OnCollisionEnter2D al volver a cargar en la misma dirección
            Ctx.Rigidbody.MovePosition(Ctx.Rigidbody.position - (Vector2.right * (int)_ctx.LookingDirection * 2f));
        }
    }

    /// <summary>
    /// Trigger que detecta solo al jugador
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStateMachine>() != null)
        {
            _hasHitPlayer = false;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Al entrar en el estado.
    /// </summary>
    public override void EnterState()
    {
        // Calcula la dirección en la que debe mirar el jefe
        _ctx.LookingDirection = GetNewLookingDirection();

        // setup de las flags
        _hasHitWall = false;
        _hasHitPlayer = false;
        _hasMissedPlayer = false;

        // Pone la animación correcta
        Ctx.Animator.SetBool("IsCharging", true);

        // Pone la velocidad del jefe
        Ctx.Rigidbody.velocity = new Vector2(_launchSpeed * (int)_ctx.LookingDirection, 0);
        _audioSource = SoundManager.Instance.PlaySFXWithAudioSource(_spinnerSound, transform, 0.6f);
    }

    /// <summary>
    /// Al salir en el estado.
    /// </summary>
    public override void ExitState()
    {
        // Calcula la dirección nueva tras terminar de cargar contra el jugador
        _ctx.LookingDirection = GetNewLookingDirection();

        // Termina la animación
        Ctx.Animator.SetBool("IsCharging", false);
        _audioSource.Stop();

        SoundManager.Instance.PlaySFX(_hitWall, transform, 1);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    /// <returns>La distancia al jugador</returns>
    private float DistanceToPlayer()
    {
        if (_ctx.Player != null)
        {
            return _ctx.Player.transform.position.x - transform.position.x;
        }
        else
        {
            return 0;
        }
    }

    /// <returns>La dirección en la que debe mirar el jefe en función de la posición del jugador</returns>
    private BossStateMachine.EnemyLookingDirection GetNewLookingDirection()
    {
        return DistanceToPlayer() < 0 ? 
            BossStateMachine.EnemyLookingDirection.Left : BossStateMachine.EnemyLookingDirection.Rigth;
    }
    /// <summary>
    /// Pone la referencia a la máquina de estados por comodidad
    /// </summary>
    protected override void OnStateSetUp()
    {
        _ctx = GetCTX<BossStateMachine>();
    }

    /// <summary>
    /// Actualización cada frame si estamos en este estado
    /// </summary>
    protected override void UpdateState()
    {
        if (_hasMissedPlayer)
        {
            // Si falla al jugador comienza a decelerar
            float nextVelocityX = Ctx.Rigidbody.velocity.x - (Mathf.Sign(Ctx.Rigidbody.velocity.x) * _speedDeceleration * Time.deltaTime);
            
            // Nos aseguramos que no pase de 0
            nextVelocityX = nextVelocityX > 0 ? Mathf.Max(nextVelocityX, 0) : Mathf.Min(nextVelocityX, 0);
            
            // Ponemos la nueva velocidad
            Ctx.Rigidbody.velocity = new Vector2(nextVelocityX, 0);
        }
        else if(GetNewLookingDirection() != _ctx.LookingDirection)
        {
            // Si debería cambiar de dirección es porque ha fallado
            _hasMissedPlayer = true;
        }
    }

    /// <summary>
    /// Comprueba si hay que cambiar de estado
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_hasHitWall)
        {
            // Si choca contra un muro vamos al estado vulnerable
            Ctx.ChangeState(Ctx.GetStateByName("Vulnerable"));
        }
        else if(_hasHitPlayer)
        {
            // Si hemos chocado contra el jugador le quitamos vida y cambiamos a idle
            _ctx.Player?.GetComponent<HealthManager>()?.RemoveHealth(_damage);
            Ctx.ChangeState(Ctx.GetStateByName("Idle"));
        }
        else if (Mathf.Abs(Ctx.Rigidbody.velocity.x) < 0.1f && _hasMissedPlayer)
        {
            // Si hemos fallado pero no hemos chocado contra el muro volvemos a cargar
            Ctx.ChangeState(Ctx.GetStateByName("Precharge"));
        }
    }
    #endregion

} // class NewStateMachineScript 
// namespace
