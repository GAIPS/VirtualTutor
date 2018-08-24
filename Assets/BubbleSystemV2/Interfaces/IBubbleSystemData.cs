using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public interface IBubbleSystemData
    {
        bool IsCleared();
        void Clear();
    }
}