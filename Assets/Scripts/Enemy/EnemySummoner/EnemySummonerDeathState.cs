//---------------------------------------------------------
// El enemigo muere
// Santiago Salto Molodojen
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado de muerte del Invocador
/// </summary>
public class EnemySummonerDeathState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// El tiempo de espera
    /// </summary>
    [SerializeField, Min(0)] private float _waitTime;
    /// <summary>
    /// Coge los objetos relacionados con el invocador para eliminarlos
    /// </summary>
    [SerializeField] GameObject _prefabEntero;
    /// <summary>
    /// Sonido del invocador al morir
    /// </summary>
    [SerializeField] AudioClip _deathSound;

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
    /// Fin de tiempo de espera
    /// </summary>
    private float _deadTime;

    /// <summary>
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    private EnemySummonerStateMachine _ctx;

    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

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

        SoundManager.Instance.PlaySFX(_deathSound, transform, 0.3f);
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemySummonerStateMachine>();

        //Coger animator del contexto
        _animator = _ctx.GetComponent<Animator>();

        //Calcular el tiempo de la muerte
        _deadTime = Time.time + _waitTime;

        _animator.SetBool("IsDead", true);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        
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
        //Tras el tiempo de espera el enemigo "muere"
        if (_ctx == null) return;
        if (Time.time > _deadTime)
        {
            Destroy(_prefabEntero);   
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


} // class EnemyInvocadorDeathState 
// namespace
