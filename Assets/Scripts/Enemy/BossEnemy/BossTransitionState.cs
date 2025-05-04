//---------------------------------------------------------
// Estado de transición entre la fase 1 y fase 2
// Se ejecuta cuando la vida del jefe pasa a menos de la mitad.
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class BossTransitionState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Posición en la que finalizar la transición de estado
    /// </summary>
    [SerializeField]
    Transform _secondPhasePosition;

    /// <summary>
    /// Velocidad a la que se mueve a la posición final
    /// </summary>
    [SerializeField]
    float _speed;

    /// <summary>
    /// Tiempo mínimo que tarda en cambiar a la fase 2
    /// Comeinza a contar desde el comienzo del estado
    /// </summary>
    [SerializeField]
    float _timeToBeginPhase2;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Tiempo en el que termina la transición
    /// </summary>
    float _endOfTransition;

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        //setup
        _endOfTransition = Time.time + _timeToBeginPhase2;
        Ctx.Animator.SetTrigger("PhaseTransition");
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
        // Mueve al jefe hacia la posición designada
        Ctx.Rigidbody.position = Vector2.MoveTowards(Ctx.Rigidbody.position, (Vector2)_secondPhasePosition.position, _speed * Time.deltaTime);
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if(Ctx.Rigidbody.position == (Vector2)_secondPhasePosition.position && Time.time > _endOfTransition)
        {
            // Cambiamos a la carga del aire tras llegar a la posición designada y haya pasado el tiempo designado
            Ctx.ChangeState(Ctx.GetStateByName("Flying Charge"));
        }
    }

    #endregion   

} // class BossTransitionState 
// namespace
