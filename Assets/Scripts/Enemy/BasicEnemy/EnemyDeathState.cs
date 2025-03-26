//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// El estado de muerte del enemigo, espera un tiempo hasta que se "muera"
/// </summary>
public class EnemyDeathState : BaseState
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
    /// Sonido para la muerte del enemigo
    /// </summary>
    [SerializeField] AudioClip _enemyDeath;

    /// <summary>
    /// Sonido de tirar el arma del enemigo
    /// </summary>
    [SerializeField] AudioClip _enemyDropWeapon;

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
    private EnemyStateMachine _ctx;


    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

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
        _ctx = GetCTX<EnemyStateMachine>();

        if (_ctx != null)
        {
            // Pone el objeto en la capa default para que no pueda ser golpeado
            _ctx.gameObject.layer = 0;

            //Coger animator del contexto
            _animator = _ctx.GetComponent<Animator>();
            // Comienza la animación de muerte
            _animator?.SetBool("IsDead", true);
        }

        //Calcular el tiempo de la muerte
        _deadTime = Time.time + _waitTime;

        // Reproduce los sonidos al morir
        SoundManager.Instance.PlaySFX(_enemyDeath, transform, 0.2f);
        SoundManager.Instance.PlaySFX(_enemyDropWeapon, transform, 0.2f);
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
        if(Time.time > _deadTime)
        {
            Destroy(_ctx.gameObject);
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

} // class EnemyDeathState 
// namespace
