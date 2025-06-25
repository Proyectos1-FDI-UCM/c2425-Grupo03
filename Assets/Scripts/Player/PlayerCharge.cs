//---------------------------------------------------------
// Script que maneja la barra de carga de las habilidades del jugador.
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEditorInternal;
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

    [SerializeField] private float OverchargeMultiplier = 0.5f;

    [SerializeField] private float OverchargePenaltyMult = 2f;

    [SerializeField] private float OverchargeOnWavesMultiplier = 0.5f;

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

        public float currentOvercharge;
        public bool isOvercharged;

        public float maxOvercharge;


    }
    private static Ability _abilityManoDeLasSombras;
    private static Ability _abilitySuperDash;

    private WaveController wavecontroller;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    public Ability ManoDeLasSombras { get => _abilityManoDeLasSombras; }
    public Ability SuperDash { get => _abilitySuperDash; }
    #endregion

    // ---- ATRIBUTOS PUBLICOS ----
    #region Atributos Públicos
    /// <summary>
    /// Evento para cuando cambia el valor de la carga de Super Dash.
    /// </summary>
    [HideInInspector]
    public UnityEvent _onChargeChangeSuperDash;
    [HideInInspector]
    public UnityEvent _onOverchargeChangeSuperDash;
    /// <summary>
    /// Evento para cuando cambia el valor de la carga de Mano de las Sombras.
    /// </summary>
    [HideInInspector]
    public UnityEvent _onChargeChangeManoSombras;
    [HideInInspector]
    public UnityEvent _onOverchargeChangeManoSombras;
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
        _abilityManoDeLasSombras.currentOvercharge = 0;


        _abilityManoDeLasSombras.isCharged = false;
        _abilityManoDeLasSombras.isOvercharged = false;


        _abilityManoDeLasSombras.maxCharge = _maxChargeManoDeLasSombras;
        _abilityManoDeLasSombras.maxOvercharge = _maxChargeManoDeLasSombras;



        _abilitySuperDash.currentCharge = 0;
        _abilitySuperDash.currentOvercharge = 0;


        _abilitySuperDash.isCharged = false;
        _abilitySuperDash.isOvercharged = false;


        _abilitySuperDash.maxCharge = _maxChargeSuperDash;
        _abilitySuperDash.maxOvercharge = _maxChargeSuperDash;

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
        if (!_abilityManoDeLasSombras.isCharged) {
            AddChargeAbility(ref _abilityManoDeLasSombras, chargePoints);
            _onChargeChangeManoSombras.Invoke();
        }
        else if (!_abilityManoDeLasSombras.isOvercharged)
        {
            chargePoints *= OverchargeMultiplier;
            if (wavecontroller.CheckWaveState()) chargePoints *= OverchargeOnWavesMultiplier;

            AddChargeAbility(ref _abilityManoDeLasSombras, chargePoints);
            _onOverchargeChangeManoSombras.Invoke();
        }

        if (!_abilitySuperDash.isCharged)
        {
            AddChargeAbility(ref _abilitySuperDash, chargePoints);
            _onChargeChangeSuperDash.Invoke();
        }
        else if (!_abilitySuperDash.isOvercharged)
        {
            chargePoints *= OverchargeMultiplier;
            if (wavecontroller.CheckWaveState()) chargePoints *= OverchargeOnWavesMultiplier;

            AddChargeAbility(ref _abilitySuperDash, chargePoints);
            _onOverchargeChangeSuperDash.Invoke();
        }
    }
    /// <summary>
    /// Quita carga de la barra.
    /// </summary>
    /// <param name="removedHealth">Los puntos de vida que se han quitado al jugador.</param>
    public void RemoveCharge(float removedHealth)
    {
        // Calcula los puntos de carga que quitar
        float chargePoints = -(_removedChargePercentage / 100 * removedHealth);
        if (!_abilityManoDeLasSombras.isCharged) 
        {
            // Quita de Mano de las ombras.

            AddChargeAbility(ref _abilityManoDeLasSombras, chargePoints);
            _onChargeChangeManoSombras.Invoke();
        }
        else if (!_abilityManoDeLasSombras.isOvercharged)
        {
            chargePoints *= OverchargePenaltyMult;

            AddChargeAbility(ref _abilityManoDeLasSombras, chargePoints);
            _onOverchargeChangeManoSombras.Invoke();
        }
        if (!_abilitySuperDash.isCharged)
        {
            // Quita del Super Dash.

            AddChargeAbility(ref _abilitySuperDash, chargePoints);
            _onChargeChangeSuperDash.Invoke();
        }
        else if (!_abilitySuperDash.isOvercharged)
        {
            chargePoints *= OverchargePenaltyMult;

            AddChargeAbility(ref _abilitySuperDash, chargePoints);
            _onOverchargeChangeSuperDash.Invoke();
        }
    }
    /// <summary>
    /// Método  que resetea la carga de super dash a 0.
    /// </summary>
    public void ResetSuperDash()
    {
        _abilitySuperDash.currentCharge = _abilitySuperDash.currentOvercharge;

        _abilitySuperDash.currentOvercharge = 0;

        if (!_abilitySuperDash.isOvercharged) _abilitySuperDash.isCharged = false;

        _abilitySuperDash.isOvercharged = false;

        _onOverchargeChangeSuperDash.Invoke();

        _onChargeChangeSuperDash.Invoke();
    }
    /// <summary>
    /// Método  que resetea la carga de mano de las sombras a 0.
    /// </summary>
    public void ResetManoDeLasSombras()
    {
        _abilityManoDeLasSombras.currentCharge = _abilityManoDeLasSombras.currentOvercharge;

        _abilityManoDeLasSombras.currentOvercharge = 0;

        if (!_abilityManoDeLasSombras.isOvercharged) _abilityManoDeLasSombras.isCharged = false;

        _abilityManoDeLasSombras.isOvercharged = false;

        _onOverchargeChangeManoSombras.Invoke();

        _onChargeChangeManoSombras.Invoke();
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
        if (!ability.isCharged) 
        {
            ability.currentCharge = Math.Clamp(ability.currentCharge + chargePoints, 0, ability.maxCharge);
            if (ability.currentCharge >= ability.maxCharge) 
            {
                ability.isCharged = true;
            }
        }
        else if (!ability.isOvercharged) 
        {
            ability.currentOvercharge = Math.Clamp(ability.currentOvercharge + chargePoints, 0, ability.maxOvercharge);
            if (ability.currentOvercharge >= ability.maxOvercharge)
            {
                ability.isOvercharged = true;
            }
        }
    }
    #endregion


} // class PlayerChargeScript 
// namespace
