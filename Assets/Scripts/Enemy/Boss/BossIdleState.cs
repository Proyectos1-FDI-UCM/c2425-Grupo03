//---------------------------------------------------------
// Estado de inactivo del jefe final
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// Estado de inactivo del jefe final
/// </summary>
[RequireComponent (typeof(BoxCollider2D))]
public class BossIdleState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Tiempo que espera el boss tras detectar al jugador por primera vez
    /// </summary>
    [SerializeField]
    [Min(0f)]
    float _timeWaitAfterDetection;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Flag para saber si hemos detectado al jugador o no
    /// </summary>
    bool _playerDetected = false;

    /// <summary>
    /// Tiempo en el que terminará el estado
    /// </summary>
    float _idleStateEnd;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Trigger que detecta solo al jugador
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerStateMachine player))
        {
            _playerDetected = true;
            // Ponemos la referencia en el contexto para que otros estado tengan acceso
            // Nosotros somos el "punto de partida" para la pelea del boss
            GetCTX<BossStateMachine>().Player = player;
        }
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
        // Setup
        _idleStateEnd = Time.time + _timeWaitAfterDetection;
        Ctx.Animator.SetBool("IsIdle", true);
        Ctx.Rigidbody.velocity = Vector3.zero;
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        Ctx.Animator.SetBool("IsIdle", false);
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
        if(_playerDetected && Time.time > _idleStateEnd)
        {
            // Cambiamos al estado de precarga
            Ctx.ChangeState(Ctx.GetStateByName("Precharge"));
        }
    }

    #endregion   

} // class BossIdleState 
// namespace
