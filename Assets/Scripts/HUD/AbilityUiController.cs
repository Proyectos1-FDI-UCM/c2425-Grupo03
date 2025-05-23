//---------------------------------------------------------
// Componente que actualiza el ui de los controles de las hablidades
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class AbilityUiController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Texto del habilidad 1
    /// </summary>
    [SerializeField] private TextMeshProUGUI _abilityOneText;

    /// <summary>
    /// Texto del habilidad 2
    /// </summary>
    [SerializeField] private TextMeshProUGUI _abilityTwoText;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

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
        //Subscribir la actualizacion de HUD al cambio de dispositivo
        InputManager.Instance._deviceChange.AddListener(UpdateControlDisplay);
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


    /// <summary>
    /// Cambia el texto mostrado segun el dispositivo que se esta utilizando
    /// </summary>
    private void UpdateControlDisplay()
    {
        InputDevice device = InputManager.Instance.Device;
        //Mando
        if (device is Gamepad)
        {
            _abilityOneText.text = "<sprite name=\"XBoxLB\">";
            _abilityTwoText.text = "<sprite name=\"XBoxRB\">";
        }

        //Teclado
        else
        {
            _abilityOneText.text = "<sprite name=\"KeyU\">"; 
            _abilityTwoText.text = "<sprite name=\"KeyI\">";
        }
    }

    #endregion   

} // class AbilityUiController 
// namespace
