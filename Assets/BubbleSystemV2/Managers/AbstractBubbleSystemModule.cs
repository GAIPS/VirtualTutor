using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public abstract class AbstractBubbleSystemModule : MonoBehaviour
    {
        public abstract void UpdateScene(BubbleSystemData data);
    }
}