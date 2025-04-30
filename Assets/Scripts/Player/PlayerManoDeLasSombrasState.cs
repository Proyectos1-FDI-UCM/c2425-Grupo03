//---------------------------------------------------------
// El estado en el que el Jugador esta tirando la habilidad ManoDeLasSombras
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerManoDeLasSombrasState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [Header("Lock/Unlock State")]
    /// <summary>
    /// Determina si está bloqueado o desbloqueado.
    /// </summary>
    [SerializeField] private bool _isLocked;
    [Header("Ability Properties")]
    /// <summary>
    /// // Daño de la habilidad.
    /// </summary>
    [SerializeField] private float _firstHitDamage = 10f;
    /// <summary>
    /// // Daño de la habilidad.
    /// </summary>
    [SerializeField] private float _secondHitDamage = 10f;
    /// <summary>
    /// Distancia máxima que la habilidad puede recorrer.
    /// </summary>
    [SerializeField] private float _skillRange = 4f;
    /// <summary>
    /// tiempo que se queda quieto el jugador al lanzar la habilidad
    /// </summary>
    [SerializeField][Tooltip("Tiempo en el que el jugador se queda inmovilizado al tirar la habilidad")] private float _cantMovePlayerTime = 1f;
    /// <summary>
    /// dibujar el rango de ataque
    /// </summary>
    [SerializeField] bool _drawRaycast = false;
    /// <summary>
    /// el rango que les atrae el primer hit
    /// </summary>
    [SerializeField][Tooltip("La distancia que atraes a los enemigos")] private float _attractDistance = 2f;
    /// <summary>
    /// el rango que les empuja el segundo hit
    /// </summary>
    [SerializeField][Tooltip("La distancia que empujas a los enemigos")] private float _pushDistance = 4f;
    /// <summary>
    /// la distancia entre el punto de comienzo de la habilidad y el jugador
    /// </summary>
    [SerializeField][Tooltip("la distancia entre el punto de comienzo de la habilidad y el jugador")] private float _startSkillPosition = 1f;
    /// <summary>
    /// lo que elevas a los enemigos
    /// </summary>
    [SerializeField][Tooltip("Altura que elevas a los enemigos")] private float _liftingHeight = 1f;
    /// <summary>
    /// el tiempo que tarda en traer a los enemigos
    /// </summary>
    [SerializeField][Tooltip("tiempo entre el primer hit y segundo hit")] private float _attractEnemyTime = 0.3f;
    /// <summary>
    /// Porcentaje de carga que aporta 
    /// </summary>
    [SerializeField] private float _abilityChargePercentage;
    /// <summary>
    /// Sonido que hace cuando atrae a los enemigos
    /// </summary>
    [SerializeField] AudioClip _attracSound;
    /// <summary>
    /// Sonido que hace cuando empuja a los enemigos
    /// </summary>
    [SerializeField] AudioClip _pushSound; 

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
    /// el tiempo en el que se lanza la habilidad
    /// </summary>
    float _startTime;
    /// <summary>
    /// coge referencia
    /// </summary>
    private PlayerCharge _chargeScript;
    /// <summary>
    /// coge referencia del ctx
    /// </summary>
    private PlayerStateMachine _ctx;

    /// <summary>
    /// Los enemigos golpeados cuando se ejecuta el ataque
    /// </summary>
    RaycastHit2D[] _hits;

    /// <summary>
    /// Cuantos enemigos han sido realmente golpeados con el ataque
    /// </summary>
    int _affectedEnemys;

    /// <summary>
    /// la dirección en la que se ejecuta el ataque
    /// </summary>
    Vector2 _direction;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    /// <summary>
    /// Propiedad que determina si el estado está bloqueado o no.
    /// </summary>
    public bool IsLocked { get => _isLocked; set => _isLocked = value; }
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// coge referencias
    /// </summary>
    private void Start()
    {
        _ctx = GetCTX<PlayerStateMachine>();
        _chargeScript = _ctx?.GetComponent<PlayerCharge>();
        _isLocked = false;
        _ctx?.OnManoSombrasPushAddListener(ShadowSecondHit);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// Llama al metodo para instanciar la bala y pone la velocidad del jugador a 0
    /// </summary>
    public override void EnterState()
    {
        // frena el jugador
        GetCTX<PlayerStateMachine>().Rigidbody.velocity = Vector2.zero;
        _startTime = Time.time;

        Ctx.Animator.SetTrigger("ManoSombras");

        // Actualiza la dirección en la que lanzar el ataque
        _direction = new Vector2((short)GetCTX<PlayerStateMachine>().LookingDirection,0);

        // Atrae a los enemigos y actualiza el arrays con los enemigos a golpear
        ShadowHandAtract();

        // Hace instantáneamente el primer golpe
        ShadowFirstHit();
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _chargeScript.ResetManoDeLasSombras();
        _chargeScript.AddCharge((_abilityChargePercentage / 100) * ((_firstHitDamage + _secondHitDamage) / 2));
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
        
    }

    private void ShadowHandAtract()
    {
        //Sonido de atraer
        SoundManager.Instance.PlaySFX(_attracSound, transform, 0.2f);

        // Posición de inicio del Raycast 
        Vector2 startPosition = (Vector2)transform.position + new Vector2(_startSkillPosition * _direction.x, 0f);

        // Realizar el Raycast
        _hits = Physics2D.RaycastAll(startPosition, _direction, _skillRange, LayerMask.GetMask("Enemy") | LayerMask.GetMask("Platform"));

        // Dibujar el Raycast 
        if (_drawRaycast) Debug.DrawRay(startPosition, _direction * _skillRange, Color.green, 0.5f);

        //La camara tiembla
        CameraManager.Instance.ShakeCamera(0.1f, 0.5f);

        bool wallHit = false; // Bandera para detectar si hemos golpeado una pared
        _affectedEnemys = 0; // Índice manual para recorrer hits[]

        while (_affectedEnemys < _hits.Length && !wallHit)
        {
            RaycastHit2D hit = _hits[_affectedEnemys];

            // Si colisiona con un muro, activamos la bandera para ignorar enemigos después de la pared
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                wallHit = true;
            }

            // Comprobar si ya golpeamos una pared
            if (!wallHit)
            {
                // Comprobar si el objeto impactado es un enemigo
                EnemyStateMachine enemy = hit.collider.gameObject.GetComponent<EnemyStateMachine>();
                EnemySummonerStateMachine enemyS = hit.collider.gameObject.GetComponent<EnemySummonerStateMachine>();

                float maxKnockback = Mathf.Min(Mathf.Abs(_attractDistance), Mathf.Abs(transform.position.x - hit.point.x));

                if (enemy != null)
                {
                    // Aplicar Knockback           
                    // Comprobar si el enemigo puede sobrepasar startPosition, limitamos la atracción
                    enemy.GetStateByType<KnockbackState>()
                        .ApplyKnockBack(-maxKnockback, _attractEnemyTime, _direction);
                }
                else if (enemyS != null)
                {
                    enemyS.GetStateByType<KnockbackState>()
                        .ApplyKnockBack(-maxKnockback, _attractEnemyTime, _direction);
                }
                _affectedEnemys++; // Añadimos 1 al indice de enemigos afectados
            }
        }
    }

    void ShadowFirstHit()
    {
        SoundManager.Instance.PlaySFX(_pushSound, transform, 0.8f);

        // Aplicar daño si tiene un HealthManager
        for (int i = 0; i < _affectedEnemys; i++)
        {
            HealthManager enemyHealth = _hits[i].collider.gameObject.GetComponent<HealthManager>();
            if (enemyHealth != null)
            {
                enemyHealth.RemoveHealth((int)_firstHitDamage); // Primer golpe
            }
        }
    }

    private void ShadowSecondHit()
    {
        for (int i = 0; i < _affectedEnemys; i++)
        {
            HealthManager enemy = _hits[i].collider == null ? null : _hits[i].collider.GetComponent<HealthManager>();

            if (enemy != null)
            {
                var enemyMelee = enemy.GetComponent<EnemyStateMachine>();
                var enemySummoner = enemy.GetComponent<EnemySummonerStateMachine>();

                KnockbackState knockback = null;

                if (enemyMelee != null)
                {
                    knockback = enemyMelee.GetStateByType<KnockbackState>();
                }
                else if (enemySummoner != null)
                {
                    knockback = enemySummoner.GetStateByType<KnockbackState>();
                }

                // Aplicar Knockback en la dirección contraria
                knockback?.ApplyKnockBack(-_pushDistance, 0.2f, -_direction + new Vector2(0, -_liftingHeight));

                // Aplicar daño si tiene un HealthManager

                 enemy.RemoveHealth((int)(_secondHitDamage)); // Segundo golpe
                
            }
        }

        //La camara tiembla
        CameraManager.Instance.ShakeCamera(0.1f, 0.5f);
    }
    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Cambia el estado de jugador si ha acabado el _cantMovePlayerTime
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Time.time - _startTime > _cantMovePlayerTime)
        {
            Ctx.ChangeState(GetCTX<PlayerStateMachine>().GetStateByType<PlayerGroundedState>());
        }
    }

    #endregion   

} // class PlayerManoDeLasSombras 
// namespace
