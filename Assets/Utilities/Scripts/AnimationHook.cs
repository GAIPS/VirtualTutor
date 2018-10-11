using HookControl;
using UnityEngine;

public class AnimationHook : Hook
{
    [SerializeField] private Animator showHideAnimator;

    public bool ShowOnStart;
    private bool _visible;

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
        if (_visible) return;
        
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
        if (!_visible) return;
        
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
        _visible = true;
        if (onShowEnded != null)
        {
            onShowEnded();
        }
    }

    public void UIOnHideEnded()
    {
        _visible = false;
        if (onHideEnded != null)
        {
            onHideEnded();
        }
    }
}