using System;
using UnityEngine;
using System.Collections;

public class TTSSpeakerUnityOnline : MonoBehaviour, ITTSSpeaker {

    public AudioSource audioS;
    public AudioClip lastLoadedClip;
    public string key = "8af02eddedb2fe025d0356d1c49ccf1b";

    public void Play(string message) {
        Play(message, Sex.Male, Language.EN_UK);
    }

    public void Play(string message, Sex sex) {
        Play(message, sex, Language.EN_UK);
    }

    public void Play(string message, Language language) {
        Play(message, Sex.Male, language);
    }

    public void Play(string message, Sex sex, Language language) {
        string endPoint = @"http://tts.readspeaker.com/a/speak";
        var languageString = "en_uk"; // Online mode only supports en_uk
        string voice;
        switch (sex) {
            case Sex.Female:
                voice = "amy";
                break;
            case Sex.Male:
            default:
                voice = "Male02";
                break;
        }

        var client = new HttpUtils.RestClient(endPoint);
        var parameters = string.Format("?key={0}&lang={1}&voice={2}&audioformat=ogg&text={3}",
                                       key,
                                       languageString,
                                       voice,
                                       System.Uri.EscapeUriString(message));
        var uri = client.MakeRequestToURI(parameters);
//        var uri = new System.Uri("http://ivotts.readspeaker.com/cgi-bin/nph-ivona/c72dec7a62c19ed42abd457c98301397.ogg");
        StartCoroutine(LoadAudioFile(uri.AbsoluteUri));
    }

    IEnumerator LoadAudioFile(string path) {
        var www = new WWW(path);
        yield return www;
        AudioClip clip = www.GetAudioClip(false);
        while (clip.loadState == AudioDataLoadState.Loading) {
            yield return 0;
        }
        if (clip.loadState == AudioDataLoadState.Loaded) {
            lastLoadedClip = clip;
            if (audioS) {
                audioS.clip = clip;
                audioS.Play();
            }
        } else {
            Debug.LogWarning("Could not load clip. Clip load state is: " + clip.loadState);
        }
    }
}
