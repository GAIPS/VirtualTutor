using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRandomizer : StateMachineBehaviour
{
    [SerializeField]
    private int maxRange;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Mood Randomizer", (float)Random.Range(0, maxRange+1));
    }
}