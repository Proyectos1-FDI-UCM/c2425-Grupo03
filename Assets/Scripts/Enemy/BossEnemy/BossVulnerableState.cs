//---------------------------------------------------------
// Estado de vulnerabilidad del jefe final
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// Estado de vulnerabilidad del jefe final
/// </summary>
public class BossVulnerableState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Tiempo que está vulnerable
    /// </summary>
    [SerializeField]
    [Min(0)]
    float _vulnerableTime;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia al healthManager por comodidad
    /// </summary>
    HealthManager _healthManager;

    /// <summary>
    /// Tiempo en el que termina el estado
    /// </summary>
    float _stateEndTime;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        Ctx?.TryGetComponent<HealthManager>(out _healthManager);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        // calcula fin del estado
        _stateEndTime = Time.time + _vulnerableTime;

        // pone el jefe vulnerable
        _healthManager.Inmune = false;

        // pone velocidad a 0 por si se estaba moviendo
        Ctx.Rigidbody.velocity = Vector3.zero;
        Ctx.Animator.SetBool("IsVulnerable", true);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        // vuelve a ponerlo en inmune
        _healthManager.Inmune = true;
        Ctx.Animator.SetBool("IsVulnerable", false);
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
        if(Time.time > _stateEndTime)
        {
            // Cambia a idle
            Ctx.ChangeState(Ctx.GetStateByName("Idle"));
        }
    }

    #endregion   

} // class BossVulnerableState 
// namespace
