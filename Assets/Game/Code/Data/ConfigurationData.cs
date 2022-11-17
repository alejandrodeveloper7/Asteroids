using UnityEngine;

[CreateAssetMenu(fileName = "Configuration", menuName = "ScriptableObjects / Configuration File", order = 0)]
public class ConfigurationData : ScriptableObject
{

    #region Variables

    [Header("Controls")]
    public KeyCode moveForwardKey;
    public KeyCode turnLeftKey;
    public KeyCode turnRightKey;
    public KeyCode shootKey;
    [Space]
    public KeyCode closeGameKey;
    
    [Header("Asteroids")]
    public int smallAsteroidSpeed;
    public int mediumAsteroidSpeed;
    public int bigAsteroidSpeed;
    [Space]    
    public int bigAsteroidScore;
    public int mediumAsteroidScore;
    public int smallAsteroidScore;
    [Space]
    public int[] asteroidsPerRound;

    [Header("Player")]
    public float movementSpeed;
    public float rotationSpeed;

    [Header("Shoots")]
    public float betweenShootsTime;
    public float shootLifeDuration;
    public float shootMovementSpeed;

    [Header("Extra Lifes")]
    public int scoreForFirstExtraLife;
    public int scoreForSecondExtraLife;
    public int scoreForThirdExtraLife;

    #endregion

    public int GetAsteroidsPerRound(int round)
    {
        if (round >= asteroidsPerRound.Length)
        {
            return asteroidsPerRound[asteroidsPerRound.Length - 1];
        }
        else
        {
            return asteroidsPerRound[round];
        }
    }

    public int GetAsteroidSpeed(AsteroidScript.AsteroidType asteroidType)
    {
        switch (asteroidType)
        {
            case AsteroidScript.AsteroidType.SmallAsteroid:
                return smallAsteroidSpeed;
                
            case AsteroidScript.AsteroidType.MediumAsteroid:
                return mediumAsteroidSpeed;

            case AsteroidScript.AsteroidType.BigAsteroid:
                return bigAsteroidSpeed;

            default:
                return 0;
        }
    }
}
