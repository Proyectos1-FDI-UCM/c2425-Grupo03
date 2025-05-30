//---------------------------------------------------------
// Reproduce en tutorial en el juego
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Video;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class TutorialController : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _descriptionText;
    [SerializeField] VideoPlayer _videoPlayer;
    [SerializeField] GameObject _tutorial;
    [SerializeField] GameObject _skipBottom;
    [SerializeField] AudioClip _acceptSound;

    [SerializeField] TutorialObject []_tutorialObjects;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    bool[] _playerTutorialsMask;

    private TutorialObject _currentTutorial;
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    public static TutorialController Instance;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

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

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _playerTutorialsMask = new bool[_tutorialObjects.Length];
        for (int i=0; i< _tutorialObjects.Length; i++)
        {
            _playerTutorialsMask[i] = false;
        }
        HideTutorial();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public void ShowTutorial(int index)
    {
        _currentTutorial = _tutorialObjects[index];
        if (!_playerTutorialsMask[index])
        {

            //_currentTutorial.ChangeButton();
            UpdateButton();

            //InputManager.Instance._deviceChange.AddListener(_currentTutorial.ChangeButton);
            InputManager.Instance._deviceChange.AddListener(UpdateButton);

            _tutorial.SetActive(true);

            //init
            _titleText.text = _currentTutorial.GetTutorialTitle();
            _descriptionText.text = _currentTutorial.GetTutorialDescription();
            _videoPlayer.clip = _currentTutorial.GetTutorialVideo();

            _videoPlayer.Play();

            PauseGame();
            
            _playerTutorialsMask[index] = true;

        }
    }

    public void PauseGame()
    {
        InputManager.Instance.DisablePlayerInput();
        InputManager.Instance.EnableMenuInput();
        Time.timeScale = 0f;
        EventSystem.current.SetSelectedGameObject(_skipBottom);
    }

    public void ContinueGame()
    {
        InputManager.Instance.EnablePlayerInput();
        InputManager.Instance.DisableMenuInput();
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySFX(_acceptSound, transform, 1);
    }
    public void HideTutorial()
    {
        _tutorial.SetActive(false);
        _videoPlayer.Stop();
        ContinueGame();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void UpdateButton()
    {
        _descriptionText.text = _currentTutorial.GetTutorialDescription();
    }



    #endregion

} // class TutorialController 
// namespace
