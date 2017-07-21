using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImage : MonoBehaviour {

    [SerializeField]
    private Image firstImage;
    [SerializeField]
    private Image secondImage;
    [SerializeField]
    private Animator animator;

    private bool firstImageActive;

	// Use this for initialization
	void Start () {
        firstImageActive = true;
	}

    public void setSprite(Sprite sprite) {
        // Switch image with animation and stuff.

        if (firstImageActive) {
            if (sprite == firstImage.sprite) {
                return;
            }
            secondImage.sprite = sprite;
            if (!animator) {
                firstImage.gameObject.SetActive(false);
                secondImage.gameObject.SetActive(true);
            }
        } else {
            if (sprite == secondImage.sprite) {
                return;
            }
            firstImage.sprite = sprite;
            if (!animator) {
                secondImage.gameObject.SetActive(false);
                firstImage.gameObject.SetActive(true);
            }
        }
        if (animator) {
            animator.SetTrigger("Switch");
        }
        firstImageActive = !firstImageActive;
    }
}
