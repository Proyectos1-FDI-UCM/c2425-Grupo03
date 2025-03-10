//---------------------------------------------------------
// El estado en el que el Jugador esta tirando la habilidad ManoDeLasSombras
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
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
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    /// <summary>
    /// // Daño de la habilidad.
    /// </summary>
    [SerializeField] private float _firstHitDamage = 10;
    /// <summary>
    /// // Daño de la habilidad.
    /// </summary>
    [SerializeField] private float _secondHitDamage = 10;
    /// <summary>
    /// Distancia máxima que la habilidad puede recorrer.
    /// </summary>
    [SerializeField] private float _skillRange = 4;
    /// <summary>
    /// tiempo que se queda quieto el jugador al lanzar la habilidad
    /// </summary>
    [SerializeField] private float _animationTime = 1;
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

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

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
    
    private void CastShadowHand(Vector2 direction)
    {
        // Posición de inicio del Raycast (en el jugador)
        Vector2 startPosition = (Vector2)transform.position + new Vector2(_startSkillPosition * direction.x, 0);

        // Realizar el Raycast
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, _skillRange);

        // Dibujar el Raycast en la escena para depuración
        if (_drawRaycast)Debug.DrawRay(startPosition, direction * _skillRange, Color.red, 0.5f);

        foreach (RaycastHit2D hit in hits)
        {
            // Verificar si el objeto impactado es un enemigo
            EnemyStateMachine enemy = hit.collider.GetComponent<EnemyStateMachine>();

            if (enemy != null)
            {

                // Aplicar Knockback
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    //float knockbackDistance = Vector2.Distance(startPosition, enemyRb.position);
                    enemy.GetStateByType<KnockbackState>()?.ApplyKnockBack(-_attractDistance + 1f, 0.1f, direction);
                }

                // Aplicar daño si tiene un HealthManager
                HealthManager health = enemy.GetComponent<HealthManager>();
                if (health != null)
                {
                    health.RemoveHealth((int)_firstHitDamage); // Primer golpe
                }
            }
        }
        if (hits.Length > 0)
        {
            StartCoroutine(ApplySecondHit(hits, direction));
        }
    }
    private IEnumerator ApplySecondHit(RaycastHit2D[] hits, Vector2 direction)
    {
        // Esperar 0.5 segundos antes del segundo golpe
        yield return new WaitForSeconds(0.5f);

        foreach (RaycastHit2D hit in hits)
        {
            EnemyStateMachine enemy = hit.collider.GetComponent<EnemyStateMachine>();

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
        CastShadowHand(new Vector2((short)GetCTX<PlayerStateMachine>().LookingDirection, 0));
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        
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
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Time.time - _startTime > _animationTime)
        {
            Ctx.ChangeState(GetCTX<PlayerStateMachine>().GetStateByType<PlayerGroundedState>());
        }
    }

    #endregion   

} // class PlayerManoDeLasSombras 
// namespace
