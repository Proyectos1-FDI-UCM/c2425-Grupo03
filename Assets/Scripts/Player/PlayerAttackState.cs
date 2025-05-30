//---------------------------------------------------------
// Estado de ataque del jugador
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase para hacer el estado de ataque del jugador
/// </summary>
public class PlayerAttackState : BaseState
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
    /// El tiempo de espera entre dos ataques
    /// </summary>
    [SerializeField] private float _attackSpeed;

    /// <summary>
    /// El daño del ataque basico
    /// </summary>
    [SerializeField] private float _damage;

    /// <summary>
    /// El porcentaje que se añade a las habilidades
    /// </summary>
    [SerializeField] private float _abilityChargePercentage;

    /// <summary>
    /// El array de sonido del ataque
    /// </summary>
    [SerializeField] private AudioClip[] _airHitList;



    [Header("Propiedad del combo")]

    /// <summary>
    /// El numero maximo de combo, cuando llega a este numero el siguiente ataque resetea el combo
    /// </summary>
    [SerializeField] private int _maxCombo;
    /// <summary>
    /// El tiempo de gracia para encadenar combo
    /// </summary>
    [SerializeField, Min(1)] float _comboDuration;
    /// <summary>
    /// El daño extra añadido al ataque si encadenas el combo
    /// </summary>
    [SerializeField,Min(0)] int _comboExtraDamage;
    /// <summary>
    /// Dibujar rango del ataque
    /// </summary>
    [SerializeField] bool _drawRange = false;
    /// <summary>
    /// How much the player moves when attacking
    /// </summary>
    [SerializeField, Min(0)] float _amountMoved;

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

    /// <summary>
    /// El índice del combo en el que esta el jugador
    /// </summary>
    private int _combo;
    ///// <summary>
    ///// El numero maximo de combo interno para hacer el calculo de combo
    ///// </summary>
    //private int _internMaxCombo;
    /// <summary>
    /// El tiempo cuando termina el tiempo de gracia
    /// </summary>
    private float _endOfCombo;
    /// <summary>
    /// La dirección donde mira el jugador
    /// </summary>
    private int _direction;
    /// <summary>
    /// El contexto del playerstatemachine
    /// </summary>
    PlayerStateMachine _ctx;
    /// <summary>
    /// El script que controla las habilidades
    /// </summary>
    private PlayerCharge _chargeScript;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.

    /// <summary>
    /// El tiempo en el que se podrá volver a hacer un ataque
    /// </summary>
    public float NextAttackTime { get; private set; }
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        //Coger el rigidbody del contexto
        _ctx = GetCTX<PlayerStateMachine>();
        _chargeScript = _ctx?.GetComponent<PlayerCharge>();
        _combo = 0;
        _ctx?.OnAttackAddListener(Attack);
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
    /// Resetear el combo a 0 para cuando se mueve el jugador
    /// </summary>
    public void ResetAttackCombo()
    {
        _combo = 0;
    }


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        //Coger la dirección donde mira el jugador del contexto
        _direction = (int)GetCTX<PlayerStateMachine>()?.LookingDirection;


        //Calcular el tiempo para realizar el siguiente ataque
        NextAttackTime = Time.time + _attackSpeed;

        //Avanza el jugador en una dirección pero lo detiene si detecta que la plataforma no continua
        Collider2D col = Physics2D.Raycast(transform.position + _direction * _amountMoved * Vector3.right, Vector2.down, 1.2f, 1 << 7).collider;
        if(col != null)
        {
            Ctx.Rigidbody.MovePosition(transform.position + _direction * _amountMoved * Vector3.right);
        }

        //Actualizar combo
        UpdateCombo();

        //La animacion
        Ctx.Animator.SetFloat("AttackIndex", _combo);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        Ctx.Animator.SetFloat("AttackIndex", 0);
    }

    /// <summary>
    /// Hacer un CirclecastAll en la capa del "Enemy" en la dirección donde mira el jugador.
    /// Añadir el daño extra del combo si lo hay, y hace daño a todos los enemigos que están en el circlecast.
    /// </summary>
    private void Attack()
    {
        SoundManager.Instance.PlaySFX(_airHitList[_combo], transform, 1);
        int extraDamage = 0;

        //Coger la informacion de los enemigos que estan en el area de ataque
        Vector2 position = transform.position + (new Vector3(_attackRadius, 0) * _direction);
        RaycastHit2D[] enemyInArea = Physics2D.CircleCastAll(position, _attackRadius, new Vector2(0, 0), _attackRadius, 1 << 10);

        //Mirar el combo para el daño extra
        if (_combo == 3)
        {
            extraDamage += _comboExtraDamage;
        }


        if (enemyInArea != null)
        {
            foreach (RaycastHit2D enemy in enemyInArea)
            {

                var enemySM = enemy.collider?.GetComponent<EnemyStateMachine>();
                var summonerSM = enemy.collider?.GetComponent<EnemySummonerStateMachine>();
                var heavySM = enemy.collider?.GetComponent<HeavyEnemyStateMachine>();

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
                HealthManager health = enemy.collider?.GetComponent<HealthManager>();

                if (health?.RemoveHealth((int)_damage + extraDamage) == true)
                {
                    _ctx.InstantiateBonesVFX();
                    _chargeScript?.AddCharge((_abilityChargePercentage / 100) * _damage);
                }
            }
        }

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
        //Poner la velocidad del rigidbody a cero
        Ctx.Rigidbody.velocity = Vector3.zero;
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// Si el jugador mantiene la tecla atacar, pasa al estado playerxhargedattack, sino pasa al grounded state.
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Time.time > NextAttackTime)
        {
            if (InputManager.Instance.attackIsPressed() && !Ctx.GetStateByType<PlayerChargedAttackState>().IsLocked)
            {
                Ctx.ChangeState(Ctx.GetStateByType<PlayerChargedAttackState>());
            }
            else
            {
                Ctx.ChangeState(Ctx.GetStateByType<PlayerGroundedState>());
            }
        }

    }

    

    /// <summary>
    /// Actualizar el tiempo de gracia, y actualiza el combo
    /// </summary>
    private void UpdateCombo()
    {
        // Solo incrementa si el tiempo aún está dentro del combo
        if (Time.time <= _endOfCombo)
        {
            _combo++;
        }
        else
        {
            _combo = 1; 
        }

        // Siempre actualiza el fin del combo
        _endOfCombo = Time.time + _comboDuration;

        // Si pasa el máximo, vuelve a 1 
        if (_combo > _maxCombo)
        {
            _combo = 1;
        }
    }
    /// <summary>
    /// Dibuja el rango de ataque basico
    /// </summary>

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_drawRange)Gizmos.DrawWireSphere(transform.position + (_direction * new Vector3(_attackRadius,0)),_attackRadius);
    }

    #endregion   

} // class PlayerAttackState 
// namespace
