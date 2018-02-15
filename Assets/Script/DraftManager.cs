using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DraftManager : MonoBehaviour
{

    public enum PlayerDraftTurn { P1_draft, P2_draft };
    public PlayerDraftTurn currentDraftTurn;

    public List<DraftPawn> pawns;
    public List<DraftPawn> P1_pawns;
    public List<DraftPawn> P2_pawns;
    BoardManager bm;

    public bool hasDrafted;

    public Image[] p1_picks, p2_picks;

    public GameObject draftButton, playButton;

    // Use this for initialization
    void Start()
    {

        pawns = FindObjectsOfType<DraftPawn>().ToList();
        currentDraftTurn = PlayerDraftTurn.P1_draft;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DraftRandomPattern()
    {
        int i = 0;
        if (!hasDrafted)
        {
            foreach (DraftPawn pawn in pawns)
            {
                pawns[i].RandomizeColor();
                i++;
            }
            hasDrafted = true;
            draftButton.SetActive(false);
        }
    }
}
