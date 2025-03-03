//---------------------------------------------------------
// El estado de muerte del jugador en la máquina de estados
// Alejandro Menéndez Fierro
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// El estado de muerte del enemigo, espera un tiempo hasta que se "muera"
/// </summary>
public class PlayerDeathState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    /// <summary>
    /// El tiempo de espera
    /// </summary>
    [SerializeField, Min(0)] private float _waitTime;
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
    /// Referencia del tipo PlayerStatemachine del contexto.
    /// </summary>
    private PlayerStateMachine _ctx;

    /// <summary>
    /// El animador del enemigo
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Fin de tiempo de espera
    /// </summary>
    private float _deadTime;

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
        _ctx = GetCTX<PlayerStateMachine>();

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
    protected override void CheckSwitchState()
    {
        //Tras el tiempo de espera el jugador reaparece.
        if (Time.time > _deadTime)
        {
            _ctx.transform.position = Vector3.zero;
            HealthManager hm = _ctx.GetComponent<HealthManager>();
            hm.SetHealth(hm.MaxHealth);
            _ctx.ChangeState(Ctx.GetStateByType<PlayerGroundedState>());
        }
    }

    protected override void UpdateState()
    {
    }

    #endregion

} // class PlayerDeathState
// namespace
