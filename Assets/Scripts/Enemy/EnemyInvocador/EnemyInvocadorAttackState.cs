//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
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
public class EnemyInvocadorAttackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
   [SerializeField] float _waitTimeShoot;

    [SerializeField] float _invokingTime;
    [SerializeField] int _abilityCooldown;
    [SerializeField][Range(0.0f, 1f)] float _invokeProbabilty;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase

    /// <summary>
    /// La dirección donde mira el enemigo, no es necesario para realizar el ataque, solo sirve para 
    /// ver el rango de ataque del enemigo
    /// </summary>
    private int _direction;

    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    private EnemyInvocadorStateMachine _ctx;

    /// <summary>
    /// El tiempo cuando el enemigo pueda volver a hacer una accion.
    /// </summary>
    private float _lastAttackTime;
    
    /// <summary>
    /// El índice del spawnpoint actual.
    /// </summary>
    static private int _spawnpointIndex = 1;
    /// <summary>
    /// El Transform del spawnpoint actual.
    /// </summary>
    private Transform _spawnpointTransform;
    /// <summary>
    /// Un número aleatorio que determina si el enemigo dispara o invoca otro enemigo.
    /// </summary>
    private float _randomNr;

    /// <summary>
    /// Tiempo de espera para disparar más tiempo del momento del juego
    /// </summary>
    private float _shootTime;

    /// <summary>
    /// Tiempo de espera para invocar más tiempo del momento del juego
    /// </summary>
    private float _invokeTime;

    /// <summary>
    /// Booleana para ver si ha terminado de atacar
    /// </summary>
    /// 
    private bool _attackFinished;

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
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Si el jugador sale del trigger pone el range a false.
        _ctx.IsPlayerInAttackRange = false;
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
        _ctx = GetCTX<EnemyInvocadorStateMachine>();

        //Coger animator del contexto
        _animator = _ctx.GetComponent<Animator>();

        _animator.SetBool("IsAttack", true);
        _attackFinished = false;

    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {   
       _animator.SetBool("IsAttack", false);
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
         if (Time.time > _lastAttackTime + _abilityCooldown )
          {
              _randomNr = UnityEngine.Random.Range(1, 11);

              if (_randomNr <= Mathf.Round(_invokeProbabilty * 10f))
              {
                  Debug.Log("Invoking!");
                  SetSubState(_ctx.GetStateByType<EnemySummonerInvokeState>());
                  _attackFinished = true;
              }
              else
              {
                  SetSubState(_ctx.GetStateByType<EnemySummonerShootState>());
                  _attackFinished = true;
              }
              _lastAttackTime = Time.time;
          }

        /* if (Time.time > _lastAttackTime + _invokeCooldown && !_attackFinished) {
             _randomNr = UnityEngine.Random.Range(1, 11);
            _invokeTime = Time.time + _invokingTime;
            _shootTime = Time.time + _waitTimeShoot;


             if (_randomNr <= Mathf.Round(_invokeProbabilty * 10f))
             {

                 Debug.Log("Invoking!");
                 _animator.SetBool("IsAttack", false);
                 _animator.SetBool("IsInvoking", true);

                 if (Time.time > _invokeTime )
                 {
                     Invoke();
                     _lastAttackTime = Time.time;
                     _animator.SetBool("IsInvoking", false);
                     _animator.SetBool("IsAttack", true);
                 }


             }
             else
             {

                 if (Time.time > _shootTime)
                 {
                     Shoot();
                     _attackFinished = true;
                     _lastAttackTime = Time.time;
                 }

             }
             
          }*/
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        /*if ( _attackfinished)
        {
            Ctx.ChangeState(Ctx.GetStateByType<EnemyInvocadorIdleState>());
            _animator.SetBool("IsAttack", false);
        }
        
        if ((_ctx.PlayerTransform.position - _ctx.transform.position).magnitude > _attackRadius)
        {
            //Si el jugador está fuera del rango de ataque, persigue al jugador
            Ctx.ChangeState(Ctx.GetStateByType<EnemyChaseState>());
        }*/
        
         if (!_ctx.IsPlayerInAttackRange && _attackFinished)
        {
            //Si el jugador está fuera del rango de ataque y no esta en el rango del Chase, pasa a idle
            Ctx.ChangeState(Ctx.GetStateByType<EnemyInvocadorIdleState>());
            _animator.SetBool("IsAttack", false);
        }
    }

    #endregion   

} // class EnemyInvocadorAttackState 
// namespace
