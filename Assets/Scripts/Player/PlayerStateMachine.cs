//---------------------------------------------------------
// Máquina de estados del jugador. Contiene el contexto para todos los estados
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

// IMPORTANTE: No uses los métodos del MonoBehaviour: Awake(), Start(), Update, etc. (NINGUNO)

using UnityEngine;
using UnityEngine.Events;
// Añadir aquí el resto de directivas using


/// <summary>
/// Máquina de estados del jugador donde se contiene el contexto de todos los estados.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] // Obliga que el GameObject que contenga a este componente tenga un Rigibody2D
[RequireComponent(typeof(Animator))] // Obliga que el GameObject que contenga a este componente tenga un Animator

// Obliga que tenga el componente HealthManager
[RequireComponent(typeof(HealthManager))]
[SelectionBase] // Hace que cuando selecciones el objeto desde el editor se seleccione el que tenga este componente automáticamente
public class PlayerStateMachine : StateMachine
{
    /// <summary>
    /// <para>
    /// Codifica las dos formas en las que puede mirar el jugador.
    /// </para>
    /// <remarks> Right = 1, Left = -1 </remarks>
    /// </summary>
    public enum PlayerLookingDirection : short
    {
        Right = 1,
        Left = -1,
    }

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    [SerializeField] AudioClip[] _playerDamaged;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    /// <summary>
    /// <para>La gravedad del Rigidbody.</para>
    /// Se usa para saber devolver el valor inicial de la gravedad al Rigidbody cuando se cambia.
    /// </summary>
    private float _gravityScale;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    /// <summary>
    /// <para>Dirección en la que mira el jugador.</para>
    /// <para>Right = 1, Left = -1.</para>
    /// Puedes hacer <c>(short)LookingDirection</c> para obtener el valor 1 o -1 directamente.
    /// </summary>
    public PlayerLookingDirection LookingDirection { get; set; } = PlayerLookingDirection.Left;

    ///comprueba si el jugador ya ha atacado en el aire durante este salto

    public bool AttackedOnAir { get; set; } = false;

    /// <summary>
    /// El sprite renderer del jugador.
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; private set; }

    /// <summary>
    /// <para>Getter de <paramref name="_gravityScale"/> de solo lectura.</para>
    /// <returns>Devuelve el valor de <paramref name="_gravityScale"/>.</returns>
    /// </summary>
    public float GravityScale => _gravityScale;



    /// <summary>
    /// El AudioSource que tiene el sonido de los pasos del jugador.
    /// </summary>
    public AudioSource PlayerAudio { get; private set; }

    /// <summary>
    /// Evento para cuando ataca el jugador
    /// </summary>
    private UnityEvent OnAttack { get;  set; }

    /// <summary>
    /// Evento para cuando ataca el jugador
    /// </summary>
    private UnityEvent OnAirAttack { get; set; }
    /// <summary>
    /// Evento para cuando el jugador hace un ataque cargado
    /// </summary>
    private UnityEvent OnChargedAttack { get;  set; }

    /// <summary>
    /// Evento llamado cuando los esqueletos son empujados por la habiliadad de Mano de las Sombras
    /// </summary>
    private UnityEvent OnManoSombrasPush { get; set; }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Forzar el cambio de estado a muerte
    /// </summary>
    public void DeathState()
    {
        ChangeState(gameObject.GetComponentInChildren<PlayerDeathState>());
    }

    /// <summary>
    /// Reproduce sonido de hacerse daño del jugador
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerDamagedSFX(float damage)
    {
        SoundManager.Instance.PlayRandomSFX(_playerDamaged, transform, 0.8f);
    }
    #region Subscripciones e invocaciones de eventos de animación
    /// <summary>
    /// Método que se invoca desde el animator cuando el evento de ataque comienza en una animación
    /// </summary>
    public void OnPlayerAttack()
    {
        OnAttack.Invoke();
    }

    public void OnPlayerAirAttack()
    {
        OnAirAttack.Invoke();
    }

    /// <summary>
    /// Método que se invoca desde el animator cuando el evento de ataque cargado comienza en una animación
    /// </summary>
    public void OnPlayerChargedAttack()
    {
        OnChargedAttack.Invoke();
    }
    /// <summary>
    /// Método que se invoca desde el animator cuando el evento de ataque cargado comienza en una animación
    /// </summary>
    public void OnPlayerManoSombrasPush()
    {
        OnManoSombrasPush.Invoke();
    }

    /// <summary>
    /// Método que permite subscribir otro método al evento OnAttack. Así, el evento queda totalmente protegido.
    /// </summary>
    /// <param name="action"></param>
    public void OnAttackAddListener(UnityAction action)
    {
        OnAttack.AddListener(action);
    }

    public void OnAirAttackAddListener(UnityAction action)
    {
        OnAirAttack.AddListener(action);
    }

    /// <summary>
    /// Método que permite subscribir otro método al evento OnAttack. Así, el evento queda totalmente protegido.
    /// </summary>
    /// <param name="action"></param>
    public void OnChargedAttackAddListener(UnityAction action)
    {
        OnChargedAttack.AddListener(action);
    }

    /// <summary>
    /// Método que permite subscribir otro método al evento OnAttack. Así, el evento queda totalmente protegido.
    /// </summary>
    /// <param name="action"></param>
    public void OnManoSombrasPushAddListener(UnityAction action)
    {
        OnManoSombrasPush.AddListener(action);
    }
    #endregion

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí

    /// <summary>
    /// Establece los valores iniciales en Awake.
    /// </summary>
    protected override void OnAwake()
    {
        OnAttack = new UnityEvent();
        OnChargedAttack = new UnityEvent();
        OnManoSombrasPush = new UnityEvent();
        OnAirAttack = new UnityEvent();

        _gravityScale = Rigidbody.gravityScale;

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        PlayerAudio = GetComponent<AudioSource>();
    }
    protected override void OnStart()
    {
        InputManager.Instance.EnablePlayerInput();

        HealthManager healthManager = GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager._onDeath.AddListener(DeathState);
            healthManager._onDamaged.AddListener(PlayerDamagedSFX);
        }

        if (GameManager.Instance.GetCheckpoint()!= null)
        {
            transform.position = (Vector3) GameManager.Instance.GetCheckpoint();
        }
    }



    #endregion

} // class PlayerStateMachine 


// namespace
