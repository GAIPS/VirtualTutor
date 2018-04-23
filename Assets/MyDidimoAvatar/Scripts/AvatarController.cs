using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Linq;
using System;

[RequireComponent(typeof(Animator))]
public partial class AvatarController : MonoBehaviour
{
    private Animator animator;
    private AvatarParameters parameters;

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
        //StartCoroutine("controllerParameterDebugRoutine");
    }

    void FixedUpdate(){
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
                if(intensity < 0.5)
                    preset = AvatarParameters.Presets.Happiness();
                else
                    preset = AvatarParameters.Presets.HappinessHigh();
                break;
            case EmotionalState.SADNESS:
                if (intensity < 0.5)
                    preset = AvatarParameters.Presets.Sadness();
                else
                    preset = AvatarParameters.Presets.SadnessHigh();
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
        float nodSpeed, nodFrequency, nodDuration, nodInterval;
        while (true)
        {
            nodFrequency = parameters.getParameter(ControllerParams.NOD_FREQUENCY);
            nodInterval = parameters.nodInterval * (1 - nodFrequency) + 0.001f;
            yield return new WaitForSeconds(nodInterval);
            if (animator.gameObject.activeSelf && animator.GetBool("Listening"))
            {
                DoNodding(NodState.NOD_START);
                //TODO: USE NOD LENGTH (FROM ANIMATOR) AS THE WAIT TIME BETWEEN NODS
                nodSpeed = parameters.getParameter(AnimatorParams.NOD_SPEED);
                nodDuration = Mathf.Abs(parameters.nodDuration) / (nodSpeed < 0.001f ? 0.001f : nodSpeed);
                yield return new WaitForSeconds(nodDuration);
            }
        }
    }

    public void DoTalking(TalkState talkState)
    {
        animator.SetInteger("Talk State", (int)talkState);
        animator.SetTrigger("Talk");
    }

    public void DoGazing(GazeState gazeState)
    {
        float rand = UnityEngine.Random.value;

        animator.SetInteger("Direction", (int)gazeState);

        // Randomizer for gaze frequency
        if (UnityEngine.Random.value <= parameters.getParameter(ControllerParams.GAZEAT_FREQUENCY))
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