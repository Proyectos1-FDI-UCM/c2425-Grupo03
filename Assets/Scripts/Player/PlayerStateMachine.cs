//---------------------------------------------------------
// Máquina de estados del jugador. Contiene el contexto para todos los estados
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

// IMPORTANTE: No uses los métodos del MonoBehaviour: Awake(), Start(), Update, etc. (NINGUNO)

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Máquina de estados del jugador donde se contiene el contexto de todos los estados.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] //Obliga que el GameObject que contenga a este componente tenga un Rigibody2D
[SelectionBase] //Hace que cuando selecciones el objeto desde el editor se seleccione el que tenga este componente automáticamente
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
    /// <para>Getter de <paramref name="_gravityScale"/> de solo lectura.</para>
    /// <returns>Devuelve el valor de <paramref name="_gravityScale"/>.</returns>
    /// </summary>
    public float GravityScale => _gravityScale;

    /// <summary>
    /// El input actions del jugador.
    /// </summary>
    public PlayerInputActions.PlayerActions PlayerInput { get; private set; }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>

    /// <summary>
    /// Establece los valores iniciales en Awake.
    /// </summary>
    protected override void OnAwake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _gravityScale = Rigidbody.gravityScale;

        PlayerInput = new PlayerInputActions().Player;
        PlayerInput.Enable();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí

    #endregion

} // class PlayerStateMachine 


// namespace
