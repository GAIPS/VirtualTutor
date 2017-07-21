#if UNITY_ANDROID
using UnityEngine;

namespace AndroidGoodiesExamples
{
    public class EnableOnClick : MonoBehaviour
    {
        public bool enable;

        public GameObject target;

        public void OnClick()
        {
            target.SetActive(enable);
        }
    }
}
#endif