//---------------------------------------------------------
// Estado de ataque del enemigo pesado
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado de ataque del enemigo pesado
/// </summary>
public class HeavyEnemyAttackState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    [Header("Propiedad del ataque")]
    /// <summary>
    /// La anchura de ataque del enemigo
    /// </summary>
    [SerializeField, Min(0)] float _attackWidth;
    /// <summary>
    /// La altura de ataque del enemigo
    /// </summary>
    [SerializeField, Min(0)] float _attackHeight;
    /// <summary>
    /// El tiempo cuando el enemigo pueda volver a atacar
    /// </summary>
    [SerializeField, Min(0)] float _attackTime;
    /// <summary>
    /// El daño del ataque basico
    /// </summary>
    [SerializeField] float _damage;

    [SerializeField, Min(0)] float _waitDamageTime;

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
    /// Referencia del tipo EnemyStatemachine del contexto.
    /// </summary>
    private HeavyEnemyStateMachine _ctx;

    /// <summary>
    /// Booleana para ver si ha terminado de atacar
    /// </summary>
    private bool _attackFinished;


    #endregion


    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    private void Start()
    {
        //Coge una referencia de la máquina de estados para evitar hacer más upcasting
        _ctx = GetCTX<HeavyEnemyStateMachine>();

        //Informar al contexto el rango de ataque del enemigo
        _ctx.AttackDistance = _attackWidth-0.6f;

    }

    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// Empieza el ataque
    /// </summary>
    public override void EnterState()
    {
        _ctx?.GetComponent<Animator>().SetBool("IsIdle", true);
        StartCoroutine(Attack((int)_ctx.LookingDirection));
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _ctx?.GetComponent<Animator>().SetBool("IsAttacking", false);
        _attackFinished = false;
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
        if (_attackFinished)
        {
            Ctx?.ChangeState(Ctx.GetStateByType<HeavyEnemyChasingState>());
        }
    }
    /// <summary>
    /// Corutina que se encarga de realizar el ataque del enemigo pesado
    /// </summary>
    /// <param name="direction">la dirección que ataca</param>
    /// <returns></returns>
    private IEnumerator Attack(int direction)
    {
        yield return new WaitForSeconds(0.7f);
        _ctx?.GetComponent<Animator>().SetBool("IsIdle", false);

        if (_ctx.IsPlayerInChaseRange)
        {
            _ctx?.GetComponent<Animator>().SetBool("IsAttacking", true);
            //Espera el tiempo de la animación de ataque para hacer el daño.
            yield return new WaitForSeconds(_waitDamageTime);

            //El rango de ataque del enemigo
            Vector2 attackBoxSize = new Vector2(_attackWidth, _attackHeight);
            Vector2 attackPosition = (Vector2)transform.position + new Vector2((_attackWidth / 2) * direction, 0);

            //Un ducktyping para ver si el raycat que hace en la direccion donde mira el enemigo tiene un HealthManager en la capa del jugador
            //Si hay devuelve el HealthManager del jugador
            Collider2D col2D = Physics2D.BoxCast(attackPosition, attackBoxSize, 0f, Vector2.zero, 0f, 1 << 6).collider;
            HealthManager HM = col2D?.GetComponent<HealthManager>();

            //Si consigue el HealthManager del jugador entonces hace daño al jugador, sino no hace anda.
            if (HM != null)
            {
                HM.RemoveHealth(_damage);
            }
            yield return new WaitForSeconds(_attackTime - _waitDamageTime);
        }
        _attackFinished = true;
    }
    /// <summary>
    /// dibuja el rango de ataque en el editor si esta el juego en ejecución.
    /// </summary>
    /*private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red; 

        // Calcular la posición del ataque en función de la dirección
        Vector2 position = (Vector2)transform.position + new Vector2((_attackWidth / 2) * (int)_ctx.LookingDirection, 0);

        // Dibujar un cuadro en la posición del ataque
        Gizmos.DrawWireCube(position, new Vector2(_attackWidth, _attackHeight));
    }*/
    #endregion   

} // class HeavyEnemyAttackState 
// namespace
