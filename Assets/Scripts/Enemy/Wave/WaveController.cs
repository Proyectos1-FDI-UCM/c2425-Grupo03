//---------------------------------------------------------
// Gestor de Oleadas- gestiona el inicio y cambios de oleadas de enemigos
// Santiago Salto Molodojen
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestiona las oleadas de enemigos y activa la puerta para seguir el nivel
/// </summary>
public class WaveController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Puerta que se abre después de las oleadas
    /// </summary>
    [SerializeField] GameObject _door;

    [SerializeField] AudioClip _doorOpen;
    [SerializeField] AudioClip _doorClose;
    [SerializeField] AudioClip _appearEnemy;

    /// <summary>
    /// El tamaño de la camara cuando entra en la zona
    /// </summary>
    [SerializeField] private int _zoom;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Numero de la oleada activa
    /// </summary>
    int _numWave;

    /// <summary>
    /// Comprobar si estan activas las oleadas
    /// </summary>
    bool _endWaves;

    /// <summary>
    /// Animator de las puertas
    /// </summary>
    private Animator _doorAnimator;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    private void Awake()
    {
        // Desactiva todos los hijos al inicio
        foreach (Transform child in transform) 
        {
            child.gameObject.SetActive(false);
        }

        // elimina aquellos emptys que no tengan enemigos dentro
        foreach (Transform child in transform) 
        {
            if (child.childCount == 0) 
            {
                Destroy(child.gameObject); 
            }
        }

        //especificamos que la oleada activada es la primera
        _numWave = 0;

        //Informacion de las puertas
        _doorAnimator = _door?.gameObject.GetComponent<Animator>();
        _door?.GetComponent<EndOfWaves>().SetWaveController(this.gameObject);
    }

    void Update()
    {
        if (!_endWaves)
        {
            // Si no tiene hijos
            if (transform.GetChild(_numWave).childCount == 0) 
            {
                // Lo eliminamos La oleada actual
                Destroy(transform.GetChild(_numWave).gameObject);

                // Activamos el siguiente oleada
                NextWave(); 
            }
        }
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        //Añadir el cheat al jugador cuando entra
        InputManager.Instance._skipWaveEvent.AddListener(DestroyWave);
        // Activa solo la primera oleada, cierra la puerta y activa el estado de oleada


        if (transform.childCount > 0) 
        {
            _doorAnimator.SetBool("Closed",true);
            SoundManager.Instance.PlaySFX(_doorClose, transform, 1);
            transform.GetChild(_numWave).gameObject.SetActive(true);
            _endWaves = false;
            if(TryGetComponent(out BoxCollider2D trigger))
            {
                trigger.enabled = false;
            }
        }

        // Cambia la cámara al centro del área de combate (el objeto vacío con este script)
        SoundManager.Instance.PlaySFX(_appearEnemy, transform, 0.7f);
        CameraManager.Instance.EnqueueInstruction(new CameraPan(this.transform.position, 1, _zoom));
    }
    #endregion


    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Activación de la siguiente oleada
    /// </summary>
    void NextWave()
    {
        // si existen más hijos a parte del actual activar el siguiente
        if (transform.childCount > 1)
        {
            _numWave++;
            transform.GetChild(_numWave).gameObject.SetActive(true);
            _numWave = 0;
            SoundManager.Instance.PlaySFX(_appearEnemy, transform, 0.7f);
        }
        // si no hay mas oleadas activar puerta, colocar la cámara en el jugador y terminar oleada
        else
        {
            SoundManager.Instance.PlaySFX(_doorOpen, transform, 1);
            _doorAnimator.SetBool("Closed", false);
            _endWaves = true;
            CameraManager.Instance.EnqueueInstruction(new CameraFollowPlayer(1, 6));
        }
    }

    #endregion

    /// <summary>
    /// Metodo que sirve para destruir el waveController directamente
    /// </summary>
    private void DestroyWave()
    {
        //Quitar el cheat al jugador para evitar posibles problemas
        InputManager.Instance._skipWaveEvent.RemoveAllListeners();


        _doorAnimator.SetBool("Closed", false);
        CameraManager.Instance.EnqueueInstruction(new CameraFollowPlayer(1, 6));
        Destroy(this.gameObject);
    }

} // class WaveController 
// namespace
