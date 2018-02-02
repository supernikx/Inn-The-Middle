using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour {

    /// <summary> Stato per indicare di chi è il turno </summary>
    public enum PlayerTurn { P1_turn, P2_turn };
    /// <summary> PlayerTurn corrente </summary>
    public PlayerTurn playerTurn;
    /// <summary> Stato per indicare la fase corrente del macroturno PlayTurn </summary>
    public enum PlayTurnState { movement, attack };
    /// <summary> PlayTurnState corrente </summary>
    public PlayTurnState currentTurnState;

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
    

    // Use this for initialization
    void Start()
    {
        playerTurn = PlayerTurn.P1_turn;



    }

    // Update is called once per frame
    void Update()
    {
        
        TurnCheckText();
        PhaseCheckText();

        //da usare una volta implementata la fase strategica
        /*if (P2PawnsLeft > 0 && strategicTurn)
        {
            StrategyTurn();
        }
        else if (P1PawnsLeft <= 0 && P2PawnsLeft <= 0)
        {
            strategicTurn = false;
            PlayTurn();
        }*/
    }

    public void TurnCheckText()
    {
        if (playerTurn == PlayerTurn.P1_turn)
        {
            p1text.enabled = true;
            p2text.enabled = false;
        }
        else if (playerTurn == PlayerTurn.P2_turn)
        {
            p2text.enabled = true;
            p1text.enabled = false;
        }
    }

    public void PhaseCheckText()
    {
        if (playerTurn == PlayerTurn.P1_turn)
        {
            p1phase.enabled = true;
            p2phase.enabled = false;
            if (currentTurnState == PlayTurnState.movement)
            {
                p1phase.text = "Movement phase";
            }
            else if (currentTurnState == PlayTurnState.attack)
            {
                p1phase.text = "Attack phase";
            }
        }
        else if (playerTurn == PlayerTurn.P2_turn)
        {
            p2phase.enabled = true;
            p1phase.enabled = false;
            if (currentTurnState == PlayTurnState.movement)
            {
                p2phase.text = "Movement phase";
            }
            else if (currentTurnState == PlayTurnState.attack)
            {
                p2phase.text = "Attack phase";
            }
        }
    }



   /* /// <summary> Funzione del macroturno di gioco con fase di selezione, movimento e attacco delle pedine </summary>
    void PlayTurn()
    {
        if (pawnScript != null)
        {
            if (!strategicTurn)
            {
                // Inizio macroturno di gioco con stato di selezione, movimento e attacco

                if (playerTurn == PlayerTurn.P1_turn && pawnScript.player == Player.player1)
                {

                    if (currentTurnState == PlayTurnState.movement)
                    {
                        //muovi pedina selezionata
                        currentTurnState = PlayTurnState.attack;
                    }
                    if (currentTurnState == PlayTurnState.attack)
                    {
                        //attacca e passa turno
                        playerTurn = PlayerTurn.P2_turn;
                    }
                }


                if (playerTurn == PlayerTurn.P2_turn && pawnScript.player == Player.player2)
                {

                    if (currentTurnState == PlayTurnState.movement)
                    {
                        //muovi pedina selezionata
                        currentTurnState = PlayTurnState.attack;
                    }
                    if (currentTurnState == PlayTurnState.attack)
                    {
                        //attacca e passa turno
                        playerTurn = PlayerTurn.P2_turn;
                    }
                }

            }
        }
    }

    /// <summary> Funzione del macroturno strategico con turni dei player e posizionamento pedine </summary>
    void StrategyTurn()
    {
        if (playerTurn == PlayerTurn.P1_turn)
        {
            // Selezione di una pedina (su 4) da posizionare sulla griglia
            P1PawnsLeft--;
            playerTurn = PlayerTurn.P2_turn;
        }
        else if (playerTurn == PlayerTurn.P2_turn)
        {
            // Selezione di una pedina (su 4) da posizionare sulla griglia
            P2PawnsLeft--;
            playerTurn = PlayerTurn.P1_turn;
        }
    }*/
}
