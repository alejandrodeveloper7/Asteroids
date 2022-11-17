using UnityEngine;

public class DataManager : MonoBehaviour
{
    //This Scripts stores the ScriptableObjects with the data, accesible by public properties,
    //and the record score stored in PlayerPrefs

    #region Variables

    [Header("Data")]
    [SerializeField] private ConfigurationData configurationsData = null; public ConfigurationData ConfigurationsData
    {
        get { return configurationsData; }
    }
    [SerializeField] private SpritesData asteroidsSpritesspritesData = null; public SpritesData AsteroidsSpritesspritesData
    {
        get { return asteroidsSpritesspritesData; }
    }

    [Header("Values")]
    private int currentRecord; public int CurrentRecord
    {
        get { return currentRecord; }
        private set
        {
            currentRecord = value;
            PlayerPrefs.SetInt("RecordScore", currentRecord);
        }
    }
    #endregion

    private void Awake()
    {
        LoadRecordData();
        EventsManager.AllDataReady();
    }

    private void LoadRecordData()
    {
        if (PlayerPrefs.HasKey("RecordScore"))
        {
            //Called directly with the variable to don´t execute the set and save the value
            currentRecord = PlayerPrefs.GetInt("RecordScore");
        }
        else
        {
            //Called with the property to execute the set an save the value in PlayerPrefs
            CurrentRecord = 0;
        }
    }

    public void CheckNewRecord(int finalScore)
    {
        if (finalScore > currentRecord)
        {
            CurrentRecord = finalScore;
            EventsManager.NewRecord(currentRecord);
        }
    }

}
