using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public abstract class AbstractBubbleSystemData : IBubbleSystemData
    {
        public abstract void Clear();
        public abstract bool IsCleared();
    }
}