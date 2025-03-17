//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase para hacer el estado de ataque del jugador
/// </summary>
public class PlayerAttackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    [Header("Propiedad del ataque")]
    /// <summary>
    /// El radio de ataque del jugador
    /// </summary>
    [SerializeField, Min(0)] private float _attackRadius;
    /// <summary>
    /// El tiempo de espera entre dos ataques
    /// </summary>
    [SerializeField] private float _attackSpeed;
    /// <summary>
    /// El daño del ataque basico
    /// </summary>
    [SerializeField] private float _damage;
    /// <summary>
    /// El porcentaje que se añade a las habilidades
    /// </summary>
    [SerializeField] private float _abilityChargePercentage;
    [SerializeField] AudioClip _airHit1;
    [SerializeField] AudioClip _airHit2;
    [SerializeField] AudioClip _airHit3;
    [SerializeField] AudioClip _airHit4;
    [SerializeField] AudioClip _bloodHit1;


    [Header("Propiedad del combo")]
    /// <summary>
    /// El tiempo de gracia para encadenar combo
    /// </summary>
    [SerializeField, Min(1)] float _comboDuration;
    /// <summary>
    /// El daño extra añadido al ataque si encadenas el combo
    /// </summary>
    [SerializeField,Min(0)] int _comboExtraDamage;
    /// <summary>
    /// Dibujar rango del ataque
    /// </summary>
    [SerializeField] bool _drawRange = false;
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
    /// El índice del combo en el que esta el jugador
    /// </summary>
    private int _combo;
    /// <summary>
    /// El tiempo cuando termina el tiempo de gracia
    /// </summary>
    private float _endOfCombo;
    /// <summary>
    /// La dirección donde mira el jugador
    /// </summary>
    private int _direction;
    /// <summary>
    /// El rigidbody del player
    /// </summary>
    private Rigidbody2D _rb;
    /// <summary>
    /// El rigidbody del player
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// El contexto del playerstatemachine
    /// </summary>
    PlayerStateMachine _ctx;

    private PlayerChargeScript _chargeScript;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.

    /// <summary>
    /// El tiempo en el que se podrá volver a hacer un ataque
    /// </summary>
    public float NextAttackTime { get; private set; }
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        //Coger el rigidbody del contexto
        _rb = GetCTX<PlayerStateMachine>().Rigidbody;
        _animator = GetCTX<PlayerStateMachine>().Animator;
        _ctx = GetCTX<PlayerStateMachine>();
        _chargeScript = _ctx.GetComponent<PlayerChargeScript>();
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
    /// Resetear el combo a 0 para cuando se mueve el jugador
    /// </summary>
    public void ResetAttackCombo()
    {
        _combo = 0;
    }


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        //Coger la dirección donde mira el jugador del contexto
        _direction = (int)GetCTX<PlayerStateMachine>().LookingDirection;


        //Calcular el tiempo para realizar el siguiente ataque
        NextAttackTime = Time.time + _attackSpeed;

        //Actualizar combo
        UpdateCombo();

        //Atacar en la dirección donde mira el jugador
        
        _animator.SetInteger("AttackIndex", _combo);

      
        //La animación
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        Attack(_direction);
        _animator.SetInteger("AttackIndex", 0);

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
        //Poner la velocidad del rigidbody a cero
        _rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// Si el jugador mantiene la tecla atacar, pasa al estado playerxhargedattack, sino pasa al grounded state.
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Time.time > NextAttackTime)
        {
            if (_ctx.PlayerInput.Attack.IsPressed())
            {
                Ctx.ChangeState(Ctx.GetStateByType<PlayerChargedAttackState>());
            }
            else
            {
                Ctx.ChangeState(Ctx.GetStateByType<PlayerGroundedState>());
            }
        }

    }

    /// <summary>
    /// Hacer un CirclecastAll en la capa del "Enemy" en la dirección donde mira el jugador.
    /// Añadir el daño extra del combo si lo hay, y hace daño a todos los enemigos que están en el circlecast.
    /// </summary>
    private void Attack(int direction)
    {
        switch (_combo)
        {
            case 0: 
                SoundManager.Instance.PlaySFX(_airHit1,transform,1);
                break;
            case 1:
                SoundManager.Instance.PlaySFX(_airHit2, transform, 1);
                break;
            case 2:
                SoundManager.Instance.PlaySFX(_airHit3, transform, 1);
                break;
            case 3:
                SoundManager.Instance.PlaySFX(_airHit4, transform, 1);
                break;
        }
        int extraDamage = 0;
        Vector2 position = transform.position + (new Vector3(_attackRadius, 0) * direction);
        RaycastHit2D[] enemyInArea = Physics2D.CircleCastAll(position, _attackRadius, new Vector2(0, 0), _attackRadius, 1 << 10);

        if (_combo == 3) 
        { 
            extraDamage += _comboExtraDamage; 
        }

        foreach (RaycastHit2D enemy in enemyInArea)
        {
            //Daño al enemigo
            enemy.collider.GetComponent<HealthManager>().RemoveHealth((int)_damage + extraDamage);
            //Añadir carga a las habilidades
            _chargeScript.AddCharge((_abilityChargePercentage / 100) * _damage);
        }
        if (enemyInArea.Length >0)
        {
            SoundManager.Instance.PlaySFX(_bloodHit1, transform, 1);
        }
    }

    /// <summary>
    /// Actualizar el tiempo de gracia, y actualiza el combo
    /// </summary>
    private void UpdateCombo()
    {
        //Comprobar si sigue en el tiempo de gracia
        if (Time.time > _endOfCombo)
        {
            _combo = 0;
        }
        //Actualizar el tiempo de gracia
        _endOfCombo = Time.time + _comboDuration;

        //Actualizar el combo
        if (_combo == 0)
        {
            _combo = 1;
        }

        else if (_combo == 1)
        {
            _combo = 2;
        }
        else if (_combo == 2)
        {
            _combo = 3;
        }
        else
        {
            _combo = 1;
        }
    }
    /// <summary>
    /// Dibuja el rango de ataque basico
    /// </summary>

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_drawRange)Gizmos.DrawWireSphere(transform.position + (_direction * new Vector3(_attackRadius,0)),_attackRadius);
    }

    #endregion   

} // class PlayerAttackState 
// namespace
