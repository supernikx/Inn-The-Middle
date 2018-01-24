using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardsGenerator : MonoBehaviour {
    public int x;
    public int y;
    public GridGenerator board1, board2;

    public void CreateBoards()
    {
        board1.CreateGrid(x, y);
        board2.CreateGrid(x, y);
    }
}
