//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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

    /// <summary>
    /// Referencia al ui del menu de pausa
    /// </summary>
    [SerializeField] GameObject _uiPauseMenu;


    [SerializeField] GameObject _continueRhombus;

    [SerializeField] GameObject _mainMenuRhombus;

    /// <summary>
    /// Deshabilitar los movimientos del jugador
    /// </summary>
    [SerializeField] PlayerStateMachine _player;

    /// <summary>
    /// Referencia al primer boton que se selecciona al abrir el menu
    /// </summary>
    [SerializeField] GameObject _firstButton;

    /// <summary>
    /// Sonido que se reproduce al cambiar de boton
    /// </summary>
    [SerializeField] AudioClip _changeBotton;
    /// <summary>
    /// sonido de click boton
    /// </summary>
    [SerializeField] AudioClip _clickBotton;

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

    private PlayerInputActions _playerInput;

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
        _uiPauseMenu.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pressed key");
            SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
            if (!_paused)
            { 
                PauseGame(); 
            }
            else
            { 
                ContinueGame(); 
            }
        }
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
    /// Metodo que pausa el juego activando el menú de pausa
    /// </summary>
    public void PauseGame()
    {
        //_uiPauseMenu.SetActive(true);
        // Activa el menú de pausa
        if (_uiPauseMenu != null)
        {
            _uiPauseMenu.SetActive(true);
        }

        // Desactiva el control del jugador
        if (_player != null)
        {
            _player.enabled = false;
        }
        _playerInput = new PlayerInputActions();
        _playerInput.Player.Disable();
        _playerInput.UI.Enable();

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
        SoundManager.Instance.PlaySFX(_changeBotton,transform,0.2f);
    }

    /// <summary>
    ///  Metodo que reanuda el juego desactivando el menu de pausa
    /// </summary>
    public void ContinueGame()
    {
        if (_uiPauseMenu != null)
        { 
            _uiPauseMenu.SetActive(false);
        }

        if (_player != null)
        {
            _player.enabled = true;
        }
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
        _playerInput = new PlayerInputActions();
        _playerInput.UI.Disable();
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
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
        SceneManager.LoadScene("MainMenu_Zhiyi");
    }

    public void OnSelectContinue()
    {
        SoundManager.Instance.PlaySFX(_changeBotton, transform, 0.5f);
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
        SoundManager.Instance.PlaySFX(_changeBotton, transform, 0.5f);
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

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    //private void Awake()
    //{
    //    _playerInput = new PlayerInputActions();

    //}

    //private void OnEnable()
    //{
    //    _playerInput.UI.Enable(); // Activa los controles de la UI cuando el script está activo.
    //}

    //private void OnDisable()
    //{
    //    _playerInput.UI.Disable(); // Desactiva los controles de la UI al salir del menú.
    //}
    #endregion

} // class PauseMenuController 
// namespace
