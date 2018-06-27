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
    public Transform VFXParent;
    public List<DraftPawn> DraftPawns;
    public List<Transform> DraftPawnsPositions;
    public List<GameObject> DraftPawnsParticles;

    public List<int> magic_pawns_picks;
    public List<int> science_pawns_picks;

    int draftpawnindex;
    int picksleft;
    BoardManager bm;
    public bool hasDrafted;
    public bool draftEnd, p1StartPressed, p2StartPressed;

    private void Start()
    {
        bm = GetComponent<BoardManager>();
        DraftPawns = new List<DraftPawn>();
        picksleft = 1;
        draftpawnindex = 0;
        draftEnd = false;
        hasDrafted = false;
        p1StartPressed = false;
        p2StartPressed = false;
    }

    /// <summary>
    /// Funzione che randomizza i pattern delle pedine nella fase di draft
    /// </summary>
    public void DraftRandomPattern()
    {
        if (!hasDrafted)
        {
            SoundManager.instance.StartDraft();
            foreach (Transform t in DraftPawnsPositions)
            {
                int indexNumber = Random.Range(0, DraftPawnsParticles.Count);
                GameObject instantiateddraftpawn = Instantiate(DraftPawnsParticles[indexNumber], t.position, new Quaternion(0, 0, 0, 0), VFXParent);
                DraftPawns.Add(new DraftPawn(instantiateddraftpawn, indexNumber));
                instantiateddraftpawn.GetComponent<ParticleSystem>().Play();
            }
            hasDrafted = true;
            bm.uiManager.UIChange();
            SelectNextDraftPawn(Directions.idle);
            bm.tutorial.DraftTutorial();
        }
    }

    /// <summary>
    /// Funzione che seleziona in base alla direction passata come parametro la prossima pedina
    /// </summary>
    /// <param name="direction"></param>
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
        bm.uiManager.UpdateDraftDissolvedChoose(DraftPawns[draftpawnindex].patternindex);
    }

    /// <summary>
    /// Funzione che seleziona per il giocatore di turno, la pedina evidenziata in questo momento
    /// </summary>
    public void ChooseSelectedDraftPawn()
    {
        SoundManager.instance.SelectDraftPawn();
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
        DraftPawns[draftpawnindex].pawnparticle.SetActive(false);
        DraftPawns.Remove(DraftPawns[draftpawnindex]);
        picksleft--;
        if (DraftPawns.Count == 1)
        {
            bm.turnManager.ChangeTurn();
            ChooseSelectedDraftPawn();
            return;
        }
        else if (DraftPawns.Count == 0)
        {
            bm.vfx.DeselectDraftPawn();
            draftEnd = true;
            bm.turnManager.ChangeTurn();
        }
        else if (picksleft == 0)
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
