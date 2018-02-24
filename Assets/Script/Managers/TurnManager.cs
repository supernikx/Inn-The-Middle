using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour
{

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
            _currentPlayerTurn = value;
            OnTurnStart(_currentPlayerTurn);
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

    public int numberOfTurns;
    public int turnsWithoutAttack;

    [Header("Camera references")]
    public Camera mainCam;
    public Camera draftCam;

    

    private void Awake()
    {

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
                if (CurrentMacroPhase != MacroPhase.placing)
                    return false;
                return true;
            default:
                return false;
        }
    }

    void OnStateStart(PlayTurnState newState)
    {
        BoardManager.Instance.uiManager.UIChange();
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
        BoardManager.Instance.uiManager.UIChange();
        switch (newPhase)
        {
            case MacroPhase.draft:
                break;
            case MacroPhase.placing:
                CurrentPlayerTurn = PlayerTurn.P1_turn;
                break;
            case MacroPhase.game:

                CurrentPlayerTurn = PlayerTurn.P1_turn;
                CurrentTurnState = PlayTurnState.check;
                break;
            default:
                break;
        }
    }

    void OnTurnStart(PlayerTurn newTurn)
    {
        BoardManager.Instance.uiManager.UIChange();
        switch (CurrentMacroPhase)
        {
            case MacroPhase.draft:
                if (BoardManager.Instance.draftManager.pawns.Count == 0)
                {
                    draftCam.enabled = false;
                    mainCam.enabled = true;
                    BoardManager.Instance.SetPawnsPattern();
                    BoardManager.Instance.uiManager.draftUI.SetActive(false);
                    BoardManager.Instance.uiManager.placingUI.SetActive(true);
                    CurrentMacroPhase = MacroPhase.placing;
                }
                break;
            case MacroPhase.placing:
                if (BoardManager.Instance.pawnsToPlace == 0)
                {
                    BoardManager.Instance.uiManager.placingUI.SetActive(false);
                    BoardManager.Instance.uiManager.gameUI.SetActive(true);
                    CurrentMacroPhase = MacroPhase.game;
                }
                break;
            case MacroPhase.game:
                CurrentTurnState = PlayTurnState.check;
                numberOfTurns++;
                if (turnsWithoutAttack >= 8)
                {
                    Debug.Log("PASSATI 6 TURNI");
                    BoardManager.Instance.WinCondition();
                }
                break;
            default:
                Debug.Log("Errore: nessuna macrofase");
                break;
        }
    }



}
