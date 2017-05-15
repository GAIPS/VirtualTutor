using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Children size fitter.
/// 
/// Size the owner of this script to the size of all the children of a given
/// game object.
/// </summary>
[ExecuteInEditMode]
public class ChildrenSizeFitter : MonoBehaviour {

    public RectTransform parentTransform;

    public Transform childrenHolder;

    public float multiplier = 1.0f;
	
    // Update is called once per frame
    void Update() {
        if (childrenHolder && parentTransform) {
            float maxY = 0;
            foreach (Transform child in childrenHolder) {
                RectTransform childRect = child as RectTransform;
                if (childRect) {
                    float maxYLocal = System.Math.Abs(child.localPosition.y) + System.Math.Abs(childRect.rect.yMax);
                    if (maxY < maxYLocal) {
                        maxY = maxYLocal;
                    }
                }
            }

            parentTransform.sizeDelta = new Vector2(parentTransform.sizeDelta.x,
                                                    maxY * multiplier);
        }
    }
}
