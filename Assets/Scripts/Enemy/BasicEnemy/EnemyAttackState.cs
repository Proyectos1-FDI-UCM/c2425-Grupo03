//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// El estado de ataque del enemigo
/// </summary>
public class EnemyAttackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [Header("Propiedad del ataque")]
    /// <summary>
    /// El radio de ataque del jugador
    /// </summary>
    [SerializeField, Min(0)] float _attackRadius;
    /// <summary>
    /// El tiempo de espera entre dos ataques
    /// </summary>
    [SerializeField] float _attackSpeed;
    /// <summary>
    /// El daño del ataque basico
    /// </summary>
    [SerializeField] float _damage;
    [SerializeField] AudioClip _attackSound;

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
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    private EnemyStateMachine _ctx;

    /// <summary>
    /// El tiempo cuando el enemigo pueda volver a atacar
    /// </summary>
    private float _nextAttackTime;

    /// <summary>
    /// Booleana para ver si ha terminado de atacar
    /// </summary>
    /// 
    private bool _attackFinished;

    /// <summary>
    /// La direccion donde apunta el enemigo
    /// </summary>
    private int _lookingDirection;


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
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemyStateMachine>();

        //Coger animator del contexto
        _animator = _ctx.GetComponent<Animator>();

        //Informar al contexto el rango de ataque del enemigo
        _ctx.AttackDistance = _attackRadius;
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
        //Coge la direccion donde mira el enemigo
        _lookingDirection = (int)_ctx.LookingDirection;

        _attackFinished = false;

        //Empezar la animacion
        _animator.SetBool("IsAttack", true);

        //Calcular el tiempo del siguiente ataque
        _nextAttackTime = Time.time + _attackSpeed;
    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _animator.SetBool("IsAttack", false);
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
        //Cuando termina la animacion pone la booleana a true
        if (Time.time > _nextAttackTime && !_attackFinished)
        {
            _attackFinished = true;
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        //Ataca cuando termina la animacion del ataque, con el _attackFinished
        if (Time.time > _nextAttackTime && _attackFinished)
        {
            Attack(_lookingDirection);

            Ctx.ChangeState(Ctx.GetStateByType<EnemyChaseState>());
        }
    }

    /// <summary>
    /// Atacar al jugador
    /// </summary>
    private void Attack(int direction)
    {
        //El rango de ataque del enemigo
        Vector2 position = transform.position + (new Vector3(_attackRadius, 0) * direction);

        //El posible HealthManager del jugador
        HealthManager player;

        //Mirar en el área del ataque
        RaycastHit2D playerInRange = Physics2D.CircleCast(position, _attackRadius, new Vector2(0, 0), _attackRadius, 1 << 6);

        //Si en el área de ataque se encuentra el jugador, entonces le hace daño
        if(playerInRange.collider != null && playerInRange.collider.GetComponent<PlayerStateMachine>() != null)
        {
            player = playerInRange.collider.gameObject.GetComponent<HealthManager>();
            player.RemoveHealth(_damage);
        }

        SoundManager.Instance.PlaySFX(_attackSound, transform, 0.5f);
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (new Vector3(_attackRadius, 0) * (int)_ctx.LookingDirection), _attackRadius);
    }
    */
    #endregion   

} // class EnemyAttackState 
// namespace
