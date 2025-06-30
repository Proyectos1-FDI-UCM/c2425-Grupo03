//---------------------------------------------------------
// El script que controla la mecánica de los orbes mágicos
// Alejandro Menéndez Fierro
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using static PlayerCharge;
using Random = UnityEngine.Random;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MagicOrbManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    [SerializeField, Range(0.001f, 1f)] private float _OrbHealPercentage = 0.2f;

    [SerializeField, Range(0.001f, 1f)] private float _OrbOverchargePercentage = 0.4f;

    [SerializeField] private float _OrbTimetoLive = 10f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private SpriteRenderer sr;

    private float Time_spent;

    private Color orbcolor;

    private int orbtype;

    private Ability abilitychosen;

    private RaycastHit2D playerInRange;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        orbcolor = sr.color;

        // Para cuando esté muy arriba o muy abajo del suelo/techo, se recolocará para que no choque con ninguna plataforma.
        while (!CheckGround())
        {
            transform.position += new Vector3(0f, 0.1f);
        }
        while (!CheckRoof())
        {
            transform.position -= new Vector3(0f, 0.1f);
        }
        // 50% de probabilidad para cada tipo de orbe.
        if (Random.Range(0, 2) == 1)
        {
            orbtype = 0;

            orbcolor.r = 1f;
            orbcolor.g = 1f;
            orbcolor.b = 1f;
        }
        else
        {
            orbtype = 1;

            orbcolor.r = 1f;
            orbcolor.g = 0.5f;
            orbcolor.b = 1f;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Time_spent += Time.deltaTime;

        orbcolor.a = 1 - (Time_spent / _OrbTimetoLive);

        sr.color = orbcolor;
        if (_OrbTimetoLive <= Time_spent)
        {
            Destroy(gameObject);
        }
        else
        {
            playerInRange = Physics2D.BoxCast(transform.position - new Vector3(0, 0.5f), new Vector3(1f, 3.5f), 0, new Vector3(0, 0));
            if (playerInRange.collider != null && playerInRange.collider.GetComponent<PlayerStateMachine>() != null)
            {
                if (orbtype == 0)
                {
                    HealthManager _hpManager;

                    _hpManager = playerInRange.collider.GetComponent<HealthManager>();

                    _hpManager?.Heal(_hpManager.GetMaxHealth() * _OrbHealPercentage);
                }

                if (orbtype == 1)
                {
                    PlayerCharge playerCharge;

                    playerCharge = playerInRange.collider.GetComponent<PlayerCharge>();

                    playerCharge.ChargeRandomAbility(_OrbOverchargePercentage);
                }
                Destroy(gameObject);
            }

        }
    }
    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
                 Vector2.down, 1f, LayerMask.GetMask("Platform"));

        return hit.collider == null;
    }
    private bool CheckRoof()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
                 Vector2.up, 1f, LayerMask.GetMask("Platform"));

        return hit.collider == null;
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

    }
    #endregion
}
// class OrbManager 
// namespace
