using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    #region Variables

    [Header("References")]
    private Collider2D playerCollider;
    private Rigidbody2D playerRigidBody;

    [Header("Configuration")]
    private float movementSpeed;
    private float rotationSpeed;

    #endregion

    #region Initialization

    public void InitializeScript(DataManager dataManager)
    {
        movementSpeed = dataManager.ConfigurationsData.movementSpeed;
        rotationSpeed = dataManager.ConfigurationsData.rotationSpeed;

        playerCollider = GetComponent<Collider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
    }    
    
    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnPlayerCanBeGenerated += ConfigurePlayerToPlay;
        EventsManager.OnPlayerDead += StopPlayerMovement;
    }
    private void OnDisable()
    {
        EventsManager.OnPlayerCanBeGenerated -= ConfigurePlayerToPlay;
        EventsManager.OnPlayerDead -= StopPlayerMovement;
    }

    public void ConfigurePlayerToPlay()
    {
        transform.SetPositionAndRotation(Vector3.zero,Quaternion.identity);
        playerCollider.enabled = true;
        playerRigidBody.WakeUp();
    }
    public void StopPlayerMovement()
    {
        playerCollider.enabled = false;
        playerRigidBody.velocity = Vector3.zero;
        playerRigidBody.Sleep();
    }

    #endregion

    #region Functionality

    public void MoveForward() => playerRigidBody.AddForce(transform.up * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);

    public void RotateAround(float value) => transform.Rotate(0, 0, value * rotationSpeed * Time.fixedDeltaTime);

    #endregion
}
