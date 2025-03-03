//---------------------------------------------------------
// Clase base para crear máquinas de estado
// Adrian Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Resources;
using UnityEngine;


/// <summary>
/// Clase usada para crear otros StateMachines a partir de ella.
/// Es un MonoBehaviour para poder ponérsela a un GameObject y para permitir el acceso rápido
/// de algunas propiedades desde el editor.
/// </summary>
public class StateMachine : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Array con los estados que contiene la máquina de estados
    /// </summary>
    [SerializeField] BaseState[] _states;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    /// <summary>
    /// El estado actual de la máquina de estados.
    /// </summary>
    // Esta en publico por que debe ser modificado por todos los estados para cambiar el comportamiento de la maquina.
    public BaseState CurrState { get; set; }

    /// <summary>
    /// El siguiente estado al que transicionar.
    /// </summary>
    BaseState NextState { get; set; }

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Inicializa todos los estados y llama al método OnAwake() para que
    /// las clases hijas también puedan hacer cosas en el Awake() con el método OnAwake().
    /// </summary>
    private void Awake()
    {
        foreach (BaseState state in _states)
        {
            state.SetupState(this);
        }

        OnAwake();
    }

    /// <summary>
    /// Pone el primer estado como el estado actual y llama a OnStart.
    /// </summary>
    private void Start()
    {
        CurrState = GetStateByIndex(0);
        CurrState.EnterState();

        OnStart();
    }

    /// <summary>
    /// Actualiza el estado actual y llama a OnUpdate.
    /// </summary>
    private void Update()
    {
        // Actualiza el estado actual
        CurrState.UpdateStates();

        // Notifica del update a la máquina de estados hija
        OnUpdate();

        // Mira si hay que hacer una transición de estados.
        CheckStateChange();
    }

    /// <summary>
    /// Actualiza el estado actual por fisicas.
    /// </summary>
    private void FixedUpdate()
    {
        CurrState.FixedUpdateStates();
    }

    /// <summary>
    /// Llama al estado actual cuando entra en un trigger.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CurrState.ExecuteTriggerEnter(collision);
    }

    /// <summary>
    /// Llama al estado actual cuando sale de un trigger.
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        CurrState.ExecuteTriggerExit(collision);
    }

    /// <summary>
    /// Llama al estado actual cuando esta en un trigger.
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        CurrState.ExecuteTriggerStay(collision);
    }

    /// <summary>
    /// Llama al estado actual cuando colisiona con un collider
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CurrState.ExecuteCollisionEnter(collision);
    }

    /// <summary>
    /// Llama al estado actual cuando deja de colisionar con un collider
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        CurrState.ExecuteCollisionExit(collision);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <param name="index">Indice con el estado deseado en el array de estados puesto desde el editor de unity.</param>
    /// <returns>Devuelve el estado con el indice <paramref name="index"/> tal y como esta puesto en el editor.</returns>
    /// <exception cref="UnityException">Si el indice esta fuera de los limites</exception>
    public BaseState GetStateByIndex(ushort index)
    {
        if (index >= 0 && index < _states.Length)
        {
            return _states[index];
        }
        else
        {
            throw new UnityException($"State with index {index} does not exist in {this.name}");
        }
    }

    /// <summary>
    /// Busca el estado con el nombre <paramref name="name"/>.
    /// </summary>
    /// <param name="name">El nombre del estado a buscar.</param>
    /// <returns>Devuelve el estado de la máquina de estados con el nombre <paramref name="name"/>.</returns>
    public BaseState GetStateByName(string name)
    {
        //Busqueda del estado con el tipo deseado (Jaime me suspende por este return)
        foreach (BaseState state in _states)
        {
            if (state.Name.Equals(name))
            {
                return state;
            }
        }
        return null;
    }

    /// <typeparam name="T">Tipo del estado deseado.</typeparam>
    /// <returns>Devuelve el primer estado de tipo <typeparamref name="T"/> del array de estados.</returns>
    public T GetStateByType<T>() where T : BaseState
    {
        //Busqueda del estado con el tipo deseado (Jaime me suspende por este return)
        foreach (BaseState state in _states)
        {
            if (state.GetType() == typeof(T))
            {
                return (T)state;
            }
        }
        return null;
    }

    /// <param name="type">Tipo del estado deseado.</param>
    /// <returns>Devuelve el primer estado de tipo <typeparamref name="T"/> del array de estados.</returns>
    public BaseState GetStateByType(Type type)
    {
        //Busqueda del estado con el tipo deseado (Jaime me suspende por este return)
        foreach (BaseState state in _states) if (state.GetType() == type) return state;
        return null;
    }

    /// <summary>
    /// Establece el estado al que transicionar tras terminar de actualizar el estado actual.
    /// </summary>
    /// <param name="nextState">Siguiente estado</param>
    public void ChangeState(BaseState nextState)
    {
        if (nextState.IsRootState)
        {
            NextState = nextState;
        }
        else
        {
            CurrState.SetSubState(nextState);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    /// <summary>
    /// Para que clases que extiendan a esta puedan hacer uso del Awake de Unity
    /// sin tener que reescribir el comportamiento ya predefinido en esta clase
    /// </summary>
    protected virtual void OnAwake() { }

    /// <summary>
    /// Para que clases que extiendan a esta puedan hacer uso del Start de Unity
    /// sin tener que reescribir el comportamiento ya predefinido en esta clase
    /// </summary>
    protected virtual void OnStart() { }

    /// <summary>
    /// Para que clases que extiendan a esta puedan hacer uso del Update de Unity
    /// sin tener que reescribir el comportamiento ya predefinido en esta clase
    /// </summary>
    protected virtual void OnUpdate() { }

    /// <summary>
    /// Comprueba si hay que cambiar de estados y hace el cambio.
    /// </summary>
    private void CheckStateChange()
    {
        if(CurrState != NextState && NextState != null)
        {
            CurrState.ExitState();
            NextState.EnterState();

            CurrState = NextState;
        }
    }

    #endregion

} // class StateMachine 
// namespace
