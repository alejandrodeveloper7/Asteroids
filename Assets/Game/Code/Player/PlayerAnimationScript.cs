using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    #region Variables

    [Header("References")]
    private Animator playerAnimator;

    #endregion

    #region Monobehaviour

    private void Awake() => playerAnimator = GetComponent<Animator>();

    #endregion

    #region Events

    private void OnEnable()
    {
        EventsManager.OnPlayerCanBeGenerated += RestartAnimator;
        EventsManager.OnPlayerDead += PlayDeadAnimation;
    }
    private void OnDisable()
    {
        EventsManager.OnPlayerCanBeGenerated -= RestartAnimator;
        EventsManager.OnPlayerDead -= PlayDeadAnimation;
    }

    public void RestartAnimator() => playerAnimator.Rebind();
    public void PlayDeadAnimation() => playerAnimator.SetTrigger("Dead");

    #endregion
}
