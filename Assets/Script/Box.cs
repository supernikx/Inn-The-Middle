using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    private BoardManager bm;
    public int index1,index2,board;
    public bool walkable;
	// Use this for initialization
	void Start () {
        walkable = true;
        bm = FindObjectOfType<BoardManager>();
    }

    private void OnMouseDown()
    {
       bm.BoxClicked(gameObject.GetComponent<Box>());
    }
}
