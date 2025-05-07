//---------------------------------------------------------
// El estado de ataque del invocador. Puede invocar o disparar.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class EnemySummonerAttackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Tiempo entre habilidades
    /// </summary>
    [SerializeField] 
    float _abilityCooldown;

    [SerializeField][Range(0.0f, 1f)] float _invokeProbabilty;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase

    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    private EnemySummonerStateMachine _ctx;

    /// <summary>
    /// Un número aleatorio que determina si el enemigo dispara o invoca otro enemigo.
    /// </summary>
    private float _randomNr;

    /// <summary>
    /// Tiempo de espera para invocar más tiempo del momento del juego
    /// </summary>
    private float _cooldownTime;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_ctx != null)
        {
            //Si el jugador sale del trigger pone el range a false.
            _ctx.IsPlayerInAttackRange = false;
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
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemySummonerStateMachine>();

        //Coger animator del contexto
        _animator = _ctx?.GetComponent<Animator>();

        //Para el movimiento del invocador antes de atacar
        Ctx.Rigidbody.velocity = Vector3.zero;

        // Pone la animación de idle
        _animator?.SetBool("IsIdle", true);

        // Establece el tiempo de cooldown
        _cooldownTime = Time.time + _abilityCooldown;
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
        if (_ctx != null)
        {
            _ctx.UpdateLookingDirection();
        }
        //si ha pasado el cooldown, aleatoriamente invocar o disparar
        if (Time.time > _cooldownTime) 
        {
              _randomNr = UnityEngine.Random.Range(1, 11);

              if (_randomNr <= Mathf.Round(_invokeProbabilty * 10f))
              {
                Ctx?.ChangeState(Ctx.GetStateByType<EnemySummonerInvokeState>());
              }
              else
              {
                Ctx?.ChangeState(Ctx.GetStateByType<EnemySummonerShootState>()); 
              }
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_ctx != null)
        {
            //Si el jugador está fuera del rango de ataque y no esta en el rango del Chase, pasa a idle
            if (!_ctx.IsPlayerInAttackRange) 
            {
                Ctx?.ChangeState(Ctx.GetStateByType<EnemySummonerIdleState>());
                _animator?.SetBool("IsIdle", false);
            }
        }
    }

    #endregion   

} // class EnemyInvocadorAttackState 
// namespace
