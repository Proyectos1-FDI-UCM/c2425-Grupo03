//---------------------------------------------------------
// Hace que un objeto se deslice en una dirección hasta
// donde se puso inicialmente en el editor al ser activado
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Hace que un objeto se deslice en una dirección al ser activado
/// </summary>
public class SlideIn : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    Vector2 _slideDirection;
    [SerializeField]
    float _slideAmount;
    [SerializeField]
    float _slideSpeed;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    Vector2 _originalPosition;
    bool _reachedEnd;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }
    private void OnEnable()
    {
        transform.localPosition -= (Vector3)_slideDirection * _slideAmount;   
        _reachedEnd = false;
    }

    private void Update()
    {
        if(Vector2.Distance((Vector2)transform.localPosition, _originalPosition) < 0.1f)
        {
            _reachedEnd=true;
        }

        if(!_reachedEnd)
        {
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, _originalPosition, _slideSpeed*Time.unscaledDeltaTime);
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

} // class SlideIn 
// namespace
