//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SoundManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    /// <summary>
    /// AudioSource temporal que se instancia en la escena
    /// </summary>
    [SerializeField] AudioSource _audioSourceObject;
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
    /// Patron singleton
    /// </summary>
    public static SoundManager Instance;
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

    /// <summary>
    /// singleton
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion
    /// <summary>
    /// Crea un audioSouce temporal en la escena para producir el sonido y despues se autodestruye
    /// </summary>
    /// <param name="audioClip"></param> audioClip que quieres reproducir
    /// <param name="spawnPosition"></param> Posicion donde se reproduce el audio
    /// <param name="volume"></param> volumen del audio
    public void PlaySFX(AudioClip audioClip, Transform spawnPosition, float volume)
    {
        AudioSource audioSource = Instantiate(_audioSourceObject, spawnPosition.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLengh = audioSource.clip.length;
        Destroy(audioSource, clipLengh);
    }

    /// <summary>
    ///     /// Crea un audioSouce temporal en la escena para producir un sonido random entre el array de audio dados y despues se autodestruye
    /// </summary>
    /// <param name="audioClip"></param> array de audioClip que quieres reproducir de manera random
    /// <param name="spawnPosition"></param> Posicion donde se reproduce el audio
    /// <param name="volume"></param> volumen del audio
    public void PlayRandomSFX(AudioClip[] audioClip, Transform spawnPosition, float volume)
    {
        int random = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(_audioSourceObject, spawnPosition.position, Quaternion.identity);
        audioSource.clip = audioClip[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLengh = audioSource.clip.length;
        Destroy(audioSource, clipLengh);
    }
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class SoundManager 
// namespace
