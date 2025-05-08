//---------------------------------------------------------
// Estado de muerte del boss
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
public class BossDeathState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Tiempo que tarda en destruirse el objeto
    /// </summary>
    [SerializeField]
    float _timeToDestroy;
    #endregion
  
    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        // Nos ponemos en inmune
        if(Ctx.TryGetComponent(out HealthManager hm))
        {
            hm.Inmune = true;
        }
  
        //Establece la animación de morir
        Ctx.Animator.SetBool("IsDead", true);

        // Nos destruimos tras un tiempo
        Destroy(transform.root.gameObject, _timeToDestroy);
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
        
    }

    #endregion   

} // class BossDeathState 
// namespace
