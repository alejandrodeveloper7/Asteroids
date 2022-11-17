using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //This script manages the gameobjects in the scene, like the asteroids, the particle systems or the player.

    #region Variables

    [Header("Pools and player")]
    [SerializeField] private GameObject playerPrefab = null;

    private Transform pooledItemsParent;
    [Space]
    [SerializeField] private GameObject smallAsteroidPrefab = null;
    [SerializeField] private GameObject mediumAsteroidPrefab = null;
    [SerializeField] private GameObject bigAsteroidPrefab = null;
    [Space]
    [SerializeField] private GameObject destructionParticleSystemPrefab = null;

    private SimplePool smallAsteroidsPool;
    private SimplePool mediumAsteroidsPool;
    private SimplePool bigAsteroidsPool;
    private SimplePool destructionParticleSystemPool;


    [Header("Private Use")]
    private LayerMask asteroidsLayer;
    private LayerMask playerLayer;
    [Space]
    private int currentRound;
    private float playerSafeAreaRadius = 2f;


    [Header("Asteroids Management")]
    private AsteroidScript[] currentAsteroidScripts = new AsteroidScript[0];
    private int screenWidth;
    private int screenHeight;
    private Camera gameCamera;


    [Header("Cache")]
    private LifesManager lifesManager;
    private DataManager dataManager;

    Coroutine currentPlayerDeadCoroutine;
    Coroutine currentInitializeGameScenarioCoroutine;

    Collider2D detectedGameObject;
    int randomWidth;
    int randomHeight;

    Vector2 newSpawnPosition;
    Quaternion newSpawnRotation;

    GameObject newAsteroid;
    AsteroidScript newAsteroidScript;

    GameObject newDestructionParticleSystemGameObject;

    Vector2 newDirection1;
    Vector2 newDirection2;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        lifesManager = GetComponent<LifesManager>();
        dataManager = GetComponent<DataManager>();

        asteroidsLayer = LayerMask.GetMask("Asteroids");
        playerLayer = LayerMask.GetMask("Player");

        screenWidth = Screen.width;
        screenHeight = Screen.height;
        gameCamera = Camera.main;

        InstantiatePlayer();
        InitializePools();
    }

    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnWaitingModeStart += InitializeWaitingModeScenario;
        EventsManager.OnGameStart += InitializeGameScenario;

        EventsManager.OnAsteroidDestroyed += AsteroidDestroyed;
        EventsManager.OnPlayerDead += PlayerDead;
    }
    private void OnDisable()
    {
        EventsManager.OnWaitingModeStart -= InitializeWaitingModeScenario;
        EventsManager.OnGameStart -= InitializeGameScenario;

        EventsManager.OnAsteroidDestroyed -= AsteroidDestroyed;
        EventsManager.OnPlayerDead -= PlayerDead;
    }
    
    private void InitializeWaitingModeScenario()
    {
        CleanCurrentAsteroids();
        GenerateDecorationAsteroids();
    }
    private void InitializeGameScenario()
    {
        if (currentInitializeGameScenarioCoroutine != null)
        {
            StopCoroutine(currentInitializeGameScenarioCoroutine);
        }
        currentInitializeGameScenarioCoroutine=StartCoroutine(InitializeGameScenarioCoroutine());
        
    }
    private IEnumerator InitializeGameScenarioCoroutine()
    {
        //Initialization is done in a coroutine to give the player time to position himself and activate their collider, so the asteroids spawn method can 
        //detect it and avoid spawning an asteroid on top of the player and causing instant death.
        currentRound = 0;
        CleanCurrentAsteroids();
        EventsManager.PlayerCanBeGenerated();
        yield return null;
        GenerateRoundAsteroids();
    }

    private void AsteroidDestroyed(AsteroidScript asteroidScript)
    {
        EraseAsteroidFromReferences(asteroidScript);
        GenerateDestructionParticleSystem(asteroidScript.transform.position);

        switch (asteroidScript.OwnAsteroidType)
        {
            case AsteroidScript.AsteroidType.BigAsteroid:
                CalculateNewAsteroidsDirections(asteroidScript);
                GenerateMediumAsteroid(asteroidScript.transform.position, newDirection1);
                GenerateMediumAsteroid(asteroidScript.transform.position, newDirection2);
                break;

            case AsteroidScript.AsteroidType.MediumAsteroid:
                CalculateNewAsteroidsDirections(asteroidScript);
                GenerateSmallAsteroid(asteroidScript.transform.position, newDirection1);
                GenerateSmallAsteroid(asteroidScript.transform.position, newDirection2);
                break;

            case AsteroidScript.AsteroidType.SmallAsteroid:
                CheckRoundComplete();
                break;
        }
    }
    private void PlayerDead()
    {
        if (currentPlayerDeadCoroutine != null)
        {
            StopCoroutine(currentPlayerDeadCoroutine);
        }
        currentPlayerDeadCoroutine = StartCoroutine(PlayerDeadCoroutine());
    }
    private IEnumerator PlayerDeadCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        if (lifesManager.CurrentLifes > 0)
        {
            //Instead generate directly the player, first checks if there are an asteroid close to the spawn point to avoid an instantly death
            bool canGeneratePlayer = false;

            while (canGeneratePlayer == false)
            {
                detectedGameObject = null;
                detectedGameObject = Physics2D.OverlapCircle(Vector2.zero, playerSafeAreaRadius, asteroidsLayer);

                if (detectedGameObject == null)
                {
                    canGeneratePlayer = true;
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
            EventsManager.PlayerCanBeGenerated();
        }
    }

    #endregion

    #region Asteroids Management

    private void GenerateDecorationAsteroids()
    {
        for (int i = 0; i < 4; i++)
        {
            GenerateBigAsteroid();
        }
    }
    private void GenerateRoundAsteroids()
    {
        for (int i = 0; i < dataManager.ConfigurationsData.GetAsteroidsPerRound(currentRound); i++)
        {
            GenerateBigAsteroid();
        }
        currentRound++;
    }

    private void CleanCurrentAsteroids()
    {
        for (int i = 0; i < currentAsteroidScripts.Length; i++)
        {
            currentAsteroidScripts[i].CleanAsteroid();
        }
        currentAsteroidScripts = new AsteroidScript[0];
    }
    private void CheckRoundComplete()
    {
        if (currentAsteroidScripts.Length == 0)
        {
            GenerateRoundAsteroids();
        }
    }

    private void GenerateBigAsteroid()
    {
        newSpawnPosition = CalculateNewBigAsteroidPosition();
        newSpawnRotation = CalculateAsteroidInitialRotation();

        float randomXAngle = UnityEngine.Random.Range(-1f, 1f);
        float randomYAngle = UnityEngine.Random.Range(-1f, 1f);
        newDirection1 = new Vector2(randomXAngle, randomYAngle).normalized;

        newAsteroid = bigAsteroidsPool.GetInstance();
        newAsteroid.transform.SetPositionAndRotation(newSpawnPosition, newSpawnRotation);
        newAsteroidScript = newAsteroid.GetComponent<AsteroidScript>();
        newAsteroidScript.InitializeAsteroid(newDirection1);

        AddAsteroidToReferences(newAsteroidScript);
    }
    private void GenerateMediumAsteroid(Vector2 position, Vector2 direction)
    {
        newAsteroid = mediumAsteroidsPool.GetInstance();
        newSpawnRotation = CalculateAsteroidInitialRotation();
        newAsteroid.transform.SetPositionAndRotation(position, newSpawnRotation);

        newAsteroidScript = newAsteroid.GetComponent<AsteroidScript>();
        newAsteroidScript.InitializeAsteroid(direction);

        AddAsteroidToReferences(newAsteroidScript);
    }
    private void GenerateSmallAsteroid(Vector2 position, Vector2 direction)
    {
        newAsteroid = smallAsteroidsPool.GetInstance();
        newSpawnRotation = CalculateAsteroidInitialRotation();
        newAsteroid.transform.SetPositionAndRotation(position, newSpawnRotation);

        newAsteroidScript = newAsteroid.GetComponent<AsteroidScript>();
        newAsteroidScript.InitializeAsteroid(direction);

        AddAsteroidToReferences(newAsteroidScript);
    }

    private void EraseAsteroidFromReferences(AsteroidScript asteroidScript)
    {
        for (int i = 0; i < currentAsteroidScripts.Length; i++)
        {
            if (currentAsteroidScripts[i] == asteroidScript)
            {
                currentAsteroidScripts[i] = null;
                currentAsteroidScripts = currentAsteroidScripts.Where(x => x != null).ToArray();
                return;
            }
        }
    }
    private void AddAsteroidToReferences(AsteroidScript asteroidScript)
    {
        Array.Resize(ref currentAsteroidScripts, currentAsteroidScripts.Length + 1);
        currentAsteroidScripts[currentAsteroidScripts.Length - 1] = asteroidScript;
    }

    #endregion

    #region Asteroids values calculation

    private void CalculateNewAsteroidsDirections(AsteroidScript asteroidScript)
    {
        newDirection1 = Quaternion.AngleAxis(30, Vector3.forward) * asteroidScript.Direction;
        newDirection2 = Quaternion.AngleAxis(-30, Vector3.forward) * asteroidScript.Direction;
    }

    private Vector3 CalculateNewBigAsteroidPosition()
    {
        bool canGenerateAsteroid = false;
        Vector3 posibleSpawnPosition = Vector3.zero;
        Vector2 screenPosition=Vector2.zero;

        //To avoid generate an Asteroid on top of the player, checks if the player is around to generate another spawn position
        while (canGenerateAsteroid == false)
        {
            //The game will be executed in diferent screens and resolutions, so to adapt the spawn to any case, I use
            //the screen width and height to calculate posible instance positions for the new Asteroides
            randomWidth = UnityEngine.Random.Range(0, screenWidth);
            randomHeight = UnityEngine.Random.Range(0, screenHeight);
            screenPosition = new Vector2(randomWidth, randomHeight);
            posibleSpawnPosition = gameCamera.ScreenToWorldPoint(screenPosition);

            detectedGameObject = null;
            detectedGameObject = Physics2D.OverlapCircle(posibleSpawnPosition, playerSafeAreaRadius, playerLayer);
            if (detectedGameObject == null)
            {
                canGenerateAsteroid = true;
            }
        }
        return posibleSpawnPosition;
    }

    private Quaternion CalculateAsteroidInitialRotation()
    {
        return Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
    }

    #endregion

    #region Player and Pools Management

    private void InstantiatePlayer()
    {
        GameObject player = Instantiate(playerPrefab, Vector3.up * 20, Quaternion.identity);

        PlayerMovementScript playerMovement = player.GetComponent<PlayerMovementScript>();
        playerMovement.InitializeScript(dataManager);
        PlayerShootingScript playerShooting = player.GetComponent<PlayerShootingScript>();
        playerShooting.InitializeScript(dataManager);

        InputManager inputManager = GetComponent<InputManager>();
        inputManager.PlayerMovementScript = playerMovement;
        inputManager.PlayerShootingScript = playerShooting;
    }

    private void InitializePools()
    {
        pooledItemsParent = new GameObject("PooledBoardManagerObjects").transform;
        pooledItemsParent.position = Vector3.up * 20;

        bigAsteroidsPool = new SimplePool(bigAsteroidPrefab, pooledItemsParent, 6, 2);
        mediumAsteroidsPool = new SimplePool(mediumAsteroidPrefab, pooledItemsParent, 12, 2);
        smallAsteroidsPool = new SimplePool(smallAsteroidPrefab, pooledItemsParent, 24, 2);
        destructionParticleSystemPool = new SimplePool(destructionParticleSystemPrefab, pooledItemsParent, 10, 2);
    }

    private void GenerateDestructionParticleSystem(Vector2 position)
    {
        newDestructionParticleSystemGameObject = destructionParticleSystemPool.GetInstance();
        newDestructionParticleSystemGameObject.transform.position = position;
        newDestructionParticleSystemGameObject.GetComponent<PooledParticleSystem>().UseParticleSystem();
    }

    #endregion
}
