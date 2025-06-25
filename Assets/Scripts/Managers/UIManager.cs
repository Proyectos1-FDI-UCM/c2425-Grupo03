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

    [SerializeField] private Slider _abilityOneSliderOV;
    /// <summary>
    /// La segunda habilidad del jugador.
    /// </summary>
    [SerializeField] private Slider _abilityTwoSlider;

    [SerializeField] private Slider _abilityTwoSliderOV;

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

    private float _currentOverchargeOne;
    /// <summary>
    /// Carga de la habilidad 2
    /// </summary>
    private float _currentChargeTwo;

    private float _currentOverchargeTwo;
    /// <summary>
    /// El script que controla el animator de Super Dash.
    /// </summary>
    private AbilityChargedManager _abilityOneCharged;

    private AbilityChargedManager _abilityOneOvercharged;
    /// <summary>
    /// El script que controla el animator de Mano de las Sombras.
    /// </summary>
    private AbilityChargedManager _abilityTwoCharged;

    private AbilityChargedManager _abilityTwoOvercharged;
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

        _abilityOneCharged = _abilityOneSlider.GetComponentInChildren<AbilityChargedManager>();
        _abilityTwoCharged = _abilityTwoSlider.GetComponentInChildren<AbilityChargedManager>();

        _abilityOneOvercharged = _abilityOneSliderOV.GetComponentInChildren<AbilityChargedManager>();
        _abilityTwoOvercharged = _abilityTwoSliderOV.GetComponentInChildren<AbilityChargedManager>();

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


        _playerCharge.ResetManoDeLasSombras();
        _playerCharge.ResetSuperDash();

        _currentChargeOne = _playerCharge.SuperDash.currentCharge;
        _currentOverchargeOne = _playerCharge.SuperDash.currentOvercharge;

        _currentChargeTwo = _playerCharge.ManoDeLasSombras.currentCharge;
        _currentOverchargeTwo = _playerCharge.ManoDeLasSombras.currentOvercharge;

        // Subscribe el método para actualizar la carga de las habilidades al evento correspondiente.

        _playerCharge._onChargeChangeSuperDash.AddListener(UpdateSuperDashCharge);
        _playerCharge._onOverchargeChangeSuperDash.AddListener(UpdateSuperDashOvercharge);

        _playerCharge._onChargeChangeManoSombras.AddListener(UpdateManoSombrasCharge);
        _playerCharge._onOverchargeChangeManoSombras.AddListener(UpdateManoSombrasOvercharge);

        // Valores iniciales de las barras.
        UpdateSuperDashCharge();
        UpdateManoSombrasCharge();

        UpdateSuperDashOvercharge();
        UpdateManoSombrasOvercharge();
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
    private void UpdateSuperDashCharge() {
        // Actualizamos los valores de las cargas
        _currentChargeOne = _playerCharge.SuperDash.currentCharge;


        // Calculamos el porcentaje de carga
        float chargePercentageOne = _currentChargeOne / _playerCharge.SuperDash.maxCharge;

        // Cambiamos la carga
        _abilityOneSlider.value = chargePercentageOne;

        if (_playerCharge.SuperDash.isCharged)
            _abilityOneCharged.StartAnimation();
    }
    private void UpdateSuperDashOvercharge()
    {
        // Actualizamos los valores de las cargas
        _currentOverchargeOne = _playerCharge.SuperDash.currentOvercharge;


        // Calculamos el porcentaje de carga
        float OverchargePercentageOne = _currentOverchargeOne / _playerCharge.SuperDash.maxOvercharge;

        // Cambiamos la carga
        _abilityOneSliderOV.value = OverchargePercentageOne;

        if (_playerCharge.SuperDash.isOvercharged)
            _abilityOneOvercharged.StartAnimation();
    }
    private void UpdateManoSombrasCharge() {
        // Actualizamos los valores de las cargas
        _currentChargeTwo = _playerCharge.ManoDeLasSombras.currentCharge;

        // Calculamos el porcentaje de carga
        float chargePercentageTwo = _currentChargeTwo / _playerCharge.ManoDeLasSombras.maxCharge;

        // Cambiamos la carga
        _abilityTwoSlider.value = chargePercentageTwo;

        if (_playerCharge.ManoDeLasSombras.isCharged)
        _abilityTwoCharged.StartAnimation();
    }
    private void UpdateManoSombrasOvercharge()
    {
        // Actualizamos los valores de las cargas

        _currentOverchargeTwo = _playerCharge.ManoDeLasSombras.currentOvercharge;

        // Calculamos el porcentaje de carga

        float OverchargePercentageTwo = _currentOverchargeTwo / _playerCharge.ManoDeLasSombras.maxOvercharge;

        // Cambiamos la carga

        _abilityTwoSliderOV.value = OverchargePercentageTwo;

        if (_playerCharge.ManoDeLasSombras.isOvercharged)
            _abilityTwoOvercharged.StartAnimation();
    }
    #endregion   

} // class UIManager 
// namespace
