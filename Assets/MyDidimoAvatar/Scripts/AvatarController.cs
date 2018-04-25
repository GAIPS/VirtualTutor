using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public partial class AvatarController : MonoBehaviour
{
    private Animator animator;
    private AvatarParameters parameters;

    private static float LOWINTENSITYTHRESHOLD = 0.5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        parameters = GetComponent<AvatarParameters>();

    }

    void Start()
    {
        float[][] paramPreset = AvatarParameters.Presets.Neutral();
        parameters.setAllParameters<AnimatorParams>(paramPreset[0]);
        parameters.setAllParameters<ControllerParams>(paramPreset[1]);

        StartCoroutine("NoddingRoutine");
    }

    void FixedUpdate(){
        //AnimationClip c = getClipByName("Head_Nod");
        //Debug.Log("CURRENT STATE INFO: [NAME = " + c.name + ", LENGHT = " + c.length + "]");
        //currentBaseState = animator.GetCurrentAnimatorStateInfo(3);
        //if (currentBaseState.IsName("Head_Nod"))
        //    Debug.Log("CURRENT STATE INFO: [LENGHT = " + currentBaseState.length + ", SPEEDMULT = " + currentBaseState.speedMultiplier + "]");
    }

    public void SetMood(EmotionalState mood, float intensity)
    {
        float[][] preset;
        switch (mood)
        {
            case EmotionalState.NEUTRAL:
                preset = AvatarParameters.Presets.Neutral();
                break;
            case EmotionalState.HAPPINESS:
                if(intensity > LOWINTENSITYTHRESHOLD)
                    preset = AvatarParameters.Presets.HappinessHigh();
                else
                    preset = AvatarParameters.Presets.HappinessLow();
                break;
            case EmotionalState.SADNESS:
                if (intensity > LOWINTENSITYTHRESHOLD)
                    preset = AvatarParameters.Presets.SadnessHigh();
                else
                    preset = AvatarParameters.Presets.SadnessLow();
                break;
            case EmotionalState.FEAR:
                preset = AvatarParameters.Presets.Fear();
                break;
            case EmotionalState.SURPRISE:
                preset = AvatarParameters.Presets.Surprise();
                break;
            case EmotionalState.ANGER:
            case EmotionalState.DISGUST:
            default:
                preset = AvatarParameters.Presets.Neutral();
                break;
        }

        parameters.setAllParameters<AnimatorParams>(preset[0]);
        parameters.setAllParameters<ControllerParams>(preset[1]);
        parameters.setParameter(AnimatorParams.MOOD_INTENSITY, intensity);

        animator.SetInteger("Mood", (int)mood);
    }

    public void ExpressEmotion(EmotionalState expression, float intensity)
    {
        parameters.setParameter(AnimatorParams.EXPRESSION_INTENSITY, intensity);
        animator.SetInteger("Expression", (int)expression);
        animator.SetTrigger("Express");
    }

    public void DoNodding(NodState nodState)
    {
        animator.SetInteger("Nod Style", (int)nodState);
        animator.SetTrigger("Nod");
    }

    IEnumerator NoddingRoutine()
    {
        int maxSuccesses = 2, rollCounter=0;
        float length, frequency;
        while (true)
        {   
            if (animator.gameObject.activeSelf && animator.GetBool("Listening"))
            {
                length = parameters.nodLength / parameters.getParameter(AnimatorParams.NOD_SPEED);
                frequency = parameters.getParameter(ControllerParams.NOD_FREQUENCY);
                if ((Random.value < frequency) && (rollCounter < maxSuccesses) &&  
                    (Random.value < frequency)) // Unlucky Rolls
                {
                    DoNodding(NodState.NOD_START);
                    rollCounter++;
                }
                else
                {
                    DoNodding(NodState.NOD_END);
                    rollCounter = 0;
                }
                yield return new WaitForSeconds(length);    
            }
            yield return null;      
        }
    }

    public void DoTalking(TalkState talkState)
    {
        animator.SetInteger("Talk State", (int)talkState);
        animator.SetTrigger("Talk");
    }

    public void DoGazing(GazeState gazeState)
    {
        animator.SetInteger("Direction", (int)gazeState);

        // Randomizer for gaze frequency
        if (Random.value < parameters.getParameter(ControllerParams.GAZEAT_FREQUENCY))
        {  
            if ((int)gazeState % 2 != 0) // (gazeAt anim. are all odd numbered states)
                animator.SetTrigger("Gaze");
        }
    }

    public void isListening(bool state)
    {
        animator.SetBool("Listening", state);
    }

    public void setParameter(AnimatorParams param, float value)
    {
        parameters.setParameter(param, value);
    }
    public void setParameter(ControllerParams param, float value)
    {
        // Gaze frequency is set for both "at" and "back" animations
        if(param == ControllerParams.GAZEAT_FREQUENCY || param == ControllerParams.GAZEBACK_FREQUENCY)
        {
            parameters.setParameter(ControllerParams.GAZEAT_FREQUENCY, value);
            parameters.setParameter(ControllerParams.GAZEBACK_FREQUENCY, value);
        }
        else
            parameters.setParameter(param, value);
    }

    public AvatarParameters getParameters()
    {
        return parameters;
    }

}