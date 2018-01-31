using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public Transform[][] board1, board2;
    private Pawn[] pawns;
    private Pawn pawnSelected;

    private TurnManager turnManager;

    // Use this for initialization
    void Start()
    {
        pawns = FindObjectsOfType<Pawn>();
        turnManager = FindObjectOfType<TurnManager>();
        SetPawnsPlayer();

    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void PassTurn()
    {
        if (turnManager.currentTurnState == TurnManager.PlayTurnState.attack)
        {
            if (turnManager.playerTurn == TurnManager.PlayerTurn.P1_turn)
            {
                turnManager.playerTurn = TurnManager.PlayerTurn.P2_turn;
                turnManager.currentTurnState = TurnManager.PlayTurnState.movement;
                Debug.Log(pawnSelected.player + " ha saltato l'attacco");
                pawnSelected.DisableAttackPattern();
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                DeselectPawn();
            }
            else if (turnManager.playerTurn == TurnManager.PlayerTurn.P2_turn)
            {
                turnManager.playerTurn = TurnManager.PlayerTurn.P1_turn;
                turnManager.currentTurnState = TurnManager.PlayTurnState.movement;
                Debug.Log(pawnSelected.player + " ha saltato l'attacco");
                pawnSelected.DisableAttackPattern();
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                DeselectPawn();
            }
        }
    }

    public void BoxClicked(Box boxclicked)
    {
        if (pawnSelected != null && boxclicked.walkable == true)
        {
            if (turnManager.currentTurnState == TurnManager.PlayTurnState.movement)
            {
                Movement(boxclicked);
            }
            else if (turnManager.currentTurnState == TurnManager.PlayTurnState.attack)
            {
                Attack(boxclicked);
            }
            else
            {
                DeselectPawn();
            }
        }
        else
        {
            DeselectPawn();
        }
    }

    private void Movement(Box boxclicked)
    {
        if (pawnSelected.player == Player.player1 && boxclicked.board == 1 && turnManager.playerTurn == TurnManager.PlayerTurn.P1_turn)
        {
            if (pawnSelected.Move(boxclicked.index1, boxclicked.index2))
            {
                turnManager.currentTurnState = TurnManager.PlayTurnState.attack;
            }
            else
            {
                DeselectPawn();
            }
        }
        else if (pawnSelected.player == Player.player2 && boxclicked.board == 2 && turnManager.playerTurn == TurnManager.PlayerTurn.P2_turn)
        {
            if (pawnSelected.Move(boxclicked.index1, boxclicked.index2))
            {
                turnManager.currentTurnState = TurnManager.PlayTurnState.attack;
            }
            else
            {
                DeselectPawn();
            }
        }
        else
        {
            DeselectPawn();
        }
    }

    private void Attack(Box boxclicked)
    {
        if (pawnSelected.player == Player.player1 && boxclicked.board == 2 && turnManager.playerTurn == TurnManager.PlayerTurn.P1_turn)
        {
            if (pawnSelected.Attack(boxclicked.index1, boxclicked.index2))
            {
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                turnManager.playerTurn = TurnManager.PlayerTurn.P2_turn;
                turnManager.currentTurnState = TurnManager.PlayTurnState.movement;
                Debug.Log(pawnSelected.player + " ha attaccato");
                DeselectPawn();
            }
        }
        else if (pawnSelected.player == Player.player2 && boxclicked.board == 1 && turnManager.playerTurn == TurnManager.PlayerTurn.P2_turn)
        {
            if (pawnSelected.Attack(boxclicked.index1, boxclicked.index2))
            {
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                turnManager.playerTurn = TurnManager.PlayerTurn.P1_turn;
                turnManager.currentTurnState = TurnManager.PlayTurnState.movement;
                Debug.Log(pawnSelected.player + " ha attaccato");
                DeselectPawn();
            }
        }
    }

    public void PawnSelected(Pawn selected)
    {
        if (turnManager.currentTurnState != TurnManager.PlayTurnState.attack)
        {
            if (turnManager.playerTurn == TurnManager.PlayerTurn.P1_turn && selected.player == Player.player1 || turnManager.playerTurn == TurnManager.PlayerTurn.P2_turn && selected.player == Player.player2)
            {
                if (pawnSelected != null)
                {
                    DeselectPawn();
                }
                selected.selected = true;
                pawnSelected = selected;
                pawnSelected.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    private void DeselectPawn()
    {
        if (pawnSelected != null)
        {
            pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
            pawnSelected.selected = false;
            pawnSelected = null;
        }
    }

    //metodo provvisorio che identifica posizione della pedina finchè non implementiamo il posizionamento delle pedine ai player
    public void SetPawnsPlayer()
    {
        for (int i = 0; i < pawns.Length; i++)
        {
            if (pawns[i].player == Player.player1)
            {
                pawns[i].transform.position = board1[pawns[i].startIndex1][pawns[i].startIndex2].position + pawns[i].offset;
                pawns[i].currentBox = board1[pawns[i].startIndex1][pawns[i].startIndex2].GetComponent<Box>();
            }else if (pawns[i].player == Player.player2)
            {
                pawns[i].transform.position = board2[pawns[i].startIndex1][pawns[i].startIndex2].position + pawns[i].offset;
                pawns[i].currentBox = board2[pawns[i].startIndex1][pawns[i].startIndex2].GetComponent<Box>();
            }
        }
    }
}

public enum Player {
    player1,player2
}
