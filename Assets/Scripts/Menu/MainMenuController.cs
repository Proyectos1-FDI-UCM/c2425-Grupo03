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
    string _playSceneName;
    /// <summary>
    /// Referencia de la flecha del boton Jugar
    /// </summary>
    [SerializeField] GameObject _playArrow;

    /// <summary>
    /// Referencia del boton Salir
    /// </summary>
    [SerializeField] GameObject _exitArrow;

    /// <summary>
    ///  Referencia al primer boton que se selecciona al abrir el menu
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
    /// <summary>
    /// el tiempo que se espera al hacer click en un boton
    /// </summary>
    [SerializeField] float _waitTime = 1f;

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

    /// <summary>
    /// tiempo en el que has dado a click
    /// </summary>
    float _clickTime;
    /// <summary>
    /// si ir a la escena play
    /// </summary>
    bool _goPlay = false;
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
        _clickTime = 0f;
        _goPlay = false;
        _playerInput = new PlayerInputActions();
        _playerInput.Player.Disable();
        _playerInput.UI.Enable();

        EventSystem.current.SetSelectedGameObject(_firstButton);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Comprueba si has dado a algun boton y si ha pasado el tiempo de espera
    /// </summary>
    void Update()
    {
        if (_clickTime != 0 && Time.time - _clickTime > _waitTime)
        {
            if (_goPlay)
            {
                SceneManager.LoadScene(_playSceneName);
            }
            else
            {
                Application.Quit();
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
    /// Metodo que carga la escena del juego cuando se presiona el botón "Jugar"
    /// </summary>
    public void OnPlayButtom()
    {
        _clickTime = Time.time;
        _goPlay = true;

        SceneManager.LoadScene(_playSceneName);

        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);

    }

    /// <summary>
    /// Metodo que cierra la aplicación cuando se presiona el botón "Salir"
    /// </summary>
    public void OnExitButton()
    {
        //Cierra la aplicacion (solo en la build)
        _clickTime = Time.time;
        SoundManager.Instance.PlaySFX(_clickBotton, transform, 0.5f);
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
    #endregion
 

} // class MainMenuController 
// namespace
