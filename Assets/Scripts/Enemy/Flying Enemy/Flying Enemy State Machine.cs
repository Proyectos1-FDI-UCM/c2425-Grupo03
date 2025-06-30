//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
using UnityEngine.Events;
// Añadir aquí el resto de directivas using


/// <summary>
/// Máquina de estados del enemigo donde se contiene el contexto de todos los estados.
/// </summary>

// Obliga que el GameObject que contenga a este componente tenga un Rigibody2D
[RequireComponent(typeof(Rigidbody2D))]

// Obliga que tenga el componente HealthManager
[RequireComponent(typeof(HealthManager))]

// Hace que cuando selecciones el objeto desde el editor se seleccione el que tenga este componente automáticamente
[SelectionBase]

public class FlyingEnemyStateMachine : StateMachine
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
    /// <summary>
    /// El sonido reproducido al ser golpeado
    /// </summary>
    [SerializeField]
    AudioClip _enemyDamaged;

    [SerializeField, Range(0, 1)] private float _orbdropchance;

    [SerializeField] GameObject _MagicOrb;
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
    /// SpriteRenderer del enemigo.
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; private set; }

    /// <summary>
    /// Variable para saber cuando el jugador entra en la distancia de detección.
    /// </summary>
    public bool IsPlayerInChaseRange { get; set; }

    /// <summary>
    /// El Transform del jugador. 
    /// </summary>
    public Transform PlayerTransform { get; set; }


    /// <summary>
    /// El rango de ataque del enemigo
    /// </summary>
    public float AttackDistance { get; set; }

    #endregion

    // ---- ATRIBUTOS PRIVADOS ---
    #region Atributos Privados

    /// <summary>
    /// Evento que se llama cuando el enemigo realiza el ataque
    /// </summary>
    private UnityEvent _onAttack;

    #endregion

    // ---- MÉTODOS PUBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Método llamado desde un animation event de cuando el enemigo ataca
    /// </summary>
    public void OnEnemyAttack()
    {
        _onAttack?.Invoke();
    }

    /// <summary>
    /// Método para subscribir otros métodos al evento de ataque y mantener el evento protegido
    /// </summary>
    /// <param name="action">El método a subscribir</param>
    public void OnEnemyAttackAddListener(UnityAction action)
    {
        _onAttack.AddListener(action);
    }

    #endregion 

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos

    /// <summary>
    /// Este metodo se llama en el Awake
    /// </summary>
    protected override void OnAwake()
    {
        // Coge el sprite renderer (para invertirlo al girarse)
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _onAttack = new UnityEvent();
    }

    protected override void OnStart()
    {
        HealthManager hm = GetComponent<HealthManager>();
        if (hm != null)
        {
            // Subscribe unos métodos a la muerte y al daño para que se llamen
            hm._onDeath.AddListener(DeathState);
            hm._onDamaged.AddListener(EnemyDamagedSFX);
        }
    }

    /// <summary>
    /// Reproduce un sonido de daño.
    /// (Usado para subscribirse al evento de daño)
    /// </summary>
    /// <param name="damageAmount">No sirve para nada, son los argumentos obligatorios del evento.</param>
    public void EnemyDamagedSFX(float damageAmount)
    {
        SoundManager.Instance.PlaySFX(_enemyDamaged, transform, 1);
    }
    /// <summary>
    /// Forzar el cambio de estado a muerte
    /// </summary>
    public void DeathState()
    {
        if (_orbdropchance >= Random.Range(0.001f, 1f)) Instantiate(_MagicOrb, transform.position, Quaternion.identity);
        ChangeState(gameObject.GetComponentInChildren<FlyingEnemyDeathState>());
    }

    #endregion

} // class FlyingEnemyStateMachine 
// namespace
