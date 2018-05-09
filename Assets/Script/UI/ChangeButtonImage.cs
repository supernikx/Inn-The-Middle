using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonImage : MonoBehaviour {

    public Sprite defaultimage;
    public Sprite pressedimage;
    Image comp;
    bool pressed;

    private void OnDisable()
    {
        defaultimage = comp.sprite;
        pressed = false;
    }

    private void Start()
    {
        comp = GetComponent<Image>();
        if (defaultimage != null)
        {
            comp.sprite = defaultimage;
        }
        else
        {
            defaultimage = comp.sprite;
        }
        pressed = false;
    }

    public void ChangeImage()
    {
        pressed = !pressed;
        if (pressed)
        {
            comp.sprite = pressedimage;
        }
        else
        {
            comp.sprite = defaultimage;
        }
    }

}
