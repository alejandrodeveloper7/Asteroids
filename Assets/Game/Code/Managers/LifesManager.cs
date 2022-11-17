using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifesManager : MonoBehaviour
{
    //This Script Manages the lifes and the UI related 

    #region Variables

    [Header("References")]
    [SerializeField] private Image[] lifesIcons = null;

    [Header("Values")]
    private int currentLifes; public int CurrentLifes
    {
        get { return currentLifes; }
        set { currentLifes = value; }
    }

    [Header("Cache")]
    Coroutine currentGameOverCoroutine;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        for (int i = 0; i < lifesIcons.Length; i++)
        {
            lifesIcons[i].enabled = false;
        }
    }
    
    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnGameStart += RestartLifesAndDisplay;
        EventsManager.OnLifeGenerated += AddLife;

        EventsManager.OnPlayerCanBeGenerated +=LifeUsed;
        EventsManager.OnPlayerDead += PlayerDead;
    }
    private void OnDisable()
    {
        EventsManager.OnGameStart -= RestartLifesAndDisplay;
        EventsManager.OnLifeGenerated -= AddLife;
        EventsManager.OnPlayerCanBeGenerated -= LifeUsed;
        EventsManager.OnPlayerDead -= PlayerDead;
    }
    
    private void RestartLifesAndDisplay()
    {
        lifesIcons[0].enabled = true;
        lifesIcons[1].enabled = true;
        lifesIcons[2].enabled = true;
        lifesIcons[3].enabled = false;
        lifesIcons[4].enabled = false;
        CurrentLifes = 3;
    }
    private void AddLife()
    {
        lifesIcons[CurrentLifes].enabled = true;
        CurrentLifes++;
    }

    public void LifeUsed()
    {
        CurrentLifes--;
        lifesIcons[CurrentLifes].enabled = false;
    }
    private void PlayerDead()
    {
        if (currentLifes == 0)
        {
            if (currentGameOverCoroutine != null)
            {
                StopCoroutine(currentGameOverCoroutine);
            }
            currentGameOverCoroutine = StartCoroutine(GameOverCoroutine());
        }
    }
    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        EventsManager.GameOver();
    }
    
    #endregion
}
