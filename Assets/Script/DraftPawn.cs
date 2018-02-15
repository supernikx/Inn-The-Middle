using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftPawn : MonoBehaviour
{


    public int[] pawnColorIndex;
    int indexNumber;

    public Color[] pawnColor = new Color[3];
    Color color;

    DraftManager dm;


    static int i, o;
    // Use this for initialization
    void Start()
    {
        dm = FindObjectOfType<DraftManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RandomizeColor()
    {
        indexNumber = pawnColorIndex[(Random.Range(0, pawnColor.Length))];
        Debug.Log(indexNumber);
        if (indexNumber == pawnColorIndex[0])
        {
            gameObject.GetComponent<MeshRenderer>().material.color = pawnColor[0];
        }
        else if (indexNumber == pawnColorIndex[1])
        {
            gameObject.GetComponent<MeshRenderer>().material.color = pawnColor[1];
        }
        else if (indexNumber == pawnColorIndex[2])
        {
            gameObject.GetComponent<MeshRenderer>().material.color = pawnColor[2];
        }
        else if (indexNumber == pawnColorIndex[3])
        {
            gameObject.GetComponent<MeshRenderer>().material.color = pawnColor[3];
        }
    }


    private void OnMouseDown()
    {
        if (dm.hasDrafted)
        {
            if (dm.currentDraftTurn == DraftManager.PlayerDraftTurn.P1_draft)
            {
                dm.P1_pawns.Add(this);
                dm.pawns.Remove(this);
                dm.p1_picks[i].color = this.gameObject.GetComponent<MeshRenderer>().material.color;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<MeshCollider>().enabled = false;
                i++;
                dm.currentDraftTurn = DraftManager.PlayerDraftTurn.P2_draft;
            }
            else if (dm.currentDraftTurn == DraftManager.PlayerDraftTurn.P2_draft)
            {
                dm.P2_pawns.Add(this);
                dm.pawns.Remove(this);
                dm.p2_picks[o].color = this.gameObject.GetComponent<MeshRenderer>().material.color;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<MeshCollider>().enabled = false;
                o++;
                dm.currentDraftTurn = DraftManager.PlayerDraftTurn.P1_draft;
            }

            if (dm.pawns.Count == 0)
            {
                dm.playButton.SetActive(true);
            }
        }
    }
}
