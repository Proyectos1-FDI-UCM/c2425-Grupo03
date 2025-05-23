//---------------------------------------------------------
// La máquina de estado del jefe final del juego
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

// IMPORTANTE: No uses los métodos del MonoBehaviour: Awake(), Start(), Update, etc. (NINGUNO)

using UnityEngine;


/// <summary>
/// La máquina de estados del jefe final del juego.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossStateMachine : StateMachine
{
    public enum EnemyLookingDirection
    {
        Rigth = 1,
        Left = -1,
    }

    // ---- ATRIBUTOS EN EL INSPECTOR ----
    #region Atributos del Inspector 
    [SerializeField] AudioClip _changeStateHowl;
    [SerializeField] AudioClip[] _damaged;
    [SerializeField] AudioClip _block;
    [SerializeField] AudioClip _wind;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// El HealthManager del boss
    /// </summary>
    HealthManager _healthManager;

    /// <summary>
    /// Dirección en la que está mirando el boss
    /// </summary>
    private EnemyLookingDirection _enemyLookingDirection = EnemyLookingDirection.Left;
    private AudioSource _audioSource;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    /// <summary>
    /// Dirección en la que está mirando el boss
    /// </summary>
    public EnemyLookingDirection LookingDirection 
    {
        get
        {
            return _enemyLookingDirection;
        }
        set
        {
            _enemyLookingDirection = value;
            // Invierte el scale del boss en función de hacia dónde mire
            transform.localScale = new Vector3((float)value, 1, 1);
        } 
    }
    
    /// <summary>
    /// Referencia al jugador. La establece la fase de idle.
    /// </summary>
    public PlayerStateMachine Player { get; set; }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Código para cambiar a la segunda fase
    /// </summary>
    /// <param name="damage"></param>
    public void StartPhase2(float damage)
    {
        // Queremos que solo se ejecute cuando tenga menos de la mitad de su vida
        if (_healthManager.Health > _healthManager.MaxHealth / 2)
            return;

        _healthManager.Inmune = true;

        // Ya no queremos volver a comprobar esto lo desubscribimos
        _healthManager._onDamaged.RemoveListener(StartPhase2);

        // Cambiamos al estado de transición
        ChangeState(GetStateByName("Transition"));

        _audioSource = SoundManager.Instance.PlaySFXWithAudioSource(_wind, transform, 0.5f);
    }

    public void BlockDamageSFX()
    {
        SoundManager.Instance.PlaySFX(_block, transform, 0.5f);   
    }
    public void PlayChangeStateHowl()
    {
        SoundManager.Instance.PlaySFX(_changeStateHowl, transform, 1);
    }

    public void PlayDamagedSFX(float damage)
    {
        SoundManager.Instance.PlayRandomSFX(_damaged,transform, 1);
    }

    public void StopWindSFX()
    {
        _audioSource?.Stop();
        Destroy(_audioSource);
    }

    private void OverradeUpdate()
    {
        if (_audioSource != null)
        {
            if (Time.timeScale == 0)
            {
                _audioSource?.Pause();
            }
            else if (Time.timeScale != 0)
            {
                _audioSource?.UnPause();
            }
        }

    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    protected override void OnStart()
    {
        if(TryGetComponent(out _healthManager))
        {
            // Setup del healthmanager
            _healthManager.Inmune = true;
            _healthManager.CanBeKnockbacked = false;
            _healthManager._onDamaged.AddListener(PlayDamagedSFX);
            _healthManager._onDamaged.AddListener(StartPhase2);
            _healthManager._onDeath.AddListener(() => { ChangeState(GetStateByName("Death")); });
            _healthManager._onDeath.AddListener(StopWindSFX);
            _healthManager._onInmune.AddListener(BlockDamageSFX);
        }


    }
    #endregion   

} // class BossStateMachine 
// namespace
