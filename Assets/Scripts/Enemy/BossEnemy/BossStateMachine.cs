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

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    bool _isOnPhase2 = false;

    HealthManager _healthManager;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    private EnemyLookingDirection _enemyLookingDirection = EnemyLookingDirection.Left;
    public EnemyLookingDirection LookingDirection 
    {
        get
        {
            return _enemyLookingDirection;
        }
        set
        {
            _enemyLookingDirection = value;
            transform.localScale = new Vector3((float)value, 1, 1);
        } 
    }

    public PlayerStateMachine Player { get; set; }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void StartPhase2(float damage)
    {
        if(_healthManager.Health > _healthManager.MaxHealth / 2 || _isOnPhase2)
            return;

        _healthManager.Inmune = true;
        _isOnPhase2 = true;
        ChangeState(GetStateByName("Transition"));
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    protected override void OnStart()
    {
        if(TryGetComponent(out _healthManager))
        {
            _healthManager.Inmune = true;
            _healthManager.CanBeKnockbacked = false;
            _healthManager._onDamaged.AddListener(StartPhase2);
        }
    }
    #endregion   

} // class BossStateMachine 
// namespace
