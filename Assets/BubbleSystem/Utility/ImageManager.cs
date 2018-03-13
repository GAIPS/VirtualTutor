using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImageManager : MonoBehaviour {

    protected void StopCoroutineWithCheck(IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
