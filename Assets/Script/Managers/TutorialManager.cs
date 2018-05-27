using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public bool _TutorialActive;
    public bool TutorialActive {
        get
        {
            return _TutorialActive;
        }
        set
        {
            _TutorialActive = value;
            if (_TutorialActive)
            {
                PlayerPrefs.SetInt("Tutorial", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Tutorial", 0);
            }
        }
    }

    private void Start()
    {
        int tutorialindex = PlayerPrefs.GetInt("Tutorial", -1);
        if (tutorialindex == -1)
        {
            PlayerPrefs.SetInt("Tutorial", 0);
            TutorialActive = false;
        }
        else
        {
            switch (tutorialindex)
            {
                case 0:
                    TutorialActive = false;                    
                    break;
                case 1:
                    TutorialActive = true;
                    BoardManager.Instance.uiManager.tutorialtoggle.ChangeImage();
                    break;
                default:
                    Debug.Log("Dato salvato non valido");
                    break;
            }
        }
    }

    public void ActiveDeactiveTutorial()
    {
        TutorialActive = !TutorialActive;
    }
}
