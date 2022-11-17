using System.Collections;
using UnityEngine;

public class PooledParticleSystemWithSound : PooledParticleSystem, IPooleableItem
{
    //This Script do the same of the "PooledParticleSystem" script but with sound. This script is not being used in this project

    private AudioSource sound;

    internal override void Awake()
    {
        base.Awake();
        sound = GetComponent<AudioSource>();
    }

    internal override IEnumerator UseParticleSystemCoroutine()
    {
        ownParticles.Play();
        sound.Play();
        yield return new WaitForSeconds(turnOffTime);
        readyToUse = true;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
