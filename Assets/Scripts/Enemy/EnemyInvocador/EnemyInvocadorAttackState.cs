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

    [Header("Shoot Properties")]
    /// <summary>
    /// El tiempo de espera entre dos ataques
    /// </summary>
    [SerializeField] float _attackSpeed;
    /// <summary>
    /// El daño del disparo.
    /// </summary>
    [SerializeField] int _damage;
    [Header("Invoke Properties")]
    [SerializeField] int _invokeCooldown;
    [SerializeField][Range(0.0f, 1f)] float _invokeProbabilty;
    [SerializeField] EnemyStateMachine _enemyToInvoke;


    /// <summary>
    /// Proyectil del enemigo.
    /// </summary>
    [SerializeField] GameObject _magicBullet;

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
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemyInvocadorStateMachine>();

        //Coger animator del contexto
        _animator = _ctx.GetComponent<Animator>();
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
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
    }

    [ContextMenu("Invoke")]
    public void Invoke() {
        Debug.Log("Invoking!");
        if (_enemyToInvoke != null) {
            _spawnpointTransform = _ctx.Spawnpoints[_spawnpointIndex];

            Instantiate(_enemyToInvoke, new Vector2(_spawnpointTransform.position.x, _spawnpointTransform.position.y - 1), _spawnpointTransform.rotation);
            if (_spawnpointIndex >= _ctx.Spawnpoints.Length - 1)
            {            
                _spawnpointIndex = 1;
            }
            else
            {
                _spawnpointIndex++;
            }
        }
    }

    public void Shoot() {
        Instantiate(_magicBullet, transform.position, transform.rotation);
        Debug.Log("Shooting!");
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
        if (Time.time > _lastAttackTime + _invokeCooldown) {
            _randomNr = UnityEngine.Random.Range(1, 11);
            if (_randomNr <= Mathf.Round(_invokeProbabilty * 10f))
            {
                Invoke();
            }
            else Shoot();
            _lastAttackTime = Time.time;
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

} // class EnemyInvocadorAttackState 
// namespace
