//---------------------------------------------------------
// Este script se encuentra en el prefab de Dummy y administra el daño
// que han sufrido cada uno de los dummies en la escena desde su ejecución.
// Por ahora, pulsa la tecla espacio para que el dummy (o los dummies) reciba(n)
// 1 de daño.
// Creador del Script: Alejandro Menéndez Fierro
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
public class HarmManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField]
    private HarmIndicatorManager HarmIndicatorManager;
    [SerializeField]
    private int damagecaused = 0;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos privados
    private Vector2 Dummypos; // La posición del dummy colocado en la escena.
    #endregion
    // ---- PROPIEDADES ----
    // No hay propiedades en el script
    #region Propiedades

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    void Awake()
    {
        Dummypos = transform.position; // Establecer posición del Dummy
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        ///<summary> 
        /// Esta condición se debería quitar tan pronto como se implemente 
        /// la forma golpear al enemigo. Con la ayuda de esta condición he podido 
        /// comprobar si funcionaba el método del Script correctament e.
        ///</summary>

        {
            DamageDummy(1); // Llama al método "DamageDummy" para que la variable "Damagecaused"
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    /// <summary>
    /// Aquí se encuentra el método principal del script, el encargado de sumar el
    /// valor actual del daño total por el valor que se le añada a su llamada.
    /// </summary>
    #region Métodos públicos
    public void DamageDummy(int damage)
    {
        damagecaused += damage;
        HarmIndicatorManager.UpdateDamageText(damagecaused);
    }
    ///<summary>
    /// Este es el método encargado de aumentar la variable del daño causado en total 
    /// ("damagecaused"), y lo suma por el valor de daño que ha recibido. Además, también 
    /// llama al método de "HarmIndicatorManager" para que actualice el texto TextMeshPro 
    /// al nuevo valor de "damagecaused".
    /// </summary>

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    // No se necesitan método privados en el script.
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra).
    #endregion

} // class HarmManager 
// Alejandro Menéndez Fierro
