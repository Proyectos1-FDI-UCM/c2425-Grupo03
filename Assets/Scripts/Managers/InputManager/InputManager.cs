//---------------------------------------------------------
// Contiene el componente de InputManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


/// <summary>
/// Manager para la gestión del Input. Se encarga de centralizar la gestión
/// de los controles del juego. Es un singleton que sobrevive entre
/// escenas.
/// La configuración de qué controles realizan qué acciones se hace a través
/// del asset llamado InputActionSettings que está en la carpeta Settings.
/// 
/// A modo de ejemplo, este InputManager tiene métodos para consultar
/// el estado de dos acciones:
/// - Move: Permite acceder a un Vector2D llamado MovementVector que representa
/// el estado de la acción Move (que se puede realizar con el joystick izquierdo
/// del gamepad, con los cursores...)
/// - Fire: Se proporcionan 3 métodos (FireIsPressed, FireWasPressedThisFrame
/// y FireWasReleasedThisFrame) para conocer el estado de la acción Fire (que se
/// puede realizar con la tecla Space, con el botón Sur del gamepad...)
///
/// Dependiendo de los botones que se quieran añadir, será necesario ampliar este
/// InputManager. Para ello:
/// - Revisar lo que se hace en Init para crear nuevas acciones
/// - Añadir nuevos métodos para acceder al estado que estemos interesados
///  
/// </summary>
public class InputManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Secuencia de teclas a presionar para activar los cheats
    /// </summary>
    [SerializeField] InputBinding[] _keyBindings;

    /// <summary>
    /// Secuencia de botones de mando para activar los cheats
    /// </summary>
    [SerializeField] InputBinding[] _gamepadBindings;

    /// <summary>
    /// Sonido para cuando se activa los cheats
    /// </summary>
    [SerializeField] AudioClip _onCheatEnable;

    /// <summary>
    /// Sonido para cuando se desactiva los cheats
    /// </summary>
    [SerializeField] AudioClip _onCheatDisable;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static InputManager _instance;

    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private @PlayerInputActions _theController;
    
    /// <summary>
    /// Acción para atacar.
    /// </summary>
    private InputAction _attack;
    /// <summary>
    /// Acción para saltar.
    /// </summary>
    private InputAction _jump;
    /// <summary>
    /// Acción para dash.
    /// </summary>
    private InputAction _dash;
    /// <summary>
    /// Acción para _manoDeLasSombras.
    /// </summary>
    private InputAction _manoDeLasSombras;
    /// <summary>
    /// Acción para _superDash.
    /// </summary>
    private InputAction _superDash;
    /// <summary>
    /// Acción para _pausar
    /// </summary>
    private InputAction _pause;
    /// <summary>
    /// Acción para despausar
    /// </summary>
    private InputAction _cancelPause;

    /// <summary>
    /// Evento de salto
    /// </summary>
    private UnityEvent _OnJumpStarted = new UnityEvent();
    /// <summary>
    /// Evento de pausa
    /// </summary>
    private UnityEvent _OnPausePressed = new UnityEvent();
    /// <summary>
    /// Evento de Despausar
    /// </summary>
    private UnityEvent _OnPauseCancel = new UnityEvent();

    /// <summary>
    /// Indice de la secuencia en el teclado
    /// </summary>
    private int _keySequenceIndex;

    /// <summary>
    /// Indice de la secuencia con el mando
    /// </summary>
    private int _gamepadSequenceIndex;

    /// <summary>
    /// Evento de invulnerabilidad
    /// </summary>
    [HideInInspector]
    public UnityEvent _invulnerabilityAction;

    /// <summary>
    /// Evento de invulnerabilidad
    /// </summary>
    [HideInInspector]
    public UnityEvent _skipWaveEvent;

    /// <summary>
    /// Evento de cambio de dispositivo de input
    /// </summary>
    [HideInInspector]
    public UnityEvent _deviceChange;

    /// <summary>
    /// Evento de cambio de dispositivo de input en el menu
    /// </summary>
    [HideInInspector]
    public UnityEvent _deviceChangeMenu;

    /// <summary>
    /// El dispositivo del input actual
    /// </summary>
    private InputDevice _device;

    /// <summary>
    /// Devuelve el dispositivo de input actual
    /// </summary>
    public InputDevice Device {  get { return _device; } }

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// 
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // InputManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Nos destruímos. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer InputManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
    } // Awake

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
    } // OnDestroy

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static InputManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    } // Instance

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>True si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Propiedad para acceder al direccion  de movimiento.
    /// Según está configurado el InputActionController,
    /// es un vector normalizado 
    /// </summary> 
    public float MoveDirection { get; private set; }

    /// <summary>
    /// Propiedad para acceder al vector de movimiento de camara.
    /// Según está configurado el InputActionController,
    /// es un vector normalizado 
    /// </summary> 
    public Vector2 MoveCamara { get; private set; }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool attackIsPressed()
    {
        return _attack.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool attackWasPressedThisFrame()
    {
        return _attack.WasPressedThisFrame();
    }
    public bool attackWasReleasedThisFrame()
    {
        return _attack.WasReleasedThisFrame();
    }
    public bool attackTriggered()
    {
        return _attack.triggered;
    }
    /// <summary>
    /// Devuelve si ha ha sido pulsado jump en este frame
    /// </summary>
    /// <returns>si ha ha sido pulsado jump en este frame</returns>
    public bool jumpWasPressedThisFrame()
    {
        return _jump.WasPressedThisFrame();
    }
    /// <summary>
    /// Devuelve true si ha ha sido pulsado dash en este frame
    /// </summary>
    /// <returns>si ha ha sido pulsado dash en este frame</returns>
    public bool DashIsPressed()
    {
        return _dash.IsPressed();
    }
    /// <summary>
    /// Activa el input del player
    /// </summary>
    public void EnablePlayerInput()
    {
        _theController.Player.Enable();
    }
    /// <summary>
    /// Desactivar el input del player
    /// </summary>
    public void DisablePlayerInput()
    {
        _theController.Player.Disable();
    }
    /// <summary>
    /// Activar el input del menu
    /// </summary>
    public void EnableMenuInput()
    {
        _theController.UI.Enable();
    }

    /// <summary>
    /// Desactiva el input del menu
    /// </summary>
    public void DisableMenuInput()
    {
        _theController.UI.Disable();
    }

    /// <summary>
    /// Devuelve si ha ha sido pulsado manoDeLasSombras en este frame
    /// </summary>
    public bool manoDeLasSombrasIsPressed()
    {
        return _manoDeLasSombras.IsPressed();
    }
    /// <summary>
    /// Devuelve si ha ha sido pulsado superDash en este frame
    /// </summary>
    public bool superDashIsPressed()
    {
        return _superDash.IsPressed();
    }

    /// <summary>
    /// Devuelve el inputactions
    /// </summary>
    public PlayerInputActions GetInputActions()
    {
        return _theController;
    }

    /// <summary>
    /// Suscribe un metodo al evento salto
    /// </summary>
    public void AddJumpStartedListener(UnityAction listener)
    {
        _OnJumpStarted.AddListener(listener);
    }
    /// <summary>
    /// Dessuscribe un metodo al evento salto
    /// </summary>
    public void RemoveJumpStartedListener(UnityAction listener)
    {
        _OnJumpStarted.RemoveListener(listener);
    }

    /// <summary>
    /// Suscribe un metodo al evento pausar
    /// </summary>
    public void AddPausePressedListener(UnityAction listener)
    {
        _OnPausePressed.AddListener(listener);
    }
    /// <summary>
    /// Dessuscribe un metodo al evento pausar
    /// </summary>
    public void RemovePausePressedListener(UnityAction listener)
    {
        _OnPausePressed.RemoveListener(listener);
    }
    /// <summary>
    /// Suscribe un metodo al evento despausar
    /// </summary>
    public void AddPauseCancelListener (UnityAction listener)
    {
        _OnPauseCancel.AddListener(listener);
    }
    /// <summary>
    /// Dessuscribe un metodo al evento despausar
    /// </summary>
    public void RemovePauseCancelListener(UnityAction listener)
    {
        _OnPauseCancel.RemoveListener(listener);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // Creamos el controlador del input y activamos los controles del jugador
        _theController = new PlayerInputActions();

        // Cacheamos la acción de movimiento
        InputAction _movement = _theController.Player.Move;
        InputAction _moveCamara = _theController.Player.MoveCamera;

        _movement.performed += ctx => MoveDirection = ctx.ReadValue<float>();
        _movement.canceled += ctx => MoveDirection = ctx.ReadValue<float>();
        _moveCamara.performed += ctx => MoveCamara = ctx.ReadValue<Vector2>();
        _moveCamara.canceled += ctx => MoveCamara = Vector2.zero;

        _attack = _theController.Player.Attack;
        _jump = _theController.Player.Jump;
        _dash = _theController.Player.Dash;
        _manoDeLasSombras = _theController.Player.ManoDeLasSombras;
        _superDash = _theController.Player.SuperDash;
        _pause = _theController.Player.Menu;
        _cancelPause = _theController.UI.Cancel;
    

        _jump.started += ctx => _OnJumpStarted?.Invoke();
        _pause.performed += ctx => _OnPausePressed?.Invoke();
        _cancelPause.performed += ctx => _OnPauseCancel?.Invoke();



        //Me da perza crear un InputAction para cada uno

        //Cuando se ejecuta la accion de invulnerabilidad, ejecuta el metodo _invulnerability_performed
        _theController.Cheats.Invulnerability.performed += _invulnerability_performed;

        //Cuando se ejecuta la accion de saltar el wave, ejecuta el metodo
        _theController.Cheats.SkipWave.performed += SkipWave_performed;

        //Cuando se ejecuta la accion de ir al siguiente Checkpoint, ejecuta el metodo
        _theController.Cheats.NextCheckpoint.performed += NextCheckpoint_performed;

        //Cuando se ejecuta la accion de ir al siguiente nivel, ejecuta el metodo 
        _theController.Cheats.NextLevel.performed += NextLevel_performed;

        _theController.UI.KeySequence.performed += KeySequence_performed;

        _theController.UI.ControllerSequence.performed += ControllerSequence_performed;

        //Cuando realiza cualquier accion, ejecuta el metodo Callback_performed
        //Se deberia poder hacerlo con el contexto general del InputAction.CallbackContext, pero no se como cogerlo desde aqui
        _theController.Player.Callback.performed += PlayerCallback_performed;

        _theController.UI.Callback.performed += PlayerCallback_performed;

        //Inicializar el dispositivo de input
        _device = null;


        // Para el disparo solo cacheamos la acción de disparo.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (FireIsPressed, FireWasPressedThisFrame 
        // y FireWasReleasedThisFrame)
        //_fire = _theController.Player.Fire;
    }

    /// <summary>
    /// Metodo para realizar cualquier cosa siempre y cuando tenga que ver con un cambio de dispositivo
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerCallback_performed(InputAction.CallbackContext obj)
    {
        if (DeviceChange(obj))
        {
            _deviceChange.Invoke();
        }
    }

    /// <summary>
    /// Metodo para detectar la secuencia de los botones del mando
    /// </summary>
    /// <param name="obj"></param>
    private void ControllerSequence_performed(InputAction.CallbackContext obj)
    {
        //Si el boton pulsado coincide con el de la secuencia
        if (obj.control.name == _gamepadBindings[_gamepadSequenceIndex].name)
        {
            _gamepadSequenceIndex++;

            Debug.Log("Current index: " + _gamepadSequenceIndex);

            //Si se ha hecho toda la secuencia, activa o desactiva el cheats
            if (_gamepadSequenceIndex == _gamepadBindings.Length)
            {
                //Si no esta activado
                if (!_theController.Cheats.enabled)
                {
                    _theController.Cheats.Enable();
                    SoundManager.Instance.PlaySFX(_onCheatEnable, transform, 1);
                }
                else
                {
                    _theController.Cheats.Disable();
                    SoundManager.Instance.PlaySFX(_onCheatDisable, transform, 1);
                }
                Debug.Log("Cheats: " + _theController.Cheats.enabled);

                //Resetear la secuencia
                _gamepadSequenceIndex = 0;
            }
        }
        //Resetear la secuencia
        else
        {
            _gamepadSequenceIndex = 0;
        }
    }
    /// <summary>
    /// Metodo para detectar secuencia de tecla para activar los cheats
    /// </summary>
    /// <param name="obj"></param>
    private void KeySequence_performed(InputAction.CallbackContext obj)
    {

        if (obj.control.name == _keyBindings[_keySequenceIndex].name)
        {
            _keySequenceIndex++;

            if(_keySequenceIndex == _keyBindings.Length)
            {
                if (!_theController.Cheats.enabled)
                {
                    _theController.Cheats.Enable();
                    SoundManager.Instance.PlaySFX(_onCheatEnable, transform, 1);
                }
                else
                {
                    _theController.Cheats.Disable();
                    SoundManager.Instance.PlaySFX(_onCheatDisable, transform, 1);
                }

                _keySequenceIndex = 0;
            }
        }
        else
        {
            _keySequenceIndex = 0;
        }

    }

    /// <summary>
    /// Metodo subscrito a la accion de ir al siguiente nivel
    /// </summary>
    /// <param name="obj"></param>
    private void NextLevel_performed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.NextLevel();
    }
    /// <summary>
    /// Metodo subscrito a la accion de ir al siguiente checkpoint
    /// </summary>
    /// <param name="obj"></param>
    private void NextCheckpoint_performed(InputAction.CallbackContext obj)
    {
        CheckpointManager.Instance.NextCheckpoint();
    }

    /// <summary>
    /// Metodo subscrito a la accion de saltar zona de combate
    /// </summary>
    /// <param name="obj"></param>
    private void SkipWave_performed(InputAction.CallbackContext obj)
    {
        _skipWaveEvent?.Invoke();
    }

    /// <summary>
    /// Metodo "subscrito" a la accion de invulnerabilidad
    /// </summary>
    /// <param name="obj"></param>
    private void _invulnerability_performed(InputAction.CallbackContext obj)
    {
        _invulnerabilityAction?.Invoke();
    }

    /// <summary>
    /// Método que es llamado por el controlador de input cuando se producen
    /// eventos de movimiento (relacionados con la acción Move)
    /// </summary>
    /// <param name="context">Información sobre el evento de movimiento</param>
    /*private void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<float>();
    }*/

    private bool DeviceChange(InputAction.CallbackContext obj)
    {
        if (obj.control.device != _device)
        {
            _device = obj.control.device;
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
} // class InputManager 
// namespace