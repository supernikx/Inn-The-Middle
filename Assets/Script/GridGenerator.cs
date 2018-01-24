using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridGenerator : MonoBehaviour {
    public GameObject TilePrefab;
    public BoardManager bm;
    public Transform[][] tilePositions;
    private List<Transform> tiles;
    public int row, column;
    public bool oneTime=false;

    private void Awake()
    {
        CreateGrid(row,column);
        SetBoards();
        oneTime = false;
    }

    public void CreateGrid(int row, int column)
    {
        if (!oneTime)
        {
            tiles = new List<Transform>();
            float size = TilePrefab.transform.localScale.x + 4f;
            tilePositions = new Transform[row][];
            for (int x = 0; x < row; x++)
            {
                tilePositions[x] = new Transform[column];
                for (int y = 0; y < column; y++)
                {
                    tilePositions[x][y] = Instantiate(TilePrefab, new Vector3(transform.position.x + x * size, transform.position.y, transform.position.z + y * size), transform.rotation, transform).transform;
                    tiles.Add(tilePositions[x][y]);
                    Box b = tilePositions[x][y].GetComponent<Box>();
                    b.index1 = x;
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
            oneTime = true;
        }
        else
        {
            DestroyGrid();
        }
    }

    public void DestroyGrid()
    {
        foreach(Transform t in tiles)
        {
            DestroyImmediate(t.gameObject);
        }
        tiles.Clear();
        oneTime = false;
    }

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
}
