using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    BoardManager bm;
    int DraftTextIndex = 0;
    int ChoosingTextIndex = 0;
    int PlacingTextIndex = 0;
    int GameP1TextIndex = 0;
    int GameP2TextIndex = 0;

    [Header("Draft")]
    public GameObject MagicDraft;
    public GameObject ScienceDraft;
    public TextMeshProUGUI MagicDraftText;
    public TextMeshProUGUI ScienceDraftText;
    public GameObject MagicADraftButton;
    public GameObject ScienceADraftButton;
    public List<string> DraftText = new List<string>();

    [Header("Choosing")]
    public GameObject MagicChoosing;
    public GameObject ScienceChoosing;
    public TextMeshProUGUI MagicChoosingText;
    public TextMeshProUGUI ScienceChoosingText;
    public List<string> ChoosingText = new List<string>();
    public bool ChoosingTutorialDone;

    [Header("Placing")]
    public GameObject MagicPlacing;
    public GameObject SciencePlacing;
    public TextMeshProUGUI MagicPlacingText;
    public TextMeshProUGUI SciencePlacingText;
    public List<string> PlacingText = new List<string>();

    [Header("Game")]
    public GameObject MagicGame;
    public GameObject ScienceGame;
    public TextMeshProUGUI MagicGameText;
    public TextMeshProUGUI ScienceGameText;
    public List<string> GameP1Text = new List<string>();
    public List<string> GameP2Text = new List<string>();
    public bool GameTutorialDone;

    public string SuperAttackText;
    public bool SuperAttackTutorialDone;

    [Header("General")]
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

        MagicChoosing.SetActive(false);
        ScienceChoosing.SetActive(false);
        ChoosingTutorialDone = false;

        MagicPlacing.SetActive(false);
        SciencePlacing.SetActive(false);

        MagicGame.SetActive(false);
        ScienceGame.SetActive(false);
        GameTutorialDone = false;
        SuperAttackTutorialDone = false;
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

    #region Choosing Tutorial

    public void ChoosingTutorial()
    {
        if (TutorialActive)
        {
            bm.TutorialInProgress = true;
            switch (bm.turnManager.CurrentPlayerTurn)
            {
                case Factions.Magic:
                    MagicChoosing.SetActive(true);
                    MagicChoosingText.text = ChoosingText[ChoosingTextIndex];
                    break;
                case Factions.Science:
                    ScienceChoosing.SetActive(true);
                    ScienceChoosingText.text = ChoosingText[ChoosingTextIndex];
                    break;
            }
            ChoosingTextIndex++;
        }
        else
        {
            ChoosingTutorialDone = true;
        }
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

    #region Game Tutorial

    public void GameTutorial()
    {
        if (TutorialActive)
        {
            bm.TutorialInProgress = true;
            if (bm.turnManager.CurrentPlayerTurn == bm.p1Faction)
            {
                switch (bm.p1Faction)
                {
                    case Factions.Magic:
                        MagicGame.SetActive(true);
                        MagicGameText.text = GameP1Text[GameP1TextIndex];
                        break;
                    case Factions.Science:
                        ScienceGame.SetActive(true);
                        ScienceGameText.text = GameP1Text[GameP1TextIndex];
                        break;
                }
                GameP1TextIndex++;
            }
            else if (bm.turnManager.CurrentPlayerTurn == bm.p2Faction)
            {
                switch (bm.p2Faction)
                {
                    case Factions.Magic:
                        MagicGame.SetActive(true);
                        MagicGameText.text = GameP2Text[GameP2TextIndex];
                        break;
                    case Factions.Science:
                        ScienceGame.SetActive(true);
                        ScienceGameText.text = GameP2Text[GameP2TextIndex];
                        break;
                }
                GameP2TextIndex++;
            }
        }
        else
        {
            GameTutorialDone = true;
        }
    }

    public void SuperAttackTutorial()
    {
        if (TutorialActive)
        {
            bm.TutorialInProgress = true;
            switch (bm.turnManager.CurrentPlayerTurn)
            {
                case Factions.Magic:
                    MagicGame.SetActive(true);
                    MagicGameText.text = SuperAttackText;
                    break;
                case Factions.Science:
                    ScienceGame.SetActive(true);
                    ScienceGameText.text = SuperAttackText;
                    break;
            }
        }
        else
        {
            SuperAttackTutorialDone = true;
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
                bm.TutorialInProgress = false;
                break;
            case TurnManager.MacroPhase.placing:
                if (bm.turnManager.CurrentTurnState == TurnManager.PlayTurnState.choosing)
                {
                    switch (bm.turnManager.CurrentPlayerTurn)
                    {
                        case Factions.Magic:
                            MagicChoosing.SetActive(false);
                            break;
                        case Factions.Science:
                            ScienceChoosing.SetActive(false);
                            break;
                    }
                    if (ChoosingTextIndex < ChoosingText.Count)
                    {
                        bm.turnManager.ChangeTurn();
                    }
                    else if (ChoosingTextIndex == ChoosingText.Count)
                    {
                        ChoosingTutorialDone = true;
                        bm.TutorialInProgress = false;
                        bm.turnManager.ChangeTurn();
                    }
                }
                else if (bm.turnManager.CurrentTurnState == TurnManager.PlayTurnState.placing)
                {
                    switch (bm.turnManager.CurrentPlayerTurn)
                    {
                        case Factions.Magic:
                            MagicPlacing.SetActive(false);
                            break;
                        case Factions.Science:
                            SciencePlacing.SetActive(false);
                            break;
                    }
                    bm.TutorialInProgress = false;
                }
                break;
            case TurnManager.MacroPhase.game:
                if (!GameTutorialDone) {
                    if (bm.turnManager.CurrentPlayerTurn == bm.p1Faction)
                    {
                        if (GameP1Text.Count > GameP1TextIndex)
                            GameTutorial();
                        else
                        {
                            switch (bm.p1Faction)
                            {
                                case Factions.Magic:
                                    MagicGame.SetActive(false);
                                    break;
                                case Factions.Science:
                                    ScienceGame.SetActive(false);
                                    break;
                            }
                            bm.TutorialInProgress = false;
                        }
                    }
                    else if (bm.turnManager.CurrentPlayerTurn == bm.p2Faction)
                    {
                        if (GameP2Text.Count > GameP2TextIndex)
                            GameTutorial();
                        else
                        {
                            switch (bm.p2Faction)
                            {
                                case Factions.Magic:
                                    MagicGame.SetActive(false);
                                    break;
                                case Factions.Science:
                                    ScienceGame.SetActive(false);
                                    break;
                            }
                            bm.TutorialInProgress = false;
                            GameTutorialDone = true;
                        }
                    }
                }
                else if (!SuperAttackTutorialDone)
                {
                    switch (bm.turnManager.CurrentPlayerTurn)
                    {
                        case Factions.Magic:
                            MagicGame.SetActive(false);
                            break;
                        case Factions.Science:
                            ScienceGame.SetActive(false);
                            break;
                    }
                    SuperAttackTutorialDone = true;
                    bm.TutorialInProgress = false;
                }
                break;
            case TurnManager.MacroPhase.end:
                break;
        }
    }
}
