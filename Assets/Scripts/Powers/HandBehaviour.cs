//---------------------------------------------------------
// Breve descripción del contenido del archivo
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
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _goSpeed;
    private float _returnSpeed;
    private float _damage;
    private float _startTime;
    private float _distance;
    bool _llegaFin = false;
    Rigidbody2D _rigidbody;
    private List<Rigidbody2D> _enemiesHit = new List<Rigidbody2D>(); //lista de enemigos afectados
    private HashSet<HealthManager> _damagedEnemies = new HashSet<HealthManager>(); //lista de los enemigos dañados
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
    /// </summary>
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _llegaFin = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        // Si la mano alcanza la distancia máxima, vuelve hacia el posicion del jugador y se destruye la mano
        if (!_llegaFin)
        {
            _rigidbody.velocity = _direction * _goSpeed;
            if(Vector2.Distance(_startPosition, transform.position) >= _distance) _llegaFin = true;
        }
        else 
        {
            _llegaFin = true;
            _rigidbody.velocity = -_direction * _returnSpeed;

            if (Mathf.Abs(_rigidbody.position.x - _startPosition.x) < 0.2f)
            {
                Destroy(gameObject);
            }

            foreach (Rigidbody2D enemy in _enemiesHit)
            {
                if (enemy != null)
                {
                    float knockbackDistance = Vector2.Distance(_startPosition, enemy.GetComponent<Rigidbody2D>().position);

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
