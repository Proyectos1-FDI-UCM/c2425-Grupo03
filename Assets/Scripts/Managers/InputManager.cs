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
    /// Acción para Fire.
    /// </summary>
    private InputAction _attack;
    /// <summary>
    /// Acción para Fire.
    /// </summary>
    private InputAction _jump;
    /// <summary>
    /// Acción para Fire.
    /// </summary>
    private InputAction _dash;
    /// <summary>
    /// Acción para Fire.
    /// </summary>
    private InputAction _manoDeLasSombras;
    /// <summary>
    /// Acción para Fire.
    /// </summary>
    private InputAction _superDash;

    private InputAction _pause;

    private InputAction _cancelPause;

    private UnityEvent _OnJumpStarted = new UnityEvent();
    private UnityEvent _OnPausePressed = new UnityEvent();
    private UnityEvent _OnPauseCancel = new UnityEvent();

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
    /// Propiedad para acceder al vector de movimiento.
    /// Según está configurado el InputActionController,
    /// es un vector normalizado 
    /// </summary>
    public float MoveDirection { get; private set; }

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
    /// Método para saber si el botón de disparo (Fire) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool jumpIsPressed()
    {
        return _jump.IsPressed();
    }
    public bool jumpWasPressedThisFrame()
    {
        return _jump.WasPressedThisFrame();
    }
    public bool DashIsPressed()
    {
        return _dash.IsPressed();
    }
    public void EnablePlayerInput()
    {
        _theController.Player.Enable();
    }
    public void DisablePlayerInput()
    {
        _theController.Player.Disable();
    }

    public void EnableMenuInput()
    {
        _theController.UI.Enable();
    }

    public void DisableMenuInput()
    {
        _theController.UI.Disable();
    }
    public bool manoDeLasSombrasIsPressed()
    {
        return _manoDeLasSombras.IsPressed();
    }

    public bool superDashIsPressed()
    {
        return _superDash.IsPressed();
    }

    public PlayerInputActions GetInputActions()
    {
        return _theController;
    }

    public void AddJumpStartedListener(UnityAction listener)
    {
        _OnJumpStarted.AddListener(listener);
    }
    public void RemoveJumpStartedListener(UnityAction listener)
    {
        _OnJumpStarted.RemoveListener(listener);
    }
    public void AddPausePressedListener(UnityAction listener)
    {
        _OnPausePressed.AddListener(listener);
    }

    public void RemovePausePressedListener(UnityAction listener)
    {
        _OnPausePressed.RemoveListener(listener);
    }

    public void AddPauseCancelListener (UnityAction listener)
    {
        _OnPauseCancel.AddListener(listener);
    }

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
        InputAction movement = _theController.Player.Move;
        // Para el movimiento, actualizamos el vector de movimiento usando
        // el método OnMove
        movement.performed += ctx => MoveDirection = ctx.ReadValue<float>();
        movement.canceled += ctx => MoveDirection = ctx.ReadValue<float>();

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

        // Para el disparo solo cacheamos la acción de disparo.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (FireIsPressed, FireWasPressedThisFrame 
        // y FireWasReleasedThisFrame)
        //_fire = _theController.Player.Fire;
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

    #endregion
} // class InputManager 
// namespace