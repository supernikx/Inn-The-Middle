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

    //parte di codice con funzioni private
    // Use this for initialization
    void Start()
    {
        bm = FindObjectOfType<BoardManager>();
        selected = false;
        mr = GetComponent<MeshRenderer>();
        pawnColor = mr.material.color;
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
        if ((boxindex1 == currentBox.index1 + 1 || boxindex1 == currentBox.index1 - 1 || boxindex1 == currentBox.index1) && (boxindex2 == currentBox.index2 || boxindex2 == currentBox.index2+1 || boxindex2 == currentBox.index2-1))
        {
            transform.LookAt(new Vector3(boxToMove.position.x, transform.position.y, boxToMove.position.z));
            transform.Rotate(new Vector3(0, 90,0));
            transform.DOMove(boxToMove.position + offset, speed);
            currentBox = boxToMove.GetComponent<Box>();
            ShowAttackPattern();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che controlla se boxToAttack rispetta il pattern di attacco della pedina, esegue l'attacco colorando la casella attaccata e disabilitando il pattern visuale
    /// altrimenti ritorna false
    /// </summary>
    /// <param name="boxToAttack"></param>
    private bool PawnAttack(Box boxToAttack)
    {
        if (CheckAttackPattern(boxToAttack))
        {
            boxToAttack.AttackBox();
            DisableAttackPattern();
            return true;
        }
        Debug.Log("nope");
        return false;
    }

    /// <summary>
    /// Confronta boxToAttack con i valori inseriti dentro la variabile pattern, in caso la box fa parte del pattern di questa pedina ritorna true, altrimenti ritorna false
    /// </summary>
    /// <param name="boxToAttack"></param>
    /// <returns></returns>
    private bool CheckAttackPattern(Box boxToAttack)
    {
        int currentColumn = currentBox.index2;
        foreach (Pattern a in patterns[activePattern].pattern)
        {
            if (currentColumn + a.index2 == boxToAttack.index2 && a.index1 - currentBox.index1 == boxToAttack.index1 && boxToAttack.walkable)
                return true;
        }
        return false;
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
    /// Mostra sulla board avversaria le caselle attaccabili da questa pedina
    /// </summary>
    public void ShowAttackPattern()
    {
        int currentColumn = currentBox.index2;
        foreach (Pattern a in patterns[activePattern].pattern)
        {
            for (int index1 = 0; index1 < bm.board1.Length; index1++)
            {
                for (int index2 = 0; index2 < bm.board1[index1].Length; index2++)
                {
                    if (currentColumn + a.index2 == index2 && a.index1 - currentBox.index1 == index1)
                    {
                        if (player == Player.player1)
                        {
                            bm.board2[index1][index2].GetComponent<Box>().ShowBox();
                        }
                        else if (player == Player.player2)
                        {
                            bm.board1[index1][index2].GetComponent<Box>().ShowBox();
                        }
                    }
                }
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
            for (int index1 = 0; index1 < bm.board1.Length; index1++)
            {
                for (int index2 = 0; index2 < bm.board1[index1].Length; index2++)
                {
                    if (currentColumn + a.index2 == index2 && a.index1 - currentBox.index1 == index1)
                    {
                        if (player == Player.player1)
                        {
                            bm.board2[index1][index2].GetComponent<Box>().SetAsDefault();
                        }
                        else if (player == Player.player2)
                        {
                            bm.board1[index1][index2].GetComponent<Box>().SetAsDefault();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Funzione che prende in input 2 interi relativi alla coordinata di una casella nell'array e identifica la board opposta in base a che player appartiene la pedina, passa tutto a PawnAttack
    /// ritorna true in caso l'attacco sia avvenuto, mentre ritorna false se non è avvenuto
    /// </summary>
    /// <param name="boxindex1"></param>
    /// <param name="boxindex2"></param>
    /// <returns></returns>
    public bool Attack(int boxindex1, int boxindex2)
    {
        Box boxToAttack = null;
        if (player == Player.player1)
        {
            boxToAttack = bm.board2[boxindex1][boxindex2].GetComponent<Box>();
        }
        else if (player == Player.player2)
        {
            boxToAttack = bm.board1[boxindex1][boxindex2].GetComponent<Box>();
        }
        return PawnAttack(boxToAttack);
    }

    /// <summary>
    /// Funzione che prende in input 2 interi relativi alla coordinata di una casella nell'array e identifica la board in base a che player appartiene la pedina, passa tutto a PawnMovement 
    /// ritorna true in caso il movimento sia avvenuto, mentre ritorna false se non è avvenuto
    /// </summary>
    /// <param name="boxindex1"></param>
    /// <param name="boxindex2"></param>
    /// <returns></returns>
    public bool Move(int boxindex1, int boxindex2)
    {
        Transform boxToMove = null;
        if (player == Player.player1)
        {
            boxToMove = bm.board1[boxindex1][boxindex2];
        }
        else if (player == Player.player2)
        {
            boxToMove = bm.board2[boxindex1][boxindex2];
        }
        selected = false;
        return PawnMovement(boxindex1, boxindex2, boxToMove);
    }

    public void RandomizePattern()
    {
        activePattern = Random.Range(0, patterns.Count);
        mr.material = patterns[activePattern].patternMaterial;
        pawnColor = mr.material.color;
    }

    #endregion
}
