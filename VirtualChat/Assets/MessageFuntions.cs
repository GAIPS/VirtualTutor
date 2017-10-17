using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MessageFuntions : MonoBehaviour {

    [SerializeField]
    Text text;
    [SerializeField]
    Button b;
    
    public void ShowMessage (string message)
    {
        text.text = message;
        b.gameObject.SetActive(false);
    }

    public void ShowMessageResponse (string message)
    {
        text.text = message;
    }

    public void HideMessage()
    {
        Destroy(gameObject);
    }
}
