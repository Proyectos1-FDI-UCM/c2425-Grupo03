//---------------------------------------------------------
// Hace una animación en la que se mueve por el escenario y si toca al jugador le hace daño
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
public class BossFlyingChargeState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Daño causado al jugador
    /// </summary>
    [Header("Damage")]
    [SerializeField]
    [Tooltip("Damage dealt when in contact with player")]
    float _damage;

    [Header("Animation Speed")]
    /// <summary>
    /// Velocidad durante la animación
    /// </summary>
    [SerializeField]
    [Tooltip("Speed while moving through the Animation Points and not charging")]
    float _speed;

    /// <summary>
    /// Velocidad durante la carga contra el juador
    /// </summary>
    [SerializeField]
    [Tooltip("Speed when the boss charges agains the player")]
    float _chargeSpeed;

    [Header("Charge Properties")]
    
    ///<summary>
    /// Punto en la animación donde comienza la carga
    /// </summary>
    [SerializeField]
    [Tooltip("Point in Animation Poins where the charge starts")]
    int _chargeStart;

    ///<summary>
    /// Punto en la animación donde termina la carga contra el jugador
    /// </summary>
    [SerializeField]
    [Tooltip("Point in Animation Poins where the charge stops")]
    int _chargeEnd;

    /// <summary>
    /// Tiempo de retraso antes de cargar contra el jugador
    /// </summary>
    [SerializeField]
    [Tooltip("Time delayed before charging")]
    float _chargeDelayTime;

    /// <summary>
    /// Tiempo de espera tras terminar la carga
    /// </summary>
    [SerializeField]
    [Tooltip("Time waited after doing the charge")]
    float _chargeEndWaitTime;

    /// <summary>
    /// Collider que hará daño al jugador
    /// </summary>
    [Header("Hit Collider")]
    [SerializeField]
    [Tooltip("Collider to enable when on this state")]
    CircleCollider2D _hitCollider;

    /// <summary>
    /// Puntos por los que se movera el boss durante este estado
    /// </summary>
    [Header("Animation points")]
    [SerializeField]
    Transform[] _animationPoints;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Posición actual en los puntos de animación
    /// </summary>
    int _currPointIndex;

    /// <summary>
    /// Tiempo para comenzar la carga tras llegar al punto de comienzo
    /// </summary>
    float _beginChargeTime;

    /// <summary>
    /// Tiempo de delay tras terminar la carga
    /// </summary>
    float _endChargeTime;
    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Al entrar en contacto con el jugador le hace daño
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out HealthManager healthManager))
        {
            healthManager.RemoveHealth(_damage);
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        // activamos el collider para hacer daño
        _hitCollider.enabled = true;
        // setup del movimiento
        _currPointIndex = 0;
        _beginChargeTime = 0;
        _endChargeTime = 0;
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        // quitamos el collider para que no haga más contactos
        _hitCollider.enabled = false;
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
        // Por cada punto de la animación (siempre que no estemos esperando a cargar)
        if( _currPointIndex < _animationPoints.Length && Time.time > _beginChargeTime && Time.time > _endChargeTime)
        {
            // Calculamos la velocidad
            float speed;
            if(_currPointIndex >= _chargeStart && _currPointIndex < _chargeEnd)
            {
                // Si estamos entre los puntos de comienzo de carga y fin de carga
                // ponemos la velocidad de carga
                speed = _chargeSpeed;
            }
            else
            {
                speed = _speed;
            }

            // Aplicamos la velocidad para mover el jefe
            Ctx.Rigidbody.position = Vector2.MoveTowards(Ctx.Rigidbody.position, (Vector2)_animationPoints[_currPointIndex].position, speed * Time.deltaTime);

            // Si hemos llegado al siguiente punto cambiamos para movernos al siguiente
            if(Ctx.Rigidbody.position == (Vector2)_animationPoints[_currPointIndex].position)
            {
                _currPointIndex++;

                // Si hemos llegado al comienzo de la carga establecemos cuando debe volver a comenzar a moverse
                // para hacer un delay antes de empezar a cargar
                if(_currPointIndex == _chargeStart)
                {
                    Ctx.Animator.SetTrigger("Compress");
                    Ctx.Animator.SetBool("IsAirCharging", true);
                    _beginChargeTime = Time.time + _chargeDelayTime;
                }
                else if(_currPointIndex == _chargeEnd)
                {
                    _endChargeTime = Time.time + _chargeEndWaitTime;
                    Ctx.Animator.SetBool("IsAirCharging", false);
                }
            }
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_currPointIndex >= _animationPoints.Length)
        {
            // Cambiamos al ataque de disparo
            Ctx.ChangeState(Ctx.GetStateByName("Shooting"));
        }
    }

    #endregion   

} // class BossFlyingChargeState 
// namespace
