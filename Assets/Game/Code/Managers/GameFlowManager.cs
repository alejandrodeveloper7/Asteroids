using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    //This Script Manages the initialization of the game and the final game flow

    #region Monobehaviour
        
    [Header("Cache")]
    Coroutine currentGameEndedCoroutine;
    
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        Application.targetFrameRate = 60;        
    }
    private void Start()
    {
        EventsManager.WaitingModeStarted();
    }

    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnGameOver +=GameEnded;
    }
    private void OnDisable()
    {
        EventsManager.OnGameOver -= GameEnded;
    }

    private void GameEnded()
    {
        if (currentGameEndedCoroutine != null)
        {
            StopCoroutine(currentGameEndedCoroutine);
        }
        currentGameEndedCoroutine = StartCoroutine(GameEndedCoroutine());
    }
    private IEnumerator GameEndedCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        EventsManager.WaitingModeStarted();
    }

    #endregion 
}
