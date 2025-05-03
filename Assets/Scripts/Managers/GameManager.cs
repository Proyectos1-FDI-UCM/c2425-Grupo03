//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Guarda la posición del último checkpoint activado
    /// </summary>
    private Vector2? _lastCheckpoint;

    float _masterVolume = 1;
    float _sfxVolume = 1;
    float _musicVolume = 1;

    /// <summary>
    /// guarda el ultimo checkpoint activado
    /// </summary>
    int activatedCheckpoint = -1;

    private Dictionary<int, string> _levels = new Dictionary<int, string>()
    {
        { 1, "Level_Tutorial"},
        { 2, "Level_2"},
        { 3, "Level_3"},
    };
    /// <summary>
    /// El nivel actual
    /// </summary>
    private int _actualLevel;

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

    protected void Start()
    {
        //Inicializa el último checkpoint en(0, 0)
        _lastCheckpoint = null;

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
        _actualLevel = 1;
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

    public void GoActualLevel()
    {
        ChangeScene(_levels[_actualLevel]);
        MusicPlayer.Instance.PlayLevelSound();
    }

    public void AddActualLevel()
    {
        if (_actualLevel < _levels.Count)
        {
            _actualLevel++;
        }
        else
        {
            _actualLevel = 1;
        }
    }
    /// <summary>
    /// resetea los checkpoint activados
    /// </summary>
    public void InitCheckpoint()
    {
        activatedCheckpoint = -1;
        _lastCheckpoint = null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(string sceneName)
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        System.GC.Collect();
    } // ChangeScene

    /// <summary>
    /// Metodo que establece la posición del último checkpoint activado
    /// </summary>
    /// <param name="checkpoint"></param>
    public void SetCheckpoint(Transform checkpoint)
    {
        // Guarda la posición del nuevo checkpoint
        _lastCheckpoint = checkpoint.position;
    }

    /// <summary>
    /// Metodo que devuelve la posición del último checkpoint guardado
    /// </summary>
    /// <returns></returns>
    public Vector2? GetCheckpoint()
    { 
        return _lastCheckpoint;
    }

    /// <summary>
    /// Metodo que verifica si un checkpoint ya ha sido activado previamente
    /// </summary>
    /// <param name="_checkPointIndex"></param>
    /// <returns></returns>
    public bool IsActivated(int _checkPointIndex) 
    {
        return activatedCheckpoint >= _checkPointIndex;
    }

    public void SetMasterVolume(float volume)
    {
        _masterVolume = volume;
    }

    public float GetMasterVolume()
    {
        return _masterVolume;
    }
    public void SetSFXVolume(float volume)
    {
        _sfxVolume = volume;
    }

    public float GetSFXVolume()
    {
        return _sfxVolume;
    }
    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }

    /// <summary>
    /// Metodo que se llama cuando activas un checkpoint para marcarlo como activado
    /// </summary>
    /// <param name="_checkPointIndex"></param>
    public void AddCheckpoint (int _checkPointIndex)
    {
        if (activatedCheckpoint < _checkPointIndex)
        {
            activatedCheckpoint = _checkPointIndex;
        }
    }

    /// <summary>
    /// Metodo para ir al siguiente nivel
    /// </summary>
    public void NextLevel()
    {
        if(_actualLevel + 1 <= _levels.Count)
        {
            Debug.Log("Yep");
            LevelLoader levelLoader = FindFirstObjectByType<LevelLoader>();
            levelLoader.ChangeScene(_levels[_actualLevel + 1]);
        }
    }
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