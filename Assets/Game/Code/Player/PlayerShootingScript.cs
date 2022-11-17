using System.Collections;
using UnityEngine;

public class PlayerShootingScript : MonoBehaviour
{
    #region Variables

    [Header("Configuration")]
    private float BetweenShootsTime;
    
    [Header("References")]
    private Transform shootInstancePoint;
    
    [Header("Shoots Pool")]
    private Transform pooledShootsParent;
    [SerializeField] private GameObject shootPrefab = null;
    private SimplePool shootPool;
    
    [Header("Cache")]
    Coroutine currentShootingCoroutine;
    GameObject newShoot;
    float nextShootTime = 0;

    #endregion

    #region Initialization

    public void InitializeScript(DataManager dataManager)
    {
        BetweenShootsTime = dataManager.ConfigurationsData.betweenShootsTime;
        shootInstancePoint = transform.GetChild(0);
        PoolInitialization();
    }

    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnPlayerDead += StopShooting;
    }
    private void OnDisable()
    {
        EventsManager.OnPlayerDead -= StopShooting;
    }

    public void StopShooting()
    {
        if (currentShootingCoroutine != null)
        {
            StopCoroutine(currentShootingCoroutine);
            currentShootingCoroutine = null;
        }
    }
   
    #endregion

    #region Pool Management

    private void PoolInitialization()
    {
        pooledShootsParent = new GameObject("PooledPlayerShoots").transform;
        pooledShootsParent.position = Vector3.up * 20;
        shootPool = new SimplePool(shootPrefab, pooledShootsParent, 4, 1);
    }

    #endregion

    #region Shooting

    public void StartShooting()
    {
        if (currentShootingCoroutine != null)
        {
            StopCoroutine(currentShootingCoroutine);
        }
        currentShootingCoroutine = StartCoroutine(ShootingCoroutine());
    }

    //The shooting is done in a Coroutine to avoid the InputManager throwing the same method every frame.
    //This Coroutine could be more simple, but this way, you avoid the player stop shooting and quickly start shoothig again without wait the time between shoots;
    private IEnumerator ShootingCoroutine()
    {
        for (; ; )
        {
            //nextShootTime need to be stored in cache to maintein from one coroutine to the next one
            if (Time.time >= nextShootTime)
            {
                GenerateShoot();
                nextShootTime = Time.time + BetweenShootsTime;
                yield return new WaitForSeconds(BetweenShootsTime);
            }
            else
            {
                yield return null;
            }
        }
    }
    private void GenerateShoot()
    {
        newShoot = shootPool.GetInstance();
        newShoot.transform.SetPositionAndRotation(shootInstancePoint.position, shootInstancePoint.rotation);

        newShoot.GetComponent<ShootScript>().InitializeShoot();
        EventsManager.ShootGenerated();
    }

    #endregion
}
