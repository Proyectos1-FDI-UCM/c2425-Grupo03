//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerMarDeLosLamentos : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    [SerializeField] private float _skillDamage = 10f;  // Daño de la habilidad
    [SerializeField] private float _effectDistance = 5f;  // Distancia de efecto
    [SerializeField] private float _waveSpeed = 3f;  // Velocidad de avance de las ondas
    [SerializeField] private float _skillDuration = 2f;  // Duración de la habilidad
    [SerializeField] private float _animationTime = 1f;
    [SerializeField] private GameObject _wavePrefab; // Prefab de la onda
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    PlayerStateMachine _ctx;
    private Animator _animator;
    float _startTime = 0;

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
        _ctx = GetCTX<PlayerStateMachine>();
        _animator = GetComponent<Animator>();
    }
    private void CastSkill()
    {   
        // Crear ondas a ambos lados del jugador
        CreateWave(Vector2.right);
        CreateWave(Vector2.left);

    }
    private void CreateWave(Vector2 direction)
    {
        GameObject wave = Instantiate(_wavePrefab, transform.position, Quaternion.identity);
        WaveBehavior waveBehavior = wave.GetComponent<WaveBehavior>();
        waveBehavior.Initialize(direction, _effectDistance, _waveSpeed, _skillDuration, _skillDamage);
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
    /// </summary>
    public override void EnterState()
    {
        _animator = GetCTX<PlayerStateMachine>().Animator;
        _startTime = Time.time;
        // Llamar a la habilidad
        CastSkill();

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
            Ctx.ChangeState(_ctx.GetStateByType<PlayerGroundedState>());
        }
    }

    #endregion   

} // class PlayerMarDeLosLamentos 
// namespace
