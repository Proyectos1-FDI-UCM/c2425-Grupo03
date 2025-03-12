//---------------------------------------------------------
// El estado en el que el Jugador esta tirando la habilidad ManoDeLasSombras
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerManoDeLasSombrasState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
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
    [SerializeField] private float _cantMovePlayerTime = 1f;
    /// <summary>
    /// dibujar el rango de ataque
    /// </summary>
    [SerializeField] bool _drawRaycast = false;
    /// <summary>
    /// el rango que les atrae el primer hit
    /// </summary>
    [SerializeField] private float _attractDistance = 2f;
    /// <summary>
    /// el rango que les empuja el segundo hit
    /// </summary>
    [SerializeField] private float _pushDistance = 4f;
    /// <summary>
    /// la distancia entre el punto de comienzo de la habilidad y el jugador
    /// </summary>
    [SerializeField] private float _startSkillPosition = 1f;
    /// <summary>
    /// la distancia entre el punto de comienzo de la habilidad y el jugador
    /// </summary>
    [SerializeField] private float _liftingHeight = 1f;
    /// <summary>
    /// el tiempo que hay entre pulsar el boton y el primer hit
    /// </summary>
    [SerializeField] private float _waitTimeForFirstHit = 0.5f;
    /// <summary>
    /// el tiempo que tarda en traer a los enemigos
    /// </summary>
    [SerializeField] private float _attractEnemyTime = 0.3f;

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
    private PlayerChargeScript _chargeScript;
    /// <summary>
    /// coge referencia del ctx
    /// </summary>
    private PlayerStateMachine _ctx;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// coge referencias
    /// </summary>
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
    /// metodo que instancia la habilidad delante del jugador.
    /// </summary>
    /// <param name="direction"></param>

    private IEnumerator CastShadowHand(Vector2 direction)
    {
        // Esperar x segundos antes del primer golpe
        yield return new WaitForSeconds(_waitTimeForFirstHit);

        // Posición de inicio del Raycast (en el jugador)
        Vector2 startPosition = (Vector2)transform.position + new Vector2(_startSkillPosition * direction.x, 0f); 

        // Realizar el Raycast
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, _skillRange, LayerMask.GetMask("Enemy")| LayerMask.GetMask("Wall"));

        // Dibujar el Raycast en la escena para depuración
        if (_drawRaycast) Debug.DrawRay(startPosition, direction * _skillRange, Color.green, 0.5f);

        foreach (RaycastHit2D hit in hits)
        {
            
            // Si colisiona con un muro, deja de atraer a los enemigos
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                break;
            }

            // Verificar si el objeto impactado es un enemigo
            EnemyStateMachine enemy = hit.collider.gameObject.GetComponent<EnemyStateMachine>();

            if (enemy != null)
            {
                // Aplicar Knockback           
                // Si el enemigo puede sobrepasar startPosition, limitamos la atracción

                float maxKnockback = Mathf.Min(Mathf.Abs(_attractDistance), Mathf.Abs(transform.position.x - hit.point.x));

                enemy.GetStateByType<KnockbackState>()
                        .ApplyKnockBack(-maxKnockback, _attractEnemyTime, direction);
                
                // Aplicar daño si tiene un HealthManager
                HealthManager enemyHealth = hit.collider.gameObject.GetComponent<HealthManager>();
                if (enemyHealth != null)
                {
                    enemyHealth.RemoveHealth((int)_firstHitDamage); // Primer golpe
                }
            }

        }
        if (hits.Length > 0)
        {
            StartCoroutine(ApplySecondHit(hits, direction));
        }
    }

    /// <summary>
    /// Metodo que realiza el segundo ataque
    /// </summary>
    /// <param name="hits"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator ApplySecondHit(RaycastHit2D[] hits, Vector2 direction)
    {
        // Esperar a que termine de atraer a los enemigos para hacer el segundo golpe
        yield return new WaitForSeconds(_attractEnemyTime+0.1f);

        foreach (RaycastHit2D hit in hits)
        {
            // Si colisiona con un muro, deja de atraer a los enemigos
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                break;
            }

            EnemyStateMachine enemy = hit.collider == null ? null: hit.collider.GetComponent<EnemyStateMachine>();

            if (enemy != null)
            {
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    // Aplicar Knockback en la dirección contraria
                    enemy.GetStateByType<KnockbackState>()?.ApplyKnockBack(-_pushDistance, 0.2f, -direction + new Vector2 (0,-_liftingHeight));
                }

                // Aplicar daño si tiene un HealthManager
                HealthManager health = enemy.GetComponent<HealthManager>();
                if (health != null)
                {
                    health.RemoveHealth((int)(_secondHitDamage)); // Segundo golpe
                }
            }
        }
    }

    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// Llama al metodo para instanciar la bala y pone la velocidad del jugador a 0
    /// </summary>
    public override void EnterState()
    {
        GetCTX<PlayerStateMachine>().Rigidbody.velocity = Vector2.zero;
        _startTime = Time.time;

        // Lanza el Raycast en la dirección en la que el jugador está mirando
        StartCoroutine(CastShadowHand(new Vector2((short)GetCTX<PlayerStateMachine>().LookingDirection, 0)));
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _chargeScript.ResetCharge(1);
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
