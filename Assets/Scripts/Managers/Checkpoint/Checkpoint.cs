//---------------------------------------------------------
// Clase que se encarga de la lógica de los checkpoints
// Zhiyi Zhou
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Clase que se encarga de la lógica de los checkpoints
/// </summary>
public class Checkpoint : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// Índice del checkpoint para identificar si ya ha sido activado
    /// </summary>
    [SerializeField] int _checkPointIndex;
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
    /// Para evitar reactivación
    /// </summary>
    private bool _isActivated = false;

    /// <summary>
    /// Referencia al componente Animator
    /// </summary>
    private Animator _animator;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    void Start()
    {
        _animator = GetComponent<Animator>(); 

        //Verifica si el checkpoint fue activado
        if (GameManager.Instance.IsActivated(_checkPointIndex))
        {
            _isActivated = true;
            _animator?.SetTrigger("CpAppear");
        }

        CheckpointManager.Instance.AddCheckPointData(_checkPointIndex, transform.position);

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public int GetCheckPointIndex()
    {
        return _checkPointIndex;
    }

    #endregion


    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Método que llama cuando otro collider entra en el trigger de este objeto
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();

        healthManager.SetHealth(int.MaxValue); // Curará al jugador por completo y al instante sin importar la vida que tenga.
        Debug.Log("Jugador curado con éxito");

        if (collision.gameObject.GetComponent<PlayerStateMachine>() != null && !_isActivated)
        {
            _isActivated = true;

            //Notificar al CheckpointManager que este es el último checkpoint
            CheckpointManager.Instance.SetCheckpoint(this.transform);

            // Guarda el checkpoint en el GameManager
            GameManager.Instance.AddCheckpoint(_checkPointIndex);

            if (_animator != null)
            {
                _animator.SetTrigger("CpAppear");
            }
        }
    }

    #endregion   

} // class Checkpoint 
// namespace
