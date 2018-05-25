using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DraftPawn
{
    public GameObject pawnparticle;
    public int patternindex;

    public DraftPawn(GameObject particle, int _patternindex)
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
                GameObject instantiateddraftpawn = Instantiate(DraftPawnsParticles[indexNumber], t.position, DraftPawnsParticles[indexNumber].transform.rotation, transform);
                DraftPawns.Add(new DraftPawn(instantiateddraftpawn, indexNumber));
                instantiateddraftpawn.GetComponent<ParticleSystem>().Play();
            }
            hasDrafted = true;
            bm.uiManager.UIChange();
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
            case Directions.up:
                int pastindexup = draftpawnindex;
                draftpawnindex -= DraftPawns.Count / 2;
                if (draftpawnindex < 0)
                    draftpawnindex = pastindexup;
                else if (draftpawnindex > DraftPawns.Count - 1)
                    draftpawnindex = pastindexup;
                break;
            case Directions.down:
                int pastindexdown = draftpawnindex;
                draftpawnindex += DraftPawns.Count / 2;
                if (draftpawnindex < 0)
                    draftpawnindex = pastindexdown;
                else if (draftpawnindex > DraftPawns.Count - 1)
                    draftpawnindex = pastindexdown;
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
        bm.uiManager.UpdateDraftChoose();
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
