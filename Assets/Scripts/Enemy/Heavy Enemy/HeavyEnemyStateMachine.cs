//---------------------------------------------------------
// La máquina de estado del enemigo pesado
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Máquina de estados del enemigo pesado donde se contiene el contexto de todos los estados.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]// Obliga que el GameObject que contenga a este componente tenga un Rigibody2D

// Obliga que tenga el componente HealthManager
[RequireComponent(typeof(HealthManager))]

[SelectionBase] // Hace que cuando selecciones el objeto desde el editor se seleccione el que tenga este componente automáticamente
public class HeavyEnemyStateMachine : StateMachine
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
    [SerializeField] AudioClip _blockSound;

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

    public bool IsPlayerInAttackRange { get; set; }

    /// <summary>
    /// El Transform del jugador. 
    /// </summary>
    public Transform PlayerTransform { get; set; }


    /// <summary>
    /// El rango de ataque del enemigo
    /// </summary>
    public float AttackDistance { get; set; }


    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// El health manager del enemigo pesado
    /// </summary>
    private HealthManager _healthManager;

    private bool _isMoving = false;
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>

    /// <summary>
    /// Compruebna si el jugador esta en su espalda
    /// </summary>
    /// <returns></returns>
    public bool CanTakeDamage()
    {
        bool canTakeDamage = false;

        /* 
         if (PlayerTransform != null)
         {
             PlayerStateMachine player = PlayerTransform.GetComponent<PlayerStateMachine>();
             canTakeDamage = (short)LookingDirection == (short)player.LookingDirection;
         }
        */

        if (PlayerTransform != null)
        {
            Vector2 directionToPlayer = (PlayerTransform.position - transform.position).normalized;
            Vector2 enemyLookingDirection = new Vector2((short)LookingDirection, 0);
            float dotProduct = Vector2.Dot(enemyLookingDirection, directionToPlayer); //si apuntan a direcciones contrarias, canTakeDamage es true

            float distanceToPlayer = Vector2.Distance(PlayerTransform.position, transform.position);
            canTakeDamage = dotProduct < 0 && distanceToPlayer >= 0.2;
        }
        return canTakeDamage; 
        
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos

    protected override void OnAwake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnStart()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager?._onDeath.AddListener(DeathState);
        _healthManager?._onInmune.AddListener(PlayBlockSound);
        IsPlayerInAttackRange = false;
        IsPlayerInChaseRange = false;
    }

    protected override void OnUpdate()
    {
        if (_healthManager != null)
        {
            _healthManager.Inmune = !CanTakeDamage();    
        }
        if (PlayerTransform != null)
        {
            IsPlayerInAttackRange = (PlayerTransform.position - transform.position).magnitude < AttackDistance;
        }
    }

    public void PlayBlockSound()
    {
        if (!CanTakeDamage())
        SoundManager.Instance.PlaySFX(_blockSound, transform, 0.3f);
    }

    /// <summary>
    /// Forzar el cambio de estado a muerte
    /// </summary>
    public void DeathState()
    {
        ChangeState(gameObject.GetComponentInChildren<HeavyEnemyDeathState>());
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
    public void CanMove()
    {
        _isMoving = true;
    }
    public void CantMove()
    {
        _isMoving = false;
    }
    #endregion

} // class EnemyStateMachine 
// namespace
