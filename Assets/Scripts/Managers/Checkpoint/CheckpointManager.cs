//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    /// <summary>
    /// Instancia para implementar el patrón Singleton
    /// </summary>
    public static CheckpointManager Instance { get; private set; }

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    [Header("Initial SpawnPoint")]
    [SerializeField] private Transform _initialPoint;

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
    /// Referencia al último punto guardado
    /// </summary>
    private Transform _lastPoint;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Patrón Singleton, asegura que solo exista una instancia del Manager
    /// </summary>
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    private void Start()
    {
        if (_lastPoint == null && _initialPoint != null)
        {
            //Cuando no hay checkpoint activado
            _lastPoint = _initialPoint;
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
    /// Método para registrar el último checkpoint
    /// </summary>
    /// <param name="checkpointTransform"></param>
    public void SetCheckpoint(Transform checkpointTransform)
    {
        // Asigna la referencia del último punto a este checkpoint
        _lastPoint = checkpointTransform;
    }

    /// <summary>
    /// Teletransporta al jugador al último checkpoint guardado
    /// </summary>
    /// <param name="player"></param>
    public void RespawnPlayer(GameObject player)
    {
        if (_lastPoint != null)
        {
            // Mueve la posición del jugador a la posición del checkpoint
            player.transform.position = _lastPoint.position;
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class CheckPointManager 
// namespace
