//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
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
    /// La dirección donde mira el enemigo, no es necesario para realizar el ataque, solo sirve para 
    /// ver el rango de ataque del enemigo
    /// </summary>
    private int _direction;

    /// <summary>
    /// El animator del player
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
        _direction = (int)_ctx.LookingDirection;
        Attack();
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        //cosas del animator
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
        //Comprueba el tiempo del siguiente ataque para atacar al jugador que sigue dentro del rango de ataque
        if(Time.time > _nextAttackTime)
        {
            Attack();
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if ((_ctx.PlayerTransform.position - _ctx.transform.position).magnitude < _attackRadius)
        {
            //Si el jugador está fuera del rango de ataque, persigue al jugador
            ChangeState(Ctx.GetStateByType<EnemyChaseState>());
        }
        //Se podria simplificar?
        else if((_ctx.PlayerTransform.position - _ctx.transform.position).magnitude < _attackRadius && !_ctx.IsPlayerInChaseRange)
        {
            //Si el jugador está fuera del rango de ataque y no esta en el rango del Chase, pasa a idle
            ChangeState(Ctx.GetStateByType<EnemyIdleState>());
        }
    }

    /// <summary>
    /// Atacar al jugador
    /// </summary>
    private void Attack()
    {
        //_ctx.PlayerTransform.HarmManager();
        _nextAttackTime = Time.time + _attackSpeed;
        Debug.Log($"El skeleton ha hecho {_damage} daño al jugador");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (_direction * new Vector3(_attackRadius, 0)), _attackRadius);
    }
    #endregion   

} // class EnemyAttackState 
// namespace
