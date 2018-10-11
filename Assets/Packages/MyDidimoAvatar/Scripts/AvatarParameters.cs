using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnimatorParams
{
    MOOD_INTENSITY = 1,
    EXPRESSION_INTENSITY,
    TALK_SPEED,
    NOD_SPEED,
    GAZEAT_SPEED,
    GAZEBACK_SPEED
}

public enum ControllerParams
{
    GAZEAT_FREQUENCY = 1,
    GAZEBACK_FREQUENCY,
    NOD_FREQUENCY,
}

internal struct Parameter
{
    public string NAME { get; set; }
    public float VALUE { get; set; }

    public Parameter(string id, float value) : this()
    {
        this.NAME = id;
        this.VALUE = value;
    }
}

[RequireComponent(typeof(Animator))]
public class AvatarParameters : MonoBehaviour
{
    [Header("Parameter Adjustments")]
    [SerializeField]
    [Tooltip("Adjust this value if your mood animations are too strong")]
    [Range(0.1f, 1.0f)]
    private float moodDampener = 1.0f;

    //[Header("Default Values")]
    [HideInInspector]
    public float nodInterval = 2.083f;

    [HideInInspector]
    public float nodLength;

    private Dictionary<AnimatorParams, Parameter> animParams;
    private Dictionary<ControllerParams, Parameter> contParams;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animParams = new Dictionary<AnimatorParams, Parameter>();
        contParams = new Dictionary<ControllerParams, Parameter>();

        //Animator Parameters
        animParams.Add(AnimatorParams.TALK_SPEED, new Parameter("Talk Speed", 0.0f));
        animParams.Add(AnimatorParams.NOD_SPEED, new Parameter("Nod Speed", 0.0f));
        animParams.Add(AnimatorParams.GAZEAT_SPEED, new Parameter("Gaze At Speed", 0.0f));
        animParams.Add(AnimatorParams.GAZEBACK_SPEED, new Parameter("Gaze Back Speed", 0.0f));
        animParams.Add(AnimatorParams.MOOD_INTENSITY, new Parameter("Mood Intensity", 0.0f));
        animParams.Add(AnimatorParams.EXPRESSION_INTENSITY, new Parameter("Expression Intensity", 0.0f));

        //Controller Parameters
        //NOTE: currently GAZEAT_FREQUENCY shares the same parameter as GAZEBACK_FREQUENCY
        contParams.Add(ControllerParams.GAZEAT_FREQUENCY, new Parameter("Gaze At Frequency", 0.0f));
        contParams.Add(ControllerParams.GAZEBACK_FREQUENCY, new Parameter("Gaze Back Frequency", 0.0f));
        contParams.Add(ControllerParams.NOD_FREQUENCY, new Parameter("Nod Frequency", 0.0f));
    }

    private void Start()
    {
        AnimationClip ac = getClipByName("Head_Nod");
        nodLength = (ac != null) ? Math.Abs(ac.length) : 2.083f; // Default Length, just in case
    }

    public void setParameter<TEnum>(TEnum paramaterKey, float value)
    {
        float cleanValue = Math.Abs(value);

        if (!typeof(TEnum).IsEnum)
            throw new ArgumentException("T must be an enumerated type");

        object key;
        if (EnumUtils.TryParse(typeof(AnimatorParams), paramaterKey.ToString(), out key))
        {
            switch ((AnimatorParams)key)
            {
                case AnimatorParams.EXPRESSION_INTENSITY:
                case AnimatorParams.MOOD_INTENSITY:
                    cleanValue = Mathf.Clamp(cleanValue, 0.0f, 1.0f); //clamping Intensity [0, 1]
                    break;

                default:
                    cleanValue = Mathf.Clamp(cleanValue, 0.01f, 5.0f); //clamping Speed [0.01, 5]
                    break;
            }
            Parameter auxParam = new Parameter(animParams[(AnimatorParams)key].NAME, cleanValue);
            animParams[(AnimatorParams)key] = auxParam;
            animator.SetFloat(auxParam.NAME, auxParam.VALUE);
        }
        else if (EnumUtils.TryParse(typeof(ControllerParams), paramaterKey.ToString(), out key))
            contParams[(ControllerParams)key] =
                new Parameter(contParams[(ControllerParams)key].NAME, Mathf.Clamp(cleanValue, 0.0f, 1.0f)); //clamping Frequency [0,1]
        else
            throw new ArgumentException("Could not parse the parameterKey");
    }

    public float getParameter<TEnum>(TEnum paramaterKey)
    {
        if (!typeof(TEnum).IsEnum)
            throw new ArgumentException("T must be an enumerated type");

        object key;
        if (EnumUtils.TryParse(typeof(AnimatorParams), paramaterKey.ToString(), out key))
            return animParams[(AnimatorParams)key].VALUE;
        else if (EnumUtils.TryParse(typeof(ControllerParams), paramaterKey.ToString(), out key))
            return contParams[(ControllerParams)key].VALUE;
        else
            throw new ArgumentException("Could not parse the parameterKey");
    }

    public float getMoodDampenerValue()
    {
        return moodDampener;
    }

    public void setAllParameters<TEnum>(float[] paramValue) where TEnum : struct, IConvertible
    {
        int paramSize = paramValue.Length, i = 0;

        if (!typeof(TEnum).IsEnum)
            throw new ArgumentException("T must be an enumerated type");

        Array enumArray = EnumUtils.AsArray<TEnum>();

        if (enumArray.Length > paramSize)
            throw new ArgumentException("Not enough values to fill all the parameter values");

        foreach (TEnum paramaterKey in enumArray)
        {
            object key;
            if (EnumUtils.TryParse(typeof(AnimatorParams), paramaterKey.ToString(), out key))
            {
                if (((AnimatorParams)key).Equals(AnimatorParams.EXPRESSION_INTENSITY) ||
                    ((AnimatorParams)key).Equals(AnimatorParams.MOOD_INTENSITY))
                {
                    i++; continue;
                }
            }
            setParameter(paramaterKey, paramValue[i++]);
        }
    }

    public AnimationClip getClipByName(string name)
    {
        if (animator != null)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            foreach (var clip in ac.animationClips)
                if (clip.name == name)
                    return clip;
        }
        return null;
    }

    public static class Presets
    {
        //  element order:
        //
        //  animator parameters:            controller parameters:
        //      (x) mood_intensity              gazeat_frequency
        //      (x) expression_intensity        gazeback_frequency
        //      talk_speed                      nod_frequency
        //      nod_speed
        //      gazeat_speed
        //      gazeback_speed

        public static float[][] Neutral()
        {
            return new float[][]
            {
                new float[] {-1.00f, -1.00f,  1.00f,  1.00f,  1.00f,  1.00f},
                new float[] { 0.75f, 0.75f, 0.35f }
            };
        }

        public static float[][] HappinessLow()
        {
            return new float[][]
            {
                new float[] {-1.00f, -1.00f,  1.50f,  1.50f,  1.20f,  1.50f},
                new float[] { 0.75f, 0.75f, 0.45f }
            };
        }

        public static float[][] HappinessHigh()
        {
            return new float[][]
            {
                new float[] {-1.00f, -1.00f,  2.00f,  2.00f,  1.50f,  1.20f},
                new float[] { 0.9f, 0.9f, 0.60f }
            };
        }

        public static float[][] SadnessLow()
        {
            return new float[][]
            {
                new float[] {-1.00f, -1.00f,  1.00f,  1.00f,  0.75f,  0.75f},
                new float[] { 0.50f, 0.50f, 0.25f }
            };
        }

        public static float[][] SadnessHigh()
        {
            return new float[][]
            {
                new float[] {-1.00f, -1.00f,  1.00f,  1.00f,  1.00f,  1.00f},
                new float[] { 0.85f, 0.85f, 0.10f }
            };
        }

        public static float[][] Fear()
        {
            return new float[][]
            {
                new float[] {-1.00f, -1.00f,  1.50f,  1.35f,  1.35f,  1.35f},
                new float[] { 0.65f,  0.65f,  0.80f }
            };
        }

        //TODO: ADD DEFAULT VALUES
        public static float[][] Surprise()
        {
            return new float[][]
            {
                new float[] {-1.00f, -1.00f,  1.25f,  1.25f,  1.00f,  1.00f},
                new float[] { 0.75f,  0.75f,  0.60f }
            };
        }
    }

    /// <summary>
    /// CURRENTY UNUSED CODE.
    /// PLANNED AS A MEANS TO HAVE THE PARAMETERS AS ATTRIBUTES FOR QUICK ACCESS
    /// AND EDIT IN THE UNITY EDITOR.
    /// </summary>
    [Serializable]
    public class ParameterSet
    {
        [Header("Animator Parameters")]
        public float moodIntensity;

        public float moodIntensity2;
        public float moodIntensity3;
        public float moodIntensity4;

        [Space(2.0f)]
        [Header("X Parameters")]
        public float SXV;

        public float WDAWFWA;
    }
}