using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour {

    /// <summary> Stato per indicare di chi è il turno </summary>
    public enum PlayerTurn { P2_turn, P1_turn };
    /// <summary> PlayerTurn corrente </summary>
    private PlayerTurn _currentPlayerTurn;
    public PlayerTurn CurrentPlayerTurn
    {
        get
        {
            return _currentPlayerTurn;
        }
        set
        {
            if (value != _currentPlayerTurn)
            {
                _currentPlayerTurn = value;
                OnTurnStart(_currentPlayerTurn);
            }
        }
    }


    public enum MacroPhase { draft, placing, game };
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
    public enum PlayTurnState { check, movement, attack };
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

    DraftManager dm;
    UIManager ui;

    [Header("Camera references")]
    public Camera mainCam;
    public Camera draftCam;

    private void Awake()
    {
        ui = GetComponent<UIManager>();
        dm = GetComponent<DraftManager>();
    }

    // Use this for initialization
    void Start()
    {
        mainCam.enabled = false;
        CurrentPlayerTurn = PlayerTurn.P1_turn;
        CurrentMacroPhase = MacroPhase.draft;
    }

    // Update is called once per frame
    void Update()
    {

    }


    bool StateChange(PlayTurnState newState)
    {
        switch (newState)
        {
            case PlayTurnState.check:
                if (CurrentTurnState != PlayTurnState.attack && CurrentTurnState != PlayTurnState.check)
                {
                    return false;
                }
                return true;
            case PlayTurnState.movement:
                if (CurrentTurnState != PlayTurnState.check)
                {
                    return false;
                }
                return true;
            case PlayTurnState.attack:
                if (CurrentTurnState != PlayTurnState.movement)
                {
                    return false;
                }
                return true;
            default:
                return false;
        }
    }

    bool MacroPhaseChange(MacroPhase newPhase)
    {
        switch (newPhase)
        {
            case MacroPhase.draft:
                if (CurrentMacroPhase != MacroPhase.draft)
                    return false;
                return true;
            case MacroPhase.placing:
                if (CurrentMacroPhase != MacroPhase.draft)
                    return false;
                return true;
            case MacroPhase.game:
                return true;
            default:
                return false;
        }
    }

    void OnStateStart(PlayTurnState newState)
    {
        ui.UIChange();
        switch (newState)
        {
            case PlayTurnState.check:
                BoardManager.Instance.movementSkipped = false;
                BoardManager.Instance.superAttackPressed = false;
                BoardManager.Instance.UnmarkKillPawns();
                if (BoardManager.Instance.pawnSelected != null)
                {
                    BoardManager.Instance.pawnSelected.DisableAttackPattern();
                }
                BoardManager.Instance.DeselectPawn();
                BoardManager.Instance.CheckBox();

                break;
            case PlayTurnState.movement:   
                break;
            case PlayTurnState.attack:
                if (BoardManager.Instance.pawnSelected != null && !BoardManager.Instance.pawnSelected.CheckAttackPattern())
                {
                    if (CurrentPlayerTurn == PlayerTurn.P1_turn)
                    {
                        CurrentPlayerTurn = PlayerTurn.P2_turn;
                    }
                    else if (CurrentPlayerTurn == PlayerTurn.P2_turn)
                    {
                        CurrentPlayerTurn = PlayerTurn.P1_turn;
                    }
                }
                break;
            default:
                break;
        }
    }

    void OnMacroPhaseStart(MacroPhase newPhase)
    {
        ui.UIChange();
        switch (newPhase)
        {
            case MacroPhase.draft:
                break;
            case MacroPhase.placing:
                break;
            case MacroPhase.game:
                CurrentTurnState = PlayTurnState.check;
                break;
            default:
                break;
        }
    }

    void OnTurnStart(PlayerTurn newTurn)
    {
        ui.UIChange();
        switch (CurrentMacroPhase)
        {
            case MacroPhase.draft:
                if (dm.pawns.Count == 0)
                {
                    draftCam.enabled = false;
                    mainCam.enabled = true;
                    ui.draftUI.SetActive(false);
                    ui.gameUI.SetActive(true);
                    BoardManager.Instance.SetPawnsPattern();
                    
                    CurrentMacroPhase = MacroPhase.placing;
                    //BoardManager.Instance.SetPawnsPlayer();
                }
                    break;
            case MacroPhase.placing:
                break;
            case MacroPhase.game:
                CurrentTurnState = PlayTurnState.check;
                break;
            default:
                Debug.Log("Errore: nessuna macrofase");
                break;
        }
    }



}
