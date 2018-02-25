using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    //variabili pubbliche
    public GameObject TilePrefab;
    public BoardManager bm;
    public Transform[][] tilePositions;

    //variabili private
    private BoardsGenerator bg;

    //funzioni private
    private void Awake()
    {
        bg = FindObjectOfType<BoardsGenerator>();
        CreateGrid(bg.x, bg.y);
        SetElements();
        SetBoards();
    }

    /// <summary>
    /// Funzione che imposta l'array generato all'interno delle variabili board del BoardManager in base al player
    /// </summary>
    private void SetBoards()
    {
        if (tag == "board1")
        {
            bm.board1 = tilePositions;
        }
        else if (tag == "board2")
        {
            bm.board2 = tilePositions;
        }
    }

    /// <summary>
    /// Funzione che imposta gli elementi sulla board come nel pattern presente in BoardsGenerator
    /// </summary>
    private void SetElements()
    {
        int k = 0;
        for (int i = 0; i < tilePositions.Length; i++)
        {
            for (int j = 0; j < tilePositions[i].Length; j++)
            {
                tilePositions[i][j].GetComponent<Box>().SetElement(bg.boardPattern[k].boxElement);
                k++;
            }
        }
    }


    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che crea la griglia, viene chiamata all'inizio del gioco e nell'editor dal BoardsGenerator, genera una griglia ricevendo in input righe e colonne, come caselle usa il TilePrefab
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void CreateGrid(int row, int column)
    {
        if (transform.Find("Board"))
        {
            DestroyImmediate(transform.Find("Board").gameObject);
        }
        Transform board = new GameObject("Board").transform;
        board.parent = transform;
        float size = TilePrefab.transform.localScale.x + 4f;
        tilePositions = new Transform[row][];
        for (int x = 0; x < row; x++)
        {
            int xToUSe = x;
            if (tag == "board1")
            {
                xToUSe = row - x - 1;
            }
            tilePositions[xToUSe] = new Transform[column];
            for (int y = 0; y < column; y++)
            {
                tilePositions[xToUSe][y] = Instantiate(TilePrefab, new Vector3(transform.position.x + x * size, transform.position.y, transform.position.z + y * size), transform.rotation, transform).transform;
                tilePositions[xToUSe][y].parent = board.transform;
                Box b = tilePositions[xToUSe][y].GetComponent<Box>();
                b.index1 = xToUSe;
                b.index2 = y;
                if (tag == "board1")
                {
                    b.board = 1;
                }
                else if (tag == "board2")
                {
                    b.board = 2;
                }     
            }
        }
    }

    #endregion
}
