using UnityEngine;

public class IdleRandomizer : StateMachineBehaviour
{
    [SerializeField]
    private int maxRange;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Idle Randomizer", Random.Range(0.0f, 1.0f));
    }
}