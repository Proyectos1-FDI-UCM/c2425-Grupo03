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
    [SerializeField]
    float _damage;

    [SerializeField]
    float _speed;

    [SerializeField]
    float _chargeSpeed;

    [SerializeField]
    float _chargeStart;

    [SerializeField]
    float _chargeEnd;

    [SerializeField]
    float _chargeDelayTime;

    [SerializeField]
    CircleCollider2D _hitCollider;

    [SerializeField]
    Transform[] _animationPoints;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    int _currPointIndex;
    float _beginChargeTime;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
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
        _hitCollider.enabled = true;
        Ctx.Animator.SetTrigger("FlyingCharge");
        _currPointIndex = 0;
        _beginChargeTime = 0;
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _hitCollider.enabled = false;
        Ctx.Animator.ResetTrigger("FlyingCharge");
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
        if( _currPointIndex < _animationPoints.Length && Time.time > _beginChargeTime)
        {
            float speed = _speed;
            if(_currPointIndex >= _chargeStart && _currPointIndex < _chargeEnd)
            {
                speed = _chargeSpeed;
            }

            Ctx.Rigidbody.position = Vector2.MoveTowards(Ctx.Rigidbody.position, (Vector2)_animationPoints[_currPointIndex].position, speed * Time.deltaTime);

            if(Ctx.Rigidbody.position == (Vector2)_animationPoints[_currPointIndex].position)
            {
                _currPointIndex++;

                if(_currPointIndex == _chargeStart)
                {
                    _beginChargeTime = Time.time + _chargeDelayTime;
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
            Ctx.ChangeState(Ctx.GetStateByName("Shooting"));
        }
    }

    #endregion   

} // class BossFlyingChargeState 
// namespace
