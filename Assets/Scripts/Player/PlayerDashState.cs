//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Adrian Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerDashState : BaseState
{
    Rigidbody2D _rb; //ESTO DEBERIA ESTAR EN EL CONTEXTO



    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField] float _distance;
    [SerializeField] float _duration;
    [SerializeField] float _rechargeTime;
    [SerializeField] float _immuneTime;
    [SerializeField] Vector2 _playerDirectionTEST; //ESTO DEBERIA ESTAR EN EL CONTEXTO


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    float _finishDashingTime;
    float _nextAvailableDashTime = -1;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
    }
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
        if (Time.time > _nextAvailableDashTime)
        {
            print("Dashing!");
            _rb.velocity = new Vector2(_distance / _duration, 0);
            _rb.gravityScale = 0;
            _finishDashingTime = Time.time + _duration;
            _nextAvailableDashTime = Time.time + _rechargeTime;
        }
        else
        {
            ChangeState(Ctx.GetStateByType<EmptyState>());
        }
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        print("Not Dashing!");
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 1;
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
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Time.time > _finishDashingTime) ChangeState(Ctx.GetStateByType<EmptyState>());
    }

    #endregion   

} // class PlayerDashState 
// namespace
