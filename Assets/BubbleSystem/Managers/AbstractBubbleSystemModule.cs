using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public abstract class AbstractBubbleSystemModule : MonoBehaviour
    {
        public abstract void UpdateScene(BubbleSystemData data);
    }
}