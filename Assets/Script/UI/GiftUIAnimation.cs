using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftUIAnimation : MonoBehaviour {

    public Sprite[] sprites;
    public float animationspeed;
    public bool animate;
    float index;
    Image img;

	// Use this for initialization
	void Start () {
        animate = true;
        img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (animate)
        {
            index = Time.time * animationspeed;
            index = index % sprites.Length;
            img.sprite = sprites[(int)index];
        }
	}
}
