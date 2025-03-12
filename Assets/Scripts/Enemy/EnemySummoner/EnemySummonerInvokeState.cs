//---------------------------------------------------------
// El estado de invocar esqueletos del invocador.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class EnemySummonerInvokeState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [Header("Invoke Properties")]
    [SerializeField] EnemyStateMachine _enemyToInvoke;

    /// <summary>
    /// Valor de tiempo para hacer invocacion
    /// </summary>

    [SerializeField][Min (0)] float _waitTimeInvoke;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    private EnemySummonerStateMachine _ctx;
    
    /// <summary>
    /// El índice del spawnpoint actual.
    /// </summary>
    static private int _spawnpointIndex = 1;
    /// <summary>
    /// El Transform del spawnpoint actual.
    /// </summary>
    private Transform _spawnpointTransform;

    /// <summary>
    /// Tiempo de espera para invocar más tiempo del momento del juego
    /// </summary>
    private float _invokeTime;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

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
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemySummonerStateMachine>();

        //Coger animator del contexto
        _animator = _ctx.GetComponent<Animator>();

        //Actualizamos la dirección en la que mira el enemigo en función de la posición respecto al jugador
        _ctx.LookingDirection = (_ctx.PlayerTransform.position.x - _ctx.transform.position.x) > 0 ?
            EnemySummonerStateMachine.EnemyLookingDirection.Left : EnemySummonerStateMachine.EnemyLookingDirection.Right;

        _ctx.SpriteRenderer.flipX = _ctx.LookingDirection == EnemySummonerStateMachine.EnemyLookingDirection.Left;

        _invokeTime = Time.time + _waitTimeInvoke;
        _animator.SetBool("IsInvoking", true);

    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _animator.SetBool("IsInvoking", false);
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

        if (Time.time > _invokeTime )
        {
            _spawnpointTransform = _ctx.Spawnpoints[_spawnpointIndex];

            Instantiate(_enemyToInvoke, new Vector2(_spawnpointTransform.position.x, _spawnpointTransform.position.y - 1), _spawnpointTransform.rotation, transform.parent.parent.parent);
            if (_spawnpointIndex >= _ctx.Spawnpoints.Length - 1)
            {
                _spawnpointIndex = 1;
            }
            else
            {
                _spawnpointIndex++;
            }
            _ctx.ChangeState(_ctx.GetStateByType<EnemySummonerAttackState>());
            _animator.SetBool("IsInvoking", false);
        }
    }
    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {

    }

    #endregion   

} // class EnemySummonerInvokeState 
// namespace
