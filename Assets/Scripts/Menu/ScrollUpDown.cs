//---------------------------------------------------------
// Script que permite mover un objeto arriba o abajo en el mundo
// Programado específicamente para los créditos
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script para mover un objeto arriba o abajo con las teclas de arriba o abajo.
/// Programado específicamente para los créditos.
/// </summary>
public class ScrollUpDown : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Velocidad de movimiento del texto
    /// </summary>
    [SerializeField]
    [Min(0)]
    float _scrollSpeed;

    [SerializeField]
    RectTransform _rectTransform;

    [SerializeField]
    float _scrollAmount;

    #endregion

    InputAction _action;
    float _currScrollAmount;
    
    private void Start()
    {
        InputManager.Instance.GetInputActions().UI.Enable();
        _action = InputManager.Instance.GetInputActions().UI.Navigate;
    }
    private void Update()
    {
        Vector2 input = _action.ReadValue<Vector2>();

        if (input == Vector2.up && _currScrollAmount < _scrollAmount)
        {
            _rectTransform.position += Vector3.up * _scrollSpeed * Time.deltaTime;
            _currScrollAmount += _scrollSpeed * Time.deltaTime;
        }
        else if (input == Vector2.down && _currScrollAmount > 0)
        {
            _rectTransform.position += Vector3.down * _scrollSpeed * Time.deltaTime;
            _currScrollAmount -= _scrollSpeed * Time.deltaTime;
        }
        
    }

} // class ScrollUpDown 
// namespace
