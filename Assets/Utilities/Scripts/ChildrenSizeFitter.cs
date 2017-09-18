using System;
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

    public Vector2 multiplier = new Vector2(1.0f, 1.0f);

    public bool adjustWidth, adjustHeight;
	
    // Update is called once per frame
    void Update() {
        if (childrenHolder && parentTransform) {
            Vector2 max = new Vector2();
            Vector2 maxLocal = new Vector2();
            foreach (Transform child in childrenHolder) {
                RectTransform childRect = child as RectTransform;
                if (childRect) {
                    maxLocal.x = Math.Abs(child.localPosition.x) + Math.Abs(childRect.rect.xMax);
                    maxLocal.y = Math.Abs(child.localPosition.y) + Math.Abs(childRect.rect.yMax);

                    if (max.y < maxLocal.y) {
                        max.y = maxLocal.y;
                    }
                    if (max.x < maxLocal.x) {
                        max.x = maxLocal.x;
                    }
                }
            }

            float width = adjustWidth ? max.x * multiplier.x : parentTransform.sizeDelta.x;
            float height = adjustHeight ? max.y * multiplier.y : parentTransform.sizeDelta.y;

            parentTransform.sizeDelta = new Vector2(width,
                                                    height);
        }
    }
}
