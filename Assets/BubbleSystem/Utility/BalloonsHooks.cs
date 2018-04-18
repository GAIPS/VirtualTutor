using HookControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalloonsHooks : Hook
{
    [SerializeField]
    public TMP_Text topicTextLeft = null;
    [SerializeField]
    public TMP_Text topicTextTop = null;
    [SerializeField]
    public TMP_Text topicTextRight = null;
    [SerializeField]
    public TMP_Text topicTextExtra = null;
    [SerializeField]
    public GameObject topicLeft = null;
    [SerializeField]
    public GameObject topicRight = null;
    [SerializeField]
    public GameObject topicTop = null;
    [SerializeField]
    public GameObject topicExtra = null;
    [SerializeField]
    public GameObject peakTopLeft = null;
    [SerializeField]
    public GameObject peakTopRight = null;
    [SerializeField]
    public GameObject peakBotLeft = null;
    [SerializeField]
    public GameObject peakBotRight = null;


    public VoidFunc onLeft;
    public VoidFunc onTop;
    public VoidFunc onRight;
    public VoidFunc onExtra;

    public void SetPeak(bool top, bool left)
    {
        if(peakTopLeft)
            peakTopLeft.SetActive(top && left);
        if(peakBotLeft)
            peakBotLeft.SetActive(!top && left);
        if(peakTopRight)
            peakTopRight.SetActive(top && !left);
        if(peakBotRight)
            peakBotRight.SetActive(!top && !left);
    }

    public void UIExtra()
    {
        if (onExtra != null)
            onExtra ();
    }
    public void UILeft ()
    {
        if (onLeft != null)
            onLeft ();
    }

    public void UITop ()
    {
        if (onTop != null)
            onTop ();
    }

    public void UIRight ()
    {
        if (onRight != null)
            onRight ();
    }

    public string ContentLeft {
        get
        {
            if (topicTextLeft)
                return this.topicTextLeft.text;
            throw new MissingComponentException("Component topic text left does not exist");
        }
        set
        {
            if (topicLeft && topicTextLeft)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    topicLeft.SetActive(true);
                    this.topicTextLeft.text = value;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    topicLeft.SetActive(false);
                }
            }
        }
    }

    public string ContentTop {
        get
        {
            if (topicTextTop)
                return this.topicTextTop.text;
            throw new MissingComponentException("Component topic text top does not exist");
        }
        set {
            if (topicTop && topicTextTop)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    topicTop.SetActive(true);
                    this.topicTextTop.text = value;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    topicTop.SetActive(false);
                }
            }
        }
    }

    public string ContentRight {
        get
        {
            if (topicTextRight)
                return this.topicTextRight.text;
            throw new MissingComponentException("Component topic text right does not exist");
        }
        set {
            if (topicRight && topicTextRight)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    topicRight.SetActive(true);
                    this.topicTextRight.text = value;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    topicRight.SetActive(false);
                }
            }
        }
    }

    public string ContentExtra {
        get
        {
            if (topicTextExtra)
                return this.topicTextExtra.text;
            throw new MissingComponentException("Component topic text extra does not exist");
        }
        set
        {
            if (topicExtra && topicTextExtra)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    topicExtra.SetActive(true);
                    this.topicTextExtra.text = value;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    topicExtra.SetActive(false);
                }
            }
        }
    }

    public void Show()
    {
        show(topicTop);
        show(topicLeft);
        show(topicRight);
        show(topicExtra);
    }

    public void Hide()
    {
        hide(topicTop);
        hide(topicLeft);
        hide(topicRight);
        hide(topicExtra);
    }

    protected void show(GameObject ballon) {
        if (!ballon || !ballon.activeSelf) {
            return;
        }
        var animator = ballon.GetComponent<Animator>();
        if (animator) {
            animator.SetBool("Showing", true);
        } else {
            ballon.SetActive(true);
        }
    }

    protected void hide(GameObject ballon) {
        if (!ballon || !ballon.activeSelf) {
            return;
        }
        var animator = ballon.GetComponent<Animator>();
        if (animator) {
            animator.SetBool("Showing", false);
        } else {
            ballon.SetActive(false);
        }
    }

}