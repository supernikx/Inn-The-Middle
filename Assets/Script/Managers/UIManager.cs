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
    public ChangeButtonImage soundtogglemenu;
    public ChangeButtonImage tutorialtogglemenu;

    [Header("Faction Chooice")]
    public GameObject MagicButton;
    public GameObject ScienceButton;

    [Header("Draft")]
    public GameObject MagicPickingText;
    public GameObject SciencePickingText;
    public GameObject StartDraftButton;
    public SetDraftImagePatterns[] magic_picks, science_picks;
    public GameObject MagicStartPressed;
    public GameObject ScienceStartPressed;
    public GameObject PressStartDraftText;

    [Header("Game")]
    public Image SFrameImage;
    public Image SOn;
    public Image SOff;
    public Image SAngry;
    public Image SHappy;
    public Image SSurprised;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public GameObject ResumePauseButton;
    public GameObject RestartPauseButton;
    public GameObject QuitPauseButton;
    public ChangeButtonImage soundtogglepause;

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
        #region Draft
        draftUI.SetActive(false);
        MagicStartPressed.SetActive(false);
        ScienceStartPressed.SetActive(false);
        #endregion
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


    /// <summary>
    /// Funzione che viene chiamata quando il joystick si disconnette
    /// </summary>
    private void JoystickDisconnected()
    {
        connectjoystick.SetActive(true);
    }

    /// <summary>
    /// Funzione che viene chiamata quando il joystick si riconnette
    /// </summary>
    private void JoystickRiconnected()
    {
        connectjoystick.SetActive(false);
    }

    #region Pause

    /// <summary>
    /// Funzione che viene chiamata quando il gioco esce dalla pausa
    /// </summary>
    private void OnGameUnPause()
    {
        pausePanel.SetActive(false);
        StartCoroutine(FocusOnReumeButton(false));
    }

    /// <summary>
    /// Funzione che viene chiamata quando il gioco entra dalla pausa
    /// </summary>
    private void OnGamePause()
    {
        pausePanel.SetActive(true);
        StartCoroutine(FocusOnReumeButton(true));
        UpdateSoundUI();
    }

    /// <summary>
    /// Funzione che imposta il focus o lo disabilita sul pulsante resume del menù di pausa, in base al bool passato come parametro
    /// </summary>
    /// <param name="focus"></param>
    /// <returns></returns>
    private IEnumerator FocusOnReumeButton(bool focus)
    {
        yield return null;
        if (focus)
            EventSystem.current.SetSelectedGameObject(ResumePauseButton);
        else
            EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    ///  Funzione richiamabile per il tasto Resume del menu di pausa
    /// </summary>
    public void ResumeGame()
    {
        if (EventManager.OnUnPause != null)
            EventManager.OnUnPause();
    }

    #endregion

    #region Game

    /// <summary>
    /// Funzione che attiva il super-atacco
    /// </summary>
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

    /// <summary>
    /// Funzione che aggiorna i contatori degli elementi 
    /// </summary>
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

    /// <summary>
    /// Funzione che imposta la tacca superattack ready nei contatori
    /// </summary>
    public void UpdateReadyElement()
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

    /// <summary>
    /// Funzione che imposta l'ui della choosing phase
    /// </summary>
    public void SetChoosingUI()
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

    public void UpdateExpressions(Expressions e)
    {
        StartCoroutine(UpdateExpressionsCoroutine(e));
    }

    public IEnumerator UpdateExpressionsCoroutine (Expressions e)
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                break;
            case Factions.Science:
                switch (e)
                {
                    case Expressions.On:
                        SFrameImage.sprite = SOn.sprite;
                        break;
                    case Expressions.Off:
                        SFrameImage.sprite = SOff.sprite;
                        break;
                    case Expressions.Happy:
                        SFrameImage.sprite = SHappy.sprite;
                        yield return new WaitForSeconds(1.5f);
                        SFrameImage.sprite = SOn.sprite;
                        break;
                    case Expressions.Angry:
                        SFrameImage.sprite = SAngry.sprite;
                        yield return new WaitForSeconds(1.5f);
                        SFrameImage.sprite = SOn.sprite;
                        break;
                    case Expressions.Surprised:
                        SFrameImage.sprite = SSurprised.sprite;
                        yield return new WaitForSeconds(1.5f);
                        SFrameImage.sprite = SOn.sprite;
                        break;
                }
                break;
        }
        yield return null;
    }

    #endregion

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
        yield return new WaitForSeconds(1f);
        EventSystem.current.SetSelectedGameObject(MagicButton);
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.idle;
        bm.turnManager.CurrentMacroPhase = TurnManager.MacroPhase.faction;
    }

    /// <summary>
    /// Funzione che imposta il focus del main menù sullo start button
    /// </summary>
    /// <returns></returns>
    public IEnumerator FocusStartButtonMenu()
    {
        yield return new WaitForSeconds(1.3f);
        EventSystem.current.SetSelectedGameObject(StartMenuButton);
    }

    /// <summary>
    /// Funzione che chiama la coroutine FactionChosenCoroutine e che imposta la fazione scelta dal giocatore 1/2
    /// </summary>
    /// <param name="factionID"></param>
    public void FactionChoosen(int factionID)
    {
        bm.FactionChosen(factionID);
        StartCoroutine(FactionChoosenCoroutine());
    }

    /// <summary>
    /// Funzione che viene chiamata quando è stata scelta la fazione ed esegue il fade fra le due fasi
    /// </summary>
    /// <returns></returns>
    private IEnumerator FactionChoosenCoroutine()
    {
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.animation;
        EventSystem.current.SetSelectedGameObject(null);
        fadeinoutmenu.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        factionUI.SetActive(false);
        draftUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        bm.turnManager.CurrentMacroPhase = TurnManager.MacroPhase.draft;
    }

    /// <summary>
    /// Funzione del pulsante tutorial che mostra il tutorial del gioco
    /// </summary>
    public void Tutorial()
    {

    }

    /// <summary>
    /// Funzione che imposta lo skip del title screen per la prossima volta che si torna al main menù
    /// </summary>
    /// <returns></returns>
    public IEnumerator SkipTitleScreen()
    {
        TitleScreen.GetComponent<Animator>().SetTrigger("KeyPressed");
        yield return new WaitForSeconds(1f);
        TitleScreen.SetActive(false);
        MainMenu.SetActive(true);
        StartCoroutine(FocusStartButtonMenu());
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

                if (!bm.draftManager.draftEnd)
                {
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
                }
                else
                {
                    PressStartDraftText.SetActive(true);
                    SciencePickingText.SetActive(false);
                    MagicPickingText.SetActive(false);
                    if (bm.draftManager.p1StartPressed)
                        switch (bm.p1Faction)
                        {
                            case Factions.Magic:
                                MagicStartPressed.SetActive(true);
                                break;
                            case Factions.Science:
                                ScienceStartPressed.SetActive(true);
                                break;
                        }
                    if (bm.draftManager.p2StartPressed)
                        switch (bm.p2Faction)
                        {
                            case Factions.Magic:
                                MagicStartPressed.SetActive(true);
                                break;
                            case Factions.Science:
                                ScienceStartPressed.SetActive(true);
                                break;
                        }
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
                        choosingUi.SetActive(false);
                        ActiveSuperAttackText();
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

    /// <summary>
    /// Funzione che aggiorna le pedine scelta dai giocatori nella fase di draft
    /// </summary>
    public void UpdateDraftChoose()
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                for (int i = 0; i < bm.draftManager.magic_pawns_picks.Count; i++)
                {
                    magic_picks[i].SetPatternImage(bm.draftManager.magic_pawns_picks[i]);
                }
                break;
            case Factions.Science:
                for (int i = 0; i < bm.draftManager.science_pawns_picks.Count; i++)
                {
                    science_picks[i].SetPatternImage(bm.draftManager.science_pawns_picks[i]);
                }
                break;
        }
    }

    /// <summary>
    /// Funzione che aggiorna le icone del suono (main menù e draft)
    /// </summary>
    public void UpdateSoundUI()
    {
        bool flag = true;
        if (!SoundManager.instance.SoundActive)
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);
                flag = false;
            }
            soundtogglepause.SetPressedImage();
            if (!flag)
                pausePanel.SetActive(false);

            flag = true;

            if (!MainMenu.activeSelf)
            {
                MainMenu.SetActive(true);
                flag = false;
            }
            soundtogglemenu.SetPressedImage();
            if (!flag)
                MainMenu.SetActive(false);
        }
        else
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);
                flag = false;
            }
            soundtogglepause.SetDefaultImage();
            if (!flag)
                pausePanel.SetActive(false);

            flag = true;

            if (!MainMenu.activeSelf)
            {
                MainMenu.SetActive(true);
                flag = false;
            }
            soundtogglemenu.SetDefaultImage();
            if (!flag)
                MainMenu.SetActive(false);
        }
    }
}

public enum Expressions
{
    On,
    Off,
    Happy,
    Angry,
    Surprised,
}
