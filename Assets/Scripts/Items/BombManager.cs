//---------------------------------------------------------
// El script que administra las variables, explosiones y animaciones de la bomba del enemigo volador
// Alejandro Menéndez Fierro
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class BombManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField] private float _Explosion_Time = 0.5f;

    [SerializeField] private float _Explosion_Size = 0.5f;

    AudioSource _audiosource;

    [SerializeField] AudioClip _ExplosionEffect;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private Animator _animator;

    private float _time_Spent;

    private bool _exploding = false;

    private bool _alreadydamaged = false;

    private Vector2 position;

    private Rigidbody2D _rb;

    static int _damage = 25;


    #endregion

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
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_exploding)
        {
            _time_Spent += Time.deltaTime; 
            _rb.velocity = Vector3.zero; // La bomba ya no se mueve más tras la explosión. Esto lo hago en caso de que la explosión comience en la esquina de una plataforma.
        }


        if (_time_Spent >= _Explosion_Time)
        {
            Destroy(gameObject);

            Destroy(_audiosource);
        }

        RaycastHit2D playerInRange = Physics2D.BoxCast(position - new Vector2(_Explosion_Size * 2, _Explosion_Size), new Vector2(_Explosion_Size, _Explosion_Size), 0, new Vector2(0, 0));
        if (playerInRange.collider != null && playerInRange.collider.GetComponent<PlayerStateMachine>() != null && !_alreadydamaged && _exploding)
        {
            HealthManager player;

            // Le quita vida al jugador cuando colisiona con la bomba.
            player = playerInRange.collider.gameObject.GetComponent<HealthManager>();
            player?.RemoveHealth(_damage);

            // Una explosión de una bomba no puede dañar al jugador más de una vez.
            _alreadydamaged = true;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_exploding)
        {
            _animator.SetBool("IsExploding", true);

            _exploding = true;

            _audiosource = SoundManager.Instance.PlaySFXWithAudioSource(_ExplosionEffect, transform, 1f);

            position = transform.position + new Vector3(1, 1);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, new Vector2(_Explosion_Size / 2, _Explosion_Size / 2));
    }

    #endregion   

} // class Bomb 
// namespace
