//---------------------------------------------------------
// Estado de giro del enemigo pesado
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado de giro del enemigo pesado
/// </summary>
public class HeavyEnemyFlipState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField] float _flipTime = 1f;
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
    /// referencia al contexto
    /// </summary>
    HeavyEnemyStateMachine _ctx;
    /// <summary>
    /// si ha terminado de girar
    /// </summary>
    bool _completed = false;
    /// <summary>
    /// referencia al rigidbody del enemigo
    /// </summary>
    Rigidbody2D _rb;
    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Coge referencias
    /// </summary>
    private void Start()
    {
        _ctx = GetCTX<HeavyEnemyStateMachine>();
        _rb = _ctx?.GetComponent<Rigidbody2D>();
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
    /// Pone la velocidad del enemigo a cero y empieza a girar
    /// </summary>
    public override void EnterState()
    {
        //Debug.Log("fLIP");
        //Para el movimiento del jugador
        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }
        //Comienza a hacer el flip
        StartCoroutine(Flip());
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _completed = false;
    }

    /// <summary>
    /// metodo donde el enemigo empieza a girar
    /// </summary>
    /// <returns></returns>
    IEnumerator Flip()
    {
        yield return new WaitForSeconds(_flipTime); // espera al flip time

        if (_ctx != null)
        {
            //gira de direccion
            _ctx.LookingDirection = (_ctx.PlayerTransform.position.x - _ctx.transform.position.x) > 0 ?
            HeavyEnemyStateMachine.EnemyLookingDirection.Right : HeavyEnemyStateMachine.EnemyLookingDirection.Left;

            //rota el sprite
            _ctx.SpriteRenderer.flipX = _ctx.LookingDirection == HeavyEnemyStateMachine.EnemyLookingDirection.Left;
        }
        _completed = true;
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
        //pasa a idle al terminar
        if (_completed)
        {
            Ctx?.ChangeState(Ctx.GetStateByType<HeavyEnemyIdleState>());
        }
    }

    #endregion   

} // class HeavyEnemyFlipState 
// namespace
