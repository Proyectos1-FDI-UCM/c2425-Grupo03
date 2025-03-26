//---------------------------------------------------------
// Estado de idle del enemigo pesado
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado de idle del enemigo pesado
/// </summary>
public class HeavyEnemyIdleState : BaseState
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Contexto del estado.
    /// </summary>
    HeavyEnemyStateMachine _ctx;
    #endregion


    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Coge referencia 
    /// </summary>
    private void Start()
    {
        _ctx = GetCTX<HeavyEnemyStateMachine>();
    }

    /// <summary>
    /// Comprueba si ha dertectado a un jugador
    /// </summary>
    /// <param name="collision"></param>trigger de deteccion 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_ctx != null && collision.GetComponent<PlayerStateMachine>() != null)
        {
            //Si el jugador está en el trigger lo indica al contexto.
            _ctx.IsPlayerInChaseRange = true;
            //Añade la posición del jugador al contexto.
            _ctx.PlayerTransform = collision.transform;
        }
    }

    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        
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
        
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        // Si el jugador está en distancia de rango cambia al estado de rango
        if (_ctx != null && _ctx.IsPlayerInChaseRange)
        {
            Ctx?.ChangeState(Ctx.GetStateByType<HeavyEnemyChasingState>());
        }
    }

    #endregion   

} // class HeavyEnemyIdleState 
// namespace
