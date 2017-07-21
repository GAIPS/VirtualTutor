#if UNITY_ANDROID
using UnityEngine;
using DeadMosquito.AndroidGoodies;

namespace AndroidGoodiesExamples
{
    public class DialTest : MonoBehaviour
    {
        private const string PhoneNumber = "123456789";

        public void OnShowDialer()
        {
            AGDialer.OpenDialer(PhoneNumber);
        }

        public void OnPlaceCall()
        {
            AGDialer.PlacePhoneCall(PhoneNumber);
        }
    }
}
#endif