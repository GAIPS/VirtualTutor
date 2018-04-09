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
    private AnimatorParameters animParams;
    float NODDURATION = 3.0f;
    // TODO: DECIDE ON THE BEST WAY TO DEFINE THE MAXIMUM INTERVAL BETWEEN NODS
    float NODMAXINTERVAL = 20.0f;

    void Start()
    {
        animator = GetComponent<Animator>();

        //TODO: MOVE ANIMATION PARAMETERS TO A SEPARATE CLASS
        animParams.talkSpeed = new KeyValuePair<string, float>("Talk Speed", 1.0f);
        animParams.nodSpeed = new KeyValuePair<string, float>("Nod Speed", 1.0f);
        animParams.nodFrequency = 1.0f;
        animParams.gazeAtSpeed = new KeyValuePair<string, float>("Gaze At Speed", 1.0f);
        animParams.gazeBackSpeed = new KeyValuePair<string, float>("Gaze Back Speed", 1.0f);
        animParams.gazeFrequency = 1.0f;

        StartCoroutine("NoddingRoutine");
        //StartCoroutine("DEBUGRoutine");
    }

    void FixedUpdate()
    {
        // Updates the parameters of the animator
        if (animator.gameObject.activeSelf)
        {  
            animator.SetFloat(animParams.talkSpeed.Key, animParams.talkSpeed.Value);
            animator.SetFloat(animParams.nodSpeed.Key, animParams.nodSpeed.Value);
            animator.SetFloat(animParams.gazeAtSpeed.Key, animParams.gazeAtSpeed.Value);
            animator.SetFloat(animParams.gazeBackSpeed.Key, animParams.gazeBackSpeed.Value);
        }
    }

    // Method for setting the controller animation parameters.
    // This method does NOT update the parameters in the animator. These are updated in the FixedUpdate() method
    private void setControllerParameters(float[] param)
    {
        //talk
        animParams.talkSpeed = new KeyValuePair<string, float>(animParams.talkSpeed.Key, param[0]);
        //nod
        animParams.nodSpeed = new KeyValuePair<string, float>(animParams.nodSpeed.Key, param[1]);
        animParams.nodFrequency = param[2];
        //gaze
        animParams.gazeAtSpeed = new KeyValuePair<string, float>(animParams.gazeAtSpeed.Key, param[3]);
        animParams.gazeBackSpeed = new KeyValuePair<string, float>(animParams.gazeBackSpeed.Key, param[4]);
        animParams.gazeFrequency = param[5];
    }

    public void ExpressEmotion(ExpressionState expression)
    {
        animator.SetInteger("Expression", (int)expression);
        animator.SetTrigger("Express");
    }

    public void SetMood(MoodState moodState)
    {
        switch (moodState)
        {
            case MoodState.NEUTRAL:
                setControllerParameters(new float[] {1.0f, 1.0f, 0.75f, 1.0f, 1.0f, 1.0f});
                break;
            case MoodState.HAPPY_LOW:
                setControllerParameters(new float[] { 1.5f, 1.5f, 0.75f, 1.5f, 1.2f, 1.0f });
                break;
            case MoodState.HAPPY_HIGH:
                setControllerParameters(new float[] { 2f, 2f, 0.90f, 1.5f, 1.2f, 1.0f });
                break;
            case MoodState.SAD_LOW:
                setControllerParameters(new float[] { 1.0f, 1.0f, 0.75f, 0.75f, 0.75f, 0.75f });
                break;
            case MoodState.SAD_HIGH:
                setControllerParameters(new float[] { 1.5f, 0.75f, 0.5f, 1.0f, 1.0f, 0.5f });
                break;
            default:
                break;
        }

        animator.SetInteger("Mood", (int)moodState);
    }

    public void DoNodding(NodState nodState)
    {
        animator.SetInteger("Nod Style", (int)nodState);
        animator.SetTrigger("Nod");
    }

    IEnumerator NoddingRoutine()
    {
        while (true)
        {
            if (animator.gameObject.activeSelf)
            {
                if (animator.GetBool("Listening"))
                {
                    DoNodding(NodState.NOD);
                    //TODO: USE NOD LENGTH (FROM ANIMATOR) AS THE WAIT TIME BETWEEN NODS
                    yield return new WaitForSeconds(Mathf.Abs(NODDURATION / (animParams.nodSpeed.Value == 0.0f ? 0.001f : animParams.nodSpeed.Value) ));
                }
            }
            yield return new WaitForSeconds(NODMAXINTERVAL * (1 - animParams.nodFrequency) + 0.001f);
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
        if (UnityEngine.Random.value <= animParams.gazeFrequency)
        {  
            if ((int)gazeState % 2 != 0)
                animator.SetTrigger("Gaze");
        }
    }

    public void isListening(bool state)
    {
        animator.SetBool("Listening", state);
    }

    public void setAnimationSpeed(string parameter, float value)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;

        if (culture.CompareInfo.IndexOf(parameter, "nod", CompareOptions.IgnoreCase) >= 0)
            animParams.nodSpeed = new KeyValuePair<string, float>(animParams.nodSpeed.Key, value);

        if (culture.CompareInfo.IndexOf(parameter, "talk", CompareOptions.IgnoreCase) >= 0)
            animParams.talkSpeed = new KeyValuePair<string, float>(animParams.talkSpeed.Key, value);

        string[] matchStrings = { "gaze", "at" };
        if (matchStrings.All(parameter.ToLowerInvariant().Contains))
            animParams.gazeAtSpeed = new KeyValuePair<string, float>(animParams.gazeAtSpeed.Key, value);

        matchStrings = new string[] { "gaze", "back" };
        if (matchStrings.All(parameter.ToLowerInvariant().Contains))
            animParams.gazeBackSpeed = new KeyValuePair<string, float>(animParams.gazeBackSpeed.Key, value);
    }

    public void setAnimationFrequency(string parameter, float value)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;

        if (culture.CompareInfo.IndexOf(parameter, "nod", CompareOptions.IgnoreCase) >= 0)
            animParams.nodFrequency = Mathf.Clamp(value, 0.0f, 1.0f);

        // TODO: SEE IF 0 TO 1 RANGE IS OK FOR GAZE FREQUENCY
        if (culture.CompareInfo.IndexOf(parameter, "gaze", CompareOptions.IgnoreCase) >= 0)
            animParams.gazeFrequency = Mathf.Clamp(value, 0.0f, 1.0f);
    }

    IEnumerator DEBUGRoutine()
    {
        while (true)
        {
            if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(animator.ToString(), "Maria", CompareOptions.IgnoreCase) >= 0)
            {
                Debug.Log(String.Format("animParams.talkSpeed: {0}", animParams.talkSpeed.Value));
                Debug.Log(String.Format("animParams.nodSpeed: {0}", animParams.nodSpeed.Value));
                Debug.Log(String.Format("animParams.nodFrequency: {0}", animParams.nodFrequency));
                Debug.Log(String.Format("animParams.gazeAtSpeed: {0}", animParams.gazeAtSpeed.Value));
                Debug.Log(String.Format("animParams.gazeBackSpeed: {0}", animParams.gazeBackSpeed.Value));
                Debug.Log(String.Format("animParams.gazeFrequency: {0}", animParams.gazeFrequency));
                //

                Debug.Log(String.Format("NODDURATIONF: {0}", Mathf.Abs(NODDURATION * animParams.nodSpeed.Value)));
                Debug.Log(String.Format("NODINTERVAL: {0}", NODMAXINTERVAL * (1 - animParams.nodFrequency) + 0.001f));
            }
            yield return new WaitForSeconds(5.0f);
        }
    }
}  

struct AnimatorParameters
{
    public KeyValuePair<string, float> talkSpeed;

    public KeyValuePair<string, float> nodSpeed;
    public float nodFrequency;

    public KeyValuePair<string, float> gazeAtSpeed;
    public KeyValuePair<string, float> gazeBackSpeed;
    public float gazeFrequency;
}