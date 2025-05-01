//---------------------------------------------------------
// Este script permite que una entidad muestre el daño que le causan al ser golpeada.
// Creador del Script: Alejandro Menéndez Fierro
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Este script se encarga de mirar la vida de la entidad y mostrar la vida quitada si le han dañado.
/// </summary>
[RequireComponent(typeof(HealthManager))]
public class HarmIndicatorManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// El prefab del texto donde se mostrará el daño.
    /// </summary>
    [SerializeField]
    private Canvas _damageText;

    /// <summary>
    /// El color del los números
    /// </summary>
    [SerializeField]
    private Color _textColor;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Awake()
    {
        // Busca el componente HealthManager entre los componentes del objeto.
        GetComponent<HealthManager>()?._onDamaged.AddListener(AskForHealth);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos
    public void AskForHealth(float damage)
    {
        EnemySummonerStateMachine enemyS = GetComponent<EnemySummonerStateMachine>();

        if ((enemyS == null || !enemyS.IsFirstHit()) && GetComponent<HealthManager>().Inmune != true && GetComponent<HealthManager>().HitButInmune != true)
        {
            // Instancia el texto con el número que va a representar el daño.
            Canvas text = Instantiate<Canvas>(_damageText, gameObject.transform.position, gameObject.transform.rotation);

            // Establece el número que debe representar el texto.
            text.GetComponent<DamageNumberScript>()?.SetText(damage.ToString(), _textColor);
        }
    }
    #endregion

} // class HarmIndicatorManager 
// namespace
