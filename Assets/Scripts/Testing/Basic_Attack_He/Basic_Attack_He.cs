//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Basic_Attack_He : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    #endregion
    [Header("Propiedad del ataque")]
    [SerializeField,Min(1)] float _attackRadius;
    [SerializeField] float _attackSpeed;
    [Header("Propiedad del combo")]
    [SerializeField,Min(1)] float _comboDuration;
    [SerializeField] int _combo;
    [SerializeField] int _comboExtraDamage;
    [Space(20f)]
    [SerializeField] int _enemyCount;
    [SerializeField] float _damage;
    
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion
    private CircleCollider2D _circleCollider;
    private PlayerInputActions _playerInput;
    private InputAction _attack;
    [SerializeField] private float _countAttackTime = 0;
    [SerializeField] private bool _hadAttacked;
    [SerializeField] private float _comboTime = 0;

    private List<Collider2D> _enemyInAttackRange = new List<Collider2D>();

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _enemyCount = 0;
        _playerInput = new PlayerInputActions();
        _attack = _playerInput.Player.Attack;
        _attack.Enable();

        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = _attackRadius;

        _hadAttacked = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        CheckAttackSpeed(_attackSpeed, ref _hadAttacked);
        if (_attack.triggered && !_hadAttacked)
        {
            _hadAttacked = true;
            CheckCombo(ref _combo);
            Attacking();
        }
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   
    private void Attacking()
    {
        int extra = 0;
        Debug.Log($"Attacking, attack radius: {_attackRadius}, enemy in attack radius: {_enemyCount}, actual combo: {_combo}");
        if (_combo == 2) extra += _comboExtraDamage;
        foreach(Collider2D enemy in _enemyInAttackRange)
        {
            enemy.gameObject.GetComponent<enemy>().RemoveHealth(_damage + extra);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemyCount++;
        _enemyInAttackRange.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _enemyCount--;
        _enemyInAttackRange.Remove(collision);
    }

    private void CheckCombo(ref int _combo)
    {
        if (_combo == 1) _combo = 2;
        else if (_combo == 2) _combo = 3;
        else _combo = 1;
    }

    private void CheckAttackSpeed(float _attackSpeed, ref bool _hadAttacked)
    {
        if (_hadAttacked) _countAttackTime = _countAttackTime + Time.deltaTime;
        if (_countAttackTime > _attackSpeed)
        {
            _countAttackTime = 0;
            _hadAttacked = false;
        }
    }

    private void CheckComboDuration()
    {
        _comboTime += _comboTime + Time.deltaTime;
        if ( _comboTime > _comboDuration )
        {
            _combo = 1;
            _comboTime = 0;
        }
    }


} // class Basic_Attack_He 
// namespace
