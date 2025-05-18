//---------------------------------------------------------
// Componente que maneja la barra de vida de todos los enemigos
// Zhiyi Zhou
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
public class BossHealthBar : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    
    /// <summary>
    /// El health manager.
    /// </summary>
    [SerializeField] HealthManager _healthManager;
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
    /// El slider de la vida del enemigo. 
    /// </summary>
    private Slider _enemyHealthSlider;
    /// <summary>
    /// El canvas donde tiene el slider.
    /// </summary>
    private Canvas _canvas;



    /// <summary>
    /// La vida máxima del enemigo.
    /// </summary>
    private float _maxHealth;

    /// <summary>
    /// La vida actual del enemigo.
    /// </summary>
    private float _currentHealth;

    private float _timer;

    private float _visibleTime = 1f;

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
        _enemyHealthSlider = GetComponentInChildren<Slider>();
        _canvas = GetComponentInChildren<Canvas>();
        //_healthManager = _enemyPrefab.GetComponent<HealthManager>();
        _healthManager = GetComponentInParent<HealthManager>();

        // Slider Settings
        _enemyHealthSlider.minValue = 0;
        _enemyHealthSlider.maxValue = 1;

        // Health Settings

        if (_healthManager != null)
        {
            _currentHealth = _healthManager.Health;
            _maxHealth = _healthManager.MaxHealth;

            // Conecta eventos del HealthManager
            _healthManager._onDamaged.AddListener(ShowEnemyHealthBar);
            _healthManager._onDeath.AddListener(HideEnemyHealthBar);
        }

        _canvas.enabled = true;




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

    private void ShowEnemyHealthBar(float _damage)
    {
        _currentHealth = _healthManager.Health;

        if (_healthManager.Health > 10)
        {
            _currentHealth = _healthManager.Health - 10;
        }
        else if (_currentHealth <= 0)
        {
            _canvas.enabled = false;
            return;
        }

        _enemyHealthSlider.value = _currentHealth / _maxHealth;


    }

    private void HideEnemyHealthBar()
    {
        _canvas.enabled = false;
    }

    #endregion   

} // class EnemyHealthBar 
// namespace
