using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pawn : MonoBehaviour
{
    //variabili pubbliche
    public bool selected;
    public Vector3 offset;
    public Player player;
    public Box currentBox;
    public float speed;
    public int startIndex1, startIndex2;
    public Color pawnColor;
    [Space]
    [Header("Attack Settings")]
    [SerializeField]
    private int activePattern;
    /// <summary>
    /// Lista che contiene i pattern d'attacco e il colore del pattern, i valori inseriti sono 2 interi che identificano quanto la casella interessata si discosta dalla nostra posizione (index 1 riga, index 2 colonna)
    /// </summary>
    public List<Attack> patterns;

    //variabili private
    private BoardManager bm;
    private MeshRenderer mr;
    private Transform[][] myboard, enemyboard;

    //parte di codice con funzioni private
    // Use this for initialization
    void Start()
    {
        bm = FindObjectOfType<BoardManager>();
        selected = false;
        mr = GetComponent<MeshRenderer>();
        pawnColor = mr.material.color;
        SetBoards();
        RandomizePattern();
    }

    /// <summary>
    /// Funzione che esegue tutti i controlli sulla casella e se rispetta i requisiti muove la pedina
    /// ritorna true se la pedina si muove, ritorna false se non è avvenuto
    /// </summary>
    /// <param name="boxindex1"></param>
    /// <param name="boxindex2"></param>
    /// <param name="boxToMove"></param>
    /// <returns></returns>
    private bool PawnMovement(int boxindex1, int boxindex2, Transform boxToMove)
    {
        if (boxToMove.GetComponent<Box>() == currentBox)
        {
            return false;
        }
        if ((boxindex1 == currentBox.index1 + 1 || boxindex1 == currentBox.index1 - 1 || boxindex1 == currentBox.index1) && (boxindex2 == currentBox.index2 || boxindex2 == currentBox.index2 + 1 || boxindex2 == currentBox.index2 - 1))
        {
            transform.LookAt(new Vector3(boxToMove.position.x, transform.position.y, boxToMove.position.z));
            transform.Rotate(new Vector3(0, 90, 0));
            transform.DOMove(boxToMove.position + offset, speed);
            DisableMovementBoxes();
            currentBox.free = true;
            currentBox = boxToMove.GetComponent<Box>();
            currentBox.free = false;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che controlla se boxToAttack è una casella del pattern e se nel pattern è presente una pedina avversaria esegue l'attacco colorando tutte le caselle nel range del pattern e disabilitando il pattern visuale
    /// altrimenti ritorna false
    /// </summary>
    /// <param name="boxToAttack"></param>
    private bool PawnAttack(Box boxToAttack)
    {
        if (boxToAttack.pattern)
        {
            int currentColumn = currentBox.index2, pHit = 0;
            foreach (Pattern a in patterns[activePattern].pattern)
            {
                foreach (Pawn p in bm.pawns)
                {
                    if (p.player != player)
                    {
                        if (((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentBox.index1 < enemyboard.Length && a.index1 - currentBox.index1 >= 0)) && ((p.currentBox.index1 == a.index1 - currentBox.index1) && (p.currentBox.index2 == currentColumn + a.index2)) && enemyboard[a.index1 - currentBox.index1][currentColumn + a.index2].GetComponent<Box>().walkable)
                        {
                            enemyboard[a.index1 - currentBox.index1][currentColumn + a.index2].GetComponent<Box>().AttackBox();
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
        CustomLogger.Log("nope");
        return false;
    }

    /// <summary>
    /// Funzione che controlla se nel pattern è presente una pedina aversaria, allora ritorna true, altrimenti ritorna false
    /// </summary>
    /// <returns></returns>
    private bool CheckAttackPattern()
    {
        int currentColumn = currentBox.index2;
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
        return false;
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
        }
        else if (player == Player.player2)
        {
            myboard = bm.board2;
            enemyboard = bm.board1;
        }
    }

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che viene chiamata ogni volta che la pedina viene premuta, e che richiama la funzione del BoardManager PawnSelected
    /// </summary>
    public void OnMouseDown()
    {
        bm.PawnSelected(gameObject.GetComponent<Pawn>());
    }

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
                    && myboard[index1][index2].GetComponent<Box>()!=currentBox)
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
        int currentColumn = currentBox.index2;
        foreach (Pattern a in patterns[activePattern].pattern)
        {
            if ((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentBox.index1 < enemyboard.Length && a.index1 - currentBox.index1 >= 0))
            {
                enemyboard[a.index1 - currentBox.index1][currentColumn + a.index2].GetComponent<Box>().ShowBoxEnemy();
            }

            if ((currentColumn + a.index2 < myboard[0].Length && currentColumn + a.index2 >= 0) && (currentBox.index1 - a.index1 < myboard.Length && currentBox.index1 - a.index1 - 1 >= 0))
            {
                myboard[currentBox.index1 - a.index1 - 1][currentColumn + a.index2].GetComponent<Box>().ShowBoxMy();
            }
        }
    }

    /// <summary>
    /// Disabilita la visione sulla board avversaria le caselle attaccabili da questa pedina
    /// </summary>
    public void DisableAttackPattern()
    {
        int currentColumn = currentBox.index2;
        foreach (Pattern a in patterns[activePattern].pattern)
        {
            if ((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentBox.index1 < enemyboard.Length && a.index1 - currentBox.index1 >= 0))
            {
                enemyboard[a.index1 - currentBox.index1][currentColumn + a.index2].GetComponent<Box>().SetAsDefault();
            }

            if ((currentColumn + a.index2 < myboard[0].Length && currentColumn + a.index2 >= 0) && (currentBox.index1 - a.index1 < myboard.Length && currentBox.index1 - a.index1 - 1 >= 0))
            {
                myboard[currentBox.index1 - a.index1 - 1][currentColumn + a.index2].GetComponent<Box>().SetAsDefault();
            }
        }
    }

    /// <summary>
    /// Funzione che prende in input 2 interi relativi alla coordinata di una casella nell'array e identifica la board opposta, passa la box e gli index a PawnAttack
    /// ritorna true in caso l'attacco sia avvenuto, mentre ritorna false se non è avvenuto
    /// </summary>
    /// <param name="boxindex1"></param>
    /// <param name="boxindex2"></param>
    /// <returns></returns>
    public bool Attack(int boxindex1, int boxindex2)
    {
        Box boxToAttack = enemyboard[boxindex1][boxindex2].GetComponent<Box>();
        return PawnAttack(boxToAttack);
    }

    /// <summary>
    /// Funzione che prende in input 2 interi relativi alla coordinata di una casella nell'array e identifica la board, passa la box e gli indici a PawnMovement 
    /// ritorna true in caso il movimento sia avvenuto, mentre ritorna false se non è avvenuto
    /// </summary>
    /// <param name="boxindex1"></param>
    /// <param name="boxindex2"></param>
    /// <returns></returns>
    public bool Move(int boxindex1, int boxindex2)
    {
        Transform boxToMove = myboard[boxindex1][boxindex2];
        selected = false;
        return PawnMovement(boxindex1, boxindex2, boxToMove);
    }

    /// <summary>
    /// Funzione che randomizza il pattern della pedina e gli assegna il colore corrispondente
    /// </summary>
    public void RandomizePattern()
    {
        activePattern = Random.Range(0, patterns.Count);
        mr.material = patterns[activePattern].patternMaterial;
        pawnColor = mr.material.color;
    }

    #endregion
}
