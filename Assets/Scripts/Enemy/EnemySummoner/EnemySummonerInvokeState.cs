//---------------------------------------------------------
// El estado de invocar esqueletos del invocador.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado con la lógica necesaria para hacer la mecánica de invocación
/// </summary>
public class EnemySummonerInvokeState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// El enemigo invocado.
    /// </summary>
    [Header("Invoke Properties")]
    [SerializeField] EnemyStateMachine _enemyToInvoke;

    /// <summary>
    /// Valor de tiempo para hacer invocacion
    /// </summary>
    [SerializeField][Min (0)] float _waitTimeInvoke;

    /// <summary>
    /// Sonido del invocador al invocar
    /// </summary>
    [SerializeField] AudioClip _invokeSound;

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
        _animator = _ctx?.GetComponent<Animator>();

        //Reproduce el sonido de invocar
        SoundManager.Instance.PlaySFX(_invokeSound, transform, 0.3f);

        if (_ctx != null)
        {
            _ctx.UpdateLookingDirection();

            //Comienza la animación de invocar
            _animator.SetBool("IsInvoking", true);
        }

        //Calcula el tiempo que tarda en invocar
        _invokeTime = Time.time + _waitTimeInvoke;
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        //Termina la animación de invocar
        _animator?.SetBool("IsInvoking", false);
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
        // Si se termina la animación de invocar ...
        if (_ctx != null && Time.time > _invokeTime )
        {
            //Coge siguiente transform en el que invocar
            _spawnpointTransform = _ctx.Spawnpoints[_spawnpointIndex];

            //Invoca el enemigo
            Instantiate(_enemyToInvoke, new Vector2(_spawnpointTransform.position.x, _spawnpointTransform.position.y - 1), _spawnpointTransform.rotation, transform.parent.parent.parent);

            //Pasa al siguiente punto de invocación
            _spawnpointIndex = (_spawnpointIndex + 1) % (_ctx.Spawnpoints.Length - 1);

            //Cambia al estado de ataque
            _ctx?.ChangeState(_ctx.GetStateByType<EnemySummonerAttackState>());

            //Termina la animación de invocar
            _animator?.SetBool("IsInvoking", false);
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
