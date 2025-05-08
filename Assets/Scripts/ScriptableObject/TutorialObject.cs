//---------------------------------------------------------
// Scriptable Object
// Chenlinjia Yi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
[CreateAssetMenu(fileName = "New TutorialObject")]
public class TutorialObject : ScriptableObject
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
    [SerializeField] string _tutorialTitle;
    [SerializeField][TextArea] string _tutorialDescription;
    [SerializeField] VideoClip _tutorialVideo;
    [SerializeField] string _keyboardButton;
    [SerializeField] string _controllerButton;
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public string GetTutorialTitle()
    {
        return _tutorialTitle;
    }

    public string GetTutorialDescription()
    {
        if (_keyboardButton != null && _controllerButton != null)
        {
            if (InputManager.Instance.Device is Gamepad)
            {
                _tutorialDescription = _tutorialDescription.Replace(_keyboardButton, _controllerButton);
            }
            else
            {
                _tutorialDescription = _tutorialDescription.Replace(_controllerButton, _keyboardButton);
            }
        }

        return _tutorialDescription;
    }

    public VideoClip GetTutorialVideo()
    {
        return _tutorialVideo;
    }

    public void ChangeButton()
    {
        if (_keyboardButton != "" && _controllerButton != "")
        {
            if(InputManager.Instance.Device is Gamepad)
            {
                _tutorialDescription = _tutorialDescription.Replace(_keyboardButton, _controllerButton);
            }
            else
            {
                _tutorialDescription = _tutorialDescription.Replace(_controllerButton,_keyboardButton);
            }
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class TutorialObject 
// namespace
