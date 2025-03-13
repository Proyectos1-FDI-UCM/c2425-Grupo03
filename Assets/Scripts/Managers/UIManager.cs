//---------------------------------------------------------
// Manager para el HUD del juego.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager para el HUD del jugador.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// El prefab del jugador.
    /// </summary>
    [SerializeField] private PlayerStateMachine _playerPrefab;
    /// <summary>
    /// La primera habilidad del jugador.
    /// </summary>
    [SerializeField] private Image _abilityOne;
    /// <summary>
    /// La segunda habilidad del jugador.
    /// </summary>
    [SerializeField] private Image _abilityTwo;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// El slider de la vida del jugador. 
    /// </summary>
    private Slider _healthSlider;
    /// <summary>
    /// El script de carga.
    /// </summary>
    private PlayerChargeScript _playerCharge;
    /// <summary>
    /// El health manager.
    /// </summary>
    private HealthManager _healthManager;
    /// <summary>
    /// La vida actual del jugador.
    /// </summary>
    private float _currentHealth;
    /// <summary>
    /// La vida máxima del jugador.
    /// </summary>
    private float _maxHealth;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        // Coge todos los componentes.
        _healthSlider = GetComponentInChildren<Slider>(); 
        _playerCharge = _playerPrefab.GetComponent<PlayerChargeScript>();
        _healthManager = _playerPrefab.GetComponent<HealthManager>();

        // Slider Settings
        _healthSlider.minValue = 0;
        _healthSlider.maxValue = 1;

        // Health Settings
        _currentHealth = _healthManager.Health;
        _maxHealth = _healthManager.MaxHealth;
        _healthManager._onDamaged.AddListener(UpdateHealthBar);
        _healthManager._onHealed.AddListener(UpdateHealthBar);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    private void UpdateHealthBar(float modifiedHealth)
    {
        // Actualizamos el valor de la vida actual
        _currentHealth = _healthManager.Health;

        // Controlamos el Slider como un porcentaje (0-1)
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

} // class UIManager 
// namespace
