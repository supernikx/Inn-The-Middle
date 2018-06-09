using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    BoardManager bm;
    int DraftTextIndex = 0;

    [Header("Draft")]
    public GameObject MagicDraft;
    public GameObject ScienceDraft;
    public TextMeshProUGUI MagicDraftText;
    public TextMeshProUGUI ScienceDraftText;
    public GameObject MagicDraftAButtonImage;
    public GameObject ScienceDraftAButtonImage;
    public List<string> DraftText = new List<string>();

    public bool _TutorialActive;
    public bool TutorialActive
    {
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
                    BoardManager.Instance.uiManager.tutorialtogglemenu.ChangeImage();
                    break;
                default:
                    Debug.Log("Dato salvato non valido");
                    break;
            }
        }

        bm = BoardManager.Instance;
        MagicDraft.SetActive(false);
        ScienceDraft.SetActive(false);
        MagicDraftAButtonImage.SetActive(false);
        ScienceDraftAButtonImage.SetActive(false);
    }

    public void ActiveDeactiveTutorial()
    {
        TutorialActive = !TutorialActive;
    }

    #region Draft Tutorial
    public void StartDraftTutorial()
    {
        if (TutorialActive)
        {
            switch (bm.p1Faction)
            {
                case Factions.Magic:
                    MagicDraft.SetActive(true);
                    MagicDraftText.text = DraftText[DraftTextIndex];
                    break;
                case Factions.Science:
                    ScienceDraft.SetActive(true);
                    ScienceDraftText.text = DraftText[DraftTextIndex];
                    break;
            }
            DraftTextIndex++;
        }
    }

    public void DraftTutorial()
    {
        if (TutorialActive)
        {
            MagicDraft.SetActive(false);
            ScienceDraft.SetActive(false);
            if (bm.draftManager.hasDrafted)
            {
                bm.TutorialInProgress = true;
                switch (bm.turnManager.CurrentPlayerTurn)
                {
                    case Factions.Magic:
                        MagicDraft.SetActive(true);
                        MagicDraftText.text = DraftText[DraftTextIndex];
                        if (DraftTextIndex < DraftText.Count - 1)
                        {
                            MagicDraftAButtonImage.SetActive(true);
                        }
                        else
                        {
                            bm.TutorialInProgress = false;
                        }
                        break;
                    case Factions.Science:
                        ScienceDraft.SetActive(true);
                        ScienceDraftText.text = DraftText[DraftTextIndex];
                        if (DraftTextIndex < DraftText.Count - 1)
                        {
                            ScienceDraftAButtonImage.SetActive(true);
                        }
                        else
                        {
                            bm.TutorialInProgress = false;
                        }
                        break;
                }
                DraftTextIndex++;
            }
        }
    }

    public void DraftDisableAButton()
    {
        if (TutorialActive)
        {
            if (MagicDraftAButtonImage.activeSelf)
                MagicDraftAButtonImage.SetActive(false);
            if (ScienceDraftAButtonImage.activeSelf)
                ScienceDraftAButtonImage.SetActive(false);
        }
    }

    #endregion
}
