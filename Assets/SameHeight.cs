using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SameHeight : MonoBehaviour {

    public RectTransform copyTransform;
    public RectTransform targetTransform;

    private float previousHeight;
	
    // Update is called once per frame
    void Update() {
        if (!copyTransform || !targetTransform) {
            return;
        }

        if (previousHeight != copyTransform.rect.height) {
            previousHeight = copyTransform.rect.height;
            targetTransform.sizeDelta = new Vector2(targetTransform.sizeDelta.x,
                                                     previousHeight); 
        }
    }
}
