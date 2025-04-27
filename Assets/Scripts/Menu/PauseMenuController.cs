//---------------------------------------------------------
// Archivo que maneja el menu de pausa durante el juego
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    [SerializeField] private GameObject _pauseMenu;
    ///<summary>
    /// El nombre de la escena principal
    ///</summary>
    [SerializeField] private string _mainMenuSceneName;

    /// <summary>
    /// Referencia a los rombos para indicar la seleccion de boton
    /// </summary>
    [SerializeField] private GameObject _continueRhombus;

    /// <summary>
    /// Referencia a los rombos para indicar la seleccion de boton
    /// </summary>
    [SerializeField] private GameObject _mainMenuRhombus;

    /// <summary>
    /// Referencia al primer boton que se selecciona al abrir el menu
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
    /// Indica si el juego esta pausado o no
    /// </summary>
    private bool _paused = false;

    PlayerStateMachine _ctx;
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
        InputManager.Instance.AddPausePressedListener(PausePress);
        InputManager.Instance.AddPauseCancelListener(UnpausePress);

        _pauseMenu.SetActive(false);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se llama cuando se pausa el juego.
    /// </summary>
    /// <param name="context"></param>
    public void PausePress()
    {
        if (!_paused) // si no está pausado, pausa el juego
        {
            SoundManager.Instance?.PlaySFX(_clickBotton, transform, 0.5f);
            PauseGame();
            Debug.Log("Game Paused");
        }
    }
    /// <summary>
    /// Método llamado para salir del menú de pausa.
    /// </summary>
    /// <param name="context"></param>
    public void UnpausePress()
    {
        if (_paused) // si está pausado, vuelve al juego
        {
            SoundManager.Instance?.PlaySFX(_clickBotton, transform, 0.5f);
            ContinueGame();
        }
    }

    /// <summary>
    /// Metodo que pausa el juego activando el menú de pausa
    /// </summary>
    public void PauseGame()
    {
        // Activa el menú de pausa
        _pauseMenu.SetActive(true);

        // Desactiva el control del jugador
        InputManager.Instance.DisablePlayerInput();

        InputManager.Instance.EnableMenuInput();

        // Detiene el tiempo del juego
        Time.timeScale = 0f;
        _paused = true;

        //_playerInput = new PlayerInputActions();

        // Selecciona el primer boton del menu de pausa 
        EventSystem.current.SetSelectedGameObject(_firstButton);

    }
    /// <summary>
    /// Reproduce el sonido _changeBotton
    /// </summary>

    public void PlayChangeBottonSFX()
    {
        SoundManager.Instance?.PlaySFX(_changeBotton, transform, 0.2f);
    }

    /// <summary>
    ///  Metodo que reanuda el juego desactivando el menu de pausa
    /// </summary>
    public void ContinueGame()
    {
        InputManager.Instance.EnablePlayerInput();
        _pauseMenu.SetActive(false);

        SoundManager.Instance?.PlaySFX(_clickBotton, transform, 0.5f);

        InputManager.Instance.DisableMenuInput();
        Time.timeScale = 1f;
        _paused = false;
    }

    /// <summary>
    /// Metodo que vuelve al menu principal
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        _paused = false;
        SoundManager.Instance?.PlaySFX(_clickBotton, transform, 0.5f);
        Invoke("ChangeScene", _clickBotton.length);
        
    }
    

    public void OnSelectContinue()
    {
        SoundManager.Instance?.PlaySFX(_changeBotton, transform, 0.5f);
        _continueRhombus.SetActive(true);
    }


    /// <summary>
    /// Desactiva la flecha del botón "Jugar"
    /// </summary>
    public void OnDeselectContinue()
    {
        _continueRhombus.SetActive(false);
    }


    /// <summary>
    /// Activa la flecha del botón "Salir"
    /// </summary>
    public void OnSelectMainMenu()
    {
        SoundManager.Instance?.PlaySFX(_changeBotton, transform, 0.5f);
        _mainMenuRhombus.SetActive(true);
    }


    /// <summary>
    /// Desactiva la flecha del botón "Salir"
    /// </summary>
    public void OnDeselectMainMenu()
    {
        _mainMenuRhombus.SetActive(false);
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos privados
    /// <summary>
    /// Cambia la escena a la del menú principal.
    /// Se llama tras terminar el sonido de click.
    /// </summary>
    void ChangeScene()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    private void OnDestroy()
    {
        if (InputManager.HasInstance())
        {
            InputManager.Instance.RemovePausePressedListener(PausePress);
            InputManager.Instance.RemovePauseCancelListener(UnpausePress);
        }
    }
    #endregion


} // class PauseMenuController 
// namespace
