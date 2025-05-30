//---------------------------------------------------------
// El enemigo  hace el disparo y vuelve a attack state
// Santiago Salto Molodojen
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado del enemigo donde hace el disparo
/// </summary>
public class EnemySummonerShootState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [Header("Shoot Properties")]
    /// <summary>
    /// El tiempo de espera entre dos ataques
    /// </summary>
    [SerializeField] float _attackSpeed;
    /// <summary>
    /// El daño del disparo.
    /// </summary>
    [SerializeField] int _damage;

    /// <summary>
    /// Valor de tiempo del estado
    /// </summary>
    [SerializeField][Min(0)] float _waitTimeShootState;
 
    /// <summary>
    /// Valor de tiempo para hacer disparo
    /// </summary>
    [SerializeField][Min(0)] float _waitTimeShoot;

    [Header("Proyectile")]
    /// <summary>
    /// Proyectil del enemigo.
    /// </summary>
    [SerializeField] MagicBullet _magicBullet;


    /// <summary>
    /// Sonido del invocador al disparar
    /// </summary>
    [SerializeField] AudioClip _shotSound;
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
    /// El animator del enemigo
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    private EnemySummonerStateMachine _ctx;

    /// <summary>
    /// Tiempo de EstadoShoot
    /// </summary>
    private float _shootStateTime;

    /// <summary>
    /// Tiempo de espera para disparar más tiempo del momento del juego
    /// </summary>
    private float _shootTime;

    /// <summary>
    /// Booleana para ver si ha terminado de atacar
    /// </summary>
    private bool _attackFinished;
    /// <summary>
    /// Booleana para ver si ha terminado de atacar
    /// </summary>
    private bool _stateFinished;


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
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<EnemySummonerStateMachine>();

        //Coger animator del contexto
        _animator = _ctx?.GetComponent<Animator>();

        //Para el movimiento del invocador para disparar
        Ctx.Rigidbody.velocity = Vector3.zero;

        // activar animator
        _animator?.SetBool("IsAttack", true);

        //dar valor al tiempo de carga del ataque
        _shootStateTime = Time.time + _waitTimeShootState;
        _shootTime = Time.time + _waitTimeShoot;

        //indicar que el ataque no está acabado puesto que acaba de empezar
        _attackFinished = false;

        // indicar que el estado no está acabado puesto que acaba de empezar
        _stateFinished = false;
    }

    /// <summary>
    /// Metodo que Instancia una bala cada vez que es llamado
    /// </summary>
    public void Shoot()
    {
        // NO HACE FALTA COMPROBAR SI _CTX ES NULL PQ YA SE HA COMPROBADO DONDE SE LLAMA
        // Reproducir el sonido de disparo
        SoundManager.Instance.PlaySFX(_shotSound, transform, 0.3f);

        // Instancia la bala
        Instantiate(_magicBullet, _ctx.CastPoint.position, transform.rotation).Setup(_ctx.PlayerTransform.position);
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        // Termina la animación de disparo
        _animator?.SetBool("IsAttack", false);

        //Eliminar Hechizo ewn Caso de ser interrumpido
        if (transform.GetChild(0).childCount != 0)
        {
            transform.GetChild(0).GetChild(0).GetComponent<MagicCast>().EndCast();
        }
    }
    public void TriggerEvent()
    {
        Shoot();
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
            _ctx.UpdateLookingDirection();
            //Disparar después del tiempo de recarga

            if (Time.time > _shootTime && !_attackFinished)
            {
                Shoot();
                // Termina el ataque
                _attackFinished = true;

            }
   
            if (Time.time > _shootStateTime && !_stateFinished)
            {
                

                // Cambia de estado al de ataque
                _ctx.ChangeState(_ctx.GetStateByType<EnemySummonerAttackState>());

                // Termina la animación de disparo
                _animator?.SetBool("IsAttack", false);

                // Estado TErminado
                _stateFinished = true;
            }

            //Actualizar PosJugador al hechizo

            if (transform.GetChild(0).childCount != 0)
            {
                transform.GetChild(0).GetChild(0).GetComponent<MagicCast>().UpdatePlayerPos(_ctx.PlayerTransform);
            }
        }
    }


    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {



    }

    #endregion   

} // class EnemySummonerShootState 
// namespace
