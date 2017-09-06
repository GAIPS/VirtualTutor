using UnityEngine;

[ExecuteInEditMode]
public class SameHeightWidth : MonoBehaviour {

    public RectTransform copyTransform;
    public RectTransform targetTransform;

    private Vector2 previous;
	
    // Update is called once per frame
    void Update() {
        if (!copyTransform || !targetTransform) {
            return;
        }

        if (previous.y != copyTransform.rect.height || previous.x != copyTransform.rect.width) {
            previous.x = copyTransform.rect.width;
            previous.y = copyTransform.rect.height;
            targetTransform.sizeDelta = new Vector2(previous.x,
                                                    previous.y);
        }
    }
}
