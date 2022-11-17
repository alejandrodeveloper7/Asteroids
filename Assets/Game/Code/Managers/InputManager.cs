using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Variables
    
    [Header("References")]
    private PlayerMovementScript playerMovementScript;  public PlayerMovementScript PlayerMovementScript
    {
        private get { return playerMovementScript; }
        set { playerMovementScript = value; }
    }
    private PlayerShootingScript playerShootingScript; public PlayerShootingScript PlayerShootingScript
    {
        private get { return playerShootingScript; }
        set { playerShootingScript = value; }
    }

    [Header("States")]
    private bool waitingStart;
    private bool isPlaying;
    
    [Header("KeyBoard Controls")]
    private KeyCode moveForwardKey;    
    private KeyCode turnLeftKey;    
    private KeyCode turnRightKey;    
    private KeyCode shootKey;    
    private KeyCode closeGameKey;   
    
    [Header("Cache")]
    float editorHorizontalAxisMovement;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        LoadKeyboardControlsData();
    }

    void FixedUpdate()
    {        
        if (isPlaying)
        {            
            if (Input.GetKey(turnLeftKey))
            {
                PlayerMovementScript.RotateAround(2);
            }
            else if (Input.GetKey(turnRightKey))
            {
                PlayerMovementScript.RotateAround(-2);
            }
            else
            {
                editorHorizontalAxisMovement = Input.GetAxis("Mouse X");
                PlayerMovementScript.RotateAround(-editorHorizontalAxisMovement * 2);
            }


            if (Input.GetKey(moveForwardKey) || Input.GetKey(KeyCode.Mouse1))
            {
                PlayerMovementScript.MoveForward();
            }

            //To avoid Throw a method every frame, the shooting logic is done in a coroutine and only called in the first up and down frames
            if (Input.GetKeyDown(shootKey) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayerShootingScript.StartShooting();
            }
            else if (Input.GetKeyUp(shootKey) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                PlayerShootingScript.StopShooting();
            }

            if (Input.GetKeyDown(closeGameKey))
            {
                Application.Quit();
            }
        }
        else if (waitingStart)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
            {
                EventsManager.GameStarted();
            }
            else if (Input.GetKeyDown(closeGameKey))
            {
                Application.Quit();
            }
        }
    }

    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnWaitingModeStart += WaitModeStart;

        EventsManager.OnPlayerCanBeGenerated += TurnOnPlayerInput;
        EventsManager.OnPlayerDead += TurnOffPlayerInput;
    }
    private void OnDisable()
    {
        EventsManager.OnWaitingModeStart -= WaitModeStart;

        EventsManager.OnPlayerCanBeGenerated -= TurnOnPlayerInput;
        EventsManager.OnPlayerDead -= TurnOffPlayerInput;
    }
    
    private void WaitModeStart() => waitingStart = true;

    private void TurnOnPlayerInput()
    {
        waitingStart = false;
        isPlaying = true;
    }
    private void TurnOffPlayerInput() => isPlaying = false;

    #endregion

    #region DataLoad

    private void LoadKeyboardControlsData()
    {
        DataManager dataManager = GetComponent<DataManager>();

        moveForwardKey = dataManager.ConfigurationsData.moveForwardKey;
        turnLeftKey = dataManager.ConfigurationsData.turnLeftKey;
        turnRightKey = dataManager.ConfigurationsData.turnRightKey;
        shootKey = dataManager.ConfigurationsData.shootKey;
        closeGameKey = dataManager.ConfigurationsData.closeGameKey;
    }

    #endregion
}
