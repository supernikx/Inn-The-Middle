using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using PawnOutlineNameSpace;
using TMPro;
using UnityEngine.EventSystems;

public class Pawn : MonoBehaviour
{
    //variabili pubbliche
    public bool selected, randomized;
    public Player player;
    public Box currentBox;
    public float speed;
    [HideInInspector]
    public List<GameObject> projections;
    [Space]
    [Header("Attack Settings")]
    public int activePattern;
    /// <summary>
    /// Lista che contiene i pattern d'attacco e il colore del pattern, i valori inseriti sono 2 interi che identificano quanto la casella interessata si discosta dalla nostra posizione (index 1 riga, index 2 colonna)
    /// </summary>
    public List<Attack> patterns;
    public bool killMarker;

    //variabili private
    private BoardManager bm;
    private Box projectionTempBox;
    private Transform[][] myboard, enemyboard;
    private PlayerElements myelements;
    private List<GameObject> graphics;
    private Vector3 startRotation;

    // Use this for initialization
    void Start()
    {
        selected = false;
        killMarker = false;
        randomized = false;
        bm = BoardManager.Instance;
        projectionTempBox = currentBox;
        startRotation = transform.eulerAngles;
        graphics = new List<GameObject>();
        projections = new List<GameObject>();
        SetGraphics();
        SetBoards();
    }

    /// <summary>
    /// Funzione che imposta qual'è la nostra board e qual'è quella dell'avversario
    /// </summary>
    private void SetBoards()
    {
        if (player == Player.player1)
        {
            myboard = bm.board1;
            enemyboard = bm.board2;
            myelements = bm.player1Elements;
        }
        else if (player == Player.player2)
        {
            myboard = bm.board2;
            enemyboard = bm.board1;
            myelements = bm.player2Elements;
        }
    }

    private void SetGraphics()
    {
        foreach (Transform child in transform)
        {
            GameObject childToAdd = child.gameObject;
            childToAdd.SetActive(false);
            graphics.Add(childToAdd);
            GameObject projectionToAdd = childToAdd.transform.GetChild(1).gameObject;
            projectionToAdd.SetActive(false);
            projections.Add(projectionToAdd);
        }
    }

    /// <summary>
    /// Funzione chiamata quando quest'oggetto viene distrutto e che lo rimuove dalla lista delle pedina
    /// </summary>
    private void OnDestroy()
    {
        bm.pawns.Remove(this);
    }

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che viene chiamata ogni volta che la pedina viene premuta, e che richiama la funzione del BoardManager PawnSelected
    /// </summary>
    public void OnMouseDown()
    {
        if (killMarker)
        {
            bm.pawnSelected.myelements.UseSuperAttack();
            bm.KillPawnMarked(this);
        }
        else
        {
            bm.PawnSelected(this);
        }
    }

    #region GraphicsFunctions

    /// <summary>
    /// Funzione che mostra le caselle in cui questa pedina può muoversi
    /// </summary>
    public void ShowMovementBoxes()
    {
        for (int index1 = 0; index1 < myboard.Length; index1++)
        {
            for (int index2 = 0; index2 < myboard[0].Length; index2++)
            {
                if ((index1 == currentBox.index1 + 1 || index1 == currentBox.index1 - 1 || index1 == currentBox.index1) && (index2 == currentBox.index2 || index2 == currentBox.index2 + 1 || index2 == currentBox.index2 - 1)
                    && myboard[index1][index2].GetComponent<Box>() != currentBox)
                {
                    myboard[index1][index2].GetComponent<Box>().ShowBoxMovement();
                }
            }
        }
    }

    /// <summary>
    /// Funzione che disabilita la visione delle caselle in cui può muoversi questa pedina
    /// </summary>
    public void DisableMovementBoxes()
    {
        for (int index1 = 0; index1 < myboard.Length; index1++)
        {
            for (int index2 = 0; index2 < myboard[0].Length; index2++)
            {
                if ((index1 == currentBox.index1 + 1 || index1 == currentBox.index1 - 1 || index1 == currentBox.index1) && (index2 == currentBox.index2 || index2 == currentBox.index2 + 1 || index2 == currentBox.index2 - 1)
                    && myboard[index1][index2].GetComponent<Box>() != currentBox)
                {
                    myboard[index1][index2].GetComponent<Box>().SetAsDefault();
                }
            }
        }
    }

    /// <summary>
    /// Mostra sulla board avversaria le caselle attaccabili da questa pedina
    /// </summary>
    public void ShowAttackPattern()
    {
        int currentColumn, currentRow;
        if (projectionTempBox != null)
        {
            currentColumn = projectionTempBox.index2;
            currentRow = projectionTempBox.index1;
        }
        else
        {
            currentColumn = currentBox.index2;
            currentRow = currentBox.index1;
        }

        if (activePattern == 2)
        {
            int patternindex1 = patterns[activePattern].pattern[0].index1;
            int patternindex2 = patterns[activePattern].pattern[0].index2;
            if ((currentColumn + patternindex2 < enemyboard[0].Length && currentColumn + patternindex2 >= 0) && (patternindex1 - currentRow < enemyboard.Length && patternindex1 - currentRow >= 0))
            {
                enemyboard[patternindex1 - currentRow][currentColumn + patternindex2].GetComponent<Box>().ShowBoxActivePattern();
            }

            if (((currentColumn + patternindex2 < myboard[0].Length && currentColumn + patternindex2 >= 0) && (currentRow - patternindex1 < myboard.Length && currentRow - patternindex1 - 1 >= 0)))
            {
                myboard[currentRow - patternindex1 - 1][currentColumn + patternindex2].GetComponent<Box>().ShowBoxActivePattern();
            }
        }
        else
        {
            foreach (Pattern p in patterns[activePattern].pattern)
            {
                if (((currentColumn + p.index2 < enemyboard[0].Length && currentColumn + p.index2 >= 0) && (p.index1 - currentRow < enemyboard.Length && p.index1 - currentRow >= 0)) && enemyboard[p.index1 - currentRow][currentColumn + p.index2].GetComponent<Box>() != currentBox)
                {
                    enemyboard[p.index1 - currentRow][currentColumn + p.index2].GetComponent<Box>().ShowBoxActivePattern();
                }

                if (((currentColumn + p.index2 < myboard[0].Length && currentColumn + p.index2 >= 0) && (currentRow - p.index1 < myboard.Length && currentRow - p.index1 - 1 >= 0)) && myboard[currentRow - p.index1 - 1][currentColumn + p.index2].GetComponent<Box>() != currentBox)
                {
                    myboard[currentRow - p.index1 - 1][currentColumn + p.index2].GetComponent<Box>().ShowBoxActivePattern();
                }
            }
        }
    }

    /// <summary>
    /// Disabilita la visione sulla board avversaria le caselle attaccabili da questa pedina
    /// </summary>
    public void DisableAttackPattern()
    {
        int currentColumn, currentRow;
        if (projectionTempBox != null)
        {
            currentColumn = projectionTempBox.index2;
            currentRow = projectionTempBox.index1;
        }
        else
        {
            currentColumn = currentBox.index2;
            currentRow = currentBox.index1;
        }
        if (activePattern == 2)
        {
            int patternindex1 = patterns[activePattern].pattern[0].index1;
            int patternindex2 = patterns[activePattern].pattern[0].index2;
            if ((currentColumn + patternindex2 < enemyboard[0].Length && currentColumn + patternindex2 >= 0) && (patternindex1 - currentRow < enemyboard.Length && patternindex1 - currentRow >= 0))
            {
                enemyboard[patternindex1 - currentRow][currentColumn + patternindex2].GetComponent<Box>().SetAsDefault();
            }

            if ((currentColumn + patternindex2 < myboard[0].Length && currentColumn + patternindex2 >= 0) && (currentRow - patternindex1 < myboard.Length && currentRow - patternindex1 - 1 >= 0))
            {
                myboard[currentRow - patternindex1 - 1][currentColumn + patternindex2].GetComponent<Box>().SetAsDefault();
            }
        }
        else
        {
            foreach (Pattern a in patterns[activePattern].pattern)
            {
                if ((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentRow < enemyboard.Length && a.index1 - currentRow >= 0))
                {
                    enemyboard[a.index1 - currentRow][currentColumn + a.index2].GetComponent<Box>().SetAsDefault();
                }

                if ((currentColumn + a.index2 < myboard[0].Length && currentColumn + a.index2 >= 0) && (currentRow - a.index1 < myboard.Length && currentRow - a.index1 - 1 >= 0))
                {
                    myboard[currentRow - a.index1 - 1][currentColumn + a.index2].GetComponent<Box>().SetAsDefault();
                }
            }
        }
    }

    #endregion

    #region Attack

    /// <summary>
    /// Funzione che controlla se nel pattern è presente una pedina aversaria, allora ritorna true, altrimenti ritorna false
    /// </summary>
    /// <returns></returns>
    public bool CheckAttackPattern()
    {
        int currentColumn = currentBox.index2;
        if (activePattern == 2)
        {
            int patternindex1 = patterns[activePattern].pattern[0].index1;
            int patternindex2 = patterns[activePattern].pattern[0].index2;
            foreach (Pawn p in bm.pawns)
            {
                if (p.player != player)
                {
                    if (((currentColumn + patternindex2 < enemyboard[0].Length && currentColumn + patternindex2 >= 0) && (patternindex1 - currentBox.index1 < enemyboard.Length && patternindex1 - currentBox.index1 >= 0)) && ((p.currentBox.index1 == patternindex1 - currentBox.index1) && (p.currentBox.index2 == currentColumn + patternindex2)))
                    {
                        CustomLogger.Log("c'è una pedina avversaria nel pattern");
                        return true;
                    }
                }
            }
        }
        else
        {
            foreach (Pattern a in patterns[activePattern].pattern)
            {
                foreach (Pawn p in bm.pawns)
                {
                    if (p.player != player)
                    {
                        if (((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentBox.index1 < enemyboard.Length && a.index1 - currentBox.index1 >= 0)) && ((p.currentBox.index1 == a.index1 - currentBox.index1) && (p.currentBox.index2 == currentColumn + a.index2)))
                        {
                            CustomLogger.Log("c'è una pedina avversaria nel pattern");
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Funzione che effettua l'attaco per tutto il pattern se è presente una pedina avversaria al suo interno, ritorna true se l'attacco è andato a buon fine o false se non è avvenuto
    /// </summary>
    /// <returns></returns>
    public bool Attack()
    {
        if (!CheckAttackPattern())
            return false;
        BoardManager.Instance.turnManager.turnsWithoutAttack = 0;
        int currentColumn = currentBox.index2, pHit = 0;
        foreach (Pattern p in patterns[activePattern].pattern)
        {
            for (int i = 0; i < bm.pawns.Count; i++)
            {
                if (bm.pawns[i].player != player)
                {
                    if (((currentColumn + p.index2 < enemyboard[0].Length && currentColumn + p.index2 >= 0) && (p.index1 - currentBox.index1 < enemyboard.Length && p.index1 - currentBox.index1 >= 0)) && ((bm.pawns[i].currentBox.index1 == p.index1 - currentBox.index1) && (bm.pawns[i].currentBox.index2 == currentColumn + p.index2)) && enemyboard[p.index1 - currentBox.index1][currentColumn + p.index2].GetComponent<Box>().walkable)
                    {
                        Box boxToAttack = enemyboard[p.index1 - currentBox.index1][currentColumn + p.index2].GetComponent<Box>();
                        switch (boxToAttack.element)
                        {
                            case Element.Purple:
                            case Element.Orange:
                            case Element.Azure:
                                myelements.AddElement(boxToAttack.element);
                                boxToAttack.AttackBox();
                                break;
                            case Element.NeutralWhite:
                                boxToAttack.ChangeNeutralType();
                                break;
                            case Element.NeutralBlack:
                                KillPawn(bm.pawns[i]);
                                boxToAttack.ChangeNeutralType();
                                break;
                            default:
                                break;
                        }
                        pHit++;
                        CustomLogger.Log("c'è una pedina avversaria nel pattern");
                    }
                }
            }
        }
        if (pHit > 0)
        {
            DisableAttackPattern();
            return true;
        }
        CustomLogger.Log("nope");
        return false;
    }

    /// <summary>
    /// Funzione che effettua l'attaco per tutto il pattern se è presente una pedina avversaria al suo interno distruggendo la prima, ritorna true se l'attacco è andato a buon fine o false se non è avvenuto
    /// </summary>
    /// <returns></returns>
    public bool SuperAttack()
    {
        if (!CheckAttackPattern())
            return false;
        BoardManager.Instance.turnManager.turnsWithoutAttack = 0;
        int currentColumn = currentBox.index2;
        List<Pawn> pawnsToKill = new List<Pawn>();
        foreach (Pattern p in patterns[activePattern].pattern)
        {
            for (int i = 0; i < bm.pawns.Count; i++)
            {
                if (bm.pawns[i].player != player)
                {
                    if (((currentColumn + p.index2 < enemyboard[0].Length && currentColumn + p.index2 >= 0) && (p.index1 - currentBox.index1 < enemyboard.Length && p.index1 - currentBox.index1 >= 0)) && ((bm.pawns[i].currentBox.index1 == p.index1 - currentBox.index1) && (bm.pawns[i].currentBox.index2 == currentColumn + p.index2)) && enemyboard[p.index1 - currentBox.index1][currentColumn + p.index2].GetComponent<Box>().walkable)
                    {
                        pawnsToKill.Add(bm.pawns[i]);
                    }
                }
            }
        }

        switch (pawnsToKill.Count)
        {
            case 0:
                CustomLogger.Log("Nessuna pedina nel Pattern");
                return false;
            case 1:
                KillPawn(pawnsToKill[0]);
                CustomLogger.Log("Pedina Uccisa");
                myelements.UseSuperAttack();
                return true;
            default:
                bm.superAttackPressed = true;
                foreach (Pawn p in pawnsToKill)
                {
                    p.killMarker = true;
                    //Color finalColor = Color.red * Mathf.LinearToGammaSpace(0.25f);
                    //p.projections[p.activePattern].GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", finalColor);           
                    p.projections[p.activePattern].SetActive(true);
                }
                CustomLogger.Log("Scegli la pedina da uccidere");
                return false;
        }
    }

    #endregion

    /// <summary>
    /// Funzione che esegue tutti i controlli sulla casella e se rispetta i requisiti muove la pedina
    /// ritorna true se la pedina si muove, ritorna false se non è avvenuto
    /// </summary>
    /// <param name="boxindex1"></param>
    /// <param name="boxindex2"></param>
    /// <param name="boxToMove"></param>
    /// <returns></returns>
    public bool Move(Box boxToMove)
    {
        if (boxToMove == currentBox)
        {
            return false;
        }
        if ((boxToMove.index1 == currentBox.index1 + 1 || boxToMove.index1 == currentBox.index1 - 1 || boxToMove.index1 == currentBox.index1) && (boxToMove.index2 == currentBox.index2 || boxToMove.index2 == currentBox.index2 + 1 || boxToMove.index2 == currentBox.index2 - 1))
        {
            transform.LookAt(new Vector3(boxToMove.transform.position.x, transform.position.y, boxToMove.transform.position.z));
            transform.Rotate(new Vector3(0, 90 - startRotation.y, 0));
            transform.DOMove(boxToMove.transform.position, speed);
            DisableMovementBoxes();
            DisableAttackPattern();
            currentBox.free = true;
            if (currentBox.element == Element.NeutralBlack)
                currentBox.walkable = true;
            currentBox = boxToMove.GetComponent<Box>();
            currentBox.free = false;
            ForceMoveProjection(true);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che sposta la proiezione nella box che gli viene passata come parametro
    /// </summary>
    /// <param name="boxToMove"></param>
    /// <returns></returns>
    public bool MoveProjection(Box boxToMove)
    {
        if ((boxToMove.index1 == currentBox.index1 + 1 || boxToMove.index1 == currentBox.index1 - 1 || boxToMove.index1 == currentBox.index1) && (boxToMove.index2 == currentBox.index2 || boxToMove.index2 == currentBox.index2 + 1 || boxToMove.index2 == currentBox.index2 - 1) && (boxToMove.free || boxToMove == currentBox))
        {
            DisableAttackPattern();
            if (boxToMove == currentBox)
            {
                transform.eulerAngles = startRotation;
            }
            else
            {
                transform.LookAt(new Vector3(boxToMove.transform.position.x, transform.position.y, boxToMove.transform.position.z));
                transform.Rotate(new Vector3(0, 90 - startRotation.y, 0));
            }
            projections[activePattern].transform.position = new Vector3(boxToMove.transform.position.x, boxToMove.transform.position.y + graphics[activePattern].transform.position.y, boxToMove.transform.position.z);
            projectionTempBox = boxToMove;
            ShowAttackPattern();
            ShowMovementBoxes();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che forza lo spostamento della proiezione nella casella della pedina corrispondente
    /// </summary>
    public void ForceMoveProjection(bool keepRotation)
    {
        projections[activePattern].transform.position = new Vector3(transform.position.x, transform.position.y + graphics[activePattern].transform.position.y, transform.position.z);
        projectionTempBox = currentBox;
        if (!keepRotation)
        {
            transform.eulerAngles = startRotation;
        }
    }

    /// <summary>
    /// Funzione che prende in input una pedina e la distrugge disattivando tutte le funzioni necessarie
    /// </summary>
    /// <param name="pawnToKill"></param>
    public void KillPawn(Pawn pawnToKill)
    {
        pawnToKill.DisableAttackPattern();
        pawnToKill.currentBox.free = true;
        pawnToKill.currentBox = null;
        DestroyImmediate(pawnToKill.gameObject);

        if (pawnToKill.player == Player.player1)
        {
            BoardManager.Instance.p1pawns--;
            if (BoardManager.Instance.p1pawns <= 0)
            {
                BoardManager.Instance.uiManager.winScreen.SetActive(true);
                BoardManager.Instance.uiManager.gameResult.text = "Science wins! \n " + "The game ended in " + BoardManager.Instance.turnManager.numberOfTurns + " turns.";

            }
        }
        else if (pawnToKill.player == Player.player2)
        {
            BoardManager.Instance.p2pawns--;
            if (BoardManager.Instance.p2pawns <= 0)
            {
                BoardManager.Instance.uiManager.winScreen.SetActive(true);
                BoardManager.Instance.uiManager.gameResult.text = "Magic wins! \n" + "The game ended in " + BoardManager.Instance.turnManager.numberOfTurns + " turns.";
            }
        }
    }

    /// <summary>
    /// Funzione che randomizza il pattern della pedina e gli assegna il colore corrispondente
    /// </summary>
    public void RandomizePattern()
    {
        graphics[activePattern].SetActive(false);
        projections[activePattern].SetActive(false);
        activePattern = UnityEngine.Random.Range(0, patterns.Count);
        if (activePattern == 4 || activePattern == 5)
        {
            bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.choosing;
        }
        graphics[activePattern].SetActive(true);
        projections[activePattern].SetActive(true);
        randomized = true;
    }

    /// <summary>
    /// Funzione che imposta il pattern attivo con il valore che gli viene passato
    /// </summary>
    /// <param name="index"></param>
    public void ChangePattern(int index)
    {
        graphics[activePattern].SetActive(false);
        activePattern = index;
        graphics[activePattern].SetActive(true);
    }
    #endregion
}
