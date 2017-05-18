using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class CSSStyle : Singleton<CSSStyle> {

    [Serializable]
    public struct TextSize {
        public string type;
        public int size;
    }

    [Serializable]
    public struct MediaQuery {
        public float width;
        public TextSize[] typeSizes;
    }

    public MediaQuery[] mediaQueries;

    private List<CSSTextType> cssTexts = new List<CSSTextType>();

    protected CSSStyle() {
    }

    // Use this for initialization
    void Start() {
        SortQueries();
    }
	
    // Update is called once per frame
    void Update() {
        float screenWidth = Screen.width;
        if (Application.isPlaying) {
            ChangeSize(cssTexts.ToArray(), screenWidth);
        } else {
//            Debug.Log("Screen Width: " + screenWidth);
            SortQueries();
            ChangeSize(FindObjectsOfType<CSSTextType>(), screenWidth);
        }
    }

    protected void ChangeSize(CSSTextType[] texts, float screenWidth) {
        foreach (MediaQuery query in mediaQueries) {
            if (query.width <= screenWidth) {
                // set sizes
                foreach (CSSTextType text in texts) {   
                    foreach (TextSize tSize in query.typeSizes) {
                        if (tSize.type.Equals(text.type)) {
                            if (text && text.text) {
                                text.text.resizeTextMaxSize = tSize.size;
                            }
                        }
                    }
                }
                break;
            }
        }
    }

    public void SortQueries() {
        Array.Sort(mediaQueries, (MediaQuery a, MediaQuery b) => {
                return a.width.CompareTo(b.width);
            });
        Array.Reverse(mediaQueries);
    }

    public void Register(CSSTextType text) {
        if (text) {
            cssTexts.Add(text);
        }
    }
}
