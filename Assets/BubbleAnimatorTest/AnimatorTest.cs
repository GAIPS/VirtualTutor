using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour {
    private Animator animator;

    public float Happiness, Sadness, Anger, Fear, Disgust, Surprise;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Happiness", Happiness);
        animator.SetFloat("Sadness", Sadness);
        animator.SetFloat("Anger", Anger);
        animator.SetFloat("Fear", Fear);
        animator.SetFloat("Disgust", Disgust);
        animator.SetFloat("Surprise", Surprise);
    }

}
