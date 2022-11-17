using UnityEditor;
using UnityEngine;

public class TopMenuScriptEditor : Editor
{
    [MenuItem("Configuration/Configuration Values")]
    private static void FocusConfigurationScriptableObject()
    {
        Selection.activeObject = Resources.Load<ConfigurationData>("Configuration");
    }

    [MenuItem("Configuration/Asteroids Sprites")]
    private static void FocusAsteroidsSpritesScriptableObject()
    {
        Selection.activeObject = Resources.Load<SpritesData>("AsteroidSprites");
    }
}
