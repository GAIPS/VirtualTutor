using System;

[Serializable]
public class MoodVariables
{
    public string neutral = "q";
    public string happy = "w";
    public string veryHappy = "e";
    public string sad = "r";
    public string verySad = "t";
    public string afraid = "1";
    public string surprised = "2";
    public string angered = "3";
    public string disgusted = "4";
}

[Serializable]
public class ExpressionVariables
{
    public string neutral = "a";
    public string happinessLow = "s";
    public string happinessHigh = "d";
    public string sadnessLow = "f";
    public string sadnessHigh = "g";
    public string fearLow = "h";
    public string fearHigh = "j";
    public string surpriseLow = "k";
    public string surpriseHigh = "l";
    public string angerLow = "z";
    public string angerHigh = "x";
    public string disgustLow = "c";
    public string disgustHigh = "v";
}

[Serializable]
public class MovementVariables
{
    public string nodStart = "n";
    public string nodStop = "m";
    public string talkStart = "u";
    public string talkStop = "i";
    public string gazeAtPartner = "o";
    public string gazeBackFromPartner = "p";
    public string gazeAtUser = "9";
    public string gazeBackFromUser = "0";
}

[Serializable]
public class ParameterVariables
{
    public string nodFrequency = "[";
    public string nodSpeed = "]";
    public string gazeFrequency = ",";
    public string gazeSpeed = ".";
}