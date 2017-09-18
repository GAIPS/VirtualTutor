using System.Collections.Generic;
using UnityEngine;

public class TTSSpeakerOfflineScripted : MonoBehaviour, ITTSSpeaker {

    public AudioSource audioSFemale = null;
    public AudioSource audioSMale = null;
    private Dictionary<int, AudioClip> loadedAudioMale1;
    private Dictionary<int, AudioClip> loadedAudioMale2;
    private Dictionary<int, AudioClip> loadedAudioFemale1;
    private Dictionary<int, AudioClip> loadedAudioFemale2;

    // Use this for initialization
    void Start() {
        loadedAudioMale1 = new Dictionary<int, AudioClip>();
        loadedAudioMale2 = new Dictionary<int, AudioClip>();
        loadedAudioFemale1 = new Dictionary<int, AudioClip>();
        loadedAudioFemale2 = new Dictionary<int, AudioClip>();
    }
    
    // Update is called once per frame
    void Update() {
        
    }

    public void Play(string message) {
        Play(message, Sex.Male, Language.PT_PT);
    }

    public void Play(string message, Sex sex) {
        Play(message, sex, Language.PT_PT);
    }

    public void Play(string message, Language language) {
        Play(message, Sex.Male, language);
    }

    public void Play(string message, Sex sex, Language language) {
        // Only Portuguese language is available.
        int indexOfAudio = -1;
        string audioFilePath = "TTS-sounds/";
        Dictionary<int, AudioClip> loadedAudio = null;
        if (sex == Sex.Male) {
            if (VT.MainScript.dialogIndex == 0) {
                indexOfAudio = DialogScripts.GetIndex(DialogScripts.Male1Script,
                                                      message);
                audioFilePath += "Male1/";
                loadedAudio = loadedAudioMale1;
            } else {
                indexOfAudio = DialogScripts.GetIndex(DialogScripts.Male2Script,
                                                      message);
                audioFilePath += "Male2/";
                loadedAudio = loadedAudioMale2;
            }
        } else {
            if (VT.MainScript.dialogIndex == 0) {
                indexOfAudio = DialogScripts.GetIndex(DialogScripts.Female1Script,
                                                      message);
                audioFilePath += "Female1/";
                loadedAudio = loadedAudioFemale1;
            } else {
                indexOfAudio = DialogScripts.GetIndex(DialogScripts.Female2Script,
                                                      message);
                audioFilePath += "Female2/";
                loadedAudio = loadedAudioFemale2;
            }
        }
        if (indexOfAudio < 0) {
            Debug.LogWarning("Failed to find Index for: " + message);
            return;
        }

        AudioClip audioFile = null;
        if (loadedAudio.ContainsKey(indexOfAudio)) {
            audioFile = loadedAudio[indexOfAudio];
        } else {
            audioFilePath += "dialog-" + indexOfAudio;
            audioFile = Resources.Load(audioFilePath) as AudioClip;
            if (audioFile) {
                loadedAudio.Add(indexOfAudio, audioFile);
            }
        }

        if (audioFile) {
            if (sex == Sex.Male && audioSMale) {
                audioSMale.clip = audioFile;
                audioSMale.Play();
            } else if (sex == Sex.Female && audioSFemale) {
                audioSFemale.clip = audioFile;
                audioSFemale.Play();
            } else {
                Debug.LogWarning("Audio Source not set");
            }
        } else {
            Debug.Log("Unable to Load audio file: " + audioFilePath);
        }
    }
}
