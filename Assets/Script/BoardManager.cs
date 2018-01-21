using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    private Transform[] boxtemppositions;
    private Box[] allbox;
    public Transform[][] boxCoordinates;
    private Pawn[] pawns;
    private Pawn pawnSelected;
    private Player player;
    public int row, column;

    // Use this for initialization
    void Start () {
        pawns = FindObjectsOfType<Pawn>();
        boxCoordinates = new Transform[row][];
        OrganizeBoxes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BoxClicked(int i, int j)
    {
        if (pawnSelected != null)
        {
            if (player == Player.player1)
            {
                pawnSelected.Move(i, j, 1);
            }
            else if (player == Player.player2)
            {
                pawnSelected.Move(i, j, 2);
            }
            pawnSelected = null;
        }
    }
    public void PawnSelected(Pawn selected)
    {
        for (int i = 0; i < pawns.Length; i++)
        {
            pawns[i].selected = false;
        }
        if (GetPawnBox(selected.transform.position).tag == "Box1")
        {
            player = Player.player1;
        }
        else if (GetPawnBox(selected.transform.position).tag == "Box2")
        {
            player = Player.player2;
        }
        selected.selected = true;
        pawnSelected = selected;
    }
    public Box GetPawnBox(Vector3 playerposition)
    {
        Box currentbox = boxCoordinates[0][0].GetComponent<Box>();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (boxCoordinates[i][j].position.x == playerposition.x && boxCoordinates[i][j].position.z == playerposition.z)
                {
                    currentbox = boxCoordinates[i][j].GetComponent<Box>();
                }
            }
        }
        return currentbox.GetComponent<Box>();
    }
    private void PrintCoordiate()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Debug.Log(boxCoordinates[i][j]);
            }
        }
    }
    private void OrganizeBoxes()
    {
        allbox = FindObjectsOfType<Box>();
        boxtemppositions = new Transform[allbox.Length];
        for (int i = 0; i < allbox.Length; i++)
        {
            boxtemppositions[i] = allbox[i].GetComponent<Transform>();
        }
        Array.Sort(boxtemppositions,Vector3Compare);
        int k = 0;
        for (int i = 0; i < row; i++)
        {
            boxCoordinates[i] = new Transform[column];
            for (int j = 0; j < column; j++)
            {
                boxCoordinates[i][j] = boxtemppositions[k];
                boxCoordinates[i][j].gameObject.GetComponent<Box>().index1 = i;
                boxCoordinates[i][j].gameObject.GetComponent<Box>().index2 = j;
                k++;
            }
        }
    }
    private int Vector3Compare(Transform value1, Transform value2)
    {
        if (value1.position.x > value2.position.x)
        {
            return -1;
        }
        else if (value1.position.x == value2.position.x)
        {
            if (value1.position.y < value2.position.y)
            {
                return -1;
            }
            else if (value1.position.y == value2.position.y)
            {
                if (value1.position.z < value2.position.z)
                {
                    return -1;
                }
                else if (value1.position.z == value2.position.z)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 1;
        }
    }
}

public enum Player {
    player1,player2
}
