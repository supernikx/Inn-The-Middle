using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DraftPawn {
    public GameObject pawnparticle;
    public int patternindex;

    public DraftPawn (GameObject particle, int _patternindex)
    {
        pawnparticle = particle;
        patternindex = _patternindex;
    }
}

public class DraftManager : MonoBehaviour
{
    public List<DraftPawn> DraftPawns;
    public List<Transform> DraftPawnsPositions;
    public List<GameObject> DraftPawnsParticles;

    public List<int> magic_pawns_picks;
    public List<int> science_pawns_picks;

    int draftpawnindex;
    int picksleft;
    BoardManager bm;
    public bool hasDrafted;
    public Image[] p1_picks, p2_picks;
    public GameObject draftButton;

    private void Start()
    {
        bm = GetComponent<BoardManager>();
        DraftPawns = new List<DraftPawn>();
        picksleft = 1;
        draftpawnindex = 0;
    }

    public void DraftRandomPattern()
    {
        if (!hasDrafted)
        {
            foreach (Transform t in DraftPawnsPositions)
            {
                int indexNumber = Random.Range(0, DraftPawnsParticles.Count);
                GameObject instantiateddraftpawn = Instantiate(DraftPawnsParticles[indexNumber], t.position, DraftPawnsParticles[indexNumber].transform.rotation);
                DraftPawns.Add(new DraftPawn(instantiateddraftpawn, indexNumber));
                instantiateddraftpawn.GetComponent<ParticleSystem>().Play();
            }
            hasDrafted = true;
            draftButton.SetActive(false);
            SelectNextDraftPawn(Directions.idle);
        }
    }

    public void SelectNextDraftPawn(Directions direction)
    {
        switch (direction)
        {
            case Directions.right:
                draftpawnindex++;
                if (draftpawnindex > DraftPawns.Count - 1)
                    draftpawnindex = DraftPawns.Count - 1;
                break;
            case Directions.left:
                draftpawnindex--;
                if (draftpawnindex < 0)
                    draftpawnindex = 0;
                break;
            case Directions.idle:
                if (draftpawnindex > DraftPawns.Count - 1)
                    draftpawnindex = DraftPawns.Count - 1;
                else if (draftpawnindex < 0)
                    draftpawnindex = 0;
                break;
        }
        bm.vfx.SelectDraftPawn(DraftPawns[draftpawnindex].pawnparticle.transform.position);
    }

    public void ChooseSelectedDraftPawn()
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                magic_pawns_picks.Add(DraftPawns[draftpawnindex].patternindex);
                break;
            case Factions.Science:
                science_pawns_picks.Add(DraftPawns[draftpawnindex].patternindex);
                break;
        }
        DraftPawns[draftpawnindex].pawnparticle.GetComponent<ParticleSystem>().Stop();
        DraftPawns.Remove(DraftPawns[draftpawnindex]);
        picksleft--;
        if (picksleft == 0 || DraftPawns.Count == 0)
        {
            bm.turnManager.ChangeTurn();
            picksleft = 2;
        }
        else
        {
            SelectNextDraftPawn(Directions.idle);
        }
    }
}
