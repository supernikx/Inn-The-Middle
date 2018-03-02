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


    public enum MacroPhase { menu, draft, placing, game };
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
    public enum PlayTurnState {choosing, placing, check, movement, attack};
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

    // Use this for initialization
    void Start()
    {
        mainCam.enabled = false;
        CurrentPlayerTurn = PlayerTurn.P1_turn;
        CurrentMacroPhase = MacroPhase.menu;
    }

    bool StateChange(PlayTurnState newState)
    {
        switch (newState)
        {
            case PlayTurnState.choosing:
                if (CurrentTurnState != PlayTurnState.choosing && CurrentTurnState!=PlayTurnState.check)
                    return false;
                return true;
            case PlayTurnState.placing:
                if (CurrentTurnState != PlayTurnState.choosing)
                    return false;
                return true;
            case PlayTurnState.check:
                if (CurrentTurnState == PlayTurnState.movement)
                    return false;
                return true;
            case PlayTurnState.movement:
                if (CurrentTurnState != PlayTurnState.check)
                    return false;
                return true;
            case PlayTurnState.attack:
                if (CurrentTurnState != PlayTurnState.movement)
                    return false;
                return true;
            default:
                return false;
        }
    }

    bool MacroPhaseChange(MacroPhase newPhase)
    {
        switch (newPhase)
        {
            case MacroPhase.menu:
                return true;
            case MacroPhase.draft:
                if (CurrentMacroPhase != MacroPhase.menu)
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
        switch (newState)
        {
            case PlayTurnState.choosing:
                break;
            case PlayTurnState.placing:
                BoardManager.Instance.uiManager.choosingUi.SetActive(false);
                BoardManager.Instance.uiManager.placingUI.SetActive(true);
                CurrentPlayerTurn = PlayerTurn.P1_turn;
                break;
            case PlayTurnState.check:
                turnsWithoutAttack++;
                BoardManager.Instance.movementSkipped = false;
                BoardManager.Instance.superAttackPressed = false;
                BoardManager.Instance.UnmarkKillPawns();
                if (BoardManager.Instance.pawnSelected != null)
                {
                    BoardManager.Instance.pawnSelected.DisableAttackPattern();
                }
                BoardManager.Instance.DeselectPawn();
                BoardManager.Instance.CheckPhaseControll();
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
        BoardManager.Instance.uiManager.UIChange();
    }

    void OnMacroPhaseStart(MacroPhase newPhase)
    {
        switch (newPhase)
        {
            case MacroPhase.menu:
                CustomLogger.Log("Sei nella fase di menu");
                break;
            case MacroPhase.draft:
                break;
            case MacroPhase.placing:
                CurrentPlayerTurn = PlayerTurn.P1_turn;
                break;
            case MacroPhase.game:
                CurrentPlayerTurn = PlayerTurn.P1_turn;
                break;
            default:
                break;
        }
        BoardManager.Instance.uiManager.UIChange();
    }

    void OnTurnStart(PlayerTurn newTurn)
    {
        switch (CurrentMacroPhase)
        {
            case MacroPhase.menu:
                CustomLogger.Log("Sei nella fase di menu");
                break;
            case MacroPhase.draft:
                if (BoardManager.Instance.draftManager.pawns.Count == 0)
                {
                    draftCam.enabled = false;
                    mainCam.enabled = true;
                    BoardManager.Instance.SetPawnsPattern();
                    BoardManager.Instance.uiManager.draftUI.SetActive(false);
                    BoardManager.Instance.uiManager.choosingUi.SetActive(true);
                    CurrentMacroPhase = MacroPhase.placing;
                }
                break;
            case MacroPhase.placing:
                switch (CurrentTurnState)
                {
                    case PlayTurnState.choosing:
                        if (!BoardManager.Instance.CheckPawnToChoose())
                            CurrentTurnState = PlayTurnState.placing;
                        else
                        {
                            BoardManager.Instance.SetPawnToChoose();
                        }
                        break;
                    case PlayTurnState.placing:
                        if (BoardManager.Instance.pawnsToPlace == 0)
                        {
                            BoardManager.Instance.uiManager.placingUI.SetActive(false);
                            BoardManager.Instance.uiManager.gameUI.SetActive(true);
                            CurrentMacroPhase = MacroPhase.game;
                        }
                        break;
                }
                break;
            case MacroPhase.game:
                CurrentTurnState = PlayTurnState.check;
                numberOfTurns++;
                if (turnsWithoutAttack >= 8)
                {
                    Debug.Log("PASSATI 8 TURNI");
                    BoardManager.Instance.WinCondition();
                }
                break;
            default:
                Debug.Log("Errore: nessuna macrofase");
                break;
        }
        BoardManager.Instance.uiManager.UIChange();
    }

    /// <summary>
    /// Funzione che cambia turno
    /// </summary>
    public void ChangeTurn()
    {
        if (CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
            CurrentPlayerTurn = TurnManager.PlayerTurn.P2_turn;
        else if (CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
            CurrentPlayerTurn = TurnManager.PlayerTurn.P1_turn;
    }

}
