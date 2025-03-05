//---------------------------------------------------------
// Estado durante el cual el enemigo se teletransporta a una posición predeterminada del mapa.
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Runtime.CompilerServices;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class EnemyTPState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    // Punto al que se teletransporta
    [SerializeField] Transform _teleportPoint;

    /// <summary>
    /// Valor de tiempo para hacer teletransporte
    /// </summary>
    [SerializeField][Min(0)] float _waitTimeTp;

    /// <summary>
    /// Valor de tiempo para terminar el estado de teletransporte después de hacer Tp
    /// </summary>
    [SerializeField][Min(0)] float _waitTimePostTp;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    private EnemySummonerStateMachine _ctx;
    private Animator _animator;
    private Collider2D _collider;
    private HealthManager _healthManager;

    /// <summary>
    /// Tiempo de espera para teletransportarse más tiempo del momento del juego
    /// </summary>
    private float _tpTime;

    /// <summary>
    /// Comprobar si ya se ha realizado el teletransporte
    /// </summary>
    private bool _tpDone;

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
   
        //Inmunidad
        _ctx.GetComponent<HealthManager>().Inmune = true;

        //Calcula el momento en el que se hará el tp
        _tpTime = Time.time + _waitTimeTp;

        _tpDone = false;

        _animator.SetBool("IsDisappearing", true);

    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _ctx.GetComponent<HealthManager>().Inmune = false;
        _animator.SetBool("IsDisappearing", false);
        _animator.SetBool("IsAppearing", false);

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
        if (Time.time > _tpTime && !_tpDone)
        {
            //Mover al enemigo a la posición de _teleportPoint
            _ctx.transform.position = _teleportPoint.position;
            _animator.SetBool("IsDisappearing", false);
            _animator.SetBool("IsAppearing", true);
            _tpDone = true;
        }
        //Después de hacer Tp
        if (Time.time > _tpTime + _waitTimePostTp && _tpDone)
        { 
            _animator.SetBool("IsAppearing", false);

            //Cambiar al estado Idle
            Ctx.ChangeState(Ctx.GetStateByType<EnemySummonerIdleState>());
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
