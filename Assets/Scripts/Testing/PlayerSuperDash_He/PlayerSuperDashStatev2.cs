//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerSuperDashState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// La distancia del dash
    /// </summary>
    [SerializeField] private int _dashDistance;

    /// <summary>
    /// El daño que hace el dash
    /// </summary>
    [SerializeField] private int _damage;

    /// <summary>
    /// El tiempo que dura el dash completo
    /// </summary>
    [SerializeField, Min(0)] private float _actionTime;

    /// <summary>
    /// El tiempo cuando hace daño
    /// </summary>
    [SerializeField, Min(0)] private float _timeOfDamage;

    /// <summary>
    /// El tiempo cuando hace el dash
    /// </summary>
    [SerializeField, Min(0)] private float _timeOfDash;

    [SerializeField] private int _abilityIndex;

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
    /// La posicion a hacer el TP
    /// </summary>
    private Vector2 _endPosition;

    /// <summary>
    /// El contexto 
    /// </summary>
    private PlayerStateMachine _ctx;

    /// <summary>
    /// La direccion donde mira el jugador
    /// </summary>
    private int _lookingDirection;

    /// <summary>
    /// Cuando termina el dash por completo
    /// </summary>
    private float _finishTime;

    /// <summary>
    /// Cuando hace daño al enemigo
    /// </summary>
    private float _damageTime;

    /// <summary>
    /// Cuando hace el TP
    /// </summary>
    private float _tpTime;

    /// <summary>
    /// Booleana para ver si ya ha hecho daño
    /// </summary>
    private bool _damageDone;

    /// <summary>
    /// Booleana para ver si ya ha hecho el dash
    /// </summary>
    private bool _tpDone;

    /// <summary>
    /// Todos los enemigos que estan el el area de dash, se puede hacer sin raycast
    /// </summary>
    private RaycastHit2D[] _enemyInArea;

    /// <summary>
    /// El script de la carga de las habilidades
    /// </summary>
    private PlayerChargeScript _chargeScript;


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
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        //No se puede atacar al jugador mientras hace el dash
        _ctx.GetComponent<HealthManager>().Inmune = true;

        _ctx.Rigidbody.velocity = Vector2.zero;

        //Comprobar la validez de los tiempos
        Initialize(_timeOfDamage, _timeOfDash);

        //La direccion donde mira el jugador
        _lookingDirection = (int)_ctx.LookingDirection;

        //Inicializar la posicion a hacer el tp
        _endPosition = new Vector2(_ctx.transform.position.x, _ctx.transform.position.y);

        //Ver los enemigos que esta en el area del dash
        _enemyInArea = GetEnemyInDash();

        //Comprobar si hay pared en la distancia del dash
        CheckWall();

    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        //Quitar inmunidad al jugador
        _ctx.GetComponent<HealthManager>().Inmune = false;
        _chargeScript.ResetCharge(0);
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
        if (Time.time >= _tpTime && !_tpDone)
        {
            Dash();
        }
        if (Time.time >= _damageTime && !_damageDone)
        {
            Damage(_enemyInArea);
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Time.time > _finishTime)
        {
            PlayerGroundedState playerGroundedState = _ctx.GetStateByType<PlayerGroundedState>();
            _ctx.ChangeState(playerGroundedState);
        }
    }

    /// <summary>
    /// Ver si hay pared dentro de la distancia del dash
    /// </summary>
    private void CheckWall()
    {
        RaycastHit2D wall = Physics2D.Raycast(_ctx.transform.position, new Vector2(_lookingDirection, 0), _dashDistance, 1 << 3);

        if (wall)
        {
            _endPosition.x = wall.point.x;
        }
        else
        {
            _endPosition.x += _dashDistance * _lookingDirection;
        }
    }

    /// <summary>
    /// Hacer el dash
    /// </summary>
    private void Dash()
    {
        _ctx.transform.position = _endPosition;
        _tpDone = true;

        //Si tras hacer el dash no hay nada debajo del jugador(Falling State) termina el estado
        RaycastHit2D floor = Physics2D.Raycast(_ctx.transform.position, new Vector2(0, -1), -1.2f , 1 << 0);
        
        if(floor.collider == null)
        {
            Ctx.ChangeState(_ctx.GetStateByType<PlayerGroundedState>());
        }
    }

    //Ver todos los enemigos que estan en el raycast
    private RaycastHit2D[] GetEnemyInDash()
    {
        RaycastHit2D[] enemyInArea = Physics2D.RaycastAll(_ctx.transform.position, new Vector2(_lookingDirection, 0), _dashDistance, 1 << 10);
        return enemyInArea;
    }

    //Hacer daño a todos los enemigos que estan en el raycast
    private void Damage(RaycastHit2D[] enemyInArea)
    {
        foreach (RaycastHit2D enemy in enemyInArea)
        {
            enemy.collider.GetComponent<HealthManager>().RemoveHealth(_damage);
        }
        _damageDone = true;
    }

    /// <summary>
    /// Inicializar los tiempos y comprobar su validez
    /// </summary>
    /// <param name="damageTime"></param>
    /// <param name="tpTime"></param>
    private void Initialize(float damageTime, float tpTime)
    {
        _finishTime = Time.time + _actionTime;

        //Comprobar la validez del tiempo de hacer daño
        if (damageTime == 0 || damageTime > _actionTime)
        {
            _damageTime = Time.time + _actionTime;
        }
        else
        {
            _damageTime = Time.time + damageTime;
        }

        //Comprobar la validez del tiempo de tp
        if (tpTime == 0 || tpTime > _actionTime)
        {
            _tpTime = Time.time + _actionTime;
        }
        else
        {
            _tpTime = Time.time + tpTime;
        }

        _tpDone = false;

        _damageDone = false;
    }

    #endregion   

} // class PlayerSuperDashStatev2 
// namespace
