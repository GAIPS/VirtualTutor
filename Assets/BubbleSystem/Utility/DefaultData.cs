using BubbleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultData : Singleton<DefaultData> {

    [HideInInspector]
    public TextData defaultTextData;
    [HideInInspector]
    public DefaultBalloonAnimationData defaultBalloonAnimationData;
    [HideInInspector]
    public SpriteData defaultBalloonData;
    [HideInInspector]
    public BackgroundAnimationData defaultBackgroundAnimationData;
    [HideInInspector]
    public Dictionary<Reason, TextureData> defaultBackgroundDataDictionary = new Dictionary<Reason, TextureData>();
    [HideInInspector]
    public RuntimeAnimatorController defaultAnimatorController;

    private DefaultData() { }

    private void Awake()
    {
        defaultTextData.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        defaultTextData.colorData.color = Color.black;
        defaultTextData.size = 12.0f;

        defaultBalloonAnimationData.animator = (RuntimeAnimatorController)Resources.Load("Balloons/Animators/BallonPopup");
        defaultBalloonAnimationData.duration = 5;

        var tex = (Texture2D)Resources.Load("Balloons/Images/SpeechBubbles/Default/balloon");
        defaultBalloonData.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        defaultBalloonData.colorData.color = Color.white;

        defaultBackgroundAnimationData.colorTransitionData.duration = 5;
        defaultBackgroundAnimationData.colorTransitionData.smoothness = 0.02f;
        defaultBackgroundAnimationData.imageFadePercentage = 5;

        TextureData defaultBackgroundData;
        defaultBackgroundData.colorData.color = Color.white;
        defaultBackgroundData.texture = (Texture2D)Resources.Load("Backgrounds/Images/joaoBackground");
        defaultBackgroundDataDictionary.Add(Reason.None, defaultBackgroundData);

        defaultBackgroundData.texture = (Texture2D)Resources.Load("Backgrounds/Images/graph");
        defaultBackgroundDataDictionary.Add(Reason.Grades, defaultBackgroundData);
        
        
    }
}
