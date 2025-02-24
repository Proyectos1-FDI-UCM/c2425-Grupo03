//---------------------------------------------------------
// Breve descripción del contenido del archivo
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
public class PlayerFallingState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField][Min(0)] float _maxCoyoteTime;
    [SerializeField] RaycastHit2D _hitLeft;
    [SerializeField] RaycastHit2D _hitRight;
    [SerializeField] float _hitDistance;
    [SerializeField] bool _DebugRayCast;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    Rigidbody2D _rigidbody;
    PlayerStateMachine _ctx;
    float _coyoteTime;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        _ctx = GetCTX<PlayerStateMachine>();
        _rigidbody = _ctx.Rigidbody;
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
    /// </summary>
    public override void EnterState()
    {
        SetSubState(Ctx.GetStateByType<PlayerIdleState>());
    }
    public void ResetCoyoteTime()
    {
        _coyoteTime = _maxCoyoteTime;
    }
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _coyoteTime = 0;
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
    /// </summary>
    protected override void UpdateState()
    {
        if (_coyoteTime > 0)
        {
            _coyoteTime -= Time.deltaTime;
        }

        _hitLeft = Physics2D.Raycast(new Vector2(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y), Vector2.down, _hitDistance, LayerMask.GetMask("Ground"));
        _hitRight = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y), Vector2.down, _hitDistance, LayerMask.GetMask("Ground"));

        if (_DebugRayCast)
        {
            Debug.DrawRay(new Vector2(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y), Vector2.down * _hitDistance, Color.red);
            Debug.DrawRay(new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y), Vector2.down * _hitDistance, Color.red);
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_hitLeft.collider != null || _hitRight.collider != null)
        {
            ChangeState(_ctx.GetStateByType<PlayerGroundedState>());
        }
        else if (_coyoteTime > 0 && _ctx.PlayerInput.Jump.IsPressed())
        {
            ChangeState(_ctx.GetStateByType<PlayerJumpState>());
        }
        else if (_ctx.PlayerInput.Dash.IsPressed())
        {
            PlayerDashState dashState = _ctx.GetStateByType<PlayerDashState>();
            if (Time.time > dashState.NextAvailableDashTime) ChangeState(dashState);
        }
    }

    #endregion   

} // class PlayerFallingState 
// namespace
