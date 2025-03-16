//---------------------------------------------------------
// Se definen las instrucciones que se le pueden dar a la cámara
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// La clase abstracta de instrucciones para la cámara.
/// Todas las instrucciones de cámara heredan de esta clase.
/// </summary>
public abstract class CameraInstruction
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    protected float _nextZoom;
    protected float _zoomSpeed;
    protected float _endTime;
    private float _actionLength;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion


    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Constructor de CameraInstruction
    /// </summary>
    /// <param name="actionLength">El tiempo que tarda en ejecutarse la instrucción.</param>
    /// <param name="nextZoom">El zoom final deseado.</param>
    public CameraInstruction(float actionLength, float nextZoom)
    {
        _actionLength = actionLength;
        _nextZoom = nextZoom;
    }
    public void SetUp()
    {
        _endTime = Time.time + _actionLength;
        _zoomSpeed = _nextZoom / _actionLength;
    }

    /// <summary>
    /// Actualiza la cámara.
    /// </summary>
    /// <param name="camera">La cámara a actualizar.</param>
    /// <returns>Devuelve true si ha terminado de ejecutar la orden.</returns>
    public virtual bool UpdateCamera(UnityEngine.Camera camera)
    {
        throw new UnityException("Not implemented camera method");
    }

    public virtual void OnDequeued()
    {

    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class CameraInstructions 

public sealed class CameraPan : CameraInstruction
{
    // ---- ATRIBUTOS PRIVADOS
    #region Atributos Privados
    /// <summary>
    /// La posición final deseada tras hacer el pan.
    /// </summary>
    Vector3 _target;

    /// <summary>
    /// La velocidad de paneo de la cámara.
    /// </summary>
    float _cameraSpeed;
    #endregion region

    // ---- MÉTODOS PUBLICOS
    #region Métodos Públicos
    public CameraPan(Vector3 target, float actionLength, float nextZoom) : base(actionLength, nextZoom)
    {
        _target = target;
        _cameraSpeed = (target - CameraManager.Instance.Camera.transform.position).magnitude / actionLength;
    }

    public override bool UpdateCamera(Camera camera)
    {
        if (camera.transform.position.sqrMagnitude == _target.sqrMagnitude)
        {
            return true;
        }

        camera.transform.position = Vector3.MoveTowards(camera.transform.position, _target, _cameraSpeed * Time.deltaTime);
        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, _nextZoom, _zoomSpeed * Time.deltaTime);

        return false;
    }
    #endregion
}

public sealed class CameraWait : CameraInstruction
{
    // ---- MÉTODOS PUBLICOS
    #region Métodos Públicos
    public CameraWait(float actionLength, float nextZoom) : base(actionLength, nextZoom)
    {

    }

    public override bool UpdateCamera(Camera camera)
    {
        if(camera.orthographicSize == _nextZoom && Time.time > _endTime)
        {
            return true;
        }

        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, _nextZoom, _zoomSpeed * Time.deltaTime);

        return false;
    }
    #endregion
}

public sealed class CameraFollowPlayer : CameraInstruction
{
    // ---- ATRIBUTOS PRIVADOS
    #region Atributos Privados
    #endregion region

    // ---- MÉTODOS PUBLICOS
    #region Métodos Públicos
    public CameraFollowPlayer(float actionLength, float nextZoom) : base(actionLength, nextZoom)
    {
    }

    public override bool UpdateCamera(Camera camera)
    {
        if (camera.orthographicSize == _nextZoom && Time.time > _endTime)
        {
            return true;
        }
        else
        {
            CameraManager.Instance.IsFollowingPlayer = true;
            camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, _nextZoom, _zoomSpeed * Time.deltaTime);
            Debug.Log(camera.orthographicSize);
            return false;
        }
    }

    public override void OnDequeued()
    {
        CameraManager.Instance.IsFollowingPlayer = false;
    }
    #endregion
}
// namespace
