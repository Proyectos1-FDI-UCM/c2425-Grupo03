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
    /// <summary>
    /// El siguiente valor de zoom deseado.
    /// </summary>
    protected float _nextZoom;
    /// <summary>
    /// La velocidad a la que se debe aumentar el zoom por segundo para llegar al zoom deseado al final de la instrucción.
    /// </summary>
    protected float _zoomSpeed;
    /// <summary>
    /// El momento desde el comienzo del programa cuando termina la instrucción. (Esta en segundos)
    /// </summary>
    protected float _endTime;
    /// <summary>
    /// Cuanto dura la instrucción en segundos.
    /// </summary>
    private float _actionLength;

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

    /// <summary>
    /// Método llamado cuando se quita de la queue a la instrucción.
    /// </summary>
    public virtual void OnDequeued() { }
    #endregion
} // class CameraInstructions 

/// <summary>
/// Instrucción de la cámara que permite mover la cámara hacia una posición.
/// </summary>
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
    /// <summary>
    /// Constructor de la instucción CameraPan.
    /// </summary>
    /// <param name="target">El objetivo hacia el que mover la cámara.</param>
    /// <param name="actionLength">La duración deseada del paneo.</param>
    /// <param name="nextZoom">El zoom deseado al final del paneo.</param>
    public CameraPan(Vector3 target, float actionLength, float nextZoom) : base(actionLength, nextZoom)
    {
        _target = target;
        _cameraSpeed = (target - CameraManager.Instance.Camera.transform.position).magnitude / actionLength;
    }

    public override bool UpdateCamera(Camera camera)
    {
        //Si hemos llegado al objetivo termina la instrucción
        if (camera.transform.position.sqrMagnitude == _target.sqrMagnitude)
        {
            return true;
        }

        //Movemos la cámara hacia el objetivo
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, _target, _cameraSpeed * Time.deltaTime);
        //Actualizamos el zoom
        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, _nextZoom, _zoomSpeed * Time.deltaTime);

        return false;
    }
    #endregion
}

/// <summary>
/// Instrucción de la cámara que permite hacer a la cámara esperar un tiempo determinado.
/// </summary>
public sealed class CameraWait : CameraInstruction
{
    // ---- MÉTODOS PUBLICOS
    #region Métodos Públicos
    /// <summary>
    /// Constructor de la instucción CameraWait.
    /// </summary>
    /// <param name="actionLength">Duración de espera hasta la siguiente instrucción.</param>
    /// <param name="nextZoom">Zoom deseado al final de la espera.</param>
    public CameraWait(float actionLength, float nextZoom) : base(actionLength, nextZoom) { }

    public override bool UpdateCamera(Camera camera)
    {
        // Si el zoom ha sido alcanzado y ha terminado el tiempo de espera termina la instrucción
        if(camera.orthographicSize == _nextZoom && Time.time > _endTime)
        {
            return true;
        }

        // Actualizamos el zoom
        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, _nextZoom, _zoomSpeed * Time.deltaTime);

        return false;
    }
    #endregion
}

/// <summary>
/// Instrucción de la cámara que permite hacer que la cámara siga al jugador un cierto tiempo. <b>(Si no hay más instrucciones lo sigue siguiendo)</b>
/// </summary>
public sealed class CameraFollowPlayer : CameraInstruction
{

    // ---- MÉTODOS PUBLICOS
    #region Métodos Públicos
    /// <summary>
    /// Constructor de la instrucción CameraFollowPlayer
    /// </summary>
    /// <param name="actionLength">Durante cuanto tiempo debe seguir al jugador.</param>
    /// <param name="nextZoom">Zoom deseado al final de la instrucción.</param>
    public CameraFollowPlayer(float actionLength, float nextZoom) : base(actionLength, nextZoom) { }

    public override bool UpdateCamera(Camera camera)
    {
        // Cuando se ha alcanzado el tiempo indicado y el zoom indicado termina la acción
        if (camera.orthographicSize == _nextZoom && Time.time > _endTime)
        {
            return true;
        }
        else
        {
            // Se le indica al manager que siga al jugador
            CameraManager.Instance.IsFollowingPlayer = true;
            // Se actualiza el zoom
            camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, _nextZoom, _zoomSpeed * Time.deltaTime);

            return false;
        }
    }

    public override void OnDequeued()
    {
        // Cuando comienze una nueva instrucción le avisamos al manager que deje de seguir al jugador.
        CameraManager.Instance.IsFollowingPlayer = false;
    }
    #endregion
}
// namespace
