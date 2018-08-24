using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public class NextData : IBubbleSystemData
    {
        public BubbleSystemData data = new BubbleSystemData();
        public bool isSet = false;

        public void Clear()
        {
            data.Clear();
            isSet = false;
        }

        public bool IsCleared()
        {
            return data.IsCleared() && isSet == false;
        }
    }
}