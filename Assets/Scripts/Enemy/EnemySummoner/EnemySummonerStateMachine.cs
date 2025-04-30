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
/// Máquina de estados del enemigo invocador donde se contiene el contexto de todos los estados.
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


    /// <summary>
    /// Sonido reproducido al dañar al enemigo
    /// </summary>
    [SerializeField] AudioClip _enemyDamaged;


    [SerializeField] GameObject _spellVFX;
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
    /// El Transform del jugador. 
    /// </summary>
    public Transform PlayerTransform { get; set; }

    /// <summary>
    /// El array de los transforms de spawnpoints.
    /// </summary>
    public Transform[] Spawnpoints { get; private set; }

    /// <summary>
    /// punto de instanciacion de la bala
    /// </summary>
    public Transform CastPoint { get; private set; }

    /// <summary>
    /// Variable para saber cuando el jugador entra en la distancia de detección.
    /// </summary>
    public bool IsPlayerInAttackRange { get; set; }



    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    /// <summary>
    /// Reproduce el sonido de ser dañado del enemigo.
    /// </summary>
    /// <param name="damageAmount">No se usa este parámetro.</param>
    public void EnemyDamagedSFX(float damageAmount)
    {
        SoundManager.Instance.PlaySFX(_enemyDamaged, transform, 1);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    /// <summary>
    /// Método llamado en el awake
    /// </summary>
    protected override void OnAwake()
    {
        // Coge el sprite renderer
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Coge todos los spawn points para invocar
        _allSpawnpoints = transform.parent.GetChild(1);
        Spawnpoints = _allSpawnpoints.GetComponentsInChildren<Transform>();


        //Coge el InstancePoint

        CastPoint = transform.GetChild(0).GetChild(0);
    }
    /// <summary>
    /// Método llamado en el start
    /// </summary>
    protected override void OnStart()
    {
        HealthManager hm = GetComponent<HealthManager>();
        if (hm != null)
        {
            //Se subscribe a los eventos de muerte y daño para ejecutar los métodos
            hm._onDeath.AddListener(DeathState);
            hm._onDamaged.AddListener(TPState);
            hm._onDamaged.AddListener(EnemyDamagedSFX);
        }
    }

    /// <summary>
    /// Forzar el cambio de estado a muerte
    /// </summary>
    public void DeathState()
    {
        ChangeState(gameObject.GetComponentInChildren<EnemySummonerDeathState>());
    }

    /// <summary>
    /// Si es el primer golpe recibido, cambia al estado de TP
    /// </summary>
    /// <param name="removedHealth"></param>
    public void TPState(float removedHealth)
    {
        if (_isFirstHit && GetComponent<HealthManager>()?.Health > 0) 
        {
            //Si es el primer golpe y todavía está vivo se teletransporta
            ChangeState(gameObject.GetComponentInChildren<EnemyTPState>());

            // Los siguientes golpes ya no serán los primeros
            _isFirstHit = false;
            GetComponent<Animator>()?.SetBool("IsKnockedBack", false);
        }
    }

    /// <summary>
    /// Actualizar la direccion a la que mira el invocador
    /// </summary>
    public void UpdateLookingDirection()
    {
        //Actualizamos la dirección en la que mira el enemigo en función de la posición respecto al jugador
        LookingDirection = (PlayerTransform.position.x -transform.position.x) > 0 ?
        EnemySummonerStateMachine.EnemyLookingDirection.Left : EnemySummonerStateMachine.EnemyLookingDirection.Right;

        //se escala en x para cambiar hijos de lado
        if (LookingDirection == EnemySummonerStateMachine.EnemyLookingDirection.Left)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void CastSpell()
    {
        if (CastPoint != null)
        {
             Instantiate(_spellVFX,CastPoint);
        }
    }


    #endregion

} // class EnemyStateMachine 
// namespace
