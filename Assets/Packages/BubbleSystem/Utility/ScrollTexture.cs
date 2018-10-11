using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {

    public float scrollSpeed = 0.5f;
    private Renderer _renderer;

    void Start () {
        _renderer = GetComponent<Renderer>();
    }
	
	
	void Update () {
        float offset = Time.time * scrollSpeed;
        for(int i = 0; i < _renderer.materials.Length; i++)
            _renderer.materials[i].SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
