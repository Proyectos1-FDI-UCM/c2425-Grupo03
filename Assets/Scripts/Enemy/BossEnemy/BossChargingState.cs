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
    /// <summary>
    /// Velocidad del jefe al iniciar la carga contra el jugador
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _launchSpeed;

    /// <summary>
    /// aceleración de la velocidad
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _speedDeceleration;

    /// <summary>
    /// Daño causado al jugador al entrar en contacto con el jefe
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _damage;

    /// <summary>
    /// Frecuencia de golpes mientras esté en contacto con el jugador
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _damageFrequency;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    BossStateMachine _ctx;

    bool _hasHitWall;

    bool _hasHitPlayer;

    bool _hasMissedPlayer;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHABIOUR ----
    #region Métodos de Monobehaviour
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerStateMachine>() != null)
        {
            _hasHitPlayer = true;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _hasHitWall = true;
            Ctx.Rigidbody.MovePosition(Ctx.Rigidbody.position - (Vector2.right * (int)_ctx.LookingDirection * 0.2f));
        }
    }
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


    public override void EnterState()
    {
        _ctx.LookingDirection = GetNewLookingDirection();

        _hasHitWall = false;
        _hasHitPlayer = false;
        _hasMissedPlayer = false;

        Ctx.Animator.SetBool("IsCharging", true);
        Ctx.Rigidbody.velocity = new Vector2(_launchSpeed * (int)_ctx.LookingDirection, 0);
    }
    

    public override void ExitState()
    {
        _ctx.LookingDirection = GetNewLookingDirection();
        Ctx.Animator.SetBool("IsCharging", false);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
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
    private BossStateMachine.EnemyLookingDirection GetNewLookingDirection()
    {
        return DistanceToPlayer() < 0 ? BossStateMachine.EnemyLookingDirection.Left : BossStateMachine.EnemyLookingDirection.Rigth;
    }

    protected override void OnStateSetUp()
    {
        _ctx = GetCTX<BossStateMachine>();
    }
    protected override void UpdateState()
    {
        if (_hasMissedPlayer)
        {
            float nextVelocityX = Ctx.Rigidbody.velocity.x - (Mathf.Sign(Ctx.Rigidbody.velocity.x) * _speedDeceleration * Time.deltaTime);
            nextVelocityX = nextVelocityX > 0 ? Mathf.Max(nextVelocityX, 0) : Mathf.Min(nextVelocityX, 0);

            Ctx.Rigidbody.velocity = new Vector2(nextVelocityX, 0);
            
        }
        else if(GetNewLookingDirection() != _ctx.LookingDirection)
        {
            _hasMissedPlayer = true;
        }
    }

    protected override void CheckSwitchState()
    {
        if (_hasHitWall)
        {
            Ctx.ChangeState(Ctx.GetStateByName("Vulnerable"));
        }
        else if(_hasHitPlayer)
        {
            _ctx.Player?.GetComponent<HealthManager>()?.RemoveHealth(_damage);
            Ctx.ChangeState(Ctx.GetStateByName("Idle"));
        }
        else if (Mathf.Abs(Ctx.Rigidbody.velocity.x) < 0.1f && _hasMissedPlayer)
        {
            Ctx.ChangeState(Ctx.GetStateByName("Precharge"));
        }
    }
    #endregion

} // class NewStateMachineScript 
// namespace
