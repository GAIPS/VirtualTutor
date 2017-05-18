using System;

public enum Sex {
    Male,
    Female
    // Apache // they told me I should remove this... Apacheofobics
}

public enum Language {
    EN_UK,
    PT_PT
}

public interface ITTSSpeaker {
    void Play(string message);
    void Play(string message, Sex sex);
    void Play(string message, Language language);
    void Play(string message, Sex sex, Language language);
}


