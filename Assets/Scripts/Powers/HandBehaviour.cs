//---------------------------------------------------------
// El comportamiento del prefan de la habilidad de ManoDeLasSombras
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class HandBehaviour : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

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
    /// Direccion que avanza la habilidad
    /// </summary>
    private Vector2 _direction; 
    /// <summary>
    /// La posicion inicial de la habilidad
    /// </summary>
    private Vector2 _startPosition;
    /// <summary>
    /// la posicion en la que el jugador lanza la habilidad
    /// </summary>
    private float _goSpeed;
    /// <summary>
    /// la velocidad de regreso de la habilidad
    /// </summary>
    private float _returnSpeed;
    /// <summary>
    /// el daño que hace la habilidad
    /// </summary>
    private float _damage;
    /// <summary>
    /// el tiempo en el que se lanza la habilidad
    /// </summary>
    private float _startTime;
    /// <summary>
    /// la distancia que recorre la habilidad
    /// </summary>
    private float _distance;
    /// <summary>
    /// si la habilidad ha llegado a la distancia maxima y ha empezado a retroceder
    /// </summary>
    bool _llegaFin = false;
    /// <summary>
    /// el regidbody de la habilidad 
    /// </summary>
    Rigidbody2D _rigidbody;
    /// <summary>
    /// Lista de enemigos afectados por la habilidad
    /// </summary>
    private List<Rigidbody2D> _enemiesHit = new List<Rigidbody2D>(); 
    /// <summary>
    /// lista de enemigos que ya han sido dañados por la habildad
    /// </summary>
    private HashSet<HealthManager> _damagedEnemies = new HashSet<HealthManager>();
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// aisgnar variables
    /// </summary>
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _llegaFin = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Si la mano alcanza la distancia máxima, vuelve hacia el posicion del jugador y se destruye la mano
    /// </summary>
    void Update()
    {

       
        if (!_llegaFin) //no ha llegado a la distancia maxima
        {
            _rigidbody.velocity = _direction * _goSpeed;
            if(Vector2.Distance(_startPosition, transform.position) >= _distance) _llegaFin = true; 
        }
        else //si que ha llegado a la distancia maxima
        {
            _rigidbody.velocity = -_direction * _returnSpeed;

            if (Mathf.Abs(_rigidbody.position.x - _startPosition.x) < 0.2f) //se destruye cuando llega al jugador
            {
                Destroy(gameObject);
            }

            foreach (Rigidbody2D enemy in _enemiesHit) //todos los enemigos afectados 
            {
                if (enemy != null)
                {
                    float knockbackDistance = Vector2.Distance(_startPosition, enemy.GetComponent<Rigidbody2D>().position); //distancia que hay entre el jugador y el enemigo

                    enemy.GetComponent<StateMachine>()
                            .GetStateByType<KnockbackState>()?.ApplyKnockBack(-knockbackDistance + 1f, 0.1f, (int)_direction.x);

                    if (!_damagedEnemies.Contains(enemy.GetComponent<HealthManager>())) //si no estan dañado de antes
                    {
                        enemy.GetComponent<HealthManager>().RemoveHealth((int)_damage);
                        _damagedEnemies.Add(enemy.GetComponent<HealthManager>()); // Lo marcamos como dañado
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Si la onda choca con una pared, se rebota.
            _llegaFin = true ;
        }

        EnemyStateMachine enemyStateMachine = collision.GetComponent<EnemyStateMachine>();

        if (enemyStateMachine != null)
        {
            enemyStateMachine.ChangeState(enemyStateMachine.GetStateByType<EnemyIdleState>());

            //añade el eneigo colisionado a la lista de enemigos afectados
            Rigidbody2D enemyRigidbody = collision.attachedRigidbody;
            if (!_enemiesHit.Contains(enemyRigidbody))
            {
                _enemiesHit.Add(enemyRigidbody);
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
    /// inicializar los parametros para que sean editables desde el editor del script PlayerManoDeLasSombras en el prefab del player
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <param name="goSpeed"></param>
    /// <param name="returnSpeed"></param>
    /// <param name="damage"></param>
    public void Initialize(Vector2 direction, float distance, float goSpeed,float returnSpeed, float damage)
    {
        _direction = direction;
        _goSpeed = goSpeed;
        _returnSpeed = returnSpeed;
        _damage = damage;
        _distance = distance;
        _startPosition = transform.position;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ManoBehaviour 
// namespace
