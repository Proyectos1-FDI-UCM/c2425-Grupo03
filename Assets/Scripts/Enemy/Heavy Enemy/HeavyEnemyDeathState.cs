//---------------------------------------------------------
// Estado de muerte del enemigo pesado
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado de muerte del enemigo pesado
/// </summary>
public class HeavyEnemyDeathState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Tiempo de espera hasta morir
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
    /// Fin de tiempo de espera
    /// </summary>
    private float _deadTime;
    /// <summary>
    /// Referencia del tipo HeavyEnemyStateMachine del contexto.
    /// </summary>
    private HeavyEnemyStateMachine _ctx;

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
    /// coge referencias y calcula el tiempo de muerte
    /// </summary>
    public override void EnterState()
    {
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<HeavyEnemyStateMachine>();

        //Pone el objeto en la capa 0 para que no le puedan volver a golpear
        _ctx.gameObject.layer = 0;

        //Calcular el tiempo de la muerte
        _deadTime = Time.time + _waitTime;

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
    /// Si ha pasado el tiempo de espera, se destruye el enemigo
    /// </summary>
    protected override void UpdateState()
    {
        // Destruye el objeto una vez pasado el tiempo de espera
        if (Time.time > _deadTime)
        {
            Destroy(_ctx.gameObject);
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        
    }

    #endregion   

} // class HeavyEnemyDeathState 
// namespace
