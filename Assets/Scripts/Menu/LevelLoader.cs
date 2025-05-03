//---------------------------------------------------------
// Breve descripción del contenido del archivo
// He Deng
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// La transicion a otra escena 
/// </summary>
public class LevelLoader : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.

    /// <summary>
    /// El nombre de la escena a cambiar
    /// </summary>
    [SerializeField] private string _sceneName;

    /// <summary>
    /// El tiempo que dura la transicion
    /// </summary>
    [SerializeField] private float _transitionTime;

    /// <summary>
    /// Los animators de las animaciones
    /// </summary>
    [SerializeField] private Animator[] _transition;

    private bool _isLoading = false;
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
    /// El healthManager del jugador
    /// </summary>
    private HealthManager _healthManager;

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
    /// 
    private void Start()
    {
        //Buscar el Player y coger su HealthManager
        _healthManager = FindFirstObjectByType<PlayerStateMachine>()?.GetComponent<HealthManager>();
        //Añadir la animacion de transicion a cuando muere el jugador
        _healthManager?._onDeath.AddListener(ReloadSceneAnimation);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_isLoading) return;
        //Cuando el jugador se encuentra en el fin de nivel cambia de escena
        _isLoading = true;
        ChangeScene(_sceneName, _transitionTime);

        //Resetear el indice del checkpoint
        CheckpointManager.Instance.ResetCheckpoint();
    }


    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Metodo que realiza el cambio de la escena que se puede llamar desde fuera
    /// </summary>
    public void ChangeScene(string sceneName, float transitionTime = 1)
    {
        GameManager.Instance.InitCheckpoint(); // resetea los checkpoint activados
        GameManager.Instance.AddActualLevel();
        StartCoroutine(LoadLevel(sceneName,transitionTime));
    }
    /// <summary>
    /// Metodo que hace la animacion de la recarga de la escena
    /// </summary>
    public void ReloadSceneAnimation()
    {
        StartCoroutine(OnDeath());
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Corutina que 
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadLevel(string sceneName, float transitionTime = 1)
    {
        //Hacer la transicion
        GetAnimator("Crossfade")?.SetTrigger("Start");

        //Esperar
        yield return new WaitForSeconds(transitionTime);

        //Hacer el cambio de la escena
        SceneManager.LoadScene(sceneName);

        //Cuando realiza un cambio de escena, ejecuta el metodo
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    }

    /// <summary>
    /// Metodo subscrito al cambio de escena con el level loader
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        CheckpointManager.Instance?.ResetCheckpoint();
    }

    private IEnumerator OnDeath(float transitionTime = 1)
    {
        //Hacer la transicion
        GetAnimator("Crossfade")?.SetTrigger("Start");

        //Esperar
        yield return new WaitForSeconds(transitionTime);
    }

    /// <summary>
    /// Buscar el animator con el nombre correspondiente en el array, hecho de esta forma para cuando queramos meter mas transiciones
    /// No es muy necesario, se puede coger el animator directamente
    /// </summary>
    /// <param name="transitionName"></param>
    /// <returns></returns>
    private Animator GetAnimator(string transitionName)
    {
        //Buscar en el array el animador con el nombre correspondiente
        int i = 0;
        while (_transition[i].name != transitionName) i++;
        //Devolver el animator correspondiente
        return _transition[i];
    }

    #endregion   

} // class LevelLoader 
// namespace
