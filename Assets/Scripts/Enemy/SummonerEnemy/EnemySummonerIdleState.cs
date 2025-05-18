//---------------------------------------------------------
// El estado inactivo del enemigo invocador.
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Contiene la lógica y métodos necesarios para realizar el estado inactivo del esqueleto invocador.
/// </summary>
public class EnemySummonerIdleState : BaseState
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// La máquina de estados para usar la información común.
    /// </summary>
    private EnemySummonerStateMachine _ctx;

    /// <summary>
    /// El animator del esqueleto.
    /// </summary>
    private Animator _animator;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemySummonerStateMachine>();

        //Coger animator del contexto
        _animator = _ctx?.GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_ctx != null)
        {
            //Si el esqueleto está en el trigger lo indica al contexto.
            _ctx.IsPlayerInAttackRange = true;

            //Añade la posición del esqueleto al ctx.
            _ctx.PlayerTransform = collision.transform;
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
    /// </summary>
    public override void EnterState()
    {
        //Para el movimiento del invocador para estar en idle
        Ctx.Rigidbody.velocity = Vector3.zero;

        _animator?.SetBool("IsIdle", true);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _animator?.SetBool("IsIdle", false);
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
        
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        // Si el jugador está en el rango de ataque cambia al estado de ataque.
        if (_ctx != null && _ctx.IsPlayerInAttackRange)
        {
            Ctx?.ChangeState(Ctx.GetStateByType<EnemySummonerAttackState>());
        }
    }

   
    #endregion   

} // class EnemyInvocadorIdleState 
// namespace
