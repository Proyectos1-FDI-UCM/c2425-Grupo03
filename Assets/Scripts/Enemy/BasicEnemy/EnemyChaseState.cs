//---------------------------------------------------------
// Estado de persecución del enemigo. Debe perseguir al jugador sin caerse de plataformas.
// Si el jugador sale del rango de detección se para, pero si entra en el rango de ataque continúa.
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class EnemyChaseState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Velocidad a la que camina el enemigo.
    /// </summary>
    [SerializeField]
    [Tooltip("Enemy walking speed in units per second")]
    float _enemyWalkingSpeed;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    EnemyStateMachine _ctx;

    /// <summary>
    /// Referencia del rigidbody del enemigo.
    /// </summary>
    Rigidbody2D _rb;

    /// <summary>
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// El audio source con el sonido que reproduce el enemigo al andar.
    /// </summary>
    AudioSource _audioSource;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemyStateMachine>();

        if (_ctx != null)
        {
            //Coger animator del contexto
            _animator = _ctx.GetComponent<Animator>();

            //Coge la referencia al rigidbody por comodidad
            _rb = _ctx.Rigidbody;
        }

        //Coge la referencia al audio source para reproducir sonido de andar.
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_ctx != null)
        {
            //Si el jugador sale del trigger pone el range a false.
            _ctx.IsPlayerInChaseRange = false;
        }
        // Para de reproducir el sonido de andar
        _audioSource?.Stop();
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
        _animator?.SetBool("IsAttack", false);
        _animator?.SetBool("IsChasing", true);

        // Reproduce sonido de andar
        _audioSource?.Play();
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        if (_rb != null)
        {
            //Al salir del estado de chase, el enemigo nunca se debería mover
            _rb.velocity = Vector3.zero;
        }

        // Pone la animación de andar
        _animator?.SetBool("IsChasing", false);

        // Para el sonido de andar
        _audioSource?.Stop();
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
        if (_ctx != null)
        {
            //Actualizamos la dirección en la que mira el enemigo en función de la posición respecto al jugador
            _ctx.LookingDirection = (_ctx.PlayerTransform.position.x - _ctx.transform.position.x) > 0 ?
                EnemyStateMachine.EnemyLookingDirection.Right : EnemyStateMachine.EnemyLookingDirection.Left;

            // Invierte el sprite en función de la dirección en la que mira el jugador
            _ctx.SpriteRenderer.flipX = _ctx.LookingDirection == EnemyStateMachine.EnemyLookingDirection.Left;
        }

        if (_rb != null)
        {
            //Si todavía hay plataforma se mueve, sino se detiene
            if (CheckGround() && CheckEnemyInFront())
            {
                _animator.SetBool("IsChasing", true);
                _animator.SetBool("IsIdle", false);
                _rb.velocity = new Vector2(_enemyWalkingSpeed * (short)_ctx.LookingDirection, 0);
            }
            else
            {
                _animator.SetBool("IsChasing", false);
                _animator.SetBool("IsIdle", true);
                _rb.velocity = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// Método para que el enemigo no se caiga de las plataformas.
    /// Hace un raycast en la dirección que mira el enemigo.
    /// </summary>
    /// <returns>Devuelve <c>true</c> si el enemigo puede moverse en la dirección en la que mira</returns>
    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + 0.5f * (float)_ctx.LookingDirection, gameObject.transform.position.y),
                 Vector2.down, 0.2f, LayerMask.GetMask("Platform"));
  
        return hit.collider != null;
    }
    /// <summary>
    /// Método para que el enemigo no empuje a otros enemigos.
    /// Hace un raycast en la dirección que mira el enemigo.
    /// </summary>
    /// <returns>Devuelve <c>true</c> si el enemigo puede moverse en la dirección en la que mira</returns>
    private bool CheckEnemyInFront()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x + (float)_ctx.LookingDirection * 0.2f, gameObject.transform.position.y+ 0.5f),
            Vector2.right * (float)_ctx.LookingDirection, 0.1f, LayerMask.GetMask("Enemy"));

        return hit.collider == null || hit.collider.gameObject.GetComponent<EnemySummonerStateMachine>() != null;
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (!_ctx.IsPlayerInChaseRange)
        {
            //Si el jugador sale de la distancia de persecución vuelve al estado inactivo.
            _animator?.SetBool("IsChasing", false);
            Ctx.ChangeState(Ctx.GetStateByType<EnemyIdleState>());
        }
        else if((_ctx.PlayerTransform.position - _ctx.transform.position).magnitude < _ctx.AttackDistance)
        {
            //Si el jugador esta en el rango de ataque, pasa a atacar
            Ctx.ChangeState(Ctx.GetStateByType<EnemyAttackState>());
        }
    }

    #endregion   

} // class EnemyChaseState 
// namespace
