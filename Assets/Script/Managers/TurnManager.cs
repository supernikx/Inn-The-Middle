using UnityEngine;
using System.Collections;

public enum Factions {None, Magic, Science};

public class TurnManager : MonoBehaviour
{
    /// <summary> PlayerTurn corrente </summary>
    private Factions _currentPlayerTurn;
    public Factions CurrentPlayerTurn
    {
        get
        {
            return _currentPlayerTurn;
        }
        set
        {
            OnTurnEnd(_currentPlayerTurn);
            _currentPlayerTurn = value;
            OnTurnStart(_currentPlayerTurn);
        }
    }

    public enum MacroPhase { menu, faction, draft, placing, game, end };
    private MacroPhase _currentMacroPhase;
    public MacroPhase CurrentMacroPhase
    {
        get
        {
            return _currentMacroPhase;
        }
        set
        {
            if (MacroPhaseChange(value))
            {
                _currentMacroPhase = value;
                OnMacroPhaseStart(_currentMacroPhase);
            }
        }
    }

    /// <summary> Stato per indicare la fase corrente del macroturno PlayTurn </summary>
    public enum PlayTurnState { choosing, placing, animation, check, movementattack, attack, idle };
    /// <summary> PlayTurnState corrente </summary>
    private PlayTurnState _currentTurnState;
    public PlayTurnState CurrentTurnState
    {
        get
        {
            return _currentTurnState;
        }
        set
        {
            if (StateChange(value))
            {
                _currentTurnState = value;
                OnStateStart(_currentTurnState);
            }
        }
    }

    public int numberOfTurns;
    public int turnsWithoutAttack;
    public bool CheckAlreadyDone;

    [Header("Camera references")]
    public Camera mainCam;
    public Camera draftCam;

    [Header("Light references")]
    public Light magicPlacementLight;
    public Light sciencePlacementLight;
    public Light centralLight;

    private void OnEnable()
    {
        EventManager.OnGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        EventManager.OnGameEnd -= OnGameEnd;
    }

    private void OnGameEnd(Factions WinFaction)
    {
        CurrentMacroPhase = MacroPhase.end;
    }

    // Use this for initialization
    void Start()
    {
        CurrentMacroPhase = MacroPhase.menu;
        mainCam.enabled = true;
        draftCam.enabled = false;
        CheckAlreadyDone = false;        
    }

    /// <summary>
    /// Funzione che viene chiamata quando si entra in un nuovo stato del turno ed esegue le funzioni necessarie a quello stato
    /// </summary>
    /// <param name="newState"></param>
    void OnStateStart(PlayTurnState newState)
    {
        switch (newState)
        {
            case PlayTurnState.choosing:
                break;
            case PlayTurnState.placing:
                BoardManager.Instance.uiManager.choosingUi.SetActive(false);
                BoardManager.Instance.uiManager.placingUI.SetActive(true);
                CurrentPlayerTurn = BoardManager.Instance.p1Faction;
                BoardManager.Instance.uiManager.UIChange();
                break;
            case PlayTurnState.animation:
                break;
            case PlayTurnState.check:
                if (!CheckAlreadyDone)
                {
                    if (BoardManager.Instance.superAttack)
                    {
                        BoardManager.Instance.uiManager.UpdateExpressions(Expressions.Angry);
                        BoardManager.Instance.superAttack = false;
                    }
                    BoardManager.Instance.UnmarkAttackMarker();
                    BoardManager.Instance.PawnHighlighted(false);
                    BoardManager.Instance.CheckSuperAttack();
                    if (BoardManager.Instance.pawnSelected != null)
                    {
                        BoardManager.Instance.DeselectPawn();
                    }
                    BoardManager.Instance.uiManager.UIChange();
                    BoardManager.Instance.CheckPhaseControll();
                }
                BoardManager.Instance.uiManager.UIChange();
                break;
            case PlayTurnState.movementattack:
                BoardManager.Instance.SelectNextPawn(Directions.idle);
                BoardManager.Instance.uiManager.UIChange();
                break;
            case PlayTurnState.attack:
                if (!BoardManager.Instance.pawnSelected.CheckAttackPattern())
                {
                    ChangeTurn();
                }
                else
                {
                    BoardManager.Instance.pawnSelected.MarkAttackPawn();
                }
                BoardManager.Instance.uiManager.UIChange();
                break;
            case PlayTurnState.idle:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Funzione che controlla se è possibile passare dallo stato del turno attuale a newState, ritorna true se è possibile altrimenti false
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    bool StateChange(PlayTurnState newState)
    {
        switch (newState)
        {
            case PlayTurnState.choosing:
                if (CurrentTurnState != PlayTurnState.choosing && CurrentTurnState != PlayTurnState.check && CurrentTurnState != PlayTurnState.animation)
                    return false;
                return true;
            case PlayTurnState.placing:
                if (CurrentTurnState != PlayTurnState.choosing && CurrentTurnState != PlayTurnState.animation)
                    return false;
                return true;
            case PlayTurnState.animation:
                return true;
            case PlayTurnState.check:
                return true;
            case PlayTurnState.movementattack:
                if (CurrentTurnState != PlayTurnState.check && CurrentTurnState != PlayTurnState.animation)
                    return false;
                return true;
            case PlayTurnState.attack:
                if (CurrentTurnState != PlayTurnState.movementattack && CurrentTurnState != PlayTurnState.animation)
                    return false;
                return true;
            case PlayTurnState.idle:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Funzione che viene chiamata quando si entra in un nuova fase della partita ed esegue le funzioni necessarie a quella fase
    /// </summary>
    /// <param name="newPhase"></param>
    void OnMacroPhaseStart(MacroPhase newPhase)
    {
        switch (newPhase)
        {
            case MacroPhase.menu:
                CustomLogger.Log("Sei nella fase di menu");
                break;
            case MacroPhase.faction:
                draftCam.enabled = true;
                mainCam.enabled = false;
                CustomLogger.Log("Sei nella fase di scelta fazione");
                break;
            case MacroPhase.draft:                
                CurrentPlayerTurn = BoardManager.Instance.p1Faction;
                BoardManager.Instance.tutorial.StartDraftTutorial();
                break;
            case MacroPhase.placing:
                Debug.Log("Sei nella fase di posizionamento");
                centralLight.enabled = false;
                magicPlacementLight.enabled = true;
                sciencePlacementLight.enabled = true;
                mainCam.GetComponent<Animator>().SetTrigger("StartGame");
                CurrentTurnState = PlayTurnState.choosing;
                CurrentPlayerTurn = BoardManager.Instance.p1Faction;               
                break;
            case MacroPhase.game:
                Debug.Log("Start Game");
                magicPlacementLight.enabled = false;
                sciencePlacementLight.enabled = false;
                centralLight.enabled = true;
                CurrentPlayerTurn = BoardManager.Instance.p1Faction;
                break;
            case MacroPhase.end:
                Debug.Log("Partita Finita");
                break;
            default:
                break;
        }
        BoardManager.Instance.uiManager.UIChange();
    }

    /// <summary>
    /// Funzione che controlla se è possibile passare dalla macro fase attuale a newPhase, ritorna true se è possibile altrimenti false
    /// </summary>
    /// <param name="newPhase"></param>
    /// <returns></returns>
    bool MacroPhaseChange(MacroPhase newPhase)
    {
        switch (newPhase)
        {
            case MacroPhase.menu:
                return true;
            case MacroPhase.faction:
                if (CurrentMacroPhase != MacroPhase.menu)
                    return false;
                return true;
            case MacroPhase.draft:
                if (CurrentMacroPhase != MacroPhase.faction)
                    return false;
                return true;
            case MacroPhase.placing:
                if (CurrentMacroPhase != MacroPhase.draft)
                    return false;
                return true;
            case MacroPhase.game:
                if (CurrentMacroPhase != MacroPhase.placing)
                    return false;
                return true;
            case MacroPhase.end:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Funzione che viene chiamata quando inizia un nuobo turno
    /// </summary>
    /// <param name="newTurn"></param>
    void OnTurnStart(Factions newTurn)
    {
        BoardManager.Instance.uiManager.UpdateExpressions(Expressions.On);
        switch (CurrentMacroPhase)
        {
            case MacroPhase.menu:
                break;
            case MacroPhase.faction:
                break;
            case MacroPhase.draft:
                if (BoardManager.Instance.draftManager.hasDrafted)
                {
                    if (BoardManager.Instance.draftManager.DraftPawns.Count == 0 && BoardManager.Instance.draftManager.draftEnd && BoardManager.Instance.draftManager.p1StartPressed && BoardManager.Instance.draftManager.p2StartPressed)
                    {
                        StartCoroutine(StartPlacingDelay());
                    }
                    else if (BoardManager.Instance.draftManager.DraftPawns.Count>0)
                    {
                        BoardManager.Instance.draftManager.SelectNextDraftPawn(Directions.idle);
                        BoardManager.Instance.tutorial.DraftTutorial();
                    }
                }                
                break;
            case MacroPhase.placing:
                switch (CurrentTurnState)
                {
                    case PlayTurnState.choosing:
                        if ((BoardManager.Instance.tutorial.ChoosingTutorialDone && BoardManager.Instance.tutorial.TutorialActive) || !BoardManager.Instance.tutorial.TutorialActive)
                        {
                            if (!BoardManager.Instance.CheckPawnToChoose())
                                CurrentTurnState = PlayTurnState.placing;
                            else
                            {
                                BoardManager.Instance.SetPawnToChoose(true);
                            }
                        }
                        else
                        {
                            BoardManager.Instance.tutorial.ChoosingTutorial();
                        }
                        break;
                    case PlayTurnState.placing:
                        if (BoardManager.Instance.pawnsToPlace == 0)
                        {
                            BoardManager.Instance.uiManager.placingUI.SetActive(false);
                            BoardManager.Instance.uiManager.gameUI.SetActive(true);
                            CurrentMacroPhase = MacroPhase.game;
                        }
                        else
                        {
                            switch (CurrentPlayerTurn)
                            {
                                case Factions.Magic:
                                    magicPlacementLight.enabled = true;
                                    sciencePlacementLight.enabled = false;
                                    break;
                                case Factions.Science:
                                    sciencePlacementLight.enabled = true;
                                    magicPlacementLight.enabled = false;
                                    break;
                            }
                            BoardManager.Instance.SelectNextPawnToPlace(Directions.idle);
                            BoardManager.Instance.tutorial.PlacingTutorial();
                        }
                        break;
                }
                break;
            case MacroPhase.game:
                CheckAlreadyDone = false;
                CurrentTurnState = PlayTurnState.check;
                numberOfTurns++;
                turnsWithoutAttack++;
                if (!BoardManager.Instance.tutorial.GameTutorialDone)
                {
                    BoardManager.Instance.tutorial.GameTutorial();
                }
                break;
            default:
                Debug.Log("Errore: nessuna macrofase");
                break;
        }
        BoardManager.Instance.uiManager.UIChange();
    }

    /// <summary>
    /// Funzione che viene chiamata alla fine del turno
    /// </summary>
    void OnTurnEnd(Factions endTurn)
    {
        if (CurrentMacroPhase == MacroPhase.game)
        {
            BoardManager.Instance.uiManager.UpdateExpressions(Expressions.Off);
        }

        if (turnsWithoutAttack >= 20)
        {
            Debug.Log("PASSATI 8 TURNI");
            BoardManager.Instance.WinCondition(true);
        }
    }

    /// <summary>
    /// Funzione che fa iniziare la placing phase triggerando il fade dalla fase di draft a quella di placing
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartPlacingDelay()
    {
        BoardManager.Instance.uiManager.fadeinoutmenu.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        draftCam.enabled = false;
        mainCam.enabled = true;
        BoardManager.Instance.vfx.DeselectDraftPawn();
        BoardManager.Instance.SetPawnsPattern();
        BoardManager.Instance.uiManager.draftUI.SetActive(false);
        BoardManager.Instance.uiManager.choosingUi.SetActive(true);
        yield return null;
        CurrentMacroPhase = MacroPhase.placing;
    }

    /// <summary>
    /// Funzione che cambia turno
    /// </summary>
    public void ChangeTurn()
    {
        if ((CurrentMacroPhase == MacroPhase.draft || CurrentMacroPhase == MacroPhase.placing) || (CurrentMacroPhase == MacroPhase.game && (CurrentTurnState != PlayTurnState.check && CurrentTurnState != PlayTurnState.choosing)))
        {            
            if (CurrentPlayerTurn == Factions.Magic)
                CurrentPlayerTurn = Factions.Science;
            else if (CurrentPlayerTurn == Factions.Science)
                CurrentPlayerTurn = Factions.Magic;            
        }
    }

}
