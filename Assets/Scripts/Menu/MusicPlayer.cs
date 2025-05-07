//---------------------------------------------------------
// Script para poner musica
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    public static MusicPlayer Instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] AudioClip _menuMusic;
    [SerializeField] AudioClip _levelMusic;
    [SerializeField] private AudioClip _tutorialMusic;
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
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    // ---- MÉTODOS PUBLICOS ----
    #region Métodos públicos

    public void PauseMusic()
    {
        _audioSource.Pause();
    }

    public void PlayMusic()
    {
        _audioSource.Play();
    }
    public void PlayMenuSound()
    {
        _audioSource.clip = _menuMusic;
        _audioSource.Play();
    }
    public void PlayLevelSound()
    {
        _audioSource.clip = _levelMusic;
        _audioSource.Play();
    }

    public void PlayTutorialSound()
    {
        _audioSource.clip = _tutorialMusic;
        _audioSource.Play();
    }
    #endregion

} // class DontDestroy 
// namespace
