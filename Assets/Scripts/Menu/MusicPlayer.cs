//---------------------------------------------------------
// Script para poner musica
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
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
    [SerializeField] private AudioSource _audioSource2;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _levelMusic;
    [SerializeField] private AudioClip _tutorialMusic;
    [SerializeField] private AudioClip _bossPhase1Music;
    [SerializeField] private AudioClip _bossPhase2Music;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private AudioSource _currentSource;
    private AudioSource _nextSource;
    private Coroutine _crossfadeCoroutine;
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
            _currentSource = _audioSource;
            _nextSource = _audioSource2;
        }
    }
    #endregion

    // ---- MÉTODOS PUBLICOS ----
    #region Métodos públicos

    public void PauseMusic()
    {
        _currentSource.Stop();
        _nextSource.Stop();
    }

    public void PlayMusic()
    {
        _currentSource.Play();
    }
    public void PlayMenuSound()
    {
        /*_audioSource.Stop();
       _audioSource.clip = _menuMusic;
       _audioSource.Play();*/
        CrossfadeTo(_menuMusic, 1.0f);
    }
    public void PlayLevelSound()
    {
        /*_audioSource.Stop();
       _audioSource.clip = _levelMusic;
       _audioSource.Play();*/
        CrossfadeTo(_levelMusic, 1.0f);
    }

    public void PlayTutorialSound()
    {
        /*_audioSource.Stop();
        _audioSource.clip = _tutorialMusic;
        _audioSource.Play();*/
        CrossfadeTo(_tutorialMusic, 1.0f);
    }

    public void PlayBossPhase1Sound()
    {
        /*_audioSource.Stop();
        _audioSource.clip = _bossPhase1Music;
        _audioSource.Play();*/
        CrossfadeTo(_bossPhase1Music, 1.0f);
    }

    public void PlayBossPhase2Sound()
    {
        /*_audioSource.Stop();
        _audioSource.clip = _bossPhase2Music;
        _audioSource.Play();*/
        CrossfadeTo(_bossPhase2Music, 1.0f);
    }
        
    public void CrossfadeTo(AudioClip newClip, float duration)
    {
        if (_crossfadeCoroutine != null)
            StopCoroutine(_crossfadeCoroutine);

        _crossfadeCoroutine = StartCoroutine(CrossfadeCoroutine(newClip, duration));
    }

    /// <summary>
    /// Hace un crossfade de la musica vieja a la musica nueva
    /// </summary>
    /// <param name="newClip">musica nueva</param>
    /// <param name="duration">cuanto dura el crossfade</param>
    /// <returns></returns>
    private IEnumerator CrossfadeCoroutine(AudioClip newClip, float duration)
    {
        _nextSource.clip = newClip;
        _nextSource.volume = 0f;
        _nextSource.Play();

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            _currentSource.volume = Mathf.Lerp(1f, 0f, t);
            _nextSource.volume = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        _currentSource.Stop();

        var temp = _currentSource;
        _currentSource = _nextSource;
        _nextSource = temp;
    }
    #endregion

} // class DontDestroy 
// namespace
