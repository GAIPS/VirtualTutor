
public enum MoodState
{
    NEUTRAL = 0,
    HAPPY_LOW = 1,
    HAPPY_HIGH = 2,
    SAD_LOW = 3,
    SAD_HIGH = 4
}

public enum ExpressionState
{
    NEUTRAL = 0,
    HAPPY_LOW = 1,
    HAPPY_HIGH = 2,
    SAD_LOW = 3,
    SAD_HIGH = 4,
    ANGER_LOW = 5,
    ANGER_HIGH = 6,
    FEAR_LOW = 7,
    FEAR_HIGH = 8,
    DISGUST_LOW = 9,
    DISGUST_HIGH = 10,
    SURPRISE_LOW = 11,
    SURPRISE_HIGH = 12,
}

public enum ActionState
{
    NEUTRAL = 0,
    EYES_CLOSE = 1,
    EYES_RIGHT = 2,
    EYES_LEFT = 3,
    EYES_DOWN = 4,
    EYES_UP = 5,
    TALK = 6,
    TALK_END = 0,
    HEAD_NOD = 7,
    GAZE_MIDDLETOLEFT = 8,
    GAZE_LEFTTOMIDDLE = -8,
    GAZE_MIDDLETORIGHT = 9,
    GAZE_RIGHTTOMIDDLE = -9
}