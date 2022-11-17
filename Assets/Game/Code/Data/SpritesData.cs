using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidSprites", menuName = "ScriptableObjects / Asteroid Sprites File", order = 1)]
public class SpritesData : ScriptableObject
{
    #region Variables

    [Header("Asteroids Sprites")]
    public Sprite[] bigAsteroidSprites;
    public Sprite[] mediumAsteroidSprites;
    public Sprite[] smallAsteroidSprites;

    #endregion

    public Sprite GetRandomAsteroidSprite(AsteroidScript.AsteroidType type)
    {
        int randomNumber;

        switch (type)
        {
            case AsteroidScript.AsteroidType.BigAsteroid:
                randomNumber = Random.Range(0, bigAsteroidSprites.Length);
                return bigAsteroidSprites[randomNumber];

            case AsteroidScript.AsteroidType.MediumAsteroid:
                randomNumber = Random.Range(0, mediumAsteroidSprites.Length);
                return mediumAsteroidSprites[randomNumber];

            case AsteroidScript.AsteroidType.SmallAsteroid:
                randomNumber = Random.Range(0, smallAsteroidSprites.Length);
                return smallAsteroidSprites[randomNumber];

            default:
                return null;
        }
    }
}
