using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public Transform[][] board1, board2;
    private Pawn[] pawns;
    private Pawn pawnSelected;



    private TurnManager turnManager;

    // Use this for initialization
    void Start () {
        pawns = FindObjectsOfType<Pawn>();
        turnManager = FindObjectOfType<TurnManager>();
        SetPawnsPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        AttackTest();
	}

    public void BoxClicked(Box boxclicked)
    {
        if (pawnSelected != null && boxclicked.walkable == true && turnManager.currentTurnState == TurnManager.PlayTurnState.movement)
        {
            if (turnManager.playerTurn == TurnManager.PlayerTurn.P1_turn)
            {
                if (pawnSelected.player == Player.player1 && boxclicked.board == 1)
                {
                    pawnSelected.Move(boxclicked.index1, boxclicked.index2);
                    turnManager.currentTurnState = TurnManager.PlayTurnState.attack;
                }
                pawnSelected = null;
            }
            else if (turnManager.playerTurn == TurnManager.PlayerTurn.P2_turn)
            {
                if (pawnSelected.player == Player.player2 && boxclicked.board == 2)
                {
                    pawnSelected.Move(boxclicked.index1, boxclicked.index2);
                    turnManager.currentTurnState = TurnManager.PlayTurnState.attack;

                }
                pawnSelected = null;
            }
        }






        //codice originale
        /*if (pawnSelected != null && boxclicked.walkable == true)
        {
            if (pawnSelected.player==Player.player1 && boxclicked.board == 1 || pawnSelected.player == Player.player2 && boxclicked.board == 2)
            {
                pawnSelected.Move(boxclicked.index1, boxclicked.index2);
            }
            pawnSelected = null;
        }*/
    }

    //test attacco
    void AttackTest()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (turnManager.playerTurn == TurnManager.PlayerTurn.P1_turn && turnManager.currentTurnState == TurnManager.PlayTurnState.attack)
            {
                turnManager.playerTurn = TurnManager.PlayerTurn.P2_turn;
                turnManager.currentTurnState = TurnManager.PlayTurnState.movement;
                Debug.Log(pawnSelected.player + " ha attaccato");
            }
            else if (turnManager.playerTurn == TurnManager.PlayerTurn.P2_turn && turnManager.currentTurnState == TurnManager.PlayTurnState.attack)
            {
                turnManager.playerTurn = TurnManager.PlayerTurn.P1_turn;
                turnManager.currentTurnState = TurnManager.PlayTurnState.movement;
                Debug.Log(pawnSelected.player + " ha attaccato");
            }
        } 



    }


    public void PawnSelected(Pawn selected)
    {
        if (pawnSelected != null)
        {
            pawnSelected.selected = false;
        }
        selected.selected = true;
        pawnSelected = selected;
    }

    //metodo provvisorio che identifica posizione della pedina finchè non implementiamo il posizionamento delle pedine ai player
    public void SetPawnsPlayer()
    {
        for (int k=0;k<pawns.Length;k++) {
            for (int i = 0; i < board1.Length; i++)
            {
                for (int j = 0; j < board1.Length; j++)
                {
                    if (Mathf.Approximately(pawns[k].transform.position.x,board1[i][j].position.x) && Mathf.Approximately(pawns[k].transform.position.z,board1[i][j].position.z))
                    {
                        pawns[k].player = Player.player1;
                        pawns[k].currentBox = board1[i][j].GetComponent<Box>();
                        break;
                    }
                    else if (Mathf.Approximately(pawns[k].transform.position.x,board2[i][j].position.x) && Mathf.Approximately(pawns[k].transform.position.z,board2[i][j].position.z))
                    {
                        pawns[k].player = Player.player2;
                        pawns[k].currentBox = board2[i][j].GetComponent<Box>();
                        break;
                    }
                }
            }
        }
    }
}

public enum Player {
    player1,player2
}
