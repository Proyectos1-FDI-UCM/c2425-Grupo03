//---------------------------------------------------------
// Gestor de Oleadas- gestiona el inicio y cambios de oleadas de enemigos
// Santiago Salto Molodojen
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem.HID;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestiona las oleadas de enemigos y activa la puerta para seguir el nivel
/// </summary>
public class WaveController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Puerta que se abre después de las oleadas
    /// </summary>
    [SerializeField] GameObject _door;
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
    /// Numero de la oleada activa
    /// </summary>
    int _numWave;

    /// <summary>
    /// Comprobar si estan activas las oleadas
    /// </summary>
    bool _endWaves;

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

    private void Awake()
    {
        foreach (Transform child in transform) // Desactiva todos los hijos al inicio
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in transform) // elimina aquellos emptys que no tengan enemigos dentro
        {
            if (child.childCount == 0) 
            {
                Destroy(child.gameObject); 
            }
        }

        _numWave = 0; //especificamos que la oleada activada es la primera

        _door.gameObject.SetActive(false); //la puerta no esta cerrada

    }

    void Update()
    {
      if (!_endWaves)
        {
            if (transform.GetChild(_numWave).childCount == 0) // Si no tiene hijos
            {
                Destroy(transform.GetChild(_numWave).gameObject); // Lo eliminamos La oleada actual
                NextWave(); // Activamos el siguiente oleada
            }
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        
        if (transform.childCount > 0) // Activa solo la primera oleada, cierra la puerta y activa el estado de oleada
        {
            transform.GetChild(_numWave).gameObject.SetActive(true);
            _door.gameObject.SetActive(true);
            _endWaves = false;
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

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Activación de la siguiente oleada
    /// </summary>
    void NextWave()
    {
        if (transform.childCount > 1) // si existen más hijos a parte del actual activar el siguiente
        {
            _numWave++;
            transform.GetChild(_numWave).gameObject.SetActive(true);
            _numWave = 0;
        }
        else  // si no hay mas oleadas activar puerta y terminar oleada
        {
            _door.gameObject.SetActive(false);
            _endWaves = true;
        }
    }

    #endregion

} // class WaveController 
// namespace
