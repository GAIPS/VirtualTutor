using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineStopper : Singleton<CoroutineStopper>
{
    private CoroutineStopper() { }

    public void StopCoroutineWithCheck(IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public void StopCoroutineWithCheck(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
