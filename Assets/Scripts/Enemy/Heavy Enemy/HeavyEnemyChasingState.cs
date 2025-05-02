//---------------------------------------------------------
// Estado de chase del enemigo pesado
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado de chase del enemigo pesado
/// </summary>
public class HeavyEnemyChasingState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    /// <summary>
    /// Velocidad a la que camina el enemigo.
    /// </summary>
    [SerializeField]
    [Tooltip("Enemy walking speed in units per second")]
    float _enemyWalkingSpeed;

    /// <summary>
    /// el tiempo que se queda parado en frentre del jugador antes de atacar
    /// </summary>
    [SerializeField]
    float _waitTimeToAttack;

    [SerializeField] AudioClip _heavyStep;
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
    /// Referencia del tipo HeavyEnemyStatemachine del contexto.
    /// </summary>
    HeavyEnemyStateMachine _ctx;

    /// <summary>
    /// Referencia del rigidbody del enemigo.
    /// </summary>
    Rigidbody2D _rb; 
    /// <summary>
    /// Referencia a la direccion que mira el enemigo
    /// </summary>
    HeavyEnemyStateMachine.EnemyLookingDirection _enemyLookingDirection;
    /// <summary>
    /// si debe girar o no de direccion
    /// </summary>
    bool _shouldFlip = false;
    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// contador local del tiempo
    /// </summary>
    private float _startAttackTime = -1;

    /// <summary>
    /// si puede atacar
    /// </summary>
    bool _goAttack = false;

    private bool _wasMoving = false;
    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<HeavyEnemyStateMachine>();
        //Coge la referencia al rigidbody por comodidad
        _rb = _ctx?.Rigidbody;
        _animator = _ctx.GetComponent<Animator>();
    }
    #endregion
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_ctx != null)
        {
            //Si el jugador sale del trigger pone el range a false.
            _ctx.IsPlayerInChaseRange = false;
        }
    }

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
        _animator?.SetBool("IsChasing", true);
        _startAttackTime = -1;
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _ctx.CantMove();
        _shouldFlip = false;
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
        }
        _animator?.SetBool("IsChasing", false);
        _startAttackTime = -1;
        _goAttack = false;
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
        if (_ctx != null && _rb != null)
        {
            //calcula la direccion que debe de mirar
            HeavyEnemyStateMachine.EnemyLookingDirection newDirection =
            (_ctx.PlayerTransform.position.x - _ctx.transform.position.x) > 0 ?
            HeavyEnemyStateMachine.EnemyLookingDirection.Right : HeavyEnemyStateMachine.EnemyLookingDirection.Left;

            //si no coincide con su direccion actual, debe de girarse
            _shouldFlip = newDirection != _ctx.LookingDirection;

            bool isMovingNow = _ctx.IsMoving();
            //Si todavía hay plataforma o no esta preparando el ataque se mueve, sino se detiene
            if (CheckGround() && _ctx.IsMoving() && _startAttackTime < 0)
            {
                _rb.velocity = new Vector2(_enemyWalkingSpeed * (short)_ctx.LookingDirection, 0);
                if (!_wasMoving)
                {
                    SoundManager.Instance.PlaySFX(_heavyStep, transform, 1);
                }
            }
            else
            {
                _rb.velocity = Vector3.zero;
            }
            _wasMoving = isMovingNow;
            //si hay un enemigo en area de ataque, empieza a preparar el ataque
            if (_ctx.IsPlayerInAttackRange)
            {
                if (_startAttackTime < 0)
                {
 
                    _startAttackTime = Time.time;
                    _animator?.SetBool("IsChasing", false);
                    _animator?.SetBool("IsIdle", true);
                }

                else if (Time.time - _startAttackTime > _waitTimeToAttack)
                {
                    //Si el jugador esta en el rango de ataque, pasa a atacar
                    _animator?.SetBool("IsChasing", false);
                    _animator?.SetBool("IsIdle", false);
                    _goAttack = true;
                }
            }
            else
            {
                _startAttackTime = -1;
                _animator?.SetBool("IsIdle", false);
                _animator?.SetBool("IsChasing", true);
            }
        }
    }
    /// <summary>
    /// Comprueba si esta en el suelo
    /// </summary>
    /// <returns>Devuelve true si está tocando el suelo, false si no</returns>
    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + 0.5f * (float)_ctx.LookingDirection, gameObject.transform.position.y),
            Vector2.down, 5f, LayerMask.GetMask("Platform"));
        return hit.collider != null;
    }
    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_shouldFlip)
        {
            Ctx.ChangeState(Ctx.GetStateByType<HeavyEnemyFlipState>());
        }
        else if (!_ctx.IsPlayerInChaseRange)
        {
            //Si el jugador sale de la distancia de persecución vuelve al estado inactivo.
            Ctx.ChangeState(Ctx.GetStateByType<HeavyEnemyIdleState>());
        }
        else if (_goAttack)
        {
            Ctx.ChangeState(Ctx.GetStateByType<HeavyEnemyAttackState>());
        }
    }

    #endregion   

} // class HeavyEnemyChasingState 
// namespace
