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
    [SerializeField] private Slider _abilityOneSlider;
    /// <summary>
    /// La segunda habilidad del jugador.
    /// </summary>
    [SerializeField] private Slider _abilityTwoSlider;

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
    private PlayerCharge _playerCharge;
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
    /// <summary>
    /// Carga de la habilidad 1
    /// </summary>
    private float _currentChargeOne;
    /// <summary>
    /// Carga de la habilidad 2
    /// </summary>
    private float _currentChargeTwo;
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
        _playerCharge = _playerPrefab.GetComponent<PlayerCharge>();
        _healthManager = _playerPrefab.GetComponent<HealthManager>();

        // Slider Settings
        _healthSlider.minValue = 0;
        _healthSlider.maxValue = 1;

        // Health Settings
        _currentHealth = _healthManager.Health;
        _maxHealth = _healthManager.MaxHealth;

        // Va a actualizar la bara de vida cuando el jugador recibe o se le quita vida. 
        _healthManager._onDamaged.AddListener(UpdateHealthBar);
        _healthManager._onHealed.AddListener(UpdateHealthBar);
        _healthManager._onDeath.AddListener(ResetHealthBar);

        //// Coge las cargas iniciales de las habilidades
        //if (_playerPrefab.GetStateByType<PlayerManoDeLasSombrasState>().IsLocked)
        //{
        //    _abilityOneSlider.gameObject.SetActive(false);
        //}
        //else
        //{
        //    _abilityOneSlider.gameObject.SetActive(true);
        //    _currentChargeOne = _playerCharge.SuperDash.currentCharge;
        //}

        //if (_playerPrefab.GetStateByType<PlayerSuperDashState>().IsLocked)
        //{
        //    _abilityTwoSlider.gameObject.SetActive(false);
        //}
        //else
        //{
        //    _abilityTwoSlider.gameObject.SetActive(true);
        //    _currentChargeTwo = _playerCharge.SuperDash.currentCharge;
        //}
        _currentChargeOne = _playerCharge.SuperDash.currentCharge;
        _currentChargeTwo = _playerCharge.SuperDash.currentCharge;

        // Subscribe el método para actualizar la carga de las habilidades al evento correspondiente.
        _playerCharge._onChargeChange.AddListener(UpdateAbilityCharge);

        // Valores iniciales de las barras.
        UpdateAbilityCharge();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    /// <summary>
    /// Actualiza la barra de carga de vida con la nueva vida
    /// </summary>
    /// <param name="modifiedHealth">Nueva vida del jugador</param>
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

    private void ResetHealthBar() {
        _healthSlider.value = 1;
    }

    /// <summary>
    /// Actualiza la carga de las habilidades
    /// </summary>
    private void UpdateAbilityCharge() {
        // Actualizamos los valores de las cargas
        _currentChargeOne = _playerCharge.SuperDash.currentCharge;
        _currentChargeTwo = _playerCharge.ManoDeLasSombras.currentCharge;
        
        // Calculamos el porcentaje de carga
        float chargePercentageOne = _currentChargeOne / _playerCharge.SuperDash.maxCharge;
        float chargePercentageTwo = _currentChargeTwo / _playerCharge.ManoDeLasSombras.maxCharge;
        
        // Cambiamos la carga
        _abilityOneSlider.value = chargePercentageOne;
        _abilityTwoSlider.value = chargePercentageTwo;
    }
    #endregion   

} // class UIManager 
// namespace
