//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
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

    [SerializeField] GameObject _uiPauseMenu;
    [SerializeField] PlayerStateMachine _player;
    [SerializeField] GameObject _firstButton;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _paused = false;

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

    public void PauseGame()
    {
        //_uiPauseMenu.SetActive(true);
        if (_uiPauseMenu != null)
        {
            _uiPauseMenu.SetActive(true);
        }

        if (_player != null)
        {
            _player.enabled = false;
        }

        Time.timeScale = 0f;
        _paused = true;

        EventSystem.current.SetSelectedGameObject(_firstButton);

    }

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

        Time.timeScale = 1f;
        _paused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        _paused = false;

        SceneManager.LoadScene("MainMenu_Zhiyi");
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class PauseMenuController 
// namespace
