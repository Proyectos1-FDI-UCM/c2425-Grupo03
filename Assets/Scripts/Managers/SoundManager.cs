//---------------------------------------------------------
// Archivo con la clase SoundManager para reproducir sonidos desde cualquier lugar
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Manager para poder reproducir sonidos desde cualquier script.
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


    // ---- ATRIBUTOS PUBLICOS ----
    #region Atributos Publicos
    /// <summary>
    /// Patron singleton
    /// </summary>
    public static SoundManager Instance;
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

    /// <summary>
    /// Crea un audioSouce temporal en la escena para producir el sonido y despues se autodestruye
    /// </summary>
    /// <param name="audioClip"> audioClip que quieres reproducir</param>
    /// <param name="spawnPosition"> Posicion donde se reproduce el audio</param>
    /// <param name="volume"> volumen del audioClip </param>
    public void PlaySFX(AudioClip audioClip, Transform spawnPosition, float volume)
    {
        AudioSource audioSource = Instantiate(_audioSourceObject, spawnPosition.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLengh = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLengh);
    }
    /// <summary>
    /// Metodo que hace lo mismo que PlaySFX pero devolviendo el audioSource para que lo pueda manejar otros
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    public AudioSource PlaySFXWithAudioSource(AudioClip audioClip, Transform spawnPosition, float volume)
    {
        AudioSource audioSource = Instantiate(_audioSourceObject, spawnPosition.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        audioSource.loop = true;

        return audioSource;
    }

    /// <summary>
    ///     Crea un audioSouce temporal en la escena para producir un sonido random entre el array de audio dados y despues se autodestruye
    /// </summary>
    /// <param name="audioClip"> array de audioClip que quieres reproducir de manera random</param>
    /// <param name="spawnPosition">Posicion donde se reproduce el audio</param> 
    /// <param name="volume">volumen del audio</param> 
    public void PlayRandomSFX(AudioClip[] audioClip, Transform spawnPosition, float volume)
    {
        int random = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(_audioSourceObject, spawnPosition.position, Quaternion.identity);
        audioSource.clip = audioClip[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLengh = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLengh);
    }
    #endregion

} // class SoundManager 
// namespace
