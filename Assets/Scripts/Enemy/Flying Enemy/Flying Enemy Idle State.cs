//---------------------------------------------------------
// Archivo con el código para el estado inactivo del enemigo.
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Dynamic;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// El estado inactivo del enemigo.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class FlyingEnemyIdleState : BaseState
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Contexto del estado.
    /// </summary>
    FlyingEnemyStateMachine _ctx;

    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        //Coge una referencia al contexto para evitar el upcasting y por comodidad
        _ctx = GetCTX<FlyingEnemyStateMachine>();

        // Coge una referencia al animator
        _animator = _ctx?.Animator;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStateMachine>() != null)
        {
            if (_ctx != null)
            {
                //Si el jugador está en el trigger lo indica al contexto.
                _ctx.IsPlayerInChaseRange = true;
                //Añade la posición del jugador al contexto.
                _ctx.PlayerTransform = collision.transform;
            }
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
        _animator?.SetBool("IsIdle", true);
    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _animator?.SetBool("IsIdle", false);
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
        if (_ctx != null)
        {
            // Si el jugador está en distancia de rango cambia al estado de rango
            if (_ctx.IsPlayerInChaseRange)
            {
                Ctx.ChangeState(Ctx.GetStateByType<FlyingEnemyChaseState>());
            }
        }
    }

    #endregion   

} // class EnemyIdleState 
// namespace
