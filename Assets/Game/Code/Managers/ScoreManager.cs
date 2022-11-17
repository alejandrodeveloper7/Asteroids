using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //This Script manages the score and the UI related

    #region Variables

    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText = null;

    [Header("Values")]
    private int currentScore; public int CurrentScore
    {
        get { return currentScore; }
        private set { currentScore = value; }
    }

    private int bigAsteroidScore;
    private int mediumAsteroidScore;
    private int smallAsteroidScore;

    private int scoreForFirstExtraLife;
    private int scoreForSecondExtraLife;
    private int scoreForThirdExtraLife;

    [Header("Private Use")]
    private int bonusLifesIndex;
    
    [Header("Cache")]
    DataManager dataManager;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        dataManager=GetComponent<DataManager>();
        LoadData();
    }

    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnWaitingModeStart += TurnOffScoretext;

        EventsManager.OnGameStart += InitializeScoreValuesAndDisplay;
        EventsManager.OnGameOver += CheckNewRecord;

        EventsManager.OnAsteroidDestroyed +=AddScore;
    }
    private void OnDisable()
    {
        EventsManager.OnWaitingModeStart -= TurnOffScoretext;
        
        EventsManager.OnGameStart -= InitializeScoreValuesAndDisplay;
        EventsManager.OnGameOver -= CheckNewRecord;

        EventsManager.OnAsteroidDestroyed -= AddScore;
    }
    
    private void TurnOffScoretext() => scoreText.enabled = false;

    private void InitializeScoreValuesAndDisplay()
    {
        bonusLifesIndex = 0;
        currentScore = 0;
        scoreText.text = currentScore.ToString();
        scoreText.enabled = true;
    }
    private void CheckNewRecord() => dataManager.CheckNewRecord(CurrentScore);

    private void AddScore(AsteroidScript asteroid)
    {
        switch (asteroid.OwnAsteroidType)
        {
            case AsteroidScript.AsteroidType.BigAsteroid:
                currentScore += bigAsteroidScore;
                break;

            case AsteroidScript.AsteroidType.MediumAsteroid:
                currentScore += mediumAsteroidScore;
                break;

            case AsteroidScript.AsteroidType.SmallAsteroid:
                currentScore += smallAsteroidScore;
                break;
        }
        scoreText.text = CurrentScore.ToString();

        if (bonusLifesIndex < 3)
        {
            CheckBonusLife();
        }
    }

    #endregion

    #region Funcionality

    private void LoadData()
    {
        bigAsteroidScore = dataManager.ConfigurationsData.bigAsteroidScore;
        mediumAsteroidScore = dataManager.ConfigurationsData.mediumAsteroidScore;
        smallAsteroidScore = dataManager.ConfigurationsData.smallAsteroidScore;

        scoreForFirstExtraLife = dataManager.ConfigurationsData.scoreForFirstExtraLife;
        scoreForSecondExtraLife = dataManager.ConfigurationsData.scoreForSecondExtraLife;
        scoreForThirdExtraLife = dataManager.ConfigurationsData.scoreForThirdExtraLife;
    }

    private void CheckBonusLife()
    {
        if (bonusLifesIndex == 0  && currentScore >= scoreForFirstExtraLife)
        {
            bonusLifesIndex++;
            EventsManager.LifeGenerated();
        }
        else if (bonusLifesIndex == 1 && currentScore >= scoreForSecondExtraLife)
        {
            bonusLifesIndex++;
            EventsManager.LifeGenerated();
        }
        if (bonusLifesIndex == 2  && currentScore >= scoreForThirdExtraLife)
        {
            bonusLifesIndex++;
            EventsManager.LifeGenerated();
        }
    }
    
    #endregion

}
