using HookControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalloonsHooks : Hook
{
    public TMP_Text text = null;
    public GameObject balloon = null;
    public GameObject peakTopLeft = null;
    public GameObject peakTopRight = null;
    public GameObject peakBotLeft = null;
    public GameObject peakBotRight = null;

    public VoidFunc onClick;

    public void SetPeak(bool top, bool left)
    {
        if (peakTopLeft)
            peakTopLeft.SetActive(top && left);
        if (peakBotLeft)
            peakBotLeft.SetActive(!top && left);
        if (peakTopRight)
            peakTopRight.SetActive(top && !left);
        if (peakBotRight)
            peakBotRight.SetActive(!top && !left);
    }

    public void UIClick()
    {
        if (onClick != null)
            onClick();
    }

    public string Content
    {
        get
        {
            if (text)
                return this.text.text;
            throw new MissingComponentException("Component topic text top does not exist");
        }
        set
        {
            if (text)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.gameObject.SetActive(true);
                    text.text = value;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Show()
    {
        show();
    }

    public void Hide()
    {
        hide();
    }

    protected void show()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        var animator = this.GetComponent<Animator>();
        if (animator)
        {
            animator.SetBool("Showing", true);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    protected void hide()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        var animator = this.GetComponent<Animator>();
        if (animator)
        {
            animator.SetBool("Showing", false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

}