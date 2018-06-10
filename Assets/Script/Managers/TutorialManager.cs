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
    public GameObject MagicADraftButton;
    public GameObject ScienceADraftButton;
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
        MagicADraftButton.SetActive(false);
        ScienceADraftButton.SetActive(false);
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
                        MagicADraftButton.SetActive(true);
                        MagicDraftText.text = DraftText[DraftTextIndex];
                        if (DraftTextIndex >= DraftText.Count - 1)
                        {
                            bm.TutorialInProgress = false;
                            MagicADraftButton.SetActive(false);
                            StartCoroutine(LastDraftText(MagicDraft));
                        }
                        break;
                    case Factions.Science:
                        ScienceDraft.SetActive(true);
                        ScienceADraftButton.SetActive(true);
                        ScienceDraftText.text = DraftText[DraftTextIndex];
                        if (DraftTextIndex >= DraftText.Count - 1)
                        {
                            bm.TutorialInProgress = false;
                            ScienceADraftButton.SetActive(false);
                            StartCoroutine(LastDraftText(ScienceDraft));
                        }
                        break;
                }
                DraftTextIndex++;
            }
        }
    }

    IEnumerator LastDraftText(GameObject button)
    {
        yield return new WaitForSeconds(2f);
        button.SetActive(false);
    }

    public void AButtonPressed()
    {
        switch (bm.turnManager.CurrentMacroPhase)
        {
            case TurnManager.MacroPhase.draft:
                switch (bm.turnManager.CurrentPlayerTurn)
                {
                    case Factions.Magic:
                        MagicDraft.SetActive(false);
                        break;
                    case Factions.Science:
                        ScienceDraft.SetActive(false);
                        break;
                }
                break;
            case TurnManager.MacroPhase.placing:
                break;
            case TurnManager.MacroPhase.game:
                break;
            case TurnManager.MacroPhase.end:
                break;
        }
        bm.TutorialInProgress = false;
    }

    #endregion
}
