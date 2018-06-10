using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    BoardManager bm;
    int DraftTextIndex = 0;
    int PlacingTextIndex = 0;

    [Header("Draft")]
    public GameObject MagicDraft;
    public GameObject ScienceDraft;
    public TextMeshProUGUI MagicDraftText;
    public TextMeshProUGUI ScienceDraftText;
    public GameObject MagicADraftButton;
    public GameObject ScienceADraftButton;
    public List<string> DraftText = new List<string>();

    [Header("Placing")]
    public GameObject MagicPlacing;
    public GameObject SciencePlacing;
    public TextMeshProUGUI MagicPlacingText;
    public TextMeshProUGUI SciencePlacingText;
    public List<string> PlacingText = new List<string>();

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

        MagicPlacing.SetActive(false);
        SciencePlacing.SetActive(false);
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

    #endregion

    #region Placing Tutorial

    public void PlacingTutorial()
    {
        if (TutorialActive)
        {
            bm.TutorialInProgress = true;
            switch (bm.turnManager.CurrentPlayerTurn)
            {
                case Factions.Magic:
                    MagicPlacing.SetActive(true);
                    MagicPlacingText.text = PlacingText[PlacingTextIndex];
                    break;
                case Factions.Science:
                    SciencePlacing.SetActive(true);
                    SciencePlacingText.text = PlacingText[PlacingTextIndex];
                    break;
            }
            PlacingTextIndex++;
        }
    }

    #endregion

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
                switch (bm.turnManager.CurrentPlayerTurn)
                {
                    case Factions.Magic:
                        MagicPlacing.SetActive(false);
                        break;
                    case Factions.Science:
                        SciencePlacing.SetActive(false);
                        break;
                }
                break;
            case TurnManager.MacroPhase.game:
                break;
            case TurnManager.MacroPhase.end:
                break;
        }
        bm.TutorialInProgress = false;
    }
}
