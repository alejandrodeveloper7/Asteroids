using System.Collections;
using UnityEngine;

public class ShootScript : MonoBehaviour, IPooleableItem
{
    #region Variables

    [Header("Configuration")]
    private float shootLifeDuration;
    private float shootMovementSpeed;
    
    [Header("States")]
    private bool readyToUse;    public bool ReadyToUse
    {
        get { return readyToUse; }
        set { readyToUse = value; }
    }
    
    [Header("Cache")]
    Coroutine currentShootCoroutine;

    #endregion
    
    #region Monobehaviour

    private void Awake()
    {
        DataManager dataManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<DataManager>();
        shootLifeDuration = dataManager.ConfigurationsData.shootLifeDuration;
        shootMovementSpeed = dataManager.ConfigurationsData.shootMovementSpeed;

        gameObject.SetActive(false);
    }

    #endregion

    #region ShootManagement

    public void InitializeShoot()
    {
        readyToUse = false;
        gameObject.SetActive(true);
        if (currentShootCoroutine != null)
        {
            StopCoroutine(currentShootCoroutine);
        }
        currentShootCoroutine = StartCoroutine(ShootCoroutine());
    }
    private IEnumerator ShootCoroutine()
    {
        float deathTime = Time.time + shootLifeDuration;
        while (Time.time < deathTime)
        {
            transform.position += transform.up * shootMovementSpeed * Time.fixedDeltaTime;
            yield return null;
        }

        transform.localPosition = Vector3.zero;
        readyToUse = true;
        gameObject.SetActive(false);
    }

    public void ShootImpact()
    {
        if (currentShootCoroutine != null)
        {
            StopCoroutine(currentShootCoroutine);
            currentShootCoroutine = null;
        }
        transform.localPosition = Vector3.zero;
        readyToUse = true;
        gameObject.SetActive(false);
    }

    #endregion
}
