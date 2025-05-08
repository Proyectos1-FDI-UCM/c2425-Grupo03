//---------------------------------------------------------
// Estado previo a la carga del jefe final
// Responsable de la creación de este archivo
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado previo a la carga del jefe final
/// </summary>
public class BossPrechargeState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Tiempo que tarda el estado
    /// </summary>
    [SerializeField]
    float _timeToCharge;
    [SerializeField] AudioClip _preSpinner;    
    
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Cuando termina el estado
    /// </summary>
    float _prechargeStateEnd;

    AudioSource _audioSource;

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        // calculamos cuando termina el estado y ponemos la animación
        _prechargeStateEnd = Time.time + _timeToCharge;
        Ctx.Animator.SetBool("IsPreparingCharge", true);
        //_audioSource = SoundManager.Instance.PlaySFXWithAudioSource(_preSpinner, transform, 1);
    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        // Termina la animación
        Ctx.Animator.SetBool("IsPreparingCharge", false);
       // _audioSource.Stop();
    }

    private void Start()
    {
        MusicPlayer.Instance.PlayBossPhase1Sound();
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
        if(Time.time > _prechargeStateEnd)
        {
            // Cambiamos a la carga contra el jugador
            Ctx.ChangeState(Ctx.GetStateByName("Charging"));
        }
    }

    #endregion   

} // class BossPrechargeState 
// namespace
