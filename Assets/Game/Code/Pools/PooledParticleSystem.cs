using System.Collections;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour,IPooleableItem
{
    //This Script allows to not looped particle Systems be pooled 

    #region Variables

    [Header("Interface")]
    internal bool readyToUse;    public bool ReadyToUse
    {
        get { return readyToUse; }
        set { readyToUse = value; }
    }

    [Header("Configuration")]
    [SerializeField] internal float turnOffTime=0;
    internal ParticleSystem ownParticles;

    [Header("Cache")]
    internal Coroutine currentUseParticleSystemCoroutine;
    
    #endregion

    internal virtual void Awake()
    {
        ownParticles = GetComponent<ParticleSystem>();
        gameObject.SetActive(false);
    }

    public void UseParticleSystem()
    {
        ReadyToUse = false;
        gameObject.SetActive(true);
        if (currentUseParticleSystemCoroutine != null)
        {
            StopCoroutine(currentUseParticleSystemCoroutine);
        }
        currentUseParticleSystemCoroutine = StartCoroutine(UseParticleSystemCoroutine());
    }
    internal virtual IEnumerator UseParticleSystemCoroutine()
    {
        ownParticles.Play();
        yield return new WaitForSeconds(turnOffTime);
        readyToUse = true;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
