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


    public enum MacroPhase { draft, placing, game };
    public MacroPhase CurrentMacroPhase;


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

    DraftManager dm;

    /// <summary> Testo per indicare di chi è il turno </summary>
    [Header("Text references")]
    public TextMeshProUGUI p1text, p2text;
    public TextMeshProUGUI p1phase, p2phase;

    [Header("Button references")]
    public GameObject skipAttackButton;
    public GameObject skipMovementButton;
    public GameObject attackButton;
    public GameObject superAttackButton;

    [Header("Camera references")]
    public Camera mainCam;
    public Camera draftCam;

    [Header("UI Holders references")]
    public GameObject draftUI;
    public GameObject gameUI;

    // Use this for initialization
    void Start()
    {
        mainCam.enabled = false;
        gameUI.SetActive(false);

        dm = FindObjectOfType<DraftManager>();

        CurrentPlayerTurn = PlayerTurn.P1_turn;
        CurrentMacroPhase = MacroPhase.draft;
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

    void OnTurnStart(PlayerTurn newTurn)
    {
        switch (CurrentMacroPhase)
        {
            case MacroPhase.draft:
                if (dm.pawns.Count == 0)
                {
                    draftCam.enabled = false;
                    mainCam.enabled = true;
                    draftUI.SetActive(false);
                    gameUI.SetActive(true);
                    BoardManager.Instance.SetPawnsPattern();
                    BoardManager.Instance.SetPawnsPlayer();
                    CurrentMacroPhase = MacroPhase.game;
                }
                break;
            case MacroPhase.placing:
                //if (pawns to place == 0)
                //{
                //     CurrentMacroPhase = MacroPhase.game;
                //}
            case MacroPhase.game:
                CurrentTurnState = PlayTurnState.check;
                break;
            default:
                Debug.Log("Errore: nessuna macrofase");
                break;
        }
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
                superAttackButton.SetActive(false);
            }
            else if (CurrentTurnState == PlayTurnState.movement)
            {
                p1phase.text = "Movement phase";
                skipMovementButton.SetActive(true);
                skipAttackButton.SetActive(false);
                attackButton.SetActive(false);
                superAttackButton.SetActive(false);
            }
            else if (CurrentTurnState == PlayTurnState.attack)
            {
                p1phase.text = "Attack phase";
                skipAttackButton.SetActive(true);
                attackButton.SetActive(true);
                superAttackButton.SetActive(true);
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
                superAttackButton.SetActive(false);
            }
            else if (CurrentTurnState == PlayTurnState.movement)
            {
                p2phase.text = "Movement phase";
                skipMovementButton.SetActive(true);
                skipAttackButton.SetActive(false);
                attackButton.SetActive(false);
                superAttackButton.SetActive(false);
            }
            else if (CurrentTurnState == PlayTurnState.attack)
            {
                p2phase.text = "Attack phase";
                skipAttackButton.SetActive(true);
                attackButton.SetActive(true);
                superAttackButton.SetActive(true);
                skipMovementButton.SetActive(false);
            }
        }
    }
}
