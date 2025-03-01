
//---------------------------------------------------------
// El estado en el que el jugador carga y realiza el ataque cargado
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerChargedAttackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    /// <summary>
    /// Radio de alcance del ataque cargado
    /// </summary>
    [SerializeField] private float _chargedAttackRadius = 2.0f;
    /// <summary>
    /// Daño que hace el ataque cargado
    /// </summary>
    [SerializeField] private float _chargedDamage = 2.0f;
    /// <summary>
    /// tiempo que necesita para cargar el ataque
    /// </summary>
    [SerializeField] private float _chargingTime = 2.0f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// El rigidbody del jugador
    /// </summary>
    Rigidbody2D _rigidbody;
    /// <summary>
    /// el contexto del playerstatemachine
    /// </summary>
    PlayerStateMachine _ctx;
    /// <summary>
    /// el animator para las animaciones
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// El tiempo en el que se empieza a cargar la habilidad
    /// </summary>
    float _startChargingTime;
    /// <summary>
    /// Si ya ha realizado el ataque
    /// </summary>
    bool _attacked = false;
    #endregion

    // ---- PROPIEDADES ----

    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Inicializa las variables y cogen referencia
    /// </summary>
    void Start()
    {
        _ctx = GetCTX<PlayerStateMachine>();
        _rigidbody = _ctx.Rigidbody;
        _animator = GetCTX<PlayerStateMachine>().Animator;

    }
    /// <summary>
    /// Metodo encargado de hacer el ataque cargado en un circulo con el centro en el jugador
    /// </summary>
    void ChargedAttack()
    {
        Vector2 position = transform.position;
        RaycastHit2D[] enemyInArea = Physics2D.CircleCastAll(position, _chargedAttackRadius, Vector2.zero ,0, 1 << 10);

        foreach (RaycastHit2D enemy in enemyInArea)
        {
            enemy.collider.GetComponent<HealthManager>()?.RemoveHealth((int)_chargedDamage);
        }
    }
    /// <summary>
    /// Dibuja el rango de ataque cargado
    /// </summary>

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _chargedAttackRadius);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// Pone al startchargingtime el timepo que entra en el estado
    /// </summary>
    public override void EnterState()
    {
        _attacked = false;
        _startChargingTime = Time.time;
        _animator.SetBool("IsCharging", true);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// desactiva las animaciones
    /// </summary>
    public override void ExitState()
    {
        _animator.SetBool("IsChargeAttaking", false);
        _animator.SetBool("IsCharging", false);
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Metodo llamado cada frame cuando este es el estado activo de la maquina de estados.
    /// si has mantenido pulsado el ataque por un tiempo y no has atacado antes, hace el ataque cargado y la animacion
    /// </summary>
    protected override void UpdateState()
    {
        if (Time.time - _startChargingTime >= _chargingTime && !_attacked)
        {
            _animator.SetBool("IsChargeAttaking", true);
            ChargedAttack();
            _attacked = true;
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// Si ya has atacado (+0.5f para que se acabe la animacion), pasa al groundedstate 
    /// </summary>
    protected override void CheckSwitchState()
    {
        if ((Time.time - _startChargingTime >= _chargingTime + 0.5f) || _ctx.PlayerInput.Attack.WasReleasedThisFrame())
        {
            Ctx.ChangeState(_ctx.GetStateByType<PlayerGroundedState>());
        }
    }
    #endregion   

} // class PlayerChargedAttackState 
// namespace
