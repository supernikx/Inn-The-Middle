﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour
{

    /// <summary> Stato per indicare di chi è il turno </summary>
    public enum PlayerTurn { P1_turn = 1, P2_turn = 2 };
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


    public enum MacroPhase { menu, faction, draft, placing, game };
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
    public enum PlayTurnState { choosing, placing, animation, check, movementattack, attack };
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
        //CurrentPlayerTurn = PlayerTurn.P1_turn;
        CurrentMacroPhase = MacroPhase.menu;
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
            default:
                return false;
        }
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
            default:
                return false;
        }
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
                CurrentPlayerTurn = (PlayerTurn)BoardManager.Instance.factionID;
                //CurrentPlayerTurn = PlayerTurn.P1_turn;
                break;
            case PlayTurnState.animation:
                break;
            case PlayTurnState.check:
                BoardManager.Instance.superAttack = false;
                BoardManager.Instance.uiManager.UIChange();                            
                BoardManager.Instance.UnmarkAttackMarker();
                turnsWithoutAttack++;
                if (BoardManager.Instance.pawnSelected != null)
                {
                    BoardManager.Instance.DeselectPawn();
                }
                BoardManager.Instance.CheckPhaseControll();
                break;
            case PlayTurnState.movementattack:
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
                break;
            default:
                break;
        }
        BoardManager.Instance.uiManager.UIChange();
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
                CustomLogger.Log("Sei nella fase di scelta fazione");
                break;
            case MacroPhase.draft:
                CurrentPlayerTurn = (PlayerTurn)BoardManager.Instance.factionID;
                break;
            case MacroPhase.placing:
                CurrentPlayerTurn = (PlayerTurn)BoardManager.Instance.factionID;
                //CurrentPlayerTurn = PlayerTurn.P1_turn;
                break;
            case MacroPhase.game:
                CurrentPlayerTurn = (PlayerTurn)BoardManager.Instance.factionID;
                //CurrentPlayerTurn = PlayerTurn.P1_turn;
                break;
            default:
                break;
        }
        BoardManager.Instance.uiManager.UIChange();
    }

    /// <summary>
    /// Funzione che viene chiamata quando inizia un nuobo turno
    /// </summary>
    /// <param name="newTurn"></param>
    void OnTurnStart(PlayerTurn newTurn)
    {
        switch (CurrentMacroPhase)
        {
            case MacroPhase.menu:
                CustomLogger.Log("Sei nella fase di menu");
                break;
            case MacroPhase.faction:
                break;
            case MacroPhase.draft:
                //       if (BoardManager.Instance.factionChosen == true)
                //       {
                //           BoardManager.Instance.uiManager.factionUI.SetActive(false);
                //           BoardManager.Instance.uiManager.draftUI.SetActive(true);
                //       }
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
                            BoardManager.Instance.uiManager.gameUIPerspective.SetActive(true);
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
        if (CurrentPlayerTurn == PlayerTurn.P1_turn)
            CurrentPlayerTurn = PlayerTurn.P2_turn;
        else if (CurrentPlayerTurn == PlayerTurn.P2_turn)
            CurrentPlayerTurn = PlayerTurn.P1_turn;
    }

}
