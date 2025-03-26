//---------------------------------------------------------
// Clase para crear todo tipo de estados a partir de ella.
// Adrian Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public abstract class BaseState : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    
    /// <summary>
    /// Booleana que determina si el estado es raiz o si es un subestado.
    /// </summary>
    [SerializeReference] 
    bool _isRootState = false;

    /// <summary>
    /// El nombre del estado.
    /// </summary>
    [SerializeReference]
    string _name;

    #endregion


    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.

    /// <summary>
    /// El contexto del estado. Es decir, la maquina de estados que lo contiene.
    /// </summary>
    protected StateMachine Ctx { get; private set; }

    /// <summary>
    /// Getter de _isRootState.
    /// </summary>
    public bool IsRootState => _isRootState;
    
    /// <summary>
    /// Getter del nombre del estado.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Contiene al estado padre actual de este estado.
    /// </summary>
    public BaseState CurrParentState { get; private set; }

    /// <summary>
    /// Contiene al estado hijo de este estado.
    /// </summary>
    public BaseState CurrSubState { get; private set; }

    /// <summary>
    /// El siguiente estado al que transicionar.
    /// </summary>
    public BaseState NextSubState { get; private set; }

    #endregion


    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Inicializa el estado estableciendo el contexto y llamando a OnStateSetUp.
    /// </summary>
    /// <param name="ctx"></param>
    public void SetupState(StateMachine ctx)
    {
        Ctx = ctx;
        OnStateSetUp();
    }

    /// <summary>
    /// Metodo llamado tras establecer el contexto de un estado.
    /// </summary>
    protected virtual void OnStateSetUp() { }

    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public abstract void EnterState();

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public abstract void ExitState();

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected virtual void CheckSwitchState() { }

    /// <summary>
    /// Actualiza el estado actual, mira si hay que cambiar de estado y si tiene un estado hijo lo actualiza.
    /// </summary>
    public void UpdateStates()
    {
        // Actualiza el estado
        UpdateState();
        // Llama al método que hace las transiciones
        CheckSwitchState();
        // Si tiene un estado hijo actualiza el estado hijo
        if (CurrSubState != null) CurrSubState.UpdateStates();

        // Mira si hay transición de subestado
        CheckNewSubState();
    }
    
    /// <summary>
    /// Actualiza el estado actual para las fisicas, y si tiene un estado hijo tambien lo actualiza.
    /// </summary>
    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if (CurrSubState != null) CurrSubState.FixedUpdateState();
    }

    /// <summary>
    /// Cambia el estado activo del contexto a <paramref name="newState"/> si el estado es un estado raiz.
    /// Sino, cambia el estado hijo de su padre a <paramref name="newState"/>.
    /// </summary>
    /// <param name="newState">El estado al que transicionar.</param>
    /// <exception cref="UnityException">Llamado si la transicion no ha sido posible.</exception>
    [Obsolete("The state no longer manages state transtitions. Instead use Ctx.ChangeState(newState)", false)]
    public void ChangeState(BaseState newState)
    {
        Ctx.ChangeState(newState);
    }

    /// <summary>
    /// Metodo llamado al entrar en un trigger.
    /// </summary>
    public void ExecuteTriggerEnter(Collider2D other)
    {
        TriggerEnter(other);
        if (CurrSubState != null)
        {
            CurrSubState.ExecuteTriggerEnter(other);
        }
    }
    /// <summary>
    /// Metodo llamado al salir de un trigger.
    /// </summary>
    public void ExecuteTriggerExit(Collider2D other)
    {
        TriggerExit(other);
        if (CurrSubState != null) 
        { 
            CurrSubState.ExecuteTriggerExit(other); 
        }
    }
    /// <summary>
    /// Metodo llamado al estar en un trigger.
    /// </summary>
    public void ExecuteTriggerStay(Collider2D other)
    {
        TriggerStay(other);
        if (CurrSubState != null) 
        {
            CurrSubState.ExecuteTriggerStay(other); 
        }
    }
    /// <summary>
    /// Metodo llamado al dejar de tocar un collider.
    /// </summary>
    public void ExecuteCollisionEnter(Collision2D collision)
    {
        CollisionEnter(collision);
        if (CurrSubState != null) 
        { 
            CurrSubState.ExecuteCollisionEnter(collision); 
        }
    }
    /// <summary>
    /// Metodo llamado al dejar de tocar un collider.
    /// </summary>
    public void ExecuteCollisionExit(Collision2D collision)
    {
        CollisionExit(collision);
        if (CurrSubState != null)
        { 
            CurrSubState.ExecuteCollisionExit(collision); 
        }
    }

    /// <summary>
    /// Metodo usado para cambiar el subestado de un estado
    /// </summary>
    /// <param name="state">El estado que sera el nuevo subestado.</param>
    public void SetSubState(BaseState state)
    {
        if (!state.IsRootState)
        {
            NextSubState = state;
        }
        else throw new UnityException("Can't make the state " + state.GetType() + " a substate of " + this + " because it is a root state.");
    }

    /// <summary>
    /// Mira si hay que hacer una transición del estado hijo.
    /// </summary>
    public void CheckNewSubState()
    {
        if (CurrSubState != NextSubState && NextSubState != null)
        {
            NextSubState.EnterState();
            CurrSubState?.ExitState();

            CurrSubState = NextSubState;
            CurrSubState.SetParentState(this);
        }
    }

    /// <summary>
    /// Metodo para cambiar el estado padre de un estado.
    /// </summary>
    /// <param name="state">El nuevo estado padre del estado.</param>
    public void SetParentState(BaseState state)
    {
        CurrParentState = state;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    /// <summary>
    /// Metodo para acceder al contexto especifico del estado.
    /// </summary>
    /// <typeparam name="StateMachineType"></typeparam>
    /// <returns>Devuelve el estado convertido hacia abajo como <typeparamref name="StateMachineType"/> o nulo si la conversion no ha sido posible.</returns>
    protected StateMachineType GetCTX<StateMachineType>() where StateMachineType : StateMachine
    {
        return Ctx as StateMachineType;
    }

    /// <summary>
    /// Metodo llamado cada frame cuando este es el estado activo de la maquina de estados.
    /// </summary>
    protected abstract void UpdateState();

    /// <summary>
    /// Metodo llamado en la actualizacion de fisicas.
    /// </summary>
    protected virtual void FixedUpdateState() { }

    /// <summary>
    /// Metodo llamado al entrar en un trigger.
    /// </summary>
    protected virtual void TriggerEnter(Collider2D other) { }

    /// <summary>
    /// Metodo llamado al salir de un trigger.
    /// </summary>
    protected virtual void TriggerExit(Collider2D other) { }

    /// <summary>
    /// Metodo llamado al estar en un trigger.
    /// </summary>
    protected virtual void TriggerStay(Collider2D other) { }

    /// <summary>
    /// Metodo llamado al tocar un collider.
    /// </summary>
    protected virtual void CollisionEnter(Collision2D other) { }

    /// <summary>
    /// Metodo llamado al dejar de tocar un collider.
    /// </summary>
    protected virtual void CollisionExit(Collision2D other) { }

    #endregion   

} // class BaseState 
// namespace
