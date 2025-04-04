//---------------------------------------------------------
// Script para el comportamiento de la bala disparada por
// el enemigo invocador.
// Santiago Salto
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Scrpit que realiza el comportamiento de la bala disaparada
/// por el enemigo invocador.
/// </summary>
public class MagicBullet : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Velocidad de la bala
    /// </summary>
    [SerializeField][Min (0)] float _velocity;

    /// <summary>
    /// Distancia maxima a la que se puede mogver la bala desde el punto de instanciacion
    /// </summary>
    [SerializeField] float _maxDistance;

    /// <summary>
    /// Daño que produce la bala
    /// </summary>
    [SerializeField] int _damage;

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
    /// Direccion en la que se dirige la bala
    /// </summary>
    private Vector3 _direction;
    
    /// <summary>
    /// Posicion de la bala al ser instanciada
    /// </summary>
    private Vector3 _originalPosition;

    /// <summary>
    /// Distancia de la bala respecto al punto inicial de instanciacion
    /// </summary>
    private Vector3 _distance;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
 

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        // Quita vida a lo que golpee (Solo puede golpear con el jugador según la matriz de colisiones
        HealthManager hm = other.GetComponent<HealthManager>();
        if(hm != null)
        {
            hm.RemoveHealth(_damage);
        }

        // Se destruye a si mismo
        Destroy(gameObject);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        transform.position +=  _direction * Time.deltaTime * _velocity;
        _distance = (_originalPosition - transform.position).normalized;

        //comprobar si ha llegado a la distancia máxima
        if ( _distance.magnitude >= _maxDistance)
        {
            Destroy(gameObject);
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
    /// Método para inicializar el movimiento de la bala
    /// </summary>
    /// <param name="_playerPosition">La posición del jugador para calcular la trayectoria hacia él.</param>
    public void Setup(Vector3 _playerPosition)
    {
        // Calcula la dirección en la que s eitene que mover
        _playerPosition.y += 0.25f;
        _direction = (_playerPosition - transform.position).normalized;

        // Calcula la rotación del sprite en función de la dirección en la que mira
        float angle = Mathf.Atan2(_direction.y , _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    #endregion


} // class Bala 
// namespace
