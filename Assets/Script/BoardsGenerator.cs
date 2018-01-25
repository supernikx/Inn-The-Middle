using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardsGenerator : MonoBehaviour
{
    public int x;
    public int y;
    public GridGenerator board1, board2;

    public void CreateBoards()
    {
        if (board1 != null && board2 != null)
        {
            if (x < 0)
            {
                x = 1;
            }
            if (y<0)
            {
                y = 1;
            }
            board1.CreateGrid(x, y);
            board2.CreateGrid(x, y);
        }
    }
}
