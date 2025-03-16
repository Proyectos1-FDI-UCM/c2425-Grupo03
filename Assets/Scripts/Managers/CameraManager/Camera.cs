//---------------------------------------------------------
// Archivo que maneja la camara y las instrucciones que se le dan.
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// La cámara del juego con la que el jugador ve el entorno.
/// </summary>
public class CameraManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Posicion Jugador u objeto.
    /// </summary>
    [SerializeField] Transform _playerPosition;
    /// <summary>
    /// velocidad de la Cámara
    /// </summary>
    [SerializeField][Min(0)] float _cameraVelocity;
    /// <summary>
    /// Margen de la Cámara respecto al objetivo
    /// </summary>
    [SerializeField] private Vector3 _cameraDisplacement; //dista de la camara (posicion del jugador)
    [SerializeField] private float _maxDisplacement;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Las instrucciones a ejecutar de la cámara.
    /// </summary>
    Queue<CameraInstruction> _cameraInstructions = new Queue<CameraInstruction>();

    /// <summary>
    /// La cámara a actualizar.
    /// </summary>
    Camera _camera;

    /// <summary>
    /// Dirección de movimiento de la cámara.
    /// </summary>
    private Vector3 _moveDir;
    private PlayerInputActions.PlayerActions _playerInput;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    /// <summary>
    /// La instancia del singleton CameraManager.
    /// </summary>
    public static CameraManager Instance { get; private set; }

    public Camera Camera => _camera;

    public bool IsFollowingPlayer { get; set; } = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Awake()
    {
        if(Instance == null)
        {
            Debug.Log("Im awaking");
            Instance = this;
            _camera = GetComponent<Camera>();
            _playerInput = new PlayerInputActions().Player;
            _playerInput.Enable();
            EnqueueInstruction(new CameraFollowPlayer(1, 7));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Actualiza la instrucción actual y cambia de instrucción cuando esta termina.
    /// </summary>
    void Update()
    {
        if (_cameraInstructions.Count > 0)
        {
            // Actualiza la camara con la instrucción actual.
            bool instructionEnded = _cameraInstructions.Peek().UpdateCamera(_camera);

            // Si la instrucción ha terminado y hay más instrucciones, pasa a la siguiente instrucción.
            if (instructionEnded && _cameraInstructions.Count > 1)
            {
                _cameraInstructions.Peek().OnDequeued();
                _cameraInstructions.Dequeue();
                _cameraInstructions.Peek().SetUp();
            }

            /* PARA TESTEAR
            string str = "Camera Queue: ";
            foreach(CameraInstruction instruction in _cameraInstructions)
            {
                str += instruction.ToString() + ", ";
            }
            Debug.Log(str);
            */
        }

        // Lee el input del jugador.
        _moveDir = _playerInput.MoveCamera.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (IsFollowingPlayer && _playerPosition != null)
        {
            Vector3 finalPos = _playerPosition.position + _moveDir * _maxDisplacement + _cameraDisplacement; // Calcula la posición final (posición del jugador + el input * el desplazamiento máximo posible)
            transform.position = Vector3.Lerp(transform.position, finalPos, _cameraVelocity);
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void EnqueueInstruction(CameraInstruction cameraInstruction)
    {
        // pone la instrucción a la cola
        _cameraInstructions.Enqueue(cameraInstruction);

        // si es la primera instrucción la inicializa
        if(_cameraInstructions.Count == 0) cameraInstruction.SetUp();
    }

    #endregion
    

} // class Camera 
// namespace
