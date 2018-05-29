using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonImage : MonoBehaviour {

    public Sprite defaultimage;
    public Sprite defaultHighlitedImage;
    public Sprite pressedHighlitedImage;
    public Sprite pressedimage;

    Image comp;
    Button button;
    bool pressed;
    SpriteState state = new SpriteState();

    private void Awake()
    {
        comp = GetComponent<Image>();
        button = GetComponent<Button>();
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
            if (pressedHighlitedImage != null)
            {
                state = button.spriteState;
                state.highlightedSprite = pressedHighlitedImage;
            }          
        }
        else
        {
            comp.sprite = defaultimage;
            if (defaultHighlitedImage != null)
            {
                state = button.spriteState;
                state.highlightedSprite = defaultHighlitedImage;
            }            
        }
        button.spriteState = state;
    }

    public void SetPressedImage()
    {
        comp.sprite = pressedimage;
        if (pressedHighlitedImage != null)
        {
            state = button.spriteState;
            state.highlightedSprite = pressedHighlitedImage;
        }
        button.spriteState = state;
        pressed = true;
    }

    public void SetDefaultImage()
    {
        comp.sprite = defaultimage;
        if (defaultHighlitedImage != null)
        {
            state = button.spriteState;
            state.highlightedSprite = defaultHighlitedImage;
        }
        button.spriteState = state;
        pressed = false;
    }

}
