using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public partial class AvatarController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ExpressEmotion(ExpressionState expression)
    {
        animator.SetInteger("Expression", (int)expression);
        animator.SetTrigger("Express");
    }

    public void SetMood(MoodState moodState)
    {
        animator.SetInteger("Mood", (int)moodState);
    }

    public void PerformAction(ActionState actionState)
    {
        animator.SetInteger("Action", (int)actionState);
        if(!((int)actionState < 0))
            animator.SetTrigger("Act");
    }
}