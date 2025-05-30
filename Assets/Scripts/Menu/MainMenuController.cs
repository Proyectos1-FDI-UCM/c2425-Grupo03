//---------------------------------------------------------
// Componente que maneja el menu principal
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// El nombre de la escena principal
    /// </summary>
    [SerializeField]
    private string _playSceneName;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private string _creditsSceneName;
    [SerializeField]
    private string _introScene;

    [SerializeField]
    private string _mainMenuSceneName;
    /// <summary>
    /// Referencia de la flecha del boton Jugar
    /// </summary>
    [SerializeField] private GameObject _playArrow;

    /// <summary>
    /// Referencia del boton Salir
    /// </summary>
    [SerializeField] private GameObject _exitArrow;

    [SerializeField] private GameObject _creditsArrow;

    /// <summary>
    ///  Referencia al primer boton que se selecciona al abrir el menu
    /// </summary>
    [SerializeField] private GameObject _firstButton;
    /// <summary>
    /// Sonido que se reproduce al cambiar de boton
    /// </summary>
    [SerializeField] private AudioClip _changeBotton;
    /// <summary>
    /// sonido de click boton
    /// </summary>
    [SerializeField] private AudioClip _clickBotton;
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
    /// El input del jugador
    /// </summary>
    private PlayerInputActions _playerInput;
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
        _playerInput = InputManager.Instance.GetInputActions();
        _playerInput.UI.Enable();
        
        EventSystem.current.SetSelectedGameObject(_firstButton);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Metodo que carga la escena del juego cuando se presiona el botón "Jugar"
    /// </summary>
    public void OnPlayButton()
    {
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
        Invoke("ChangeScene", _clickBotton.length);
    }

    /// <summary>
    /// Metodo que cierra la aplicación cuando se presiona el botón "Salir"
    /// </summary>
    public void OnExitButton()
    {
        //Cierra la aplicacion (solo en la build)
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
        Application.Quit();
    }

    /// <summary>
    /// Cambia a la escena de creditos
    /// </summary>
    public void OnCreditsButton()
    {
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
        Invoke("LoadCreditsScene", _clickBotton.length);
    }

    /// <summary>
    /// Vuelve al menú principal
    /// </summary>
    public void OnReturnButton()
    {
        SoundManager.Instance?.PlaySFX(_clickBotton, transform, 0.5f);
        Invoke("ReturnToMainMenu", _clickBotton.length);
    }
    public void OnEnterButton() {
        Invoke("LoadIntro", _clickBotton.length);
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    /// <summary>
    /// Activa la flecha del botón "Jugar"
    /// </summary>
    public void OnSelectPlay()
    {
        SoundManager.Instance.PlaySFX(_changeBotton, transform, 0.5f);
        _playArrow.SetActive(true);
    }

    /// <summary>
    /// Desactiva la flecha del botón "Jugar"
    /// </summary>
    public void OnDeselectPlay()
    {
        _playArrow.SetActive(false);
    }

    /// <summary>
    /// Activa la flecha del botón "Salir"
    /// </summary>
    public void OnSelectExit()
    {
        SoundManager.Instance.PlaySFX(_changeBotton, transform, 0.5f);
        _exitArrow.SetActive(true);
    }

    /// <summary>
    /// Desactiva la flecha del botón "Salir"
    /// </summary>
    public void OnDeselectExit()
    {
        _exitArrow.SetActive(false);
    }

    public void OnSelectCredits()
    {
        SoundManager.Instance.PlaySFX(_changeBotton, transform, 0.5f);
        _creditsArrow.SetActive(true);
    }

    public void OnDeselectCredits()
    {
        _creditsArrow.SetActive(false);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region
    /// <summary>
    /// Método llamado tras terminar el sonido de click para transicionar de escena
    /// </summary>
    void ChangeScene()
    {
        GameManager.Instance.GoActualLevel();
    }
    private void LoadIntro()
    {
        SceneManager.LoadScene(_introScene);
    }
    #endregion

    private void LoadCreditsScene()
    {
        SceneManager.LoadScene(_creditsSceneName);
    }


} // class MainMenuController 
// namespace
