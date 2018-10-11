using System;
using UnityEngine;

namespace HookControl
{
    [Serializable]
    public class Control : IControl
    {
        /*
         * SUPPORT to create or destroy UI
         */
        [SerializeField] public GameObject prefab;

        public GameObject Instance { get; set; }

        public bool DontDestroyOnLoad { get; set; }

        public Control()
        {
            DontDestroyOnLoad = false;
        }

        public Control(GameObject prefab) : this()
        {
            this.prefab = prefab;
        }

        public virtual string GetName()
        {
            return "Control";
        }

        public ShowResult Show()
        {
            var result = ShowResult.OK;
            if (Instance == null)
            {
                if (prefab == null)
                    return ShowResult.FAIL;
                Instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);

                if (!Instance)
                    return ShowResult.FAIL;
                if (DontDestroyOnLoad)
                {
                    GameObject.DontDestroyOnLoad(Instance.gameObject);
                }

                result = ShowResult.FIRST;
            }
            else if (Instance.activeSelf == false)
            {
                Instance.SetActive(true);
            }

            var animationHook = Instance.GetComponent<AnimationHook>();
            if (animationHook)
            {
                //Default hiding behaviour
                animationHook.onHideEnded = DestroyGameObject;
                animationHook.Show();
            }

            return result;
        }

        public void Destroy()
        {
            if (Instance == null) return;

            var animationHook = Instance.GetComponent<AnimationHook>();
            if (animationHook)
            {
                animationHook.onHideEnded = DestroyGameObject;
                animationHook.Hide();
            }
            else
            {
                DestroyGameObject();
            }
        }

        private void DestroyGameObject()
        {
            GameObject.Destroy(Instance.gameObject);
            Instance = null;
        }

        public void Disable()
        {
            if (Instance == null) return;

            var animationHook = Instance.GetComponent<AnimationHook>();
            if (animationHook)
            {
                animationHook.onHideEnded = () => { Instance.SetActive(false); };
                animationHook.Hide();
            }
            else
            {
                Instance.SetActive(false);
            }
        }

        public bool IsVisible()
        {
            return Instance != null && Instance.activeSelf;
        }
    }
}