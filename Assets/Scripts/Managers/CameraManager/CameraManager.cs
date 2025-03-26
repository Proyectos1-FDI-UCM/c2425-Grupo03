//---------------------------------------------------------
// Archivo que maneja la camara y las instrucciones que se le dan.
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manager de la cámara. Maneja el movimiento e instrucciones de la cámara.
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
    [SerializeField] private Vector3 _cameraDisplacement;

    /// <summary>
    /// Desplazamiento máximo de la cámara desde el jugador (al moverla con las teclas o joystick)
    /// </summary>
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

    /// <summary>
    /// El input del jugador.
    /// </summary>
    private PlayerInputActions.PlayerActions _playerInput;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    /// <summary>
    /// La instancia del singleton CameraManager.
    /// </summary>
    public static CameraManager Instance { get; private set; }

    /// <summary>
    /// Getter de la cámara para tener acceso global.
    /// </summary>
    public Camera Camera => _camera;

    /// <summary>
    /// Booleana que le indica al manager si debe seguir al jugador o no.
    /// </summary>
    public bool IsFollowingPlayer { get; set; } = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Awake()
    {
        // Patrón singleton
        if(Instance == null)
        {
            Instance = this;

            // Establece la variable de cámara
            _camera = GetComponent<Camera>();

            // Coge el input del jugador
            _playerInput = new PlayerInputActions().Player;
            _playerInput.Enable();

            //Pone la instrucción de seguir al jugador por defecto
            EnqueueInstruction(new CameraFollowPlayer(0.1f, _camera.orthographicSize));
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
                // Avisa que va a cambiar de instrucción
                _cameraInstructions.Peek().OnDequeued();

                // Cambia la instrucción
                _cameraInstructions.Dequeue();

                // Avisa de que ha comenzado la instrucción
                _cameraInstructions.Peek().SetUp();
            }
            /*
            string str = $"Camera Queue ({instructionEnded}): ";
            foreach(CameraInstruction instruction in _cameraInstructions)
            {
                str += $"{instruction.ToString()}, ";
            }
            Debug.Log(str);*/
            
        }

        // Lee el input del jugador.
        _moveDir = _playerInput.MoveCamera.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Si la cámara está en la instrucción de seguir al jugador la mueve hacia el jugador y aplica el desplazamiento dado por el jugador
        if (IsFollowingPlayer && _playerPosition != null)
        {
            Vector3 finalPos = _playerPosition.position + _moveDir * _maxDisplacement + _cameraDisplacement; // Calcula la posición final (posición del jugador + el input * el desplazamiento máximo posible)
            transform.position = Vector3.Lerp(transform.position, finalPos, _cameraVelocity);
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Pone una nueva instrucción a la cola de instrucciones de la cámara.
    /// </summary>
    /// <param name="cameraInstruction">La instrucción a ejecutar por la cámara.</param>
    public void EnqueueInstruction(CameraInstruction cameraInstruction)
    {
        // pone la instrucción a la cola
        _cameraInstructions.Enqueue(cameraInstruction);

        // si es la primera instrucción la inicializa
        if(_cameraInstructions.Count == 0) cameraInstruction.SetUp();
    }

    /// <summary>
    /// Hace vibrar la cámara un cierto tiempo.
    /// </summary>
    /// <param name="duration">Durante cuanto tiempo vibra la cámara en segundos.</param>
    /// <param name="magnitude">Cuanto se puede desplazar la cámara en cualquier dirección.</param>
    public void ShakeCamera(float duration, float magnitude)
    {
        // Llama a la corrutina que hace el shake de la cámara
        // Si es necesario se puede cancelar la corrutina de shake desde aquí antes de llamara otra
        StartCoroutine(ShakeCameraAsync(duration, magnitude));
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos privados
    private IEnumerator ShakeCameraAsync(float duration, float magnitude)
    {
        // Coge la posición original
        //Vector3 originalPosition = transform.position;
        // Para guardar el tiempo desde el comienzo
        
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calcula valores aleatorios de offset para mover la cámara
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica el offset a la posición original
            transform.position = new Vector3(_playerPosition.position.x + x, _playerPosition.position.y + y, transform.position.z);
            // Guarda el tiempo pasado
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Coloca la cámara en la posición original
        transform.position = _playerPosition.position;
    }


    #endregion


} // class Camera 
// namespace
