using System.Collections;
using UnityEngine;

public class TeleportableObject : MonoBehaviour
{
    // Functionallity script that allows to the Gameobject that have it relocate in the game Area when go out from the viewport

    #region Variables

    [Header("Private Use")]
    private Camera camara;

    [Header("Memory Allocation")]
    Coroutine currentRelocationCoroutine;
    Vector2 viewportPosition;
    Vector2 newPosition;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        camara = Camera.main;
    }

    private void OnEnable()
    {
        StartRelocationCoroutine();
    }
    private void OnDisable()
    {
        StopRelocationCoroutine();
    }

    #endregion

    #region Functionality

    private void StartRelocationCoroutine()
    {
        if (currentRelocationCoroutine != null)
        {
            StopCoroutine(currentRelocationCoroutine);
        }
        currentRelocationCoroutine = StartCoroutine(RelocationCoroutine());
    }
    private void StopRelocationCoroutine()
    {
        if (currentRelocationCoroutine != null)
        {
            StopCoroutine(currentRelocationCoroutine);
            currentRelocationCoroutine = null;
        }
    }
    private IEnumerator RelocationCoroutine()
    {
        //With this logic in a Coroutine is more easy control when is executing and when is not doing.
        //Being a coroutine instead an Update allows dont execute the code in all frames. In this case is executting in one of each 3 frames.

        //I use not exact values because the viewport position is based in the pivot of the object. Increasing by 0.05f this values 
        //the object will move a little more out of the screen before teleport, not when the pivot go out of the screen

        //The positions are multiplied by -0.98 to decrease slightly the value and in the next frame dont detonate the other case

        for (; ; )
        {
            viewportPosition = camara.WorldToViewportPoint(transform.position);

            if (viewportPosition.x > 1.05f || viewportPosition.x < -0.05f)
            {
                newPosition = transform.position;
                newPosition.x *= -0.98f;
                transform.position = newPosition;
            }
            else if (viewportPosition.y > 1.05f || viewportPosition.y < -0.05f)
            {
                newPosition = transform.position;
                newPosition.y *= -0.98f;
                transform.position = newPosition;
            }
            yield return null;
            yield return null;
            yield return null;
        }
    }

    #endregion

}
