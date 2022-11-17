public static class EventsManager : object
{
    //This Script is a static class and manages all the events and the methods to call them 
    // With "= () => { }" in the events, they will not generate an exception when they are used and don´t have something subscribed. 
    
    #region GameFlow
            
    public delegate void WaitingModeStartAction();
    public static event WaitingModeStartAction OnWaitingModeStart = () => { };
    public static void WaitingModeStarted()
    {
        OnWaitingModeStart();
    }

    public delegate void StartGameAction();
    public static event StartGameAction OnGameStart = () => { };
    public static void GameStarted()
    {
        OnGameStart();
    }

    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver = () => { };
    public static void GameOver()
    {
        OnGameOver();
    }

    #endregion

    #region Player Management

    public delegate void PlayerCanBeGeneratedAction();
    public static event PlayerCanBeGeneratedAction OnPlayerCanBeGenerated = () => { };
    public static void PlayerCanBeGenerated()
    {
        OnPlayerCanBeGenerated();
    }

    public delegate void PlayerDeadAction();
    public static event PlayerDeadAction OnPlayerDead = () => { };
    public static void PlayerDead()
    {
        OnPlayerDead();
    }

    #endregion
    
    #region Miscellany

    public delegate void AsteroidDestroyedAction(AsteroidScript asteroidScript);
    public static event AsteroidDestroyedAction OnAsteroidDestroyed = (AsteroidScript asteroidScript) => { };
    public static void AsteroidDestroyed(AsteroidScript asteroidScript)
    {
        OnAsteroidDestroyed(asteroidScript);
    }
    
    public delegate void ShootGeneratedAction();
    public static event ShootGeneratedAction OnShootGenerated = () => { };
    public static void ShootGenerated()
    {
        OnShootGenerated();
    }

    public delegate void NewRecordAction(int newRecord);
    public static event NewRecordAction OnNewRecord = (int newRecord) => { };
    public static void NewRecord(int newRecord)
    {
        OnNewRecord(newRecord);
    }
    
    public delegate void LifeGeneratedAction();
    public static event LifeGeneratedAction OnLifeGenerated = () => { };
    public static void LifeGenerated()
    {
        OnLifeGenerated();
    }
    
    public delegate void DataReadyAction();
    public static event DataReadyAction OnAllDataReady = () => { };
    public static void AllDataReady()
    {
        OnAllDataReady();
    }
    
    #endregion
}
