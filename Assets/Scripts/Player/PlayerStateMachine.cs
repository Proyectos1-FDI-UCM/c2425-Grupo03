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
[RequireComponent(typeof(Rigidbody2D))]
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

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    /// <summary>
    /// <para>Dirección en la que mira el jugador.</para>
    /// <para>Right = 1, Left = -1.</para>
    /// Puedes hacer <c>(short)LookingDirection</c> para obtener el valor 1 o -1 directamente.
    /// </summary>
    public PlayerLookingDirection LookingDirection { get; set; }

    /// <summary>
    /// Rigidbody2D del jugador.
    /// </summary>
    public Rigidbody2D Rigidbody { get; private set; }

    /// <summary>
    /// El input actions del jugador.
    /// </summary>
    public PlayerInputActions.PlayerActions PlayerInput { get; private set; }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>

    /// <summary>
    /// 
    /// </summary>
    protected override void OnAwake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();

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
