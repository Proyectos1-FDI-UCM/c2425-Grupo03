//---------------------------------------------------------
// Script que maneja la barra de carga de las habilidades del jugador.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerCharge : MonoBehaviour
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
    [SerializeField] private int _maxChargeManoDeLasSombras;

    [Header("Super Dash")]
    /// <summary>
    /// El valor máximo de la carga de Super Dash
    /// Y la carga actual de la Super Dash
    /// </summary>
    [SerializeField] private int _maxChargeSuperDash;

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
    /// Una estructura que define la habilidad.
    /// </summary>
    public struct Ability {
        public float currentCharge;
        public bool isCharged;
        public float maxCharge;
    }
    private static Ability _abilityManoDeLasSombras;
    private static Ability _abilitySuperDash;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    public Ability ManoDeLasSombras { get => _abilityManoDeLasSombras; }
    public Ability SuperDash { get => _abilitySuperDash; }
    #endregion

    // ---- ATRIBUTOS PUBLICOS ----
    #region Atributos Públicos
    /// <summary>
    /// Evento para cuando cambia el valor de las cargas.
    /// </summary>
    [HideInInspector]
    public UnityEvent _onChargeChange;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        GetComponent<HealthManager>()._onDamaged.AddListener(RemoveCharge);
        
        _abilityManoDeLasSombras.currentCharge = 0;
        _abilityManoDeLasSombras.isCharged = false;
        _abilityManoDeLasSombras.maxCharge = _maxChargeManoDeLasSombras;

        _abilitySuperDash.currentCharge = 0;
        _abilitySuperDash.isCharged = false;
        _abilitySuperDash.maxCharge = _maxChargeSuperDash;

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Añade carga a la barra.
    /// </summary>
    /// <param name="chargePoints">Los puntos que se van a añadir a la barra.</param>
    public void AddCharge(float chargePoints) 
    {   
        AddChargeAbility(ref _abilityManoDeLasSombras, chargePoints);
        AddChargeAbility(ref _abilitySuperDash, chargePoints);
        _onChargeChange.Invoke();   
    }
    /// <summary>
    /// Quita carga de la barra.
    /// </summary>
    /// <param name="removedHealth">Los puntos de vida que se han quitado al jugador.</param>
    public void RemoveCharge(float removedHealth)
    {
        // Calcula los puntos de carga que quitar
        float chargePoints = -(_removedChargePercentage / 100 * removedHealth);
        // Quita de la mano de las sombras.
        AddChargeAbility(ref _abilityManoDeLasSombras, chargePoints);
        // Quita del Super Dash.
        AddChargeAbility(ref _abilitySuperDash, chargePoints);
        _onChargeChange.Invoke();
    }
    /// <summary>
    /// Método  que resetea la carga de super dash a 0.
    /// </summary>
    public void ResetSuperDash()
    {
        _abilitySuperDash.currentCharge = 0;
        _abilitySuperDash.isCharged = false;
        _onChargeChange.Invoke();
    }
    /// <summary>
    /// Método  que resetea la carga de mano de las sombras a 0.
    /// </summary>
    public void ResetManoDeLasSombras()
    {
        _abilityManoDeLasSombras.currentCharge = 0;
        _abilityManoDeLasSombras.isCharged = false;
        _onChargeChange.Invoke();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos privados
    /// <summary>
    /// Método que añade puntos de carga a cierta habilidad.
    /// </summary>
    /// <param name="ability">La habilidad a cual se le aplica el cambio.</param>
    /// <param name="chargePoints">El número de puntos de carga para añadir.</param>
    private void AddChargeAbility(ref Ability ability, float chargePoints) {
        if (!ability.isCharged) {
            ability.currentCharge = Math.Clamp(ability.currentCharge + chargePoints, 0, ability.maxCharge);
            if (ability.currentCharge >= ability.maxCharge) {
                ability.isCharged = true;
            }
        }
    }
    #endregion


} // class PlayerChargeScript 
// namespace
