//---------------------------------------------------------
// Estado de disparo del jefe final
// Adrián Isasi
// Kingless Dungeon
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class BossShootingState : BaseState
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    MagicBullet _bulletPrefab;

    [SerializeField]
    [Min(0)]
    int _bulletNumber;

    [SerializeField]
    [Min(0)]
    float _distanceFromCenter;

    [SerializeField]
    [Min(0)]
    float _waitTimeBeforeShot;

    [SerializeField]
    [Min(0)]
    float _waitTimeAfterShot;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    float _timeToShoot;
    float _timeToEndState;
    bool _hasShot;

    #endregion

    // ---- PROPIEDADES ----
    #region Propiedades
    // Documentar cada propiedad que aparece aquí.
    // Escribir con PascalCase.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void OnDrawGizmosSelected()
    {
        if (_bulletNumber != 0)
        {
            // Rotación entre cada punto
            float angleBetweenPoints = -Mathf.PI / (_bulletNumber+1);

            //Dibuja todos los puntos
            for (int i = 1; i < _bulletNumber+1; i++)
            {
                Vector3 spawnPos = transform.position + 
                    //Rota los puntos en función de cuantos hayas
                    _distanceFromCenter * new Vector3(Mathf.Cos(angleBetweenPoints * i), Mathf.Sin(angleBetweenPoints * i), 0);
                Gizmos.DrawSphere(spawnPos, 0.2f);
            }
        }
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    /// <summary>
    /// Metodo llamado cuando al transicionar a este estado.
    /// </summary>
    public override void EnterState()
    {
        _timeToShoot = Time.time + _waitTimeBeforeShot;
        _timeToEndState = _timeToShoot + _waitTimeAfterShot;
        _hasShot = false;
    }
    
    /// <summary>
    /// Metodo llamado antes de cambiar a otro estado.
    /// </summary>
    public override void ExitState()
    {
        
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS O PROTEGIDOS ----
    #region Métodos Privados o Protegidos

    /// <summary>
    /// Metodo llamado cada frame cuando este es el estado activo de la maquina de estados.
    /// </summary>
    protected override void UpdateState()
    {
        if (!_hasShot && Time.time > _timeToShoot && _bulletNumber != 0)
        {
            // Rotación entre cada punto
            float angleBetweenPoints = -Mathf.PI / (_bulletNumber + 1);

            //Dibuja todos los puntos
            for (int i = 1; i < _bulletNumber + 1; i++)
            {
                Vector3 spawnPos = transform.position +
                    //Rota los puntos en función de cuantos hayas
                    _distanceFromCenter * new Vector3(Mathf.Cos(angleBetweenPoints * i), Mathf.Sin(angleBetweenPoints * i), 0);

                //Solo para reusar la magic bullet
                Vector3 targetsPos = transform.position +
                    //Rota los puntos en función de cuantos hayas
                    (_distanceFromCenter+1) * new Vector3(Mathf.Cos(angleBetweenPoints * i), Mathf.Sin(angleBetweenPoints * i), 0);
                Instantiate(_bulletPrefab, spawnPos, Quaternion.AxisAngle(Vector3.forward, angleBetweenPoints * i))
                    .Setup(targetsPos);
            }
            _hasShot = true;
        }
    }

    /// <summary>
    /// Metodo llamado tras UpdateState para mirar si hay que cambiar a otro estado.
    /// Principalmente es para mantener la logica de cambio de estado separada de la logica del estado en si
    /// </summary>
    protected override void CheckSwitchState()
    {
        if (Time.time > _timeToEndState)
        {
            Ctx.ChangeState(Ctx.GetStateByName("Flying Charge"));
        }
    }

    #endregion   

} // class BossShootingState 
// namespace
