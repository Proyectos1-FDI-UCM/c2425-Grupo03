//---------------------------------------------------------
// Breve descripción del contenido del archivo;
// Script del movimiento de cámara para que siga al jugador
// Responsable: SANTIAGO SALTO
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Numerics;

// using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.Scripting.APIUpdating;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente responsable de la gestión de la cámara del juego
/// </summary>
public class CameraManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // Puesto que son atributos globales en la clase debes usar "_" + camelCase para su nombre.
  
    /// <summary>
    /// Posicion Jugador u objeto.
    /// </summary>
    [SerializeField] Transform _playerPosition;
    /// <summary>
    /// velocidad de la Cámara
    /// </summary>
    [SerializeField][Min(0)] float _cameraVelocity;
    /// <summary>
    /// Margen de la Cámara respecto al objetivo
    /// </summary>
    [SerializeField] private Vector3 _cameraDisplacement; //dista de la camara (posicion del jugador)
    [SerializeField] private float _maxDisplacement;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Dirección de movimiento de la cámara.
    /// </summary>
    private Vector3 _moveDir; 
    private PlayerInputActions.PlayerActions _playerInput;
    private Vector3 _finalPos;
    
    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion 
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    void Awake() {
        _playerInput = new PlayerInputActions().Player;
        _playerInput.Enable();
    }

    void Update()
    {
        // Lee el input del jugador.
        _moveDir = _playerInput.MoveCamera.ReadValue<Vector2>();
       
    }

    void FixedUpdate()
    {
        if (_playerPosition != null) 
        {
             _finalPos = _playerPosition.position + _moveDir * _maxDisplacement + _cameraDisplacement; // Calcula la posición final (posición del jugador + el input * el desplazamiento máximo posible)
            Move(_finalPos); // la cámara sigue al jugador
        }
    }

 
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos
    /// <summary>
    /// Método para mover la cámara a la posición indicada.
    /// </summary>
    /// <param name="movePos">La posición a la que mover la cámara.</param>
    private void Move(Vector3 movePos) {
        //Hacer que el movimiento se vea gradual
        Vector3 smoothedMovement = Vector3.Lerp(transform.position, movePos, _cameraVelocity);
        //Mover la Cámara
        transform.position = smoothedMovement;
    }

    #endregion   

} // class CameraManager 
// namespace
