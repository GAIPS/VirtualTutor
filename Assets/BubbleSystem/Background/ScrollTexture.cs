﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {

    public float scrollSpeed = 0.5f;
    private new Renderer renderer;

    void Start () {
        renderer = GetComponent<Renderer>();
    }
	
	
	void Update () {
        float offset = Time.time * scrollSpeed;
        for(int i = 0; i < renderer.materials.Length; i++)
            renderer.materials[i].SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
