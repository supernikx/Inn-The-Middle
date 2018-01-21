using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {
    public bool selected;
    public Vector3 offset;
    private BoardManager bm;
    // Use this for initialization
    void Start () {
        bm = FindObjectOfType<BoardManager>();
        selected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        bm.PawnSelected(gameObject.GetComponent<Pawn>());
    }

    public void Move(int boxindex1, int boxindex2, int player)
    {
        Box currentBox = bm.GetPawnBox(transform.position);
        if (player == 1 && bm.boxCoordinates[boxindex1][boxindex2].tag == "Box1")
        {
            PawnMovement(boxindex1, boxindex2, currentBox);
        }
        else if (player == 2 && bm.boxCoordinates[boxindex1][boxindex2].tag == "Box2")
        {
            PawnMovement(boxindex1, boxindex2, currentBox);
        }
        selected = false;
    }

    private void PawnMovement(int boxindex1, int boxindex2, Box currentBox)
    {
            if (boxindex1 == currentBox.index1 + 1 && boxindex2 == currentBox.index2)
            {
                transform.position = bm.boxCoordinates[boxindex1][boxindex2].position + offset;
            }
            else if (boxindex1 == currentBox.index1 - 1 && boxindex2 == currentBox.index2)
            {
                transform.position = bm.boxCoordinates[boxindex1][boxindex2].position + offset;
            }
            else if (boxindex2 == currentBox.index2 + 1 && boxindex1 == currentBox.index1)
            {
                transform.position = bm.boxCoordinates[boxindex1][boxindex2].position + offset;
            }
            else if (boxindex2 == currentBox.index2 - 1 && boxindex1 == currentBox.index1)
            {
                transform.position = bm.boxCoordinates[boxindex1][boxindex2].position + offset;
            }
    }
}
