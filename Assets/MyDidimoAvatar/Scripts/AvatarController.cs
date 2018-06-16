using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public partial class AvatarController : MonoBehaviour
{
    private IEnumerator corrotina; 
    [SerializeField]
    public string controllerID;

    [Space(2.0f)]
    [Header("Speech Approach Test")]
    //TEMP
    [SerializeField]
    private bool ApproachNeutral;
    [SerializeField]
    private bool ApproachLowerIntensity = true;
    //TEMPSTORE
    private int storedMood;
    private float storedMoodIntensity;

    private Animator animator;
    private AvatarParameters parameters;

    private static float LOWINTENSITYTHRESHOLD = 0.5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        parameters = GetComponent<AvatarParameters>();
    }

    private void Start()
    {
        float[][] paramPreset = AvatarParameters.Presets.Neutral();
        parameters.setAllParameters<AnimatorParams>(paramPreset[0]);
        parameters.setAllParameters<ControllerParams>(paramPreset[1]);
    }

    private void FixedUpdate()
    {
        //AnimationClip c = getClipByName("Head_Nod");
        //Debug.Log("CURRENT STATE INFO: [NAME = " + c.name + ", LENGHT = " + c.length + "]");
        //currentBaseState = animator.GetCurrentAnimatorStateInfo(3);
        //if (currentBaseState.IsName("Head_Nod"))
        //    Debug.Log("CURRENT STATE INFO: [LENGHT = " + currentBaseState.length + ", SPEEDMULT = " + currentBaseState.speedMultiplier + "]");
    }

    public void SetMood(EmotionalState mood, float intensity, float duration = 2.0f)
    {
        float finalIntensity = intensity;
        float[][] preset;

        switch (mood)
        {
            case EmotionalState.NEUTRAL:
                preset = AvatarParameters.Presets.Neutral();
                break;

            case EmotionalState.HAPPINESS:
                if (finalIntensity > LOWINTENSITYTHRESHOLD)
                    preset = AvatarParameters.Presets.HappinessHigh();
                else
                    preset = AvatarParameters.Presets.HappinessLow();
                break;

            case EmotionalState.SADNESS:
                if (finalIntensity > LOWINTENSITYTHRESHOLD)
                    preset = AvatarParameters.Presets.SadnessHigh();
                else
                    finalIntensity = finalIntensity * 0.3f;
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
        finalIntensity = finalIntensity * parameters.getMoodDampenerValue();

        if (BubbleSystem.BubbleSystemUtility.CheckCoroutine(ref corrotina)) { StopCoroutine(corrotina); }
        corrotina = MoodTransition(animator.GetFloat("Mood Intensity"), finalIntensity, duration);
        StartCoroutine(corrotina);

        animator.SetInteger("Mood", (int)mood);

        animator.SetFloat("Desired Intensity", finalIntensity);
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

    public void DoTalking(TalkState talkState)
    {
        if (talkState == TalkState.TALK_START)
        {
            storedMood = animator.GetInteger("Mood");
            storedMoodIntensity = animator.GetFloat("Mood Intensity");
            if (ApproachNeutral)
                SetMood(EmotionalState.NEUTRAL, 0.0f);
            if (ApproachLowerIntensity)
            {
                if (BubbleSystem.BubbleSystemUtility.CheckCoroutine(ref corrotina)){ StopCoroutine(corrotina); }
                
                corrotina = MoodTransition(animator.GetFloat("Mood Intensity"), animator.GetFloat("Desired Intensity") * 0.5f, 1.0f);
                StartCoroutine(corrotina);
            }
            //SetMood((EmotionalState)storedMood, (storedMoodIntensity / parameters.getMoodDampenerValue())*0.5f, 1.0f);
        }
        else
        {
            if (ApproachNeutral || ApproachLowerIntensity) {
                if (BubbleSystem.BubbleSystemUtility.CheckCoroutine(ref corrotina)) { StopCoroutine(corrotina); }
                corrotina = MoodTransition(animator.GetFloat("Mood Intensity"), animator.GetFloat("Mood Intensity") / 0.5f, 1.0f);
                StartCoroutine(corrotina);
            }
                
            //SetMood((EmotionalState)storedMood, storedMoodIntensity / parameters.getMoodDampenerValue(), 1.0f);
        }

        animator.SetInteger("Talk State", (int)talkState);
        animator.SetTrigger("Talk");
    }

    public void DoGazing(GazeState gazeState)
    {
        animator.SetInteger("Direction", (int)gazeState);

        // Randomizer for gaze frequency
        if (UnityEngine.Random.value < parameters.getParameter(ControllerParams.GAZEAT_FREQUENCY))
        {
            if ((int)gazeState % 2 != 0) // (gazeAt anim. are all odd numbered states)
                animator.SetTrigger("Gaze");
        }
    }

    public void setParameter(AnimatorParams param, float value)
    {
        parameters.setParameter(param, value);
    }

    public void setParameter(ControllerParams param, float value)
    {
        // Gaze frequency is set for both "at" and "back" animations
        if (param == ControllerParams.GAZEAT_FREQUENCY || param == ControllerParams.GAZEBACK_FREQUENCY)
        {
            parameters.setParameter(ControllerParams.GAZEAT_FREQUENCY, value);
            parameters.setParameter(ControllerParams.GAZEBACK_FREQUENCY, value);
        }
        else
            parameters.setParameter(param, value);
    }

    private IEnumerator MoodTransition(float start, float end, float duration)
    {
        float t = 0.0f;
        while (t <= 1.0f)
        {
            parameters.setParameter(AnimatorParams.MOOD_INTENSITY, Mathf.Lerp(start, end, t));
            t +=  Time.deltaTime / duration;
            yield return null;
        }
    }

    public void isListening(bool state)
    {
        animator.SetBool("Listening", state);
    }

    public AvatarParameters getParameters()
    {
        return parameters;
    }
}