//---------------------------------------------------------
// Archivo que maneja los checkpoints
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    /// <summary>
    /// Instancia para implementar el patrón Singleton
    /// </summary>
    public static CheckpointManager Instance { get; private set; }

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// El punto de spawn
    /// </summary>
    [Header("Initial SpawnPoint")]
    [SerializeField] private Transform _initialPoint;
    /// <summary>
    /// El sonido del jugador al respawnear en el checkpoint
    /// </summary>
    [SerializeField] AudioClip _respawnSound;
    /// <summary>
    /// Sonido al activar checkpoint
    /// </summary>
    [SerializeField] AudioClip _takeCheckPoint;

    /// <summary>
    /// Array de los checkpoints de nivel actual
    /// </summary>
    private CheckPointData[] _checkPointDatas;

    /// <summary>
    /// El indice el checkpoint actual
    /// </summary>
    private int _currentCheckPointIndex;

    #endregion
    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.

    struct CheckPointData
    {
        public int index;
        public Vector3 position;
    }
    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Patrón Singleton, asegura que solo exista una instancia del Manager
    /// </summary>
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Inicializar el array de checkpoint
        _checkPointDatas = new CheckPointData[FindObjectsByType(typeof(Checkpoint), FindObjectsSortMode.None).Length];
        InitCheckPointDatas(ref _checkPointDatas);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    private void Start()
    {
        if (GameManager.Instance.GetCheckpoint() == null && _initialPoint != null)
        {
            // Si no hay checkpoint guardado, establece el punto inicial como checkpoint
            GameManager.Instance.SetCheckpoint(_initialPoint);
        }

        //Inicializar el indice del checkpoint actual
        _currentCheckPointIndex = -1;
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
    /// Método para registrar el último checkpoint
    /// </summary>
    /// <param name="checkpointTransform"></param>
    public void SetCheckpoint(Transform checkpointTransform)
    {
        // Reproduce un sonido al activar el checkpoint
        SoundManager.Instance.PlaySFX(_takeCheckPoint, transform, 0.1f);

        // Guarda el nuevo checkpoint en el GameManager
        GameManager.Instance.SetCheckpoint(checkpointTransform);
        // Asigna la referencia del último punto a este checkpoint

        _currentCheckPointIndex = checkpointTransform.GetComponent<Checkpoint>().GetCheckPointIndex();
    }

    /// <summary>
    /// Teletransporta al jugador al último checkpoint guardado
    /// </summary>
    /// <param name="player"></param>
    public void RespawnPlayer(GameObject player)
    {
        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (GameManager.Instance.GetCheckpoint() != null)
        {
            SoundManager.Instance.PlaySFX(_respawnSound, transform, 0.1f);
            // Mueve la posición del jugador a la posición del checkpoint
            player.transform.position = (Vector3) GameManager.Instance.GetCheckpoint();

            //Restablece la camara para que siga al jugador
            CameraManager.Instance.EnqueueInstruction(new CameraFollowPlayer(0.1f, CameraManager.Instance.GetComponent<Camera>().orthographicSize));
        }
    }

    public void AddCheckPointData(int index, Vector3 position)
    {
        _checkPointDatas[index].position = position;  
    }

    public void ResetCurrentCheckpointIndex()
    {
        _currentCheckPointIndex = -1;
    }
    #endregion

    //public void PrintCheckPointData()
    //{
    //    for(int i = 0; i < _checkPointDatas.Length; i++)
    //    {
    //        Debug.Log($"Checkpoint {i}: {_checkPointDatas[i].position}");
    //    }
    //}

    /// <summary>
    /// Ir al siguiente checkpoint disponible
    /// </summary>
    public void NextCheckpoint()
    {
        PlayerStateMachine player = FindFirstObjectByType<PlayerStateMachine>();

        if (_currentCheckPointIndex + 1 < _checkPointDatas.Length)
        {
            player.transform.position = _checkPointDatas[_currentCheckPointIndex + 1].position;
        }
    }

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos

    /// <summary>
    /// Inicializar los datos del checkpoint
    /// </summary>
    /// <param name="datas"></param>
    private void InitCheckPointDatas(ref CheckPointData[] datas)
    {
        for (int i = 0;i < datas.Length;i++)
        {
            datas[i].index = i;
            datas[i].position = Vector3.zero;
        }
    }
    #endregion

} // class CheckPointManager 
// namespace
