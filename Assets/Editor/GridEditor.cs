using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardsGenerator))]
[CanEditMultipleObjects]
public class GridEditor : Editor {
    private BoardsGenerator grid;
    private void OnEnable()
    {
        grid = (BoardsGenerator)target;
        grid.board1.DestroyGrid();
        grid.board1.oneTime = true;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        grid.CreateBoards();
    }
}
