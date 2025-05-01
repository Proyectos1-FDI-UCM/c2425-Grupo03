//---------------------------------------------------------
// ataque en el aire del jugado
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
public class PlayerAirAttackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [Header("Propiedad del ataque")]
    /// <summary>
    /// El radio de ataque del jugador
    /// </summary>
    [SerializeField, Min(0)] private float _attackRadius;
    /// <summary>
    /// El daño del ataque basico
    /// </summary>
    [SerializeField] private float _damage;

    /// <summary>
    /// El porcentaje que se añade a las habilidades
    /// </summary>
    [SerializeField] private float _abilityChargePercentage;

    /// <summary>
    /// el tiempo que tarda en hacer el ataque
    /// </summary>
    [SerializeField] private float _attackTime;

    /// <summary>
    /// el sfx de atacar
    /// </summary>
    [SerializeField] AudioClip _airHit;

    [Header("Knockback Properties")]
    [SerializeField, Min(0)] float _knockbackDistance;
    [SerializeField, Min(0)] float _knockbackTime;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _startAttackTime;
    /// <summary>
    /// La dirección donde mira el jugador
    /// </summary>
    private int _direction;

    /// <summary>
    /// El script que controla las habilidades
    /// </summary>
    private PlayerCharge _chargeScript;

    /// <summary>
    /// el ctx del playerstatemachine del player
    /// </summary>
    private PlayerStateMachine _ctx;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        _chargeScript = Ctx.GetComponent<PlayerCharge>();
        _ctx = GetCTX<PlayerStateMachine>();
        _ctx?.OnAirAttackAddListener(AirAttack);
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
        //Coger la dirección donde mira el jugador del contexto
        _direction = (int)GetCTX<PlayerStateMachine>()?.LookingDirection;
        //La animacion
        Ctx.Animator.SetBool("IsAirAttacking", true);
        _startAttackTime = Time.time;
        _ctx.Rigidbody.velocity = Vector3.zero;
        Ctx.Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _ctx.AttackedOnAir = true;
        Ctx.Animator.SetBool("IsAirAttacking", false);
        _ctx.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }   
    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// el metodo donde realiza el ataque en el aire
    /// </summary>
    private void AirAttack()
    {
        SoundManager.Instance.PlaySFX(_airHit, transform, 1);

        //Coger la informacion de los enemigos que estan en el area de ataque
        Vector2 position = transform.position + (new Vector3(_attackRadius, 0) * _direction);
        Collider2D[] enemiesInArea = Physics2D.OverlapCircleAll(position, _attackRadius, 1 << 10);

        if (enemiesInArea != null)
        {
            foreach (Collider2D enemy in enemiesInArea)
            {
                var enemySM = enemy?.GetComponent<EnemyStateMachine>();
                var summonerSM = enemy?.GetComponent<EnemySummonerStateMachine>();
                var heavySM = enemy?.GetComponent<HeavyEnemyStateMachine>();

                KnockbackState knockback = null;

                if (enemySM != null)
                {
                    knockback = enemySM?.GetStateByType<KnockbackState>();
                }
                else if (summonerSM != null)
                {
                    knockback = summonerSM?.GetStateByType<KnockbackState>();
                }
                else if (heavySM != null)
                {
                    knockback = heavySM?.GetStateByType<KnockbackState>();
                }

                knockback?.ApplyKnockBack(_knockbackDistance, _knockbackTime, (Vector2.right * (float)_ctx.LookingDirection));
                //Añadir carga a las habilidades

                //Daño al enemigo
                HealthManager health = enemy?.GetComponent<HealthManager>();

                if (health?.RemoveHealth((int)_damage) == true)
                {
                    _chargeScript?.AddCharge((_abilityChargePercentage / 100) * _damage);
                }
            }
        }
    }

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
        if (Time.time - _startAttackTime > _attackTime)
        {
           Ctx.ChangeState(Ctx.GetStateByType<PlayerFallingState>());
        }
    }

    #endregion   

} // class PlayerAirAttackState 
// namespace
