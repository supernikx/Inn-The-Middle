using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
                OnStateEnter(_currentTurnState);
            }
        }
    }

    [HideInInspector]
    /// <summary> Booleana che indica se il macroturno StrategicTurn è ancora attivo o no (temporaneamente false in assenza di fase strategica)</summary>
    public bool strategicTurn = false;

    [HideInInspector]
    /// <summary> Numero massimo di pedine per giocatore </summary>
    public int P1Pawns, P2Pawns;
    [HideInInspector]
    /// <summary> Numero di pedine rimanenti per giocatore </summary>
    public int P1PawnsLeft, P2PawnsLeft;

    /// <summary> Testo per indicare di chi è il turno </summary>
    
    public TextMeshProUGUI p1text, p2text;
    public TextMeshProUGUI p1phase, p2phase;

    Pawn pawnScript;

    public GameObject skipAttackButton;
    public GameObject skipMovementButton;
    public GameObject attackButton;


    // Use this for initialization
    void Start()
    {
        CurrentPlayerTurn = PlayerTurn.P1_turn;
    }

    // Update is called once per frame
    void Update()
    {
        TurnCheckText();
        PhaseCheckText();
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

    void OnStateEnter(PlayTurnState newState)
    {
        switch (newState)
        {
            case PlayTurnState.check:
                if (BoardManager.Instance.pawnSelected != null)
                {
                    BoardManager.Instance.pawnSelected.DisableAttackPattern();
                }
                BoardManager.Instance.DeselectPawn();
                BoardManager.Instance.CheckBox();

                break;
            case PlayTurnState.movement:
                BoardManager.Instance.movementSkipped = false;
                break;
            case PlayTurnState.attack:

                break;
            default:
                break;
        }
    }

    void OnTurnStart(PlayerTurn newTurn)
    {
        CurrentTurnState = PlayTurnState.check;
    }

    public void TurnCheckText()
    {
        if (CurrentPlayerTurn == PlayerTurn.P1_turn)
        {
            p1text.enabled = true;
            p2text.enabled = false;
        }
        else if (CurrentPlayerTurn == PlayerTurn.P2_turn)
        {
            p2text.enabled = true;
            p1text.enabled = false;
        }
    }

    public void PhaseCheckText()
    {
        if (CurrentPlayerTurn == PlayerTurn.P1_turn)
        {
            p1phase.enabled = true;
            p2phase.enabled = false;
            if (CurrentTurnState == PlayTurnState.check)
            {
                p1phase.text = "Check phase";
                skipAttackButton.SetActive(false);
                skipMovementButton.SetActive(false);
                attackButton.SetActive(false);

            }
            else if (CurrentTurnState == PlayTurnState.movement)
            {
                p1phase.text = "Movement phase";
                skipMovementButton.SetActive(true);
                skipAttackButton.SetActive(false);
                attackButton.SetActive(false);
            }
            else if (CurrentTurnState == PlayTurnState.attack)
            {
                p1phase.text = "Attack phase";
                skipAttackButton.SetActive(true);
                attackButton.SetActive(true);
                skipMovementButton.SetActive(false);
            }
        }
        else if (CurrentPlayerTurn == PlayerTurn.P2_turn)
        {
            p2phase.enabled = true;
            p1phase.enabled = false;
            if (CurrentTurnState == PlayTurnState.check)
            {
                p2phase.text = "Check phase";
                skipAttackButton.SetActive(false);
                skipMovementButton.SetActive(false);
                attackButton.SetActive(false);
            }
            else if (CurrentTurnState == PlayTurnState.movement)
            {
                p2phase.text = "Movement phase";
                skipMovementButton.SetActive(true);
                skipAttackButton.SetActive(false);
                attackButton.SetActive(false);
            }
            else if (CurrentTurnState == PlayTurnState.attack)
            {
                p2phase.text = "Attack phase";
                skipAttackButton.SetActive(true);
                attackButton.SetActive(true);
                skipMovementButton.SetActive(false);
            }
        }
    }
}
