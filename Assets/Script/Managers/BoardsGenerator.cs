using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardsGenerator : MonoBehaviour
{
    //variabili pubbliche
    public int x;
    public int y;
    public GridGenerator board1, board2;
    public List<BoardPattern> boardPattern;

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che viene gestita dal GridEditor e che chiama la funzione GridGenerator per creare 2 board di grandezza uguale (x,y)
    /// </summary>
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

    #endregion
}

[System.Serializable]
public class BoardPattern
{
    public Element boxElement;
}

public enum Element { Purple, Orange, Azure, NeutralWhite, NeutralBlack };
