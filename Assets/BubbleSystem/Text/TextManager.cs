using UnityEngine;

namespace BubbleSystem
{
    public class TextManager : MonoBehaviour
    {

        [SerializeField]
        private TextModifier textModifier;

        public TextData SelectText(Data data)
        {
            return textModifier.SelectText(data);
        }
    }
}