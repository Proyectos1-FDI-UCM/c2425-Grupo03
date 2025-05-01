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
    /// Velocidad máxima del jefe mientras corre
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _maxSpeed;

    /// <summary>
    /// aceleración de la velocidad
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _speedAcceleration;

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

    float _nextHit;

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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _hasHitWall = true;
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
        _hasHitWall = false;
        Ctx.Animator.SetTrigger("PrepareChase");
    }

    public override void ExitState()
    {
        Ctx.Animator.ResetTrigger("PrepareChase");
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

    protected override void OnStateSetUp()
    {
        _ctx = GetCTX<BossStateMachine>();
    }
    protected override void UpdateState()
    {
        _ctx.LookingDirection = DistanceToPlayer() < 0 ? BossStateMachine.EnemyLookingDirection.Left : BossStateMachine.EnemyLookingDirection.Rigth;

        float nextVelocityX = Ctx.Rigidbody.velocity.x + ((int)_ctx.LookingDirection * _speedAcceleration * Time.deltaTime);
        nextVelocityX = Mathf.Clamp(nextVelocityX, -_maxSpeed, _maxSpeed);

        Ctx.Rigidbody.velocity = new Vector2(nextVelocityX, 0);

        if(_hasHitPlayer && Time.time > _nextHit)
        {
            _nextHit = Time.time + _damageFrequency;
            _ctx.Player?.GetComponent<HealthManager>()?.RemoveHealth(_damage);
        }
    }

    protected override void CheckSwitchState()
    {
        if (_hasHitWall)
        {
            Ctx.ChangeState(Ctx.GetStateByName("Vulnerable State"));
        }
    }
    #endregion

} // class NewStateMachineScript 
// namespace
