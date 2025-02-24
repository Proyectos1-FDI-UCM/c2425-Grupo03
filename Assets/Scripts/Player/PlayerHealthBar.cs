//---------------------------------------------------------
// Barra de vida basica que puede sumar y restar vida.
// Responsable de la creación de este archivo
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerHealthBar : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Valor de vida maxima.
    /// </summary>
    [Tooltip("Vida maxima del jugador")]
    [SerializeField] private float _maxHealth = 100f;

    /// <summary>
    /// Slider del inspector.
    /// </summary>
    [SerializeField] private Slider _healthSlider;

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
    /// Vida actual del jugador.
    /// </summary>
    private float _currentHealth;

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
        // Inicializar la vida al max.
        _currentHealth = _maxHealth;

        UpdateHealthBar();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DecreaseHealth(10f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            IncreaseHealth(10f);
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
    /// Metodo que puede ser invocado desde el inspector para sumar vida
    /// </summary>
    [ContextMenu("Sumar 10 de Vida")]
    public void IncreaseHealth10()
    {
        IncreaseHealth(10f);
    }

    /// <summary>
    /// Metodo que puede ser invocado desde el inpector para restar vida.
    /// </summary>
    [ContextMenu("Restar 10 de Vida")]
    public void DecreaseHealth10()
    {
        DecreaseHealth(10f);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    
    /// <summary>
    /// Metodo para sumar vida
    /// </summary>
    /// <param name="_health"></param>
    private void IncreaseHealth(float _health)
    {
        // Asegurar que la vida no baje de 0 ni sale de _maxHealth.
        _currentHealth = Mathf.Clamp(_currentHealth + _health, 0f, _maxHealth);
        UpdateHealthBar();
    }

    /// <summary>
    /// Metodo para restar vida.
    /// </summary>
    /// <param name="_health"></param>
    private void DecreaseHealth(float _health)
    {
        // Asegurar que la vida no baje de 0 ni sale de _maxHealth.
        _currentHealth = Mathf.Clamp(_currentHealth - _health, 0f, _maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Controlamos el Slider como un porcentaje (0-1)
        _healthSlider.value = _currentHealth;
    }
    #endregion

} // class PlayerHealthBar 
// namespace
