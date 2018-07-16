using HookControl;
using UnityEngine;

public class AnimationHook : Hook
{
    [SerializeField] private Animator showHideAnimator;

    public bool ShowOnStart;

    /// <summary>
    /// Called when the Show animation ended.
    /// </summary>
    public VoidFunc onShowEnded;

    /// <summary>
    /// Called when the Hide animation ended.
    /// </summary>
    public VoidFunc onHideEnded;

    private void Start()
    {
        if (ShowOnStart)
        {
            Show();
        }
    }

    public void Show()
    {
        if (showHideAnimator)
        {
            showHideAnimator.SetBool("Showing", true);
        }
        else
        {
            Debug.LogWarning("Not animator found while trying to show.");
            UIOnShowEnded();
        }
    }

    public void Hide()
    {
        if (showHideAnimator)
        {
            showHideAnimator.SetBool("Showing", false);
        }
        else
        {
            Debug.LogWarning("Not animator found while trying to hide.");
            UIOnHideEnded();
        }
    }

    public void UIOnShowEnded()
    {
        if (onShowEnded != null)
        {
            onShowEnded();
        }
    }

    public void UIOnHideEnded()
    {
        if (onHideEnded != null)
        {
            onHideEnded();
        }
    }
}