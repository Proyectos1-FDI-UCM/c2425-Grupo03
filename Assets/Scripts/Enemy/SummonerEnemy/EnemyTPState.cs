//---------------------------------------------------------
// Estado durante el cual el enemigo se teletransporta a una posición predeterminada del mapa.
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase con la lógica de la mécanica de teletransporte del enemigo invocador
/// </summary>
public class EnemyTPState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Punto al que se teletransporta
    /// </summary>
    [SerializeField] Transform _teleportPoint;

    /// <summary>
    /// Valor de tiempo para hacer teletransporte
    /// </summary>
    [SerializeField][Min(0)] float _waitTimeTp;

    /// <summary>
    /// Valor de tiempo para terminar el estado de teletransporte después de hacer Tp
    /// </summary>
    [SerializeField][Min(0)] float _waitTimePostTp;

    /// <summary>
    /// Sonido del invocador al hacer TP
    /// </summary>
    [SerializeField] AudioClip _teleportSound;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    /// <summary>
    /// La máquina de estados del enemigo invocador
    /// </summary>
    private EnemySummonerStateMachine _ctx;

    /// <summary>
    /// El animator del enemigo invocador
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Tiempo de espera para teletransportarse más tiempo del momento del juego
    /// </summary>
    private float _tpTime;

    /// <summary>
    /// Comprobar si ya se ha realizado el teletransporte
    /// </summary>
    private bool _tpDone;

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
   
        //Inmunidad
        _ctx.GetComponent<HealthManager>().Inmune = true;

        //Calcula el momento en el que se hará el tp
        _tpTime = Time.time + _waitTimeTp;

        _tpDone = false;

        _animator?.SetBool("IsDisappearing", true);

        //Reproduce le sonido de teletransporte
        SoundManager.Instance.PlaySFX(_teleportSound, transform, 0.3f);

        if (_ctx != null)
        {
            _ctx.GetComponent<HealthManager>().CanBeKnockbacked = false;
        }
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        HealthManager hm = _ctx?.GetComponent<HealthManager>();

        if(_ctx != null)
        {
            hm.Inmune = false;
        }
        
        _animator?.SetBool("IsDisappearing", false);
        _animator?.SetBool("IsAppearing", false);

        if (_ctx != null)
        {
            _ctx.GetComponent<HealthManager>().CanBeKnockbacked = true;
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
        //Hacer Tp
        if (_ctx != null && Time.time > _tpTime && !_tpDone)
        {
            //Mover al enemigo a la posición de _teleportPoint
            _ctx.transform.position = _teleportPoint.position;
            _animator?.SetBool("IsDisappearing", false);
            _animator?.SetBool("IsAppearing", true);
            _tpDone = true;
        }
        //Después de hacer Tp
        if (Time.time > _tpTime + _waitTimePostTp && _tpDone)
        { 
            _animator?.SetBool("IsAppearing", false);

            //Cambiar al estado Idle
            Ctx?.ChangeState(Ctx.GetStateByType<EnemySummonerIdleState>());
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

} // class EnemyTPState 
// namespace
