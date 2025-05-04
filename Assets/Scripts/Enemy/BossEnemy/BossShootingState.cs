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
    [Header("Bullet Properties")]

    ///<summary>
    /// La bala a disparar
    /// </summary>
    [SerializeField]
    MagicBullet _bulletPrefab;

    /// <summary>
    /// Cuantas balas se quieren disparar
    /// </summary>
    [SerializeField]
    [Min(0)]
    int _bulletNumber;

    /// <summary>
    /// La distancia desde el centro a la que queremos disparar las balas
    /// </summary>
    [SerializeField]
    [Min(0)]
    float _distanceFromCenter;

    [Header("Time Properties")]
    ///<summary>
    /// Tiempo que espera antes de disparar
    /// </summary>
    [SerializeField]
    [Min(0)]
    float _waitTimeBeforeShot;

    /// <summary>
    /// Tiempo que espera tras disparar
    /// </summary>
    [SerializeField]
    [Min(0)]
    float _waitTimeAfterShot;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Tiempo en el que comienza el disparo
    /// </summary>
    float _timeToShoot;

    /// <summary>
    /// Tiempo en el que termina el estado de disparo
    /// </summary>
    float _timeToEndState;

    /// <summary>
    /// Flag para saber si ya ha disparado
    /// </summary>
    bool _hasShot;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Método para que dibuje las spawns de balas en el editor.
    /// <para>
    /// Así es más fácil ver cómo se verán las balas sin tener que jugarlo
    /// </para>
    /// </summary>
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
                    //Rota los puntos en función de cuantos hayas (Un poco de algebra lineal jeje )
                    _distanceFromCenter * new Vector3(Mathf.Cos(angleBetweenPoints * i), Mathf.Sin(angleBetweenPoints * i), 0);
               
                // Dibuja los puntos
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
        // Setup
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
            // Rotación entre cada bala
            float angleBetweenPoints = -Mathf.PI / (_bulletNumber + 1);

            //Instancia todas las balas en arco
            for (int i = 1; i < _bulletNumber + 1; i++)
            {
                Vector3 spawnPos = transform.position +
                    //Rota los puntos en función de cuantos hayas
                    _distanceFromCenter * new Vector3(Mathf.Cos(angleBetweenPoints * i), Mathf.Sin(angleBetweenPoints * i), 0);

                //Solo para reusar la magic bullet
                Vector3 targetsPos = transform.position +
                    //Rota los puntos en función de cuantos hayas
                    (_distanceFromCenter+1) * new Vector3(Mathf.Cos(angleBetweenPoints * i), Mathf.Sin(angleBetweenPoints * i), 0);

                // Instancia la bala en la rotación adecuada
                Instantiate(_bulletPrefab, spawnPos, Quaternion.AngleAxis(angleBetweenPoints * i, Vector3.forward))
                    .Setup(targetsPos);
            }

            // Ponemos la flag de que ha disparado
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
            // cambiamos al estado de vulnerabilidad
            Ctx.ChangeState(Ctx.GetStateByName("Air Vulnerable"));
        }
    }

    #endregion   

} // class BossShootingState 
// namespace
