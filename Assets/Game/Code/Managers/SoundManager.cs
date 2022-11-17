using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private AudioSource shootSource = null;
    [SerializeField] private AudioSource playerDeadSource = null;
    [SerializeField] private AudioSource asteroidDestroyedSource = null;

    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnAsteroidDestroyed += PlayAsteroidDestroyedSound;
        EventsManager.OnPlayerDead += PlayPlayerDeadSound;
        EventsManager.OnShootGenerated += PlayShootSound;
    }
    private void OnDisable()
    {
        EventsManager.OnAsteroidDestroyed -= PlayAsteroidDestroyedSound;
        EventsManager.OnPlayerDead -= PlayPlayerDeadSound;
        EventsManager.OnShootGenerated += PlayShootSound;
    }

    public void PlayAsteroidDestroyedSound(AsteroidScript asteroidScript)
    {
        //To change slightly the sound of the asteroid destruction I change the pitch a little before use it.
        float randomFloat = Random.Range(0.8f, 1.2f);
        asteroidDestroyedSource.pitch = randomFloat;
        asteroidDestroyedSource.Play();
    }
    public void PlayPlayerDeadSound() => playerDeadSource.Play();
    public void PlayShootSound() => shootSource.Play();

    #endregion   
}
