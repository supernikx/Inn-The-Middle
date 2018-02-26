using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using PawnOutlineNameSpace;


public class BoardManager : MonoBehaviour
{

    public static BoardManager Instance;

    //variabili pubbliche
    public Transform[][] board1, board2;
    public PlayerElements player1Elements, player2Elements;
    public List<Pawn> pawns;
    [HideInInspector]
    public Pawn pawnSelected;
    [HideInInspector]
    public bool movementSkipped, superAttackPressed;
    public int pawnsToPlace;
    public int p1pawns, p2pawns;
    public int p1tiles, p2tiles;
    public Box[] boxesArray;
    public int placingsLeft;

    //managers
    [Header("Managers")]
    public TurnManager turnManager;
    public DraftManager draftManager;
    public UIManager uiManager;

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
        draftManager = FindObjectOfType<DraftManager>();
        turnManager = FindObjectOfType<TurnManager>();
        uiManager = FindObjectOfType<UIManager>();
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
        placingsLeft = 1;
        pawnsToPlace = 8;
        movementSkipped = false;
        superAttackPressed = false;
        pawns = FindObjectsOfType<Pawn>().ToList();
        boxesArray = FindObjectsOfType<Box>();

        int i = 0;
        foreach (Pawn pawn in BoardManager.Instance.pawns)
        {
            if (BoardManager.Instance.pawns[i].player == Player.player1)
            {
                BoardManager.Instance.p1pawns++;
                i++;
            }
            else if (BoardManager.Instance.pawns[i].player == Player.player2)
            {
                BoardManager.Instance.p2pawns++;
                i++;
            }
        }



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

    private void PlacingTeleport(Box boxclicked)
    {
        if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing && turnManager.CurrentTurnState == TurnManager.PlayTurnState.placing)
        {
            if (pawnSelected.player == Player.player1 && turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn && boxclicked.board == 1 && boxclicked.index1 == 3 && boxclicked.free)
            {
                Debug.Log(boxclicked);
                pawnSelected.gameObject.transform.position = boxclicked.gameObject.transform.position + pawnSelected.offset;
                pawnSelected.currentBox = boxclicked;
                pawnSelected.currentBox.free = false;
                DeselectPawn();
                pawnsToPlace--;
                placingsLeft--;
                if (placingsLeft == 0 || pawnsToPlace == 0)
                {
                    turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P2_turn;
                    placingsLeft = 2;
                }
            }
            else if (pawnSelected.player == Player.player2 && turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn && boxclicked.board == 2 && boxclicked.index1 == 3 && boxclicked.free)
            {
                Debug.Log(boxclicked);
                pawnSelected.gameObject.transform.position = boxclicked.gameObject.transform.position + pawnSelected.offset;
                pawnSelected.currentBox = boxclicked;
                pawnSelected.currentBox.free = false;
                DeselectPawn();
                pawnsToPlace--;
                placingsLeft--;
                if (placingsLeft == 0)
                {
                    turnManager.CurrentPlayerTurn = TurnManager.PlayerTurn.P1_turn;
                    placingsLeft = 2;
                }
            }
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

    /// <summary>
    /// Funzione che controlla se la casella che gli è stata passata in input è già occupata da un altro player o se non è walkable
    /// se è libera ritorna true, altrimenti se è occupata ritorna false
    /// </summary>
    /// <param name="boxclicked"></param>
    /// <returns></returns>
    private bool CheckFreeBoxes(Pawn pawnToCheck)
    {
        Transform[][] boardToUse;
        Box currentBox = pawnToCheck.currentBox;
        if (pawnToCheck.player == Player.player1)
        {
            boardToUse = board1;
        }
        else
        {
            boardToUse = board2;
        }

        for (int index1 = 0; index1 < boardToUse.Length; index1++)
        {
            for (int index2 = 0; index2 < boardToUse[0].Length; index2++)
            {
                if ((index1 == currentBox.index1 + 1 || index1 == currentBox.index1 - 1 || index1 == currentBox.index1) && (index2 == currentBox.index2 || index2 == currentBox.index2 + 1 || index2 == currentBox.index2 - 1)
                    && boardToUse[index1][index2].GetComponent<Box>() != currentBox && CheckFreeBox(boardToUse[index1][index2].GetComponent<Box>()))
                {
                    return true;
                }
            }
        }
        CustomLogger.Log("Non c'è una casella libera");
        return false;
    }

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che toglie il marchio di Kill a tutte le pedine
    /// </summary>
    public void UnmarkKillPawns()
    {
        foreach (Pawn p in pawns)
        {
            if (p.killMarker)
            {
                p.GetComponent<PawnOutline>().eraseRenderer = true;
                p.GetComponent<PawnOutline>().color = 2;
                p.killMarker = false;
            }
        }
    }

    /// <summary>
    /// Funzione che richiama la funzione Attack della pawnselected e se avviene l'attacco passa il turno
    /// </summary>
    /// <param name="boxclicked"></param>
    public void Attack()
    {
        if (pawnSelected != null && !superAttackPressed)
        {
            if (pawnSelected.Attack())
            {
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                CustomLogger.Log(pawnSelected.player + " ha attaccato");
                turnManager.ChangeTurn();
            }
        }
    }

    /// <summary>
    /// Funzione che richiama la funzione SuperAttack della pawnselected e se avviene l'attacco passa il turno
    /// </summary>
    /// <param name="boxclicked"></param>
    public void SuperAttack()
    {
        if (pawnSelected != null && !superAttackPressed)
        {
            if (pawnSelected.SuperAttack())
            {
                pawnSelected.GetComponent<Renderer>().material.color = pawnSelected.pawnColor;
                CustomLogger.Log(pawnSelected.player + " ha attaccato");
                turnManager.ChangeTurn();
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
            if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game)
            {
                pawnSelected.DisableMovementBoxes();
                pawnSelected.DisableAttackPattern();
            }
            pawnSelected.GetComponent<PawnOutline>().eraseRenderer = true;
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
                    if (CheckFreeBoxes(pawns[i]))
                    {
                        CustomLogger.Log(pawns[i] + " è in casella !walkable");
                        pawns[i].RandomizePattern();
                        PawnSelected(pawns[i]);
                        return;
                    }
                    else
                    {
                        CustomLogger.Log(pawns[i] + " non ha caselle libere adiacenti");
                        pawns[i].KillPawn(pawns[i]);
                        CheckBox();
                    }
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
        switch (turnManager.CurrentMacroPhase)
        {
            case TurnManager.MacroPhase.placing:
                if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.placing)
                {
                    Debug.Log("In Macro Fase Placing");
                    if (((turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn && selected.player == Player.player1) || (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn && selected.player == Player.player2)) && !selected.currentBox)
                    {
                        if (pawnSelected != null)
                        {
                            DeselectPawn();
                        }
                        selected.selected = true;
                        pawnSelected = selected;
                        pawnSelected.GetComponent<PawnOutline>().eraseRenderer = false;
                    }
                }
                break;
            case TurnManager.MacroPhase.game:
                if (pawnSelected == null && turnManager.CurrentTurnState == TurnManager.PlayTurnState.check)
                {
                    selected.selected = true;
                    pawnSelected = selected;
                    pawnSelected.GetComponent<PawnOutline>().eraseRenderer = false;
                    pawnSelected.ShowMovementBoxes();
                }
                else if ((turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn && selected.player == Player.player1 || turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn && selected.player == Player.player2) && movementSkipped && !superAttackPressed && turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack)
                {
                    if (pawnSelected != null)
                    {
                        DeselectPawn();
                    }
                    selected.selected = true;
                    pawnSelected = selected;
                    pawnSelected.GetComponent<PawnOutline>().eraseRenderer = false;
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
                    pawnSelected.GetComponent<PawnOutline>().eraseRenderer = false;
                    pawnSelected.ShowAttackPattern();
                    pawnSelected.ShowMovementBoxes();
                }
                break;
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
            if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing)
            {
                PlacingTeleport(boxclicked);
            }
            else if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game)
            {
                switch (turnManager.CurrentTurnState)
                {
                    case TurnManager.PlayTurnState.check:
                        MovementCheckPhase(boxclicked);
                        break;
                    case TurnManager.PlayTurnState.movement:
                        Movement(boxclicked);
                        break;
                    case TurnManager.PlayTurnState.attack:
                        CustomLogger.Log("Clicca il pulsante Attack se c'è una pedina in range");
                        break;
                    default:
                        DeselectPawn();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Funzione che setta il pattern delle pedine a seconda della scelta fatta nella fase di draft
    /// </summary>
    public void SetPawnsPattern()
    {
        int j = 0;
        int k = 0;
        for (int i = 0; i < pawns.Count; i++)
        {
            if (pawns[i].player == Player.player1)
            {
                pawns[i].ChangePattern(draftManager.p1_pawns_picks[j]);
                j++;
            }
            else if (pawns[i].player == Player.player2)
            {
                pawns[i].ChangePattern(draftManager.p2_pawns_picks[k]);
                k++;
            }
        }
    }

    /// <summary>
    /// Funzione che gestisce le condizioni di vittoria
    /// </summary>
    public void WinCondition()
    {

        if (p1pawns > p2pawns)
        {
            uiManager.winScreen.SetActive(true);
            uiManager.gameResult.text = "Player 1 wins by having more pawns! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
        }
        else if (p2pawns > p1pawns)
        {
            uiManager.winScreen.SetActive(true);
            uiManager.gameResult.text = "Player 2 wins by having more pawns! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
        }
        else if (p1pawns == p2pawns)
        {
            foreach (Box box in boxesArray)
            {
                if (box.board == 1)
                {
                    p1tiles++;
                }
                else if (box.board == 2)
                {
                    p2tiles++;
                }
            }


            if (p1tiles > p2tiles)
            {
                uiManager.winScreen.SetActive(true);
                uiManager.gameResult.text = "Player 1 wins by destroying more tiles! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
            }
            else if (p2tiles > p1tiles)
            {
                uiManager.winScreen.SetActive(true);
                uiManager.gameResult.text = "Player 2 wins by destroying more tiles! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
            }
            else if (p1tiles == p2tiles)
            {
                uiManager.winScreen.SetActive(true);
                uiManager.gameResult.text = "DRAW! Both players had the same amount of pawns and destroyed the same amount of tiles! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
            }
        }
    }

    /// <summary>
    /// Controlla se sono presenti delle pedine da scegliere (bianche/nere) e ritorna true se ci sono o false se non ci sono
    /// </summary>
    /// <returns></returns>
    public bool CheckPawnToChoose()
    {
        bool foundPawn = false;
        foreach (Pawn p in pawns)
        {
            if (p.activePattern == 4 || p.activePattern == 5)
            {
                foundPawn = true;
            }
        }
        return foundPawn;
    }

    /// <summary>
    /// Imposta la pedina di cui bisogna scegliere il pattern in base al turno del giocatore
    /// </summary>
    public void SetPawnToChoose()
    {
        bool foundPawn = false;
        if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
        {
            foreach (Pawn p in pawns)
            {
                if ((p.activePattern == 4 || p.activePattern == 5) && p.player == Player.player1)
                {
                    pawnSelected = p;
                    pawnSelected.GetComponent<PawnOutline>().eraseRenderer = false;
                    foundPawn = true;
                    CustomLogger.Log("trovata una nel p1");
                    break;
                }
            }
        }
        else if (turnManager.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
        {
            foreach (Pawn p in pawns)
            {
                if ((p.activePattern == 4 || p.activePattern == 5) && p.player == Player.player2)
                {
                    pawnSelected = p;
                    pawnSelected.GetComponent<PawnOutline>().eraseRenderer = false;
                    foundPawn = true;
                    CustomLogger.Log("trovata una nel p2");
                    break;
                }
            }
        }
        if (foundPawn)
            return;
        turnManager.ChangeTurn();
        CustomLogger.Log("Cambio turno");
    }

    /// <summary>
    /// Funzione che imposta il pattern della selectedPawn con il valore passato in input (usata quando viene premuto il pulsante del rispettivo colore)
    /// </summary>
    /// <param name="patternIndex"></param>
    public void ChoosePawnPattern(int patternIndex)
    {
        pawnSelected.ChangePattern(patternIndex);
        DeselectPawn();
        turnManager.ChangeTurn();
    }
    #endregion
}

//enumeratore che contiene i player possibili
public enum Player
{
    player1, player2
}
