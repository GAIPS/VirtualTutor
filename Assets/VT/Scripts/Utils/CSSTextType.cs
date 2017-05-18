using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSSTextType : MonoBehaviour {

    public Text text {
        get {
            return GetComponent<Text>();
        }
    }

    public string type = "p";

    // Use this for initialization
    void Start() {
        StartCoroutine(register());
    }

    private IEnumerator register() {
        while (!CSSStyle.Instance) {
            yield return 0;
        }
        CSSStyle.Instance.Register(this);
    }
}
