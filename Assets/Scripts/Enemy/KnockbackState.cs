//---------------------------------------------------------
// Estado genérico de todos los enemigos para que puedan sufrir knockback
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// Clase que se encarga de producir el knockback.
/// </summary>
public class KnockbackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// El tiempo cuando termina el knockback.
    /// </summary>
    float _knockBackEndTime;

    /// <summary>
    /// La distancia que recorre la entidad durante el knockback
    /// </summary>
    float _knockBackDistance;

    /// <summary>
    /// El tiempo que tarda la entidad en recorrer la distancia.
    /// </summary>
    float _knockBackTime;

    /// <summary>
    /// La dirección en la que se aplica le knockback.
    /// </summary>
    int _direction;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que inicializa los valores del knockback y aplica el knockback.
    /// </summary>
    /// <param name="distance"> La distancia que recorrida durante el knockback.</param>
    /// <param name="time"> El timepo que tarda el estado de knockback.</param>
    /// <param name="direction"> La dirección en la que se aplica el knockback.</param>
    public void ApplyKnockBack(float distance, float time, int direction)
    {
        _knockBackDistance = distance;
        _knockBackTime = time;
        _direction = direction;

        if (Ctx.Rigidbody.gameObject.GetComponent<HealthManager>().Health > 0)
        {
            Ctx.ChangeState(this);
        }
    }

    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        Ctx.Rigidbody.velocity = (_knockBackDistance / _knockBackTime) * _direction * Vector2.right;
        _knockBackEndTime = Time.time + _knockBackTime;
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        Ctx.Rigidbody.velocity = Vector2.zero;
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos

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
        if (Time.time > _knockBackEndTime)
        {
            Ctx.ChangeState(Ctx.GetStateByName("FallingState"));
        }
    }

    #endregion   

} // class KnockbackState 
// namespace
