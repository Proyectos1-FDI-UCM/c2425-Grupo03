//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using Unity.Burst.Intrinsics;
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
    [SerializeField, Min(0)] float _attackRadius;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _damage;
    [Header("Propiedad del combo")]
    [SerializeField, Min(1)] float _comboDuration;

    [SerializeField] int _comboExtraDamage;
    [SerializeField] float _endOfCombo;

    [Space(20f)]




    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion
    private PlayerInputActions _playerInput;
    private InputAction _attack;
    private int _combo;

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion
    public float NextAttackTime{ get; private set; }


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
        _playerInput = new PlayerInputActions();
        _attack = _playerInput.Player.Attack;
        _attack.Enable();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Time.time > _endOfCombo) _combo = 1;

        if (_attack.triggered && Time.time > NextAttackTime)
        {
            NextAttackTime = Time.time + _attackSpeed;
            UpdateCombo();
            Attack(1);
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
    private void Attack(int direction)
    {
        int extraDamage = 0;
        Vector2 position = transform.position + (new Vector3(_attackRadius,0) * direction);
        RaycastHit2D[] enemyInArea;

        enemyInArea = Physics2D.CircleCastAll(position, _attackRadius, new Vector2(0,0), _attackRadius, 1 << 10);

        if (_combo == 3) extraDamage += _comboExtraDamage;

        foreach (RaycastHit2D enemy in enemyInArea)
        {
            enemy.collider.GetComponent<enemy>().RemoveHealth(_damage + extraDamage);
        }
    }
    private void UpdateCombo()
    {
        _endOfCombo = Time.time + _comboDuration;

        if (_combo == 1) _combo = 2;
        else if (_combo == 2) _combo = 3;
        else _combo = 1;
    }


} // class Basic_Attack_He 
// namespace
