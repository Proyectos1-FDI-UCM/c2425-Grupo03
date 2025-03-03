//---------------------------------------------------------
// Máquina de estados del jugador. Contiene el contexto para todos los estados
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

// IMPORTANTE: No uses los métodos del MonoBehaviour: Awake(), Start(), Update, etc. (NINGUNO)

using UnityEngine;
using UnityEngine.Rendering;
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

    /// <summary>
    /// Rigidbody2D del jugador.
    /// </summary>
    public Rigidbody2D Rigidbody { get; private set; }

    /// <summary>
    /// El sprite renderer del jugador.
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; private set; }

    /// <summary>
    /// <para>Getter de <paramref name="_gravityScale"/> de solo lectura.</para>
    /// <returns>Devuelve el valor de <paramref name="_gravityScale"/>.</returns>
    /// </summary>
    public float GravityScale => _gravityScale;

    public Animator Animator { get; private set; }

    /// <summary>
    /// El input actions del jugador.
    /// </summary>
    public PlayerInputActions.PlayerActions PlayerInput { get; private set; }

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
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí

    /// <summary>
    /// Establece los valores iniciales en Awake.
    /// </summary>
    protected override void OnAwake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _gravityScale = Rigidbody.gravityScale;

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        PlayerInput = new PlayerInputActions().Player;
        PlayerInput.Enable();

        Animator = GetComponent<Animator>();
    }
    protected override void OnStart()
    {
        GetComponent<HealthManager>()._onDeath.AddListener(DeathState);
    }

    #endregion

} // class PlayerStateMachine 


// namespace
