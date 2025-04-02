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
    /// Deshabilitar los movimientos del jugador
    /// </summary>
    [SerializeField] private PlayerStateMachine _player;

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

    /// <summary>
    /// El input del menu
    /// </summary>
    private PlayerInputActions _menuInput;

    /// <summary>
    /// El input del jugador
    /// </summary>
    private PlayerInputActions.PlayerActions _playerInput;
    //private PlayerInputActions _playerInput;
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

    private void Awake()
    {
        _menuInput = new PlayerInputActions();
    }
    void Start()
    {
        _pauseMenu.SetActive(false);
        _menuInput.Disable();

        //coge referencia al input del jugador
        if (_player != null)
        {
            if (_player.GetComponent<PlayerStateMachine>() != null)
            {
                _playerInput = _player.GetComponent<PlayerStateMachine>().PlayerInput;
            }
        }

        _menuInput.UI.Cancel.performed += UnpausePress;
        _playerInput.Menu.performed += PausePress;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método que se llama cuando se pausa el juego.
    /// </summary>
    /// <param name="context"></param>
    public void PausePress(InputAction.CallbackContext context) {
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
        if (!_paused) // si no está pausado, pausa el juego
        {
            PauseGame(); 
        }
    }
    /// <summary>
    /// Método llamado para salir del menú de pausa.
    /// </summary>
    /// <param name="context"></param>
    public void UnpausePress(InputAction.CallbackContext context) {
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
        if (_paused) // si está pausado, vuelve al juego
        {
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
        _playerInput.Disable();

        _menuInput.UI.Enable();

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
        SoundManager.Instance?.PlaySFX(_changeBotton,transform,0.2f);
    }

    /// <summary>
    ///  Metodo que reanuda el juego desactivando el menu de pausa
    /// </summary>
    public void ContinueGame()
    {
        _playerInput.Enable();
        _pauseMenu.SetActive(false);

        if (_player != null)
        {
            _player.enabled = true;
        }
        SoundManager.Instance?.PlaySFX(_clickBotton, transform, 0.5f);

        _menuInput.UI.Disable();
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
        SceneManager.LoadScene(_mainMenuSceneName); 
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


} // class PauseMenuController 
// namespace
