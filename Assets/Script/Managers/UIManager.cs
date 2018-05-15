using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

    TurnManager tm;
    BoardManager bm;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    bool isPaused;

    /// <summary> Testo per indicare di chi è il turno </summary>
    [Header("Turn Text")]
    public TextMeshProUGUI gameTurnText;
    public GameObject superattackText;
    public GameObject p1PickingText;
    public GameObject p2PickingText;
    public GameObject p1placingText, p2placingText;

    [Header("Magic Elements")]
    public GameObject MBlueBarReady;
    public List<GameObject> MBlueBars = new List<GameObject>(8);
    private int MBlueBarindex;
    public GameObject MGreenBarReady;
    public List<GameObject> MGreenBars = new List<GameObject>(8);
    private int MGreenBarindex;
    public GameObject MRedBarReady;
    public List<GameObject> MRedBars = new List<GameObject>(8);
    private int MRedBarindex;

    [Header("Science Elements")]
    public GameObject SBlueBarReady;
    public List<GameObject> SBlueBars = new List<GameObject>(8);
    private int SBlueBarindex;
    public GameObject SGreenBarReady;
    public List<GameObject> SGreenBars = new List<GameObject>(8);
    private int SGreenBarindex;
    public GameObject SRedBarReady;
    public List<GameObject> SRedBars = new List<GameObject>(8);
    private int SRedBarindex;

    [Header("Choosing References")]
    public GameObject choosingPhaseText;
    public GameObject p1ChoosingPanel;
    public GameObject p2ChoosingPanel;
    public GameObject p1ChoosingTextMy;
    public GameObject p2ChoosingTextMy;
    public GameObject p1ChoosingTextEnemy;
    public GameObject p2ChoosingTextEnemy;

    [Header("UI Holders references")]
    public GameObject draftUI;
    public GameObject placingUI;
    public GameObject gameUI;
    public GameObject choosingUi;
    public GameObject gameUIPerspective;
    public GameObject factionUI;

    [Header("Main Menu ")]
    public GameObject mainMenuPanel;

    [Header("Win Screen and texts")]
    public GameObject winScreen;
    public TextMeshProUGUI gameResult;

    [Header("Pattern images")]
    public GameObject tooltipPattern;
    public Image cross, T, L, diagonal;

    private void Awake()
    {
        bm = GetComponent<BoardManager>();
        tm = GetComponent<TurnManager>();
    }
    // Use this for initialization
    void Start()
    {
        tooltipPattern.SetActive(false);
        winScreen.SetActive(false);
        gameUI.SetActive(false);
        gameUIPerspective.SetActive(false);
        placingUI.SetActive(false);
        pausePanel.SetActive(false);
        draftUI.SetActive(false);
        choosingUi.SetActive(false);
        factionUI.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Funzioni che iscrivono/disiscrivono l'uiManager agli eventi appena viene abilitato/disabilitato
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnPause += OnGamePause;
        EventManager.OnUnPause += OnGameUnPause;
    }

    private void OnDisable()
    {
        EventManager.OnPause -= OnGamePause;
        EventManager.OnUnPause -= OnGameUnPause;
    }

    private void OnGameUnPause()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

    private void OnGamePause()
    {
        pausePanel.SetActive(true);
        isPaused = true;
    }

    /// <summary> Funzione richiamabile per il tasto Resume del menu di pausa </summary>
    public void ResumeGame()
    {
        if (EventManager.OnUnPause != null)
            EventManager.OnUnPause();
    }

    public void UpdateElementsUI()
    {
        int mredelements = bm.MagicElements.redElement;
        int mblueelements = bm.MagicElements.blueElement;
        int mgreenelements = bm.MagicElements.greenElement;
        int sredelements = bm.ScienceElements.redElement;
        int sblueelements = bm.ScienceElements.blueElement;
        int sgreenelements = bm.ScienceElements.greenElement;

        //Elemento rosso magia
        if (mredelements > 0)
        {
            MRedBars[MRedBarindex].SetActive(false);
            MRedBarindex = mredelements - 1;
            MRedBars[MRedBarindex].SetActive(true);
        }
        else
        {
            MRedBars[MRedBarindex].SetActive(false);
        }

        //Elemento blu magia
        if (mblueelements > 0)
        {
            MBlueBars[MBlueBarindex].SetActive(false);
            MBlueBarindex = mblueelements - 1;
            MBlueBars[MBlueBarindex].SetActive(true);
        }
        else
        {
            MBlueBars[MBlueBarindex].SetActive(false);
        }

        //Elemento verde magia
        if (mgreenelements > 0)
        {
            MGreenBars[MGreenBarindex].SetActive(false);
            MGreenBarindex = mgreenelements - 1;
            MGreenBars[MGreenBarindex].SetActive(true);
        }
        else
        {
            MGreenBars[MGreenBarindex].SetActive(false);
        }

        //Elemento rosso scienza
        if (sredelements > 0)
        {
            SRedBars[SRedBarindex].SetActive(false);
            SRedBarindex = sredelements - 1;
            SRedBars[SRedBarindex].SetActive(true);
        }
        else
        {
            SRedBars[SRedBarindex].SetActive(false);
        }

        //Elemento blu scienza
        if (sblueelements > 0)
        {
            SBlueBars[SBlueBarindex].SetActive(false);
            SBlueBarindex = sblueelements - 1;
            SBlueBars[SBlueBarindex].SetActive(true);
        }
        else
        {
            SBlueBars[SBlueBarindex].SetActive(false);
        }

        //Elemento verde scienza
        if (sgreenelements > 0)
        {
            SGreenBars[SGreenBarindex].SetActive(false);
            SGreenBarindex = sgreenelements - 1;
            SGreenBars[SGreenBarindex].SetActive(true);
        }
        else
        {
            SGreenBars[SGreenBarindex].SetActive(false);
        }
    }

    void SetChoosingUI()
    {
        if (tm.CurrentPlayerTurn == Factions.Magic)
        {
            if (bm.pawnSelected.activePattern == 4)
            {
                p1ChoosingTextEnemy.SetActive(false);
                p1ChoosingTextMy.SetActive(true);
                p2ChoosingTextEnemy.SetActive(false);
                p2ChoosingTextMy.SetActive(false);
                p1ChoosingPanel.SetActive(true);
                p2ChoosingPanel.SetActive(false);
            }
            else if (bm.pawnSelected.activePattern == 5)
            {
                p1ChoosingTextEnemy.SetActive(false);
                p1ChoosingTextMy.SetActive(false);
                p2ChoosingTextEnemy.SetActive(true);
                p2ChoosingTextMy.SetActive(false);
                p1ChoosingPanel.SetActive(false);
                p2ChoosingPanel.SetActive(true);
            }
        }
        else if (tm.CurrentPlayerTurn == Factions.Science)
        {
            if (bm.pawnSelected.activePattern == 4)
            {
                p1ChoosingTextEnemy.SetActive(false);
                p1ChoosingTextMy.SetActive(false);
                p2ChoosingTextEnemy.SetActive(false);
                p2ChoosingTextMy.SetActive(true);
                p1ChoosingPanel.SetActive(false);
                p2ChoosingPanel.SetActive(true);
            }
            else if (bm.pawnSelected.activePattern == 5)
            {
                p1ChoosingTextEnemy.SetActive(true);
                p1ChoosingTextMy.SetActive(false);
                p2ChoosingTextEnemy.SetActive(false);
                p2ChoosingTextMy.SetActive(false);
                p1ChoosingPanel.SetActive(true);
                p2ChoosingPanel.SetActive(false);
            }
        }
    }

    #region MainMenu

    /// <summary>
    /// Funzione del pulsante start che chiude il main menu e avvia la fase di draft
    /// </summary>
    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        factionUI.SetActive(true);
        bm.turnManager.CurrentMacroPhase = TurnManager.MacroPhase.faction;
    }

    /// <summary>
    /// Funzione del pulsante Options che mostra le opzioni
    /// </summary>
    public void Options()
    {

    }

    /// <summary>
    /// Funzione del pulsante tutorial che mostra il tutorial del gioco
    /// </summary>
    public void Tutorial()
    {

    }

    /// <summary>
    /// Funzione del pulsante Credits che mostra i crediti del gioco
    /// </summary>
    public void Credits()
    {

    }

    #endregion

    /// <summary>
    /// Funzione che aggiorna l'ui in base alla fase e stato del turno
    /// </summary>
    public void UIChange()
    {

        switch (tm.CurrentMacroPhase)
        {
            case TurnManager.MacroPhase.menu:
                tooltipPattern.SetActive(false);
                winScreen.SetActive(false);
                gameUI.SetActive(false);
                gameUIPerspective.SetActive(false);
                placingUI.SetActive(false);
                pausePanel.SetActive(false);
                draftUI.SetActive(false);
                choosingUi.SetActive(false);
                mainMenuPanel.SetActive(true);
                break;
            case TurnManager.MacroPhase.draft:
                switch (tm.CurrentPlayerTurn)
                {
                    case Factions.Magic:
                        p1PickingText.SetActive(true);
                        p2PickingText.SetActive(false);
                        break;
                    case Factions.Science:
                        p1PickingText.SetActive(false);
                        p2PickingText.SetActive(true);
                        break;
                }
                break;
            case TurnManager.MacroPhase.placing:
                switch (tm.CurrentTurnState)
                {
                    case TurnManager.PlayTurnState.choosing:
                        SetChoosingUI();
                        break;
                    case TurnManager.PlayTurnState.placing:
                        tooltipPattern.SetActive(false);
                        switch (tm.CurrentPlayerTurn)
                        {
                            case Factions.Magic:
                                p1placingText.SetActive(true);
                                p2placingText.SetActive(false);
                                break;
                            case Factions.Science:
                                p1placingText.SetActive(false);
                                p2placingText.SetActive(true);
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case TurnManager.MacroPhase.game:
                switch (tm.CurrentPlayerTurn)
                {
                    case Factions.Magic:
                        gameTurnText.text = "MAGIC TURN";
                        break;
                    case Factions.Science:
                        gameTurnText.text = "SCIENCE TURN";
                        break;
                }

                switch (tm.CurrentTurnState)
                {
                    case TurnManager.PlayTurnState.choosing:
                        choosingUi.SetActive(true);
                        choosingPhaseText.SetActive(false);
                        SetChoosingUI();
                        break;
                    case TurnManager.PlayTurnState.check:
                        ActiveSuperAttackText();
                        UpdateReadyElement();
                        break;
                    case TurnManager.PlayTurnState.movementattack:
                        switch (tm.CurrentPlayerTurn)
                        {
                            case Factions.Magic:
                                break;
                            case Factions.Science:
                                break;
                        }
                        break;
                    case TurnManager.PlayTurnState.attack:
                        switch (tm.CurrentPlayerTurn)
                        {
                            case Factions.Magic:
                                break;
                            case Factions.Science:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    private void UpdateReadyElement()
    {
        //Magic
        if (bm.MagicElements.redElement >= 1 && bm.MagicElements.blueElement >= 1 && bm.MagicElements.greenElement >= 1)
        {
            MRedBarReady.SetActive(true);
            MBlueBarReady.SetActive(true);
            MGreenBarReady.SetActive(true);
        }
        else
        {
            if (bm.MagicElements.redElement >= 3 && !MRedBarReady.activeSelf)
            {
                MRedBarReady.SetActive(true);
            }
            else if (bm.MagicElements.redElement < 3 && MRedBarReady.activeSelf)
            {
                MRedBarReady.SetActive(false);
            }

            if (bm.MagicElements.blueElement >= 3 && !MBlueBarReady.activeSelf)
            {
                MBlueBarReady.SetActive(true);
            }
            else if (bm.MagicElements.blueElement < 3 && MBlueBarReady.activeSelf)
            {
                MBlueBarReady.SetActive(false);
            }

            if (bm.MagicElements.greenElement >= 3 && !MGreenBarReady.activeSelf)
            {
                MGreenBarReady.SetActive(true);
            }
            else if (bm.MagicElements.greenElement < 3 && MGreenBarReady.activeSelf)
            {
                MGreenBarReady.SetActive(false);
            }
        }

        //Science
        if (bm.ScienceElements.redElement >= 1 && bm.ScienceElements.blueElement >= 1 && bm.ScienceElements.greenElement >= 1)
        {
            SRedBarReady.SetActive(true);
            SBlueBarReady.SetActive(true);
            SGreenBarReady.SetActive(true);
        }
        else
        {
            if (bm.ScienceElements.redElement >= 3 && !SRedBarReady.activeSelf)
            {
                SRedBarReady.SetActive(true);
            }
            else if (bm.ScienceElements.redElement < 3 && SRedBarReady.activeSelf)
            {
                SRedBarReady.SetActive(false);
            }

            if (bm.ScienceElements.blueElement >= 3 && !SBlueBarReady.activeSelf)
            {
                SBlueBarReady.SetActive(true);
            }
            else if (bm.ScienceElements.blueElement < 3 && SBlueBarReady.activeSelf)
            {
                SBlueBarReady.SetActive(false);
            }

            if (bm.ScienceElements.greenElement >= 3 && !SGreenBarReady.activeSelf)
            {
                SGreenBarReady.SetActive(true);
            }
            else if (bm.ScienceElements.greenElement < 3 && SGreenBarReady.activeSelf)
            {
                SGreenBarReady.SetActive(false);
            }
        }
    }

    public void PassTurn()
    {
        if (!bm.pause && (tm.CurrentTurnState == TurnManager.PlayTurnState.attack || tm.CurrentTurnState == TurnManager.PlayTurnState.movementattack))
        {
            tm.ChangeTurn();
        }
    }

    //Funzione provvisoria
    public void ActiveSuperAttackText()
    {
        if (BoardManager.Instance.superAttack)
        {
            superattackText.SetActive(true);
        }
        else
        {
            superattackText.SetActive(false);
        }
    }
}
