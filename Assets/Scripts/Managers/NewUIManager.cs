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
public class NewUIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// El prefab del jugador.
    /// </summary>
    [SerializeField] private PlayerStateMachine _playerPrefab1;
    /// <summary>
    /// La primera habilidad del jugador.
    /// </summary>
    [SerializeField] private Image _abilityOneImg1;
    /// <summary>
    /// La segunda habilidad del jugador.
    /// </summary>
    [SerializeField] private Image _abilityTwoImg1;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// El slider de la vida del jugador. 
    /// </summary>
    private Slider _healthSlider1;
    /// <summary>
    /// El script de carga.
    /// </summary>
    private PlayerCharge _playerCharge1;
    /// <summary>
    /// El health manager.
    /// </summary>
    private HealthManager _healthManager1;
    /// <summary>
    /// La vida actual del jugador.
    /// </summary>
    private float _currentHealth1;
    /// <summary>
    /// La vida máxima del jugador.
    /// </summary>
    private float _maxHealth1;
    private float _currentChargeOne1;
    private float _currentChargeTwo1;
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
        _healthSlider1 = GetComponentInChildren<Slider>(); 
        _playerCharge1 = _playerPrefab1.GetComponent<PlayerCharge>();
        _healthManager1 = _playerPrefab1.GetComponent<HealthManager>();

        // Slider Settings
        _healthSlider1.minValue = 0;
        _healthSlider1.maxValue = 1;

        // Health Settings
        _currentHealth1 = _healthManager1.Health;
        _maxHealth1 = _healthManager1.MaxHealth;
        // Va a actualizar la bara de vida cuando el jugador recibe o se le quita vida. 
        _healthManager1._onDamaged.AddListener(UpdateHealthBar1);
        _healthManager1._onHealed.AddListener(UpdateHealthBar1);

        // Coge las cargas iniciales de las habilidades
        _currentChargeOne1 = _playerCharge1.SuperDash.currentCharge;
        _currentChargeTwo1 = _playerCharge1.ManoDeLasSombras.currentCharge;
    }

    void Update()
    {
        UpdateAbilityCharge1();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    private void UpdateHealthBar1(float modifiedHealth)
    {
        // Actualizamos el valor de la vida actual
        _currentHealth1 = _healthManager1.Health;

        // Controlamos el Slider como un porcentaje (0-1)
        if (_currentHealth1 <= 0)
        {
            _healthSlider1.value = 0;
        }
        else
        {
            //El slider value = 1, es decir, barra llena si _currentHealth == _maxHealth.
            _healthSlider1.value = _currentHealth1 / _maxHealth1;
        }
    }

    private void UpdateAbilityCharge1() {
        // Actualizamos los valores de las cargas
        _currentChargeOne1 = _playerCharge1.SuperDash.currentCharge;
        _currentChargeTwo1 = _playerCharge1.ManoDeLasSombras.currentCharge;
        
        // Calculamos el porcentaje de carga
        float chargePercentageOne = _currentChargeOne1 / _playerCharge1.SuperDash.maxCharge;
        float chargePercentageTwo = _currentChargeTwo1 / _playerCharge1.ManoDeLasSombras.maxCharge;
        
        // Cambiamos el color de las imagenes
        _abilityOneImg1.color = new Color(chargePercentageOne, chargePercentageOne, chargePercentageOne, 1f);
        _abilityTwoImg1.color = new Color(chargePercentageTwo, chargePercentageTwo, chargePercentageTwo, 1f);
    }
    #endregion   

} // class UIManager 
// namespace
