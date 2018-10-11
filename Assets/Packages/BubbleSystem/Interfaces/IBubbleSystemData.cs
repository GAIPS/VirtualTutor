using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public interface IBubbleSystemData
    {
        bool IsCleared();
        void Clear();
    }
}