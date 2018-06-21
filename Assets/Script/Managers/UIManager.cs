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
    public GameObject MagicChoosingPanel;
    public GameObject ScienceChoosingPanel;

    [Header("UI Holders references")]
    public GameObject TitleScreen;
    public GameObject OptionsMenu;
    public GameObject factionUI;
    public GameObject draftUI;
    public GameObject choosingUi;
    public GameObject placingUI;
    public GameObject gameUI;
    public GameObject winScreen;
    public GameObject connectjoystick;
    public Animator fadeinoutmenu;

    [Header("Main Menu")]
    public GameObject Title;
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

    [Header("Placing")]
    public Image MagicPlacingFrame;
    public Image SciencePlacingFrame;

    [Header("Game Science")]
    public Image SSuperAttackImage;
    public Sprite SSuperAttackOn;
    public Sprite SSuperAttackOff;
    public Image SFrameImage;
    public Sprite SOn;
    public Sprite SOff;
    public Sprite SAngry;
    public Sprite SHappy;
    public Sprite SSurprised;

    [Header("Game Magic")]
    public Image MSuperAttackImage;
    public Sprite MSuperAttackOn;
    public Sprite MSuperAttackOff;
    public Image MFrameImage;
    public Sprite MOn;
    public Sprite MOff;
    public Sprite MAngry;
    public Sprite MHappy;
    public Sprite MSurprised;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public GameObject ResumePauseButton;
    public GameObject RestartPauseButton;
    public GameObject QuitPauseButton;
    public ChangeButtonImage soundtogglepause;

    [Header("Win Screen and texts")]
    public GameObject MagicWinImage;
    public GameObject ScienceWinImage;
    public GameObject DrawWinImage;
    public TextMeshProUGUI TurnWinText;

    [Header("Turn Button")]
    public Material ScienceTurnMaterial;
    public Material MagicTurnMaterial;
    public Material ScienceTurnMaterialEmission;
    public Material MagicTurnMaterialEmission;
    public GameObject TurnButton;
    MeshRenderer TurnButtonActualMeshRender;
    Animator TurnButtonAnimator;

    private void Awake()
    {
        bm = GetComponent<BoardManager>();
        tm = GetComponent<TurnManager>();
    }

    // Use this for initialization
    void Start()
    {
        #region WinCondition
        winScreen.SetActive(false);
        MagicWinImage.SetActive(false);
        ScienceWinImage.SetActive(false);
        DrawWinImage.SetActive(false);
        #endregion
        OptionsMenu.SetActive(true);
        gameUI.SetActive(false);
        placingUI.SetActive(false);
        pausePanel.SetActive(false);
        Title.SetActive(true);
        #region Draft
        draftUI.SetActive(false);
        MagicStartPressed.SetActive(false);
        ScienceStartPressed.SetActive(false);
        #endregion
        choosingUi.SetActive(false);
        factionUI.SetActive(false);
        TurnButtonActualMeshRender = TurnButton.GetComponent<MeshRenderer>();
        TurnButtonAnimator = TurnButton.GetComponent<Animator>();
        if (DataManager.instance._SkipTitleScreen)
        {
            TitleScreen.SetActive(false);
            Title.GetComponent<Animator>().SetTrigger("MainMenu");
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
        EventManager.OnGameEnd += SetWinScreen;
    }
    private void OnDisable()
    {
        EventManager.OnPause -= OnGamePause;
        EventManager.OnUnPause -= OnGameUnPause;
        EventManager.OnJoystickDisconnected -= JoystickDisconnected;
        EventManager.OnJoystickRiconnected -= JoystickRiconnected;
        EventManager.OnGameEnd -= SetWinScreen;
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
        if (bm.superAttack)
        {
            switch (bm.turnManager.CurrentPlayerTurn)
            {
                case Factions.Magic:
                    MSuperAttackImage.sprite = MSuperAttackOn;
                    break;
                case Factions.Science:
                    SSuperAttackImage.sprite = SSuperAttackOn;
                    break;
            }
        }
        else
        {
            MSuperAttackImage.sprite = MSuperAttackOff;
            SSuperAttackImage.sprite = SSuperAttackOff;
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
                if (tm.CurrentMacroPhase == TurnManager.MacroPhase.placing)
                {
                    MagicPlacingFrame.sprite = MOn;
                    SciencePlacingFrame.sprite = SOff;
                }
                MagicChoosingPanel.SetActive(true);
                ScienceChoosingPanel.SetActive(false);
            }
            else if (bm.pawnSelected.activePattern == 5)
            {
                if (tm.CurrentMacroPhase == TurnManager.MacroPhase.placing)
                {
                    MagicPlacingFrame.sprite = MOn;
                    SciencePlacingFrame.sprite = SOff;
                }
                MagicChoosingPanel.SetActive(false);
                ScienceChoosingPanel.SetActive(true);
            }
        }
        else if (tm.CurrentPlayerTurn == Factions.Science)
        {
            if (bm.pawnSelected.activePattern == 4)
            {
                if (tm.CurrentMacroPhase == TurnManager.MacroPhase.placing)
                {
                    SciencePlacingFrame.sprite = SOn;
                    MagicPlacingFrame.sprite = MOff;
                }
                MagicChoosingPanel.SetActive(false);
                ScienceChoosingPanel.SetActive(true);
            }
            else if (bm.pawnSelected.activePattern == 5)
            {
                if (tm.CurrentMacroPhase == TurnManager.MacroPhase.placing)
                {
                    SciencePlacingFrame.sprite = SOn;
                    MagicPlacingFrame.sprite = MOff;
                }
                MagicChoosingPanel.SetActive(true);
                ScienceChoosingPanel.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Funzione che chiama la coroutine che aggiorna le espressioni dei 2 personaggi
    /// </summary>
    /// <param name="e"></param>
    public void UpdateExpressions(Expressions e)
    {
        StartCoroutine(UpdateExpressionsCoroutine(e));
    }

    /// <summary>
    /// Coroutine che imposta al personaggio di turno l'espressione passata come parametro
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public IEnumerator UpdateExpressionsCoroutine(Expressions e)
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                switch (e)
                {
                    case Expressions.On:
                        MFrameImage.sprite = MOn;
                        break;
                    case Expressions.Off:
                        MFrameImage.sprite = MOff;
                        break;
                        /*case Expressions.Happy:
                            MFrameImage.sprite = MHappy;
                            yield return new WaitForSeconds(1.5f);
                            MFrameImage.sprite = MOn;
                            break;
                        case Expressions.Angry:
                            MFrameImage.sprite = MAngry;
                            yield return new WaitForSeconds(1.5f);
                            MFrameImage.sprite = MOn;
                            break;
                        case Expressions.Surprised:
                            MFrameImage.sprite = MSurprised;
                            yield return new WaitForSeconds(1.5f);
                            MFrameImage.sprite = MOn;
                            break;*/
                }
                break;
            case Factions.Science:
                switch (e)
                {
                    case Expressions.On:
                        SFrameImage.sprite = SOn;
                        break;
                    case Expressions.Off:
                        SFrameImage.sprite = SOff;
                        break;
                        /*case Expressions.Happy:
                            SFrameImage.sprite = SHappy;
                            yield return new WaitForSeconds(1.5f);
                            SFrameImage.sprite = SOn;
                            break;
                        case Expressions.Angry:
                            SFrameImage.sprite = SAngry;
                            yield return new WaitForSeconds(1.5f);
                            SFrameImage.sprite = SOn;
                            break;
                        case Expressions.Surprised:
                            SFrameImage.sprite = SSurprised;
                            yield return new WaitForSeconds(1.5f);
                            SFrameImage.sprite = SOn;
                            break;*/
                }
                break;
        }
        yield return null;
    }

    /// <summary>
    /// Funzione che stoppa e riavvia la funzione che cambia il pulsante centrale della plancia che indica il turno
    /// </summary>
    IEnumerator changebuttonroutine;
    public void ChangeButton()
    {
        if (changebuttonroutine != null)
        {
            StopCoroutine(changebuttonroutine);
        }        
        changebuttonroutine = ChangeTurnButtonCoroutne();
        StartCoroutine(changebuttonroutine);
    }

    /// <summary>
    /// Funzione che aggiorna il pulsante centrale sulla board che indica il turno
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChangeTurnButtonCoroutne()
    {
        TurnButtonAnimator.SetTrigger("Flip");
        yield return new WaitForSeconds(0.23f);
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                TurnButtonActualMeshRender.material = MagicTurnMaterial;
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.3f);
                    TurnButtonActualMeshRender.material = MagicTurnMaterialEmission;
                    yield return new WaitForSeconds(0.3f);
                    TurnButtonActualMeshRender.material = MagicTurnMaterial;
                }                
                break;
            case Factions.Science:
                TurnButtonActualMeshRender.material = ScienceTurnMaterial;
                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.3f);
                    TurnButtonActualMeshRender.material = ScienceTurnMaterialEmission;
                    yield return new WaitForSeconds(0.3f);
                    TurnButtonActualMeshRender.material = ScienceTurnMaterial;
                }
                break;
        }
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
        Title.SetActive(false);
        factionUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        EventSystem.current.SetSelectedGameObject(MagicButton);
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.idle;
        bm.turnManager.CurrentMacroPhase = TurnManager.MacroPhase.faction;
    }

    /// <summary>
    /// Funzione che invoca la coroutine
    /// </summary>
    public void FocusStartButton()
    {
        StartCoroutine(FocusStartButtonMenu());
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
        Title.GetComponent<Animator>().SetTrigger("KeyPressed");
        yield return new WaitForSeconds(1f);
        TitleScreen.SetActive(false);
        MainMenu.SetActive(true);
        StartCoroutine(FocusStartButtonMenu());
    }

    #endregion

    #region Draft

    int magicpickindex = 0;
    int sciencepickindex = 0;
    /// <summary>
    /// Funzione che aggiorna le pedine scelta dai giocatori nella fase di draft
    /// </summary>
    public void UpdateDraftChoose()
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                magic_picks[magicpickindex].SetPatternImage(bm.draftManager.magic_pawns_picks[magicpickindex]);
                magicpickindex++;
                break;
            case Factions.Science:
                science_picks[sciencepickindex].SetPatternImage(bm.draftManager.science_pawns_picks[sciencepickindex]);
                sciencepickindex++;
                break;
        }
    }

    public void UpdateDraftDissolvedChoose(int patternindex)
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                magic_picks[magicpickindex].SetDissolvedPatternImage(patternindex);
                break;
            case Factions.Science:
                science_picks[sciencepickindex].SetDissolvedPatternImage(patternindex);
                break;
        }
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
                        break;
                    case TurnManager.PlayTurnState.placing:
                        switch (tm.CurrentPlayerTurn)
                        {
                            case Factions.Magic:
                                MagicPlacingFrame.sprite = MOn;
                                SciencePlacingFrame.sprite = SOff;
                                break;
                            case Factions.Science:
                                SciencePlacingFrame.sprite = SOn;
                                MagicPlacingFrame.sprite = MOff;
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case TurnManager.MacroPhase.game:
                switch (tm.CurrentTurnState)
                {
                    case TurnManager.PlayTurnState.choosing:
                        choosingUi.SetActive(true);
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

    public void SetWinScreen(Factions WinFaction)
    {
        winScreen.SetActive(true);
        switch (WinFaction)
        {
            case Factions.None:
                DrawWinImage.SetActive(true);
                break;
            case Factions.Magic:
                MagicWinImage.SetActive(true);
                break;
            case Factions.Science:
                ScienceWinImage.SetActive(true);
                break;
        }
        TurnWinText.text = bm.turnManager.numberOfTurns.ToString() + " Turns";
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
