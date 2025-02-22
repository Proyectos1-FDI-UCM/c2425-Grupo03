//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al GameManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            TransferSceneState();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
    }

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    #endregion
} // class GameManager 
// namespace