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
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField, Range(0f, 1f)] private float _removedChargePercentage;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// El valor actual de la carga.
    /// </summary>
    private float _currentCharge = 0;
    /// <summary>
    /// El valor máximo de la carga.
    /// </summary>
    private int _maxCharge = 100;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    public float CurrentCharge { get; private set; } = 0;
    public bool IsCharged { get; private set; } = false;
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
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void AddCharge(int chargePoints) {
        if (!IsCharged) CurrentCharge += chargePoints;
        if (CurrentCharge >= _maxCharge) {
            CurrentCharge = _maxCharge;
            IsCharged = true;
        }
    }

    public void RemoveCharge(float removedHealth) {
        float chargePoints = _removedChargePercentage / 100 * removedHealth;
        if (!IsCharged) CurrentCharge -= chargePoints;
    }

    public void ResetCharge()
    {
        CurrentCharge = 0;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class PlayerChargeScript 
// namespace
