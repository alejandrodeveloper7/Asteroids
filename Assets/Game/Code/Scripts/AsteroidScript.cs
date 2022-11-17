using UnityEngine;

public class AsteroidScript : MonoBehaviour, IPooleableItem
{
    //The diferences between the three type of asteroids are so little that instead use inheritance, an enumerator 
    //can manage the flows, reduce the number of methods and have only one Script

    #region Enumerations

    public enum AsteroidType
    {
        BigAsteroid = 0,
        MediumAsteroid = 1,
        SmallAsteroid = 2,
    }

    #endregion

    #region Variables

    [Header("States")]
    private bool readyToUse; public bool ReadyToUse
    {
        get { return readyToUse; }
        set { readyToUse = value; }
    }

    [Header("Configuration")]
    [SerializeField] private AsteroidType ownAsteroidType=0;    public AsteroidType OwnAsteroidType
    {
        get { return ownAsteroidType; }
    }
    private float movementSpeed=0;

    [Header("Private Use")]
    private Vector2 direction;    public Vector2 Direction
    {
        get { return direction; }
        private set { direction = value; }
    }

    [Header("References")]
    protected PolygonCollider2D ownCollider;
    protected SpriteRenderer ownSpriteRenderer;
    protected Rigidbody2D ownRigidBody;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        DataManager dataManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<DataManager>();
        movementSpeed = dataManager.ConfigurationsData.GetAsteroidSpeed(OwnAsteroidType);

        ownRigidBody = GetComponent<Rigidbody2D>();

        ownSpriteRenderer = GetComponent<SpriteRenderer>();
        ownSpriteRenderer.sprite = dataManager.AsteroidsSpritesspritesData.GetRandomAsteroidSprite(ownAsteroidType);
        
        //The collider is added in runtime just after define the sprite of the asteroid, this way the collider adapts to the shape of the sprite
        ownCollider = gameObject.AddComponent<PolygonCollider2D>();
        ownCollider.isTrigger = true;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shoot"))
        {
            other.GetComponent<ShootScript>().ShootImpact();
            AsteroidDestroyed();
        }
        else if (other.CompareTag("Player"))
        {
            EventsManager.PlayerDead();
        }
    }

    #endregion

    #region Asteroid Management

    public void InitializeAsteroid(Vector2 direction)
    {
        gameObject.SetActive(true);

        readyToUse = false;

        ownRigidBody.WakeUp();
        Direction = direction;
        ownRigidBody.velocity = Direction * movementSpeed * Time.fixedDeltaTime;

        int randomNumber = Random.Range(-50, 51);
        ownRigidBody.AddTorque(randomNumber);
    }

    public void AsteroidDestroyed()
    {
        EventsManager.AsteroidDestroyed(this);
        CleanAsteroid();
    }

    public void CleanAsteroid()
    {
        ownRigidBody.velocity = Vector2.zero;
        ownRigidBody.Sleep();

        transform.localPosition = Vector3.zero;

        gameObject.SetActive(false);
        readyToUse = true;
    }

    #endregion
}