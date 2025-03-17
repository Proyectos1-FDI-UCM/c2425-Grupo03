//---------------------------------------------------------
// Breve descripción del contenido del archivo
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
        EventSystem.current.SetSelectedGameObject(_firstButton);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
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
        SceneManager.LoadScene("LevelTest_Zhiyi");
    }

    /// <summary>
    /// Metodo que cierra la aplicación cuando se presiona el botón "Salir"
    /// </summary>
    public void OnExitButton()
    {
        //Cierra la aplicacion (solo en la build)
        Application.Quit();
        Debug.Log("Salir de la aplicación");
    }

    /// <summary>
    /// Activa la flecha del botón "Jugar"
    /// </summary>
    public void OnSelectPlay()
    {
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

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)



    #endregion   

} // class MainMenuController 
// namespace
