//---------------------------------------------------------
// Estado que permite hacer un dash al jugador.
// Adrian Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// Clase que extiende BaseState para hacer el estado Dash del jugador.
/// </summary>
public class PlayerDashState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.

    [Header("Dash properties")]
    /// <summary>
    /// Distancia que se mueve el jugador al hacer un dash
    /// </summary>
    [Tooltip("Distance moved while dashing in units.")]
    [SerializeField][Range(0, 8)] float _distance;

    /// <summary>
    /// Cuanto tarda en recorrer la distancia indicada
    /// </summary>
    [Tooltip("Duration of the dash in seconds.")]
    [SerializeField][Range(0.1f, 1)] float _duration;

    /// <summary>
    /// Cuanto tarda en poder volver a hacer un dash
    /// </summary>
    [Tooltip("Time needed after dashing to dash again in seconds. Starts counting after dash began.")]
    [SerializeField][Min(0)] float _rechargeTime;

    /// <summary>
    /// El trigger a desactivar mientras hace el dash para que los enemigos no hagan daño al jugador.
    /// </summary>
    [Tooltip("Trigger deactivated while dashing.")]
    [SerializeField] BoxCollider2D _playerHitTrigger; //Preferiblemente debería estar en el contexto

    /// <summary>
    /// Sonido para el dash
    /// </summary>
    [SerializeField] AudioClip _dashSound;

    [SerializeField] ParallaxEffect ParallaxEffect;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    /// <summary>
    /// Tiempo en el que debe terminar el dash.
    /// </summary>
    float _finishDashingTime;


    /// <summary>
    /// La posicion donde debe terminar el dash
    /// </summary>
    float _finishDashingPositionX;

    /// <summary>
    /// El Rigidbody del jugador.
    /// </summary>
    Rigidbody2D _rb;

    /// <summary>
    /// Velocidad a la que se hace el dash.
    /// </summary>
    float _dashSpeed;

    /// <summary>
    /// El SpriteRenderer del jugador.
    /// </summary>
    SpriteRenderer _spriteRenderer;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.

    /// <summary>
    /// Tiempo en el que se podrá volver a hacer un dash
    /// </summary>
    public float NextAvailableDashTime { get; private set; }
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Coge el Rigidbody del contexto para tener la referencia.
    /// </summary>
    void Start()
    {
        _rb = Ctx?.Rigidbody;
        _spriteRenderer = Ctx?.GetComponent<SpriteRenderer>();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// Pone los valores que hacen el dash y establece el tiempo para finalizar el dash.
    /// Si no se puede hacer un dash cambia a otro estado.
    /// </summary>
    public override void EnterState()
    {
        SoundManager.Instance.PlaySFX(_dashSound, transform, 1);
        //Acelera al jugador para hacer el dash, calcula la velocidad haciendo. -> v = d / t
        _dashSpeed = _distance * (short)GetCTX<PlayerStateMachine>()?.LookingDirection / _duration;
        _rb.velocity = new Vector2(_dashSpeed, 0);

        //Quita la gravedad para que no caiga si dasheas en el aire.
        _rb.gravityScale = 0;

        //establece el tiempo que dura el dash.
        _finishDashingTime = _duration;

        //calcula el siguiente timpo cuando se puede volver a hacer el dash.
        NextAvailableDashTime = Time.time + _rechargeTime;

        //Desactiva el trigger para que no se pueda golpear al jugador.
        _playerHitTrigger.enabled = false;

        //Mira a ver si el dash podrías atravesar una pared.
        //CheckDashLimit(); Se ha quitado porque las plataformas son lo suficientemente gruesas y el dash lo suficientemente lento como para que no se puedan atravesar

        // Cambia la opacidad del jugador para mostrar al jugador que es invulnerable durante el dash
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.2f);
        }

        //Comienza la animación del dash
        Ctx?.Animator.SetBool("IsDashing", true);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        if (_rb != null)
        {
            //Quita la velocidad.
            _rb.velocity = Vector2.zero;

            //Reestablece la gravedad.
            _rb.gravityScale = GetCTX<PlayerStateMachine>().GravityScale;
        }

        if (_playerHitTrigger != null)
        {
            //Vuelve a activar el trigger para que golpeen al jugador.
            _playerHitTrigger.enabled = true;
        }

        //Reestablece la posición final del dash al máximo para que permita volver a hacer un dash.
        _finishDashingPositionX = 0;

        if (_spriteRenderer != null)
        {
            // Cambia la opacidad del jugador para mostrar al jugador que termina la invulnerabilidad
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        }

        //Termina la animación del dash
        Ctx?.Animator.SetBool("IsDashing", false);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí

    /// <summary>
    /// Metodo llamado cada frame cuando este es el estado activo de la maquina de estados.
    /// </summary>
    protected override void UpdateState()
    {
        //Baja el tiempo de para ver cuando termina el dash
        if (_finishDashingTime > 0)
        {
            _finishDashingTime -= Time.deltaTime;
        }

        //Vuelve a establecer la velocidad del dash (Hay problemas si no se hace, creo que es por la fricción)
        _rb.velocity = new Vector2(_dashSpeed, 0);
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en sí.
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_finishDashingTime <= 0 || (_finishDashingPositionX != 0 && Mathf.Abs(_rb.transform.position.x - _finishDashingPositionX) < 1))
        {
            Ctx?.ChangeState(Ctx.GetStateByType<PlayerFallingState>());
        }
    }

    /// <summary>
    /// <para>Hace un raycast en la dirección del dash para ver si el dash podría atravesar una pared.</para>
    /// Simplemente es una doble comprobación para asegurarme de que el jugador no atraviesa un muro.
    /// </summary>
    private void CheckDashLimit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2((int)GetCTX<PlayerStateMachine>().LookingDirection, 0), _distance, 1 << 3);
        if (hit.distance < _distance)
        {
            _finishDashingPositionX = hit.point.x;
        }
    }

    #endregion   

} // class PlayerDashState 
// namespace
