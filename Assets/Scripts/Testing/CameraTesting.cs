//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.ComponentModel;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CameraTesting : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [Header("Pan attributes")]
    [SerializeField] Transform _cameraTarget;
    [SerializeField] float _panTime;
    [SerializeField] float _nextZoomPan;

    [Header("Wait attributes")]
    [SerializeField] float _waitTime;
    [SerializeField] float _nextZoomWait;

    [Header("Follow player attributes")]
    [SerializeField] float _followTime;
    [SerializeField] float _nextZoomFollow;

    [Header("Shake attributes")]
    [SerializeField] float _shakeDuration;
    [SerializeField] float _shakeStrength;

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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    [ContextMenu("Pan Camera")]
    public void PanCamera()
    {
        CameraManager.Instance.EnqueueInstruction(new CameraPan(_cameraTarget.position, _panTime, _nextZoomPan));
    }
    [ContextMenu("Wait Camera")]
    public void WaitCamera()
    {
        CameraManager.Instance.EnqueueInstruction(new CameraWait(_waitTime, _nextZoomWait));

    }
    [ContextMenu("Follow Player Camera")]
    public void FollowPlayerCamera()
    {
        CameraManager.Instance.EnqueueInstruction(new CameraFollowPlayer(_followTime, _nextZoomFollow));

    }
    [ContextMenu("Shake Camera")]
    public void CameraShake()
    {
        CameraManager.Instance.ShakeCamera(_shakeDuration, _shakeStrength);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class CameraTesting 
// namespace
