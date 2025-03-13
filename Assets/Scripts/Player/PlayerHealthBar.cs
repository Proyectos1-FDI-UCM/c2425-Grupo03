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
    private float _maxHealth;

    /// <summary>
    /// Slider del inspector.
    /// </summary>
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private HealthManager HealthManager;

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
        _healthSlider.minValue = 0;
        _healthSlider.maxValue = 1;
        UpdateHealthBar();
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
    /// Setter que permite ajustar vida desde fuera.
    /// </summary>
    public void SetHealth(float newHealth)
    {
        _currentHealth = MathfClampHealth(newHealth);
        UpdateHealthBar();
    }
    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        UpdateHealthBar();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private float MathfClampHealth(float value)
    {
        return Mathf.Clamp(value, 0f, _maxHealth);
    }

    /// <summary>
    /// Metodo para sumar vida
    /// </summary>
    /// <param name="health"></param>
    public void IncreaseHealth(float health)
    {
        // Asegurar que la vida no baje de 0 ni sale de _maxHealth.
        _currentHealth = MathfClampHealth(_currentHealth + health);
        UpdateHealthBar();
    }

    /// <summary>
    /// Metodo para restar vida.
    /// </summary>
    /// <param name="health"></param>
    public void DecreaseHealth(float health)
    {
        // Asegurar que la vida no baje de 0 ni sale de _maxHealth.
        _currentHealth = MathfClampHealth(_currentHealth - health);

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        // Controlamos el Slider como un porcentaje (0-1)
        //_healthSlider.value = _currentHealth;
        if (_currentHealth <= 0)
        {
            _healthSlider.value = 0;
        }
        else
        {
            //El slider value = 1, es decir, barra llena si _currentHealth == _maxHealth.
            _healthSlider.value = _currentHealth / _maxHealth;
           
        }
    }
    #endregion

} // class PlayerHealthBar 
// namespace
