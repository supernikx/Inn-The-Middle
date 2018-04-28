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
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //grid.CreateBoards();
    }
}
