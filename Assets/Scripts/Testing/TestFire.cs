//---------------------------------------------------------
// Componente de test para probar el input de disparo 
// Guillermo Jiménez Díaz
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Componente de prueba que se comunica con el InputManager
/// para mostrar por consola los eventos de la acción Fire.
/// Como los eventos IsPressed se muestran cada frame y
/// saturan la consola, tenemos un tick en el editor para
/// habilitarlos
/// </summary>
public class TestFire : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)
    
    /// <summary>
    /// Si está activado, se muestran todos los eventos de que
    /// la acción está siendo realizada (uno por frame)
    /// </summary>
    [SerializeField]
    private bool displayIsPressed = false;

    #endregion



    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.FireWasPressedThisFrame())
            Debug.Log($"{Time.frameCount}[{Time.deltaTime}]: Fire was pressed this frame");

        if (InputManager.Instance.FireWasReleasedThisFrame())
            Debug.Log($"{Time.frameCount}[{Time.deltaTime}]: Fire was released this frame");

        if (displayIsPressed && InputManager.Instance.FireIsPressed())
        {
            Debug.Log($"{Time.frameCount}[{Time.deltaTime}]: Fire was pressed");
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

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion
} // class TestFire 
// namespace