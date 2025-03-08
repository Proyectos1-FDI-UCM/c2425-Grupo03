//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

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
    private float _speed;
    private float _damage;
    private float _startTime;
    private float _distance;
    Rigidbody2D _rigidbody;
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
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        // Mover la onda
        _rigidbody.velocity = _direction * _speed;

        // Si la mano alcanza la distancia máxima, destruir la onda
        if (Vector2.Distance(_startPosition, transform.position) >= _distance)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        HealthManager healthManager = collision.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager.RemoveHealth((int)_damage);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Si la onda choca con una pared, se desaparece.
            Destroy(gameObject);
        }

        EnemyStateMachine enemyStateMachine = collision.GetComponent<EnemyStateMachine>();

        if (enemyStateMachine != null)
        {
            enemyStateMachine.ChangeState(enemyStateMachine.GetStateByType<EnemyIdleState>());
            collision.attachedRigidbody.position = _startPosition + new Vector2 (1,0)* _direction;
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
    public void Initialize(Vector2 direction, float distance, float speed, float damage)
    {
        _direction = direction;
        _speed = speed;
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
