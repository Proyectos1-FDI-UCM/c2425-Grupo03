//---------------------------------------------------------
// Script que maneja la barra de carga de las habilidades del jugador.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerChargeScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// El porcentaje del daño que se va a quitar de la barra de carga.
    /// </summary>

    
    [Header("Mano de la sombra")]
    /// <summary>
    /// El valor máximo de la carga de Mano de La Sombra
    /// Y la carga actual de la Mano de la Sombra
    /// </summary>
    [SerializeField] private int _maxChargeManoDeLaSombra;
    private float _currentChargeManoDeLaSombra;

    [Header("Super Dash")]
    /// <summary>
    /// El valor máximo de la carga de Super Dash
    /// Y la carga actual de la Super Dash
    /// </summary>
    [SerializeField] private int _maxChargeSuperDash;
    private float _currentChargeSuperDash;

    /// <summary>
    /// El porcentaje de carga que le quita al jugador de las habilidades
    /// en funcion del daño del enemigo
    /// </summary>
    /// 
    [SerializeField] private float _removedChargePercentage;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// El numero de habilidades que tiene el jugador.
    /// </summary>
    private static int _abilityNr = 2;

    /// <summary>
    /// El valor minimo de la carga.
    /// </summary>
    private int _MIN_CHARGE = 0;    
    /// <summary>
    /// Una estructura que define la habilidad.
    /// </summary>
    public struct Ability {
        public float currentCharge;
        public bool isCharged;
        public float maxCharge;
    }
    private static Ability abilityOne;
    private static Ability abilityTwo;
    public Ability[] abilities = new Ability[_abilityNr];
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
        abilities[0] = abilityOne;
        abilities[1] = abilityTwo;
        GetComponent<HealthManager>()._onDamaged.AddListener(RemoveCharge);
        for (int i = 0; i < abilities.Length; i++) {
            abilities[i].currentCharge = 0;
            abilities[i].isCharged = false;
        }

        abilities[0].maxCharge = _maxChargeSuperDash;
        abilities[1].maxCharge = _maxChargeManoDeLaSombra;
    }

    private void Update()
    {
        _currentChargeManoDeLaSombra = abilities[1].currentCharge;
        _currentChargeSuperDash = abilities[0].currentCharge;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Añade carga a la barra.
    /// </summary>
    /// <param name="chargePoints">Los puntos que se van a añadir a la barra.</param>
    public void AddCharge(int chargePoints) 
    {
        for (int i = 0; i < abilities.Length; i++) 
        {
            if (!abilities[i].isCharged) abilities[i].currentCharge += chargePoints;

            if ((abilities[i].currentCharge >= abilities[i].maxCharge))
            {
                abilities[i].currentCharge = abilities[i].maxCharge;
                abilities[i].isCharged = true;
            }
        }
        
    }
    /// <summary>
    /// Quita carga de la barra.
    /// </summary>
    /// <param name="removedHealth">Los puntos de vida que se han quitado al jugador.</param>
    public void RemoveCharge(float removedHealth)
    {
        float chargePoints = (_removedChargePercentage / 100 * removedHealth);
        Debug.Log(chargePoints);
        for (int i = 0; i < abilities.Length; i++)
        {
            Ability currAbility = abilities[i];
            if (!currAbility.isCharged)
            {
                currAbility.currentCharge -= chargePoints;
            }

            if(currAbility.currentCharge <= _MIN_CHARGE)
            {
                currAbility.currentCharge = _MIN_CHARGE;
            }

            abilities[i] = currAbility;
        }
    }
    /// <summary>
    /// Método  que resetea la carga de la barra a 0.
    /// </summary>
    /// <param name="abilityNr">El número de la habilidad.</param>
    public void ResetCharge(int abilityNr)
    {
        abilities[abilityNr].currentCharge = 0;
        abilities[abilityNr].isCharged = false;
    }

    /// <summary>
    /// Devuelve true o false dependiendo de si la habilidad está cargada o no.
    /// </summary>
    /// <param name="abilityNr">El número de la habilidad.</param>
    /// <returns>True si la habilidad está cargada, false en caso contrario.</returns>
    public bool GetCharge(int abilityNr) {
        return abilities[abilityNr].isCharged;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    #endregion

} // class PlayerChargeScript 
// namespace
