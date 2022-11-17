using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables
    
    [Header("References")]
    [SerializeField] private TextMeshProUGUI presToStartText = null;
    [SerializeField] private TextMeshProUGUI gameOverText = null;
    [Space]
    [SerializeField] private TextMeshProUGUI recordText = null;
    [SerializeField] private TextMeshProUGUI recordIndicatorText = null;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        LoadRecordData();
    }
    
    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnAllDataReady += LoadRecordData;
        EventsManager.OnWaitingModeStart += WaitingModeStarted;

        EventsManager.OnGameStart += GameStarted;
        EventsManager.OnGameOver += GameEnded;

        EventsManager.OnNewRecord += NewRecordAchieved;
    }
    private void OnDisable()
    {
        EventsManager.OnAllDataReady -= LoadRecordData;
        EventsManager.OnWaitingModeStart -= WaitingModeStarted;

        EventsManager.OnGameStart -= GameStarted;
        EventsManager.OnGameOver -= GameEnded;

        EventsManager.OnNewRecord -= NewRecordAchieved;
    }

    public void LoadRecordData()
    {
        DataManager dataManager = GetComponent<DataManager>();
        recordText.text = dataManager.CurrentRecord.ToString();
    }
    private void WaitingModeStarted()
    {
        presToStartText.enabled = true;
        gameOverText.enabled = false;
        recordIndicatorText.enabled = false;
    }

    private void GameStarted()=>  presToStartText.enabled = false;
    private void GameEnded()=>  gameOverText.enabled = true;

    private void NewRecordAchieved(int newRecord)
    {
        recordText.text = newRecord.ToString();
        recordIndicatorText.enabled = true;
    }

    #endregion
}