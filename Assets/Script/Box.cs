using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    private BoardManager bm;
    public int index1,index2;
	// Use this for initialization
	void Start () {
        bm = FindObjectOfType<BoardManager>();
    }

    private void OnMouseDown()
    {
       bm.BoxClicked(index1,index2);
    }
}
