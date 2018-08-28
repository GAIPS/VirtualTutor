using HookControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BalloonsHooks : Hook
    {
        public TMP_Text text = null;
        public GameObject balloon = null;
        public GameObject tailTopLeft = null;
        public GameObject tailTopRight = null;
        public GameObject tailBotLeft = null;
        public GameObject tailBotRight = null;

        public TextMeshData textData = new TextMeshData();
        public VoidFunc onClick;

        void Awake()
        {
            textData.m_TextComponent = text;
            textData.rectTransform = text.GetComponent<RectTransform>();
            textData.m_TextComponent.ForceMeshUpdate();
            textData.initialColor = textData.m_TextComponent.color;
            textData.initialRectX = textData.rectTransform.localScale.x;
            textData.initialRectY = textData.rectTransform.localScale.y;
        }

        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(textData.ON_TEXT_CHANGED);
        }

        void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(textData.ON_TEXT_CHANGED);
        }

        public void SetTail(bool top, bool left)
        {
            if (tailTopLeft)
                tailTopLeft.SetActive(top && left);
            if (tailBotLeft)
                tailBotLeft.SetActive(!top && left);
            if (tailTopRight)
                tailTopRight.SetActive(top && !left);
            if (tailBotRight)
                tailBotRight.SetActive(!top && !left);
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
            if (animator && animator.isActiveAndEnabled)
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
            if (animator && animator.isActiveAndEnabled)
            {
                animator.SetBool("Showing", false);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

    }
}