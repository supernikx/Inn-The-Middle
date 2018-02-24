using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftPawn : MonoBehaviour
{

    //variabili per confrontare un numero random e il colore da assegnare al pawn
    public int[] pawnColorIndex;
    int indexNumber;
    public Color[] pawnColor = new Color[3];
    Color color;

    DraftManager dm;
    TurnManager tm;
    static int i, o;

    static int picksLeft;
    // Use this for initialization
    void Start()
    {
        dm = FindObjectOfType<DraftManager>();
        tm = FindObjectOfType<TurnManager>();
        i = 0;
        o = 0;
        picksLeft = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RandomizeColor()
    {
        indexNumber = pawnColorIndex[(Random.Range(0, pawnColor.Length))];
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
            if (tm.CurrentPlayerTurn == TurnManager.PlayerTurn.P1_turn)
            {
                dm.p1_pawns_picks.Add(indexNumber);
                dm.pawns.Remove(this);
                dm.p1_picks[i].color = this.gameObject.GetComponent<MeshRenderer>().material.color;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<MeshCollider>().enabled = false;
                i++;
                picksLeft--;
                if (picksLeft == 0 || dm.pawns.Count == 0)
                {
                    tm.CurrentPlayerTurn = TurnManager.PlayerTurn.P2_turn;
                    picksLeft = 2;
                }
            }
            else if (tm.CurrentPlayerTurn == TurnManager.PlayerTurn.P2_turn)
            {
                dm.p2_pawns_picks.Add(indexNumber);
                dm.pawns.Remove(this);
                dm.p2_picks[o].color = this.gameObject.GetComponent<MeshRenderer>().material.color;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<MeshCollider>().enabled = false;
                o++;
                picksLeft--;
                if (picksLeft == 0 || dm.pawns.Count == 0)
                {
                    tm.CurrentPlayerTurn = TurnManager.PlayerTurn.P1_turn;
                    picksLeft = 2;
                }
            }
        }
    }


    private void OnMouseEnter()
    {
        if (dm.hasDrafted)
        {
            if (indexNumber == 0)
            {
                BoardManager.Instance.uiManager.tooltipPattern.SetActive(true);
                BoardManager.Instance.uiManager.L.enabled = true;
                BoardManager.Instance.uiManager.T.enabled = false;
                BoardManager.Instance.uiManager.diagonal.enabled = false;
                BoardManager.Instance.uiManager.cross.enabled = false;
            }
            else if (indexNumber == 1)
            {
                BoardManager.Instance.uiManager.tooltipPattern.SetActive(true);
                BoardManager.Instance.uiManager.T.enabled = true;
                BoardManager.Instance.uiManager.L.enabled = false;
                BoardManager.Instance.uiManager.diagonal.enabled = false;
                BoardManager.Instance.uiManager.cross.enabled = false;
            }
            else if (indexNumber == 2)
            {
                BoardManager.Instance.uiManager.tooltipPattern.SetActive(true);
                BoardManager.Instance.uiManager.diagonal.enabled = true;
                BoardManager.Instance.uiManager.L.enabled = false;
                BoardManager.Instance.uiManager.T.enabled = false;
                BoardManager.Instance.uiManager.cross.enabled = false;
            }
            else if (indexNumber == 3)
            {
                BoardManager.Instance.uiManager.tooltipPattern.SetActive(true);
                BoardManager.Instance.uiManager.cross.enabled = true;
                BoardManager.Instance.uiManager.L.enabled = false;
                BoardManager.Instance.uiManager.T.enabled = false;
                BoardManager.Instance.uiManager.diagonal.enabled = false;
            }
        }
    }

    private void OnMouseExit()
    {
        BoardManager.Instance.uiManager.tooltipPattern.SetActive(false);
    }
}
