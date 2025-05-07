//---------------------------------------------------------
// Breve descripción del contenido del archivo //Poner descripcion
// Responsable de la creación de este archivo //Poner nombre
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class IsGroundedCheck : MonoBehaviour
{
    // ---- ATRIBUTOS DE INSPECTOR ----
    #region Atributos de Inspector 
    [SerializeField]
    float _distanceToFloor;

    #endregion
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool _isGrounded;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _isGrounded = true;
        }

    }
    /// <summary>
    /// El trigger debe solo tocar la layer del suelo.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            // Al salir de un trigger hacemos una comprobación por si todavía estamos tocando otro trigger de suelo
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _distanceToFloor, 1 << 7);
            if(hit.collider == null)
            {
                _isGrounded = false;
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public bool IsGrounded()
    {
        return _isGrounded;
    }

    #endregion


} // class IsGroundedCheck 
// namespace
