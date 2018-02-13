using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;


public class BoardManager : MonoBehaviour
{

    public static BoardManager Instance;

    //variabili pubbliche
    public Transform[][] board1, board2;
    public List<Pawn> pawns;
    public Pawn pawnSelected;
    public bool movementSkipped;

    //variabili private
    private TurnManager turnManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }

    }

    private void Update()
    {
        if ((turnManager.CurrentTurnState == TurnManager.PlayTurnState.movement || turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack) && pawnSelected != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                pawnSelected.ChangePatternSide();
                pawnSelected.DisableAttackPattern();
                pawnSelected.ShowAttackPattern();
                if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.movement)
                {
                    pawnSelected.ShowMovementBoxes();
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        movementSkipped = false;
        pawns = FindObjectsOfType<Pawn>().ToList();
        turnManager = FindObjectOfType<TurnManager>();
        SetPawnsPlayer();
    }

    /// <summary>
    /// Funzione che obbliga il giocatore a muoversi durante la fase di check non deselezionando mai la pedina finchè non si è mossa in una delle caselle disponibili
    /// </summary>
    /// <param name="boxclicked"></param>
    private void MovementCheckPhase(Box boxclicked)
    {
        if (CheckFreeBox(boxclicked))
        {
            if (pawnSelected.player == Player.player1 && boxclicked.board == 1 && turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
            {
                if (pawnSelected.Move(boxclicked.index1, boxclicked.index2))
                {
                    CustomLogger.Log(pawnSelected.player + " si è mosso");
                    DeselectPawn();
                    CheckBox();
                }
                else
                {
                    CustomLogger.Log("Casella non valida");
                }
            }
            else if (pawnSelected.player == Player.player2 && boxclicked.board == 2 && turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
            {
                if (pawnSelected.Move(boxclicked.index1, boxclicked.index2))
                {
                    CustomLogger.Log(pawnSelected.player + " si è mosso");
                    DeselectPawn();
                    CheckBox();
                }
                else
                {
                    CustomLogger.Log("Casella non valida");
                }
            }
            else
            {
                CustomLogger.Log("Casella non valida");
            }
        }
        else
        {
            CustomLogger.Log("Casella non valida");
        }
    }

    /// <summary>
    /// Funzione che passandogli la casella cliccata fa i primi controlli relativi al turno e alla fase del turno per poi passare i dati della casella alla funzione Movement della classe pawn
    /// se il movimento va a buon fine cambia la fase del turno da movimento ad attacco
    /// </summary>
    /// <param name="boxclicked"></param>
    private void Movement(Box boxclicked)
    {
        if (CheckFreeBox(boxclicked))
        {
            if (pawnSelected.player == Player.player1 && boxclicked.board == 1 && turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
            {
                if (pawnSelected.Move(boxclicked.index1, boxclicked.index2))
                {
                    CustomLogger.Log(pawnSelected.player + " si è mosso");
                    pawnSelected.ShowAttackPattern();
                    turnManager.CurrentTurnState = TurnManager.PlayTurnState.attack;
                }
                else
                {
                    DeselectPawn();
                }
            }
            else if (pawnSelected.player == Player.player2 && boxclicked.board == 2 && turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
            {
                if (pawnSelected.Move(boxclicked.index1, boxclicked.index2))
                {
                    CustomLogger.Log(pawnSelected.player + " si è mosso");
                    pawnSelected.ShowAttackPattern();
                    turnManager.CurrentTurnState = TurnManager.PlayTurnState.attack;
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
        else
        {
            DeselectPawn();
        }
    }

    /// <summary>
    /// Funzione che controlla se la casella che gli è stata passata in input è già occupata da un altro player o se non è walkable
    /// se è libera ritorna true, altrimenti se è occupata ritorna false
    /// </summary>
    /// <param name="boxclicked"></param>
    /// <returns></returns>
    private bool CheckFreeBox(Box boxclicked)
    {
        if (boxclicked.walkable && boxclicked.free)
        {
            return true;
        }
        return false;
    }

    //metodo provvisorio che identifica posizione della pedina finchè non implementiamo il posizionamento delle pedine ai player
    private void SetPawnsPlayer()
    {
        for (int i = 0; i < pawns.Count; i++)
        {
            if (pawns[i].player == Player.player1)
            {
                pawns[i].transform.position = board1[pawns[i].startIndex1][pawns[i].startIndex2].position + pawns[i].offset;
                pawns[i].currentBox = board1[pawns[i].startIndex1][pawns[i].startIndex2].GetComponent<Box>();
                board1[pawns[i].startIndex1][pawns[i].startIndex2].GetComponent<Box>().free = false;
            }
            else if (pawns[i].player == Player.player2)
            {
                pawns[i].transform.position = board2[pawns[i].startIndex1][pawns[i].startIndex2].position + pawns[i].offset;
                pawns[i].currentBox = board2[pawns[i].startIndex1][pawns[i].startIndex2].GetComponent<Box>();
                board2[pawns[i].startIndex1][pawns[i].startIndex2].GetComponent<Box>().free = false;
            }
        }
    }
    
    /// <summary>
    /// Funzione che toglie il marchio di Kill a tutte le pedine
    /// </summary>
    private void UnmarkKillPawns()
    {
        foreach (Pawn p in pawns)
        {
            if (p.killMarker)
                p.killMarker = false;
        }
    }

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che richiama la funzione Attack della pawnselected e se avviene l'attacco passa il turno
    /// </summary>
    /// <param name="boxclicked"></param>
    public void Attack()
    {
        if (pawnSelected != null)
        {
            if (pawnSelected.Attack())
            {
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                CustomLogger.Log(pawnSelected.player + " ha attaccato");
                if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
                {
                    turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P2_turn;
                }
                else if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
                {
                    turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P1_turn;
                }
            }
        }
    }

    /// <summary>
    /// Funzione che richiama la funzione SuperAttack della pawnselected e se avviene l'attacco passa il turno
    /// </summary>
    /// <param name="boxclicked"></param>
    public void SuperAttack()
    {
        if (pawnSelected != null)
        {
            if (pawnSelected.SuperAttack())
            {
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                CustomLogger.Log(pawnSelected.player + " ha attaccato");
                if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
                {
                    turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P2_turn;
                }
                else if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
                {
                    turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P1_turn;
                }
            }
        }
    }

    /// <summary>
    /// Funzione che imposta la variabile pawnSelected a null, prima reimposta il colore della pedina a quello di default e imposta a false il bool selected
    /// </summary>
    public void DeselectPawn()
    {
        if (pawnSelected != null)
        {
            pawnSelected.GetComponent<Outline>().eraseRenderer = true;
            pawnSelected.DisableMovementBoxes();
            pawnSelected.DisableAttackPattern();
            pawnSelected.selected = false;
            pawnSelected = null;
        }
    }

    /// <summary>
    /// Controlla se una pedina si trova su una casella non walkable la obbliga a muoversi
    /// </summary>
    public void CheckBox()
    {
        if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.check)
        {
            for (int i = 0; i < pawns.Count; i++)
            {
                if (!pawns[i].currentBox.walkable)
                {
                    CustomLogger.Log(pawns[i] + " è in casella !walkable");
                    pawns[i].RandomizePattern();
                    PawnSelected(pawns[i]);
                    return;
                }
            }
            DeselectPawn();
            turnManager.CurrentTurnState = TurnManager.PlayTurnState.movement;
        }
    }

    /// <summary>
    /// Funzione che imposta nella variabile pawnSelected l'oggetto Pawn passato in input, solo se la pedina selezionata appartiene al giocatore del turno in corso e se la fase del turno e quella di movimento
    /// prima di impostarla chiama la funzione DeselectPawn per resettare l'oggetto pawnSelected precedente
    /// </summary>
    /// <param name="selected"></param>
    public void PawnSelected(Pawn selected)
    {
        if (pawnSelected == null && turnManager.CurrentTurnState == TurnManager.PlayTurnState.check)
        {
            selected.selected = true;
            pawnSelected = selected;
            pawnSelected.GetComponent<Outline>().eraseRenderer = false;
            pawnSelected.ShowMovementBoxes();
        }
        else if ((turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn && selected.player == Player.player1 || turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn && selected.player == Player.player2) && movementSkipped && turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack)
        {
            if (pawnSelected != null)
            {
                UnmarkKillPawns();
                DeselectPawn();
            }
            selected.selected = true;
            pawnSelected = selected;
            pawnSelected.GetComponent<Outline>().eraseRenderer = false;
            pawnSelected.ShowAttackPattern();
        }
        else if ((turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn && selected.player == Player.player1 || turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn && selected.player == Player.player2) && turnManager.CurrentTurnState == TurnManager.PlayTurnState.movement)
        {
            if (pawnSelected != null)
            {
                DeselectPawn();
            }
            selected.selected = true;
            pawnSelected = selected;
            pawnSelected.GetComponent<Outline>().eraseRenderer = false;
            pawnSelected.ShowAttackPattern();
            pawnSelected.ShowMovementBoxes();
        }
    }

    /// <summary>
    /// Funzione che prende in input una pedina con il bool killMarker=true e la uccide
    /// </summary>
    /// <param name="pawnToKill"></param>
    public void KillPawnMarked(Pawn pawnToKill)
    {
        pawnSelected.KillPawn(pawnToKill);
        UnmarkKillPawns();
        CustomLogger.Log(pawnSelected.player + " ha attaccato");
        if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
        {
            turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P2_turn;
        }
        else if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
        {
            turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P1_turn;
        }
    }

    /// <summary>
    /// Funzione che salta la fase d'attacco del player corrente e passa il turno
    /// </summary>
    public void PassTurn()
    {
        if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.movement)
        {
            DeselectPawn();
            movementSkipped = true;
            turnManager.CurrentTurnState = TurnManager.PlayTurnState.attack;
            CustomLogger.Log("Il player ha saltato il movimento");
        }
        else if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack)
        {
            if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
            {
                CustomLogger.Log("Player 1 ha saltato l'attacco");
                turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P2_turn;
            }
            else if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
            {
                CustomLogger.Log("Player 2 ha saltato l'attacco");
                turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P1_turn;
            }
        }

    }

    /// <summary>
    /// Funzione che viene chiamata quando si clicca una casella e la si riceve in input, si controlla che fase del turno è e si passano le informazioni della casella alle funzioni interessate
    /// </summary>
    /// <param name="boxclicked"></param>
    public void BoxClicked(Box boxclicked)
    {
        if (pawnSelected != null)
        {
            if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.movement)
            {
                Movement(boxclicked);
            }
            else if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack)
            {
                CustomLogger.Log("Clicca il pulsante Attack se c'è una pedina in range");
            }
            else if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.check)
            {
                MovementCheckPhase(boxclicked);
            }
            else
            {
                DeselectPawn();
            }
        }
    }

    #endregion
}

//enumeratore che contiene i player possibili
public enum Player
{
    player1, player2
}
