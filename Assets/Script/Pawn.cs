using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pawn : MonoBehaviour
{
    public bool selected;
    public Vector3 offset;
    private BoardManager bm;
    public Player player;
    public Box currentBox;
    public float speed;
    public int startIndex1, startIndex2;
    public List<Attack> pattern;
    public Color pawnColor;
    

    // Use this for initialization
    void Start()
    {
        bm = FindObjectOfType<BoardManager>();
        selected = false;
        pawnColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        bm.PawnSelected(gameObject.GetComponent<Pawn>());
    }

    public bool Move(int boxindex1, int boxindex2)
    {
        Transform boxToMove=currentBox.transform;
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

    private bool PawnMovement(int boxindex1, int boxindex2, Transform boxToMove)
    {
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
        if (CheckAttackPattern(boxToAttack))
        {
            PawnAttack(boxToAttack);
            return true;
        }
        else
        {
            Debug.Log("nope");
            return false;
        }
    }

    private void PawnAttack(Box boxToAttack)
    {
        boxToAttack.AttackBox();
        DisableAttackPattern();
    }

    private bool CheckAttackPattern(Box boxToAttack)
    {
        int currentColumn = currentBox.index2;
        foreach (Attack a in pattern)
        {
            if (currentColumn + a.index2 == boxToAttack.index2 && a.index1 - currentBox.index1 == boxToAttack.index1)
                return true;
        }
        return false;
    }

    public void ShowAttackPattern()
    {
        int currentColumn = currentBox.index2;
        foreach (Attack a in pattern)
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

    public void DisableAttackPattern()
    {
        int currentColumn = currentBox.index2;
        foreach (Attack a in pattern)
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
}
