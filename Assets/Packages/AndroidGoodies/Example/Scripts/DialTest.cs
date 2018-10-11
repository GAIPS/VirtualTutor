#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies;
using UnityEngine;

namespace AndroidGoodiesExamples {
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