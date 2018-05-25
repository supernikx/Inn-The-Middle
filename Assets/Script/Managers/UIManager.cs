using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{

    TurnManager tm;
    BoardManager bm;

    /// <summary> Testo per indicare di chi è il turno </summary>
    [Header("Turn Text")]
    public TextMeshProUGUI gameTurnText;
    public GameObject superattackText;
    public GameObject MagicPlacingText, SciencePlacingText;

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
    
    public GameObject TitleScreen;
    public GameObject factionUI;
    public GameObject draftUI;
    public GameObject choosingUi;
    public GameObject placingUI;
    public GameObject gameUI;
    public GameObject connectjoystick;
    public Animator fadeinoutmenu;

    [Header("Main Menu")]
    public GameObject MainMenu;
    public GameObject StartMenuButton;

    [Header("Faction Chooice")]
    public GameObject MagicButton;
    public GameObject ScienceButton;

    [Header("Draft")]
    public GameObject MagicPickingText;
    public GameObject SciencePickingText;
    public GameObject StartDraftButton;
    public Image[] magic_picks, science_picks;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public GameObject ResumePauseButton;
    public GameObject RestartPauseButton;
    public GameObject QuitPauseButton;

    [Header("Win Screen and texts")]
    public GameObject winScreen;
    public TextMeshProUGUI gameResult;

    private void Awake()
    {
        bm = GetComponent<BoardManager>();
        tm = GetComponent<TurnManager>();
    }
    // Use this for initialization
    void Start()
    {
        winScreen.SetActive(false);
        gameUI.SetActive(false);
        placingUI.SetActive(false);
        pausePanel.SetActive(false);
        draftUI.SetActive(false);
        choosingUi.SetActive(false);
        factionUI.SetActive(false);
        if (DataManager.instance._SkipTitleScreen)
        {
            TitleScreen.SetActive(false);
            MainMenu.SetActive(true);
            StartCoroutine(FocusStartButtonMenu());
        }
        else
        {
            MainMenu.SetActive(false);
            TitleScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Funzioni che iscrivono/disiscrivono l'uiManager agli eventi appena viene abilitato/disabilitato
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnPause += OnGamePause;
        EventManager.OnUnPause += OnGameUnPause;
        EventManager.OnJoystickDisconnected += JoystickDisconnected;
        EventManager.OnJoystickRiconnected += JoystickRiconnected;
    }

    private void OnDisable()
    {
        EventManager.OnPause -= OnGamePause;
        EventManager.OnUnPause -= OnGameUnPause;
        EventManager.OnJoystickDisconnected -= JoystickDisconnected;
        EventManager.OnJoystickRiconnected -= JoystickRiconnected;
    }

    private void OnGameUnPause()
    {
        pausePanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnGamePause()
    {
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ResumePauseButton);
    }

    private void JoystickDisconnected()
    {
        connectjoystick.SetActive(true);
    }

    private void JoystickRiconnected()
    {
        connectjoystick.SetActive(false);
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
        StartCoroutine(StartGameRoutine());
    }

    /// <summary>
    /// Coroutine che viene chiamata alla pressione del pulsante start del menù principale e che fa da transizione tra il menù e la scelta della fazione
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGameRoutine()
    {
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.animation;
        EventSystem.current.SetSelectedGameObject(null);
        fadeinoutmenu.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        MainMenu.SetActive(false);
        factionUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(MagicButton);
        bm.turnManager.CurrentMacroPhase = TurnManager.MacroPhase.faction;
    }

    public void FactionChoosen(int factionID)
    {
        bm.FactionChosen(factionID);
        StartCoroutine(FactionChoosenCoroutine());
    }

    private IEnumerator FactionChoosenCoroutine()
    {
        EventSystem.current.SetSelectedGameObject(null);
        fadeinoutmenu.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        factionUI.SetActive(false);
        draftUI.SetActive(true);
        bm.turnManager.CurrentMacroPhase = TurnManager.MacroPhase.draft;
    }

    /// <summary>
    /// Funzione del pulsante tutorial che mostra il tutorial del gioco
    /// </summary>
    public void Tutorial()
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
                winScreen.SetActive(false);
                gameUI.SetActive(false);
                placingUI.SetActive(false);
                pausePanel.SetActive(false);
                draftUI.SetActive(false);
                choosingUi.SetActive(false);
                MainMenu.SetActive(true);
                break;
            case TurnManager.MacroPhase.draft:
                if (bm.draftManager.hasDrafted)
                {
                    StartDraftButton.SetActive(false);
                }
                
                switch (tm.CurrentPlayerTurn)
                {
                    case Factions.Magic:
                        MagicPickingText.SetActive(true);
                        SciencePickingText.SetActive(false);
                        break;
                    case Factions.Science:
                        MagicPickingText.SetActive(false);
                        SciencePickingText.SetActive(true);
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
                        switch (tm.CurrentPlayerTurn)
                        {
                            case Factions.Magic:
                                MagicPlacingText.SetActive(true);
                                SciencePlacingText.SetActive(false);
                                break;
                            case Factions.Science:
                                MagicPlacingText.SetActive(false);
                                SciencePlacingText.SetActive(true);
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

    public void UpdateDraftChoose()
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                for (int i = 0; i < bm.draftManager.magic_pawns_picks.Count; i++)
                {
                    if (!magic_picks[i].enabled)
                    {
                        magic_picks[i].enabled = true;
                        switch (bm.draftManager.magic_pawns_picks[i])
                        {
                            case 0:
                                magic_picks[i].color = Color.blue;
                                break;
                            case 1:
                                magic_picks[i].color = Color.green;
                                break;
                            case 2:
                                magic_picks[i].color = Color.yellow;
                                break;
                            case 3:
                                magic_picks[i].color = Color.red;
                                break;
                            case 4:
                                magic_picks[i].color = Color.white;
                                break;
                            case 5:
                                magic_picks[i].color = Color.black;
                                break;
                        }
                    }
                }
                break;
            case Factions.Science:
                for (int i = 0; i < bm.draftManager.science_pawns_picks.Count; i++)
                {
                    if (!science_picks[i].enabled)
                    {
                        science_picks[i].enabled = true;
                        switch (bm.draftManager.science_pawns_picks[i])
                        {
                            case 0:
                                science_picks[i].color = Color.blue;
                                break;
                            case 1:
                                science_picks[i].color = Color.green;
                                break;
                            case 2:
                                science_picks[i].color = Color.yellow;
                                break;
                            case 3:
                                science_picks[i].color = Color.red;
                                break;
                            case 4:
                                science_picks[i].color = Color.white;
                                break;
                            case 5:
                                science_picks[i].color = Color.black;
                                break;
                        }
                    }
                }
                break;
        }
    }

    public void PassTurn()
    {
        if (!bm.pause && (tm.CurrentTurnState == TurnManager.PlayTurnState.attack || tm.CurrentTurnState == TurnManager.PlayTurnState.movementattack))
        {
            tm.ChangeTurn();
        }
    }

    public IEnumerator FocusStartButtonMenu()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(StartMenuButton);
    }

    public IEnumerator SkipTitleScreen()
    {
        TitleScreen.GetComponent<Animator>().SetTrigger("KeyPressed");
        yield return new WaitForSeconds(1f);
        TitleScreen.SetActive(false);
        StartCoroutine(FocusStartButtonMenu());
    }

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
