//---------------------------------------------------------
// Máquina de estados de los enemigos. Contiene el contexto para todos los estados
// Alexandra Lenta
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

// IMPORTANTE: No uses los métodos del MonoBehaviour: Awake(), Start(), Update, etc. (NINGUNO)

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Máquina de estados del jugador donde se contiene el contexto de todos los estados.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]// Obliga que el GameObject que contenga a este componente tenga un Rigibody2D

// Obliga que tenga el componente HealthManager
[RequireComponent(typeof(HealthManager))]
[SelectionBase] // Hace que cuando selecciones el objeto desde el editor se seleccione el que tenga este componente automáticamente
public class EnemySummonerStateMachine : StateMachine
{
    /// <summary>
    /// <para>
    /// Codifica las dos formas en las que puede mirar el enemigo.
    /// </para>
    /// <remarks> Right = 1, Left = -1 </remarks>
    /// </summary>
    public enum EnemyLookingDirection : short
    {
        Right = 1,
        Left = -1,
    }

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Bool para determinar si el enemigo ha sido dañado o no.
    /// </summary>
    private bool _isFirstHit = true;
    /// <summary>
    /// El transform de la carpeta de Spawnpoints. 
    /// </summary>
    private Transform _allSpawnpoints;
    /// <summary>
    /// El numero de spawnpoints.
    /// </summary>
    static private int _spawnpointCount = 3;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    /// <summary>
    /// <para>Dirección en la que mira el enemigo.</para>
    /// <para>Right = 1, Left = -1.</para>
    /// Puedes hacer <c>(short)LookingDirection</c> para obtener el valor 1 o -1 directamente.
    /// </summary>
    public EnemyLookingDirection LookingDirection { get; set; } = EnemyLookingDirection.Right;

    /// <summary>
    /// Rigidbody2D del enemigo.
    /// </summary>
    public Rigidbody2D Rigidbody { get; private set; }

    /// <summary>
    /// SpriteRenderer del enemigo.
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; private set; }

    /// <summary>
    /// El Transform del jugador. 
    /// </summary>
    public Transform PlayerTransform { get; set; }

    /// <summary>
    /// El array de los transforms de spawnpoints.
    /// </summary>
    public Transform[] Spawnpoints { get; private set; }

    /// <summary>
    /// Variable para saber cuando el jugador entra en la distancia de detección.
    /// </summary>
    public bool IsPlayerInAttackRange { get; set; }

    /// <summary>
    /// El rango de ataque del enemigo
    /// </summary>
    public float AttackDistance { get; set; }

    /// <summary>
    /// La vida del enemigo
    /// </summary>
    public float Health { get; set; }

    /// <summary>
    /// El animator del enemigo.
    /// </summary>
    public Animator Animator { get; private set; }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos

    protected override void OnAwake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _allSpawnpoints = transform.parent.GetChild(1);
        Spawnpoints = _allSpawnpoints.GetComponentsInChildren<Transform>();
    }

    protected override void OnStart()
    {
        GetComponent<HealthManager>()._onDeath.AddListener(DeathState);
        GetComponent<HealthManager>()._onDamaged.AddListener(TPState);
    }

    /// <summary>
    /// Forzar el cambio de estado a muerte
    /// </summary>
    public void DeathState()
    {
        ChangeState(gameObject.GetComponentInChildren<EnemySummonerDeathState>());
    }

    public void TPState(int removedHealth)
    {
        if (_isFirstHit) {
            ChangeState(gameObject.GetComponentInChildren<EnemyTPState>());
            _isFirstHit = false;
        }
    }

    #endregion

} // class EnemyStateMachine 
// namespace
