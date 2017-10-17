using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsFuctions : MonoBehaviour {

    [SerializeField]
    Button option1;
    [SerializeField]
    Text text1;
    [SerializeField]
    Button option2;
    [SerializeField]
    Text text2;
    [SerializeField]
    Button option3;
    [SerializeField]
    Text text3;

    public void ShowMessage(string message, string message2, string message3)
    {
        text1.text = message;
        text2.text = message2;
        text3.text = message3;
    }

    public void ShowMessage(string message)
    {
        text1.text = message;
        option2.gameObject.SetActive(false);
        option3.gameObject.SetActive(false);
    }

    public void ShowMessage(string message, string message2)
    {
        text1.text = message;
        text2.text = message2;
        option3.gameObject.SetActive(false);
    }

    public void AddFunction()
    {
        option1.onClick.AddListener(option1Click);
        option1.onClick.AddListener(HideButton);
        option2.onClick.AddListener(option2Click);
        option2.onClick.AddListener(HideButton);
        option3.onClick.AddListener(option3Click);
        option3.onClick.AddListener(HideButton);
    }

    public void option1Click()
    {
        ChatBoxFunctions.Instance.Message("Eu: " + text1.text);
        ChatBoxFunctions.Instance.replying();
    }

    public void option2Click()
    {
        ChatBoxFunctions.Instance.Message("Eu: " + text2.text);
        ChatBoxFunctions.Instance.replying();
    }

    public void option3Click()
    {
        ChatBoxFunctions.Instance.Message("Eu: " + text3.text);
        ChatBoxFunctions.Instance.replying();
    }

    public void HideButton ()
    {
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        option3.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
