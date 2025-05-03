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
    [SerializeField]
    [Min(0)]
    float _vulnerableTime;

    [SerializeField]
    Transform _vulnerablePoint;

    [SerializeField]
    float _movementSpeed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    HealthManager _healthManager;
    float _endOfVulnerability;
    int _animationState;
    Vector2 _originalPos;

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
        _originalPos = Ctx.Rigidbody.position;
        _animationState = 0;
        _healthManager.Inmune = false;
        Ctx.Animator.SetBool("IsVulnerable", true);
    }

    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        _healthManager.Inmune = true;
        Ctx.Animator.SetBool("IsVulnerable", false);
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
        if (_animationState == 0) 
        {
            Ctx.Rigidbody.position = Vector2.MoveTowards(Ctx.Rigidbody.position, (Vector2)_vulnerablePoint.position, _movementSpeed * Time.deltaTime);
            if (Ctx.Rigidbody.position == (Vector2)_vulnerablePoint.position)
            {
                _endOfVulnerability = Time.time + _vulnerableTime;
                _animationState++;
            }
        }
        else if( _animationState == 1 && Time.time > _endOfVulnerability)
        {
            _animationState++;
        }
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
            Ctx.ChangeState(Ctx.GetStateByName("Flying Charge"));
        }
    }

    #endregion   

} // class BossAirVulnerableState 
// namespace
