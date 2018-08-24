using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public interface IStopper
    {
        void Play(IEnumerator coroutine);
        void Stop(IEnumerator coroutine);
    }
}