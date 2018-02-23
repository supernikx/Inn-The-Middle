using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    TurnManager tm;
    BoardManager bm;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    bool isPaused;

    /// <summary> Testo per indicare di chi è il turno </summary>
    [Header("Turn Text")]
    public TextMeshProUGUI p1text;
    public TextMeshProUGUI p2text;
    public GameObject p1PickingText;
    public GameObject p2PickingText;

    [Header("Phase Text")]
    public TextMeshProUGUI p1phase;
    public TextMeshProUGUI p2phase;

    [Header("Elements Text")]
    public TextMeshProUGUI p1elements;
    public TextMeshProUGUI p2elements;


    [Header("Button references")]
    public GameObject skipAttackButton;
    public GameObject skipMovementButton;
    public GameObject attackButton;
    public GameObject superAttackButton;


    [Header("UI Holders references")]
    public GameObject draftUI;
    public GameObject gameUI;

    private void Awake()
    {
        bm = GetComponent<BoardManager>();
        tm = GetComponent<TurnManager>();
    }
    // Use this for initialization
    void Start()
    {
        gameUI.SetActive(false);
        pausePanel.SetActive(false);
        draftUI.SetActive(true);
        UpdateElementsText();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            ResumeGame();
        }
    }

    /// <summary> Funzione richiamabile per il tasto Resume del menu di pausa </summary>
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void UpdateElementsText()
    {
        p1elements.SetText("<color=purple>" + bm.player1Elements.purpleElement + "</color>-<color=#00ffffff>" + bm.player1Elements.azureElement + "</color>-<color=orange>" + bm.player1Elements.orangeElement);
        p2elements.SetText("<color=purple>" + bm.player2Elements.purpleElement + "</color>-<color=#00ffffff>" + bm.player2Elements.azureElement + "</color>-<color=orange>" + bm.player2Elements.orangeElement);
    }

    public void UIChange()
    {
        switch (tm.CurrentPlayerTurn)
        {
            case TurnManager.PlayerTurn.P2_turn:
                switch (tm.CurrentMacroPhase)
                {
                    case TurnManager.MacroPhase.draft:
                        p1PickingText.SetActive(false);
                        p2PickingText.SetActive(true);
                        break;
                    case TurnManager.MacroPhase.placing:
                        break;
                    case TurnManager.MacroPhase.game:
                        p2text.enabled = true;
                        p1text.enabled = false;
                        p2phase.enabled = true;
                        p1phase.enabled = false;
                        switch (tm.CurrentTurnState)
                        {
                            case TurnManager.PlayTurnState.check:
                                p2phase.text = "Check phase";
                                skipAttackButton.SetActive(false);
                                skipMovementButton.SetActive(false);
                                attackButton.SetActive(false);
                                superAttackButton.SetActive(false);
                                break;
                            case TurnManager.PlayTurnState.movement:
                                p2phase.text = "Movement phase";
                                skipMovementButton.SetActive(true);
                                skipAttackButton.SetActive(false);
                                attackButton.SetActive(false);
                                superAttackButton.SetActive(false);
                                break;
                            case TurnManager.PlayTurnState.attack:
                                p2phase.text = "Attack phase";
                                skipAttackButton.SetActive(true);
                                attackButton.SetActive(true);
                                if (bm.player2Elements.CheckSuperAttack())
                                {
                                    superAttackButton.SetActive(true);
                                }
                                else
                                {
                                    superAttackButton.SetActive(false);
                                }
                                skipMovementButton.SetActive(false);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case TurnManager.PlayerTurn.P1_turn:
                switch (tm.CurrentMacroPhase)
                {
                    case TurnManager.MacroPhase.draft:
                        p1PickingText.SetActive(true);
                        p2PickingText.SetActive(false);
                        break;
                    case TurnManager.MacroPhase.placing:
                        break;
                    case TurnManager.MacroPhase.game:
                        p1text.enabled = true;
                        p2text.enabled = false;
                        p1phase.enabled = true;
                        p2phase.enabled = false;
                        switch (tm.CurrentTurnState)
                        {
                            case TurnManager.PlayTurnState.check:
                                p1phase.text = "Check phase";
                                skipAttackButton.SetActive(false);
                                skipMovementButton.SetActive(false);
                                attackButton.SetActive(false);
                                superAttackButton.SetActive(false);
                                break;
                            case TurnManager.PlayTurnState.movement:
                                p1phase.text = "Movement phase";
                                skipMovementButton.SetActive(true);
                                skipAttackButton.SetActive(false);
                                attackButton.SetActive(false);
                                superAttackButton.SetActive(false);
                                break;
                            case TurnManager.PlayTurnState.attack:
                                p1phase.text = "Attack phase";
                                skipAttackButton.SetActive(true);
                                attackButton.SetActive(true);
                                if (bm.player1Elements.CheckSuperAttack())
                                {
                                    superAttackButton.SetActive(true);
                                }
                                else
                                {
                                    superAttackButton.SetActive(false);
                                }
                                skipMovementButton.SetActive(false);
                                break;
                            default:
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
        UpdateElementsText();
    }
}
