//---------------------------------------------------------
// Estado vulnerable del jefe en la segunda fase del combate
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Estado vulnerable del jefe en la segunda fase del combate
/// </summary>
public class BossAirVulnerableState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Tiempo que está vulnerable
    /// </summary>
    [SerializeField]
    [Min(0)]
    [Tooltip("Time of vulnerability after reaching the designed position")]
    float _vulnerableTime;

    /// <summary>
    /// Posición en la que estará vulnerable
    /// </summary>
    [SerializeField]
    [Tooltip("World position where the boss will be vulnerable")]
    Transform _vulnerablePoint;

    /// <summary>
    /// Velocidad en la que se mueve hacia el punto de vulnerabilidad
    /// </summary>
    [SerializeField]
    [Min(0)]
    [Tooltip("Speed at which the boss will move towards the vulnerable position")]
    float _movementSpeed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// El healthManager del boss
    /// </summary>
    HealthManager _healthManager;

    /// <summary>
    /// Tiempo en el que dejará de ser vulnerable
    /// </summary>
    float _endOfVulnerability;

    /// <summary>
    /// Estado de la "animación" de vulnerable
    /// <list>
    /// <item>
    /// 0 = Moviendo hacia la posición de vulnerabilidad
    /// </item>
    /// 
    /// <item>
    /// 1 = Tiempo vulnerable
    /// </item>
    /// 
    /// <item>
    /// 2 = Vuelve a la posición original
    /// </item>
    /// </list>
    /// </summary>
    int _animationState;

    /// <summary>
    /// Posición original
    /// </summary>
    Vector2 _originalPos;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        // Coge referencia al healthmanager
        Ctx?.TryGetComponent<HealthManager>(out _healthManager);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        // Guarda la posición original
        _originalPos = Ctx.Rigidbody.position;
        
        // Comienza la animación en 0
        _animationState = 0;

        // Hace que puedas golpear al jefe
        _healthManager.Inmune = false;
        
        // Pone la animación correcta en el animator
        Ctx.Animator.SetBool("IsVulnerable", true);
    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        // Hace invulnerable al jefe
        _healthManager.Inmune = true;

        // Termina la animación de vulnerable
        Ctx.Animator.SetBool("IsVulnerable", false);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    /// <summary>
    /// Metodo llamado cada frame cuando este es el estado activo de la maquina de estados.
    /// </summary>
    protected override void UpdateState()
    {
        // Estado de ir al punto de vulnerabilidad
        if (_animationState == 0) 
        {
            Ctx.Rigidbody.position = Vector2.MoveTowards(Ctx.Rigidbody.position, (Vector2)_vulnerablePoint.position, _movementSpeed * Time.deltaTime);

            // Una vez llegue al punto designado calcula el tiempo que debe mantenerse ahí y cambia al siguiente estado
            if (Ctx.Rigidbody.position == (Vector2)_vulnerablePoint.position)
            {
                _endOfVulnerability = Time.time + _vulnerableTime;
                _animationState++;
            }
        }

        // Estado de quedarse en el sitio un tiempo
        else if( _animationState == 1 && Time.time > _endOfVulnerability)
        {
            // Una vez termine el tiempo designado de vulnerabilidad cambiamos al estado de volver al punto original
            _animationState++;
        }

        // Estado de volver al punto original
        else if(_animationState == 2)
        {
            Ctx.Rigidbody.position = Vector2.MoveTowards(Ctx.Rigidbody.position, _originalPos, _movementSpeed * Time.deltaTime);
        }
        
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (_animationState == 2 && Ctx.Rigidbody.position == _originalPos)
        {
            // Cambia al siguiente ataque si ha vuelto a la posición original
            Ctx.ChangeState(Ctx.GetStateByName("Flying Charge"));
        }
    }

    #endregion   

} // class BossAirVulnerableState 
// namespace
