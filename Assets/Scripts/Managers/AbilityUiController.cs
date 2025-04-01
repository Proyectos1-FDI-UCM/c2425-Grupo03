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

    /// <summary>
    /// Si el jugador esta usando mando o no
    /// </summary>
    private bool _isUsingController;

    private bool _isControllerActive;

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
        //Detecta que dispositivo esta en uso al inicio del juego
        DetectInputDevice();
        UpdateControlDisplay();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        DetectInputDevice();
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
    /// Metodo que detecta si el jugador esta usando teclado o mando
    /// </summary>
    private void DetectInputDevice()
    {
        // Si hay mando conectado
        Gamepad gamepad = Gamepad.current;

        //Si hay mando conectado y se esta usando o teclado detectado
        _isControllerActive = gamepad != null && (gamepad.IsActuated() || Keyboard.current == null);

        // Si el tipo de control ha cambiado
        if (_isControllerActive != _isUsingController)
        {
            _isUsingController = _isControllerActive;
            UpdateControlDisplay();
        }
    }


    /// <summary>
    /// Cambia el texto mostrado segun el dispositivo que se esta utilizando
    /// </summary>
    private void UpdateControlDisplay()
    {
        //Mando
        if (_isUsingController)
        {
            _abilityOneText.text = "LeftButton";
            _abilityTwoText.text = "RightButton";
        }

        //Teclado
        else
        {
            _abilityOneText.text = "U"; 
            _abilityTwoText.text = "I";
        }
    }

    #endregion   

} // class AbilityUiController 
// namespace
