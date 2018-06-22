using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolState
{
    inPool,
    inUse,
}

public class PoolParticleEffects
{
    public PoolState state;
    public ParticleSystem particle;
    public GameObject owner;

    public PoolParticleEffects(PoolState _state, ParticleSystem _particle)
    {
        state = _state;
        particle = _particle;
    }

    public void SetOwner(GameObject g)
    {
        owner = g;
    }
}

public class VFXManager : MonoBehaviour
{
    public Vector3 poolPosition;
    public GameObject selectDraftPawnParticlePrefab;
    public GameObject selectPawnParticlePrefab;
    public GameObject reDarftPawnParticlePrefab;
    public int maxReDraft;
    public GameObject attackMarkerParticlePrefab;
    public int maxMarker;
    public GameObject trapTilePrefab;
    public int maxTrapTile;
    public GameObject MagicDeathVFXPrefab;
    public int maxMagicDeathVFX;
    public GameObject ScienceDeathVFXPrefab;
    public int maxScienceDeathVFX;

    Transform parentSelected;
    Transform parentDraftSelected;
    Transform parentReDraftPawn;
    Transform parentMarker;
    Transform parentTrap;
    Transform parentMagicDeath;
    Transform parentScienceDeath;

    List<PoolParticleEffects> mark = new List<PoolParticleEffects>();
    List<PoolParticleEffects> trap = new List<PoolParticleEffects>();
    List<PoolParticleEffects> redraftpawn = new List<PoolParticleEffects>();
    List<PoolParticleEffects> magicdeathvfx = new List<PoolParticleEffects>();
    List<PoolParticleEffects> sciencedeathvfx = new List<PoolParticleEffects>();
    PoolParticleEffects select;
    PoolParticleEffects draftselect;
    

    private void Start()
    {
        parentMarker = new GameObject("AttackMarker").transform;
        parentMarker.parent = transform;
        for (int i = 0; i < maxMarker; i++)
        {
            ParticleSystem instantiedMarker = Instantiate(attackMarkerParticlePrefab, poolPosition, attackMarkerParticlePrefab.transform.rotation, parentMarker).GetComponent<ParticleSystem>();
            instantiedMarker.Stop();
            mark.Add(new PoolParticleEffects(PoolState.inPool, instantiedMarker));
        }

        parentTrap = new GameObject("TrapTile").transform;
        parentTrap.parent = transform;
        for (int i = 0; i < maxTrapTile; i++)
        {
            ParticleSystem instantietedTrap = Instantiate(trapTilePrefab, poolPosition, trapTilePrefab.transform.rotation, parentTrap).GetComponent<ParticleSystem>();
            instantietedTrap.Stop();
            trap.Add(new PoolParticleEffects(PoolState.inPool, instantietedTrap));
        }

        parentReDraftPawn = new GameObject("ReDraftPawn").transform;
        parentReDraftPawn.parent = transform;
        for (int i = 0; i < maxReDraft; i++)
        {
            ParticleSystem instantiatedReDraftPawn = Instantiate(reDarftPawnParticlePrefab, poolPosition, reDarftPawnParticlePrefab.transform.rotation, parentReDraftPawn).GetComponent<ParticleSystem>();
            instantiatedReDraftPawn.Stop();
            redraftpawn.Add(new PoolParticleEffects(PoolState.inPool, instantiatedReDraftPawn));
        }

        parentMagicDeath = new GameObject("MagicDeathVFX").transform;
        parentMagicDeath.parent = transform;
        for (int i = 0; i < maxMagicDeathVFX; i++)
        {
            ParticleSystem instantiatedReDraftPawn = Instantiate(MagicDeathVFXPrefab, poolPosition, MagicDeathVFXPrefab.transform.rotation, parentMagicDeath).GetComponent<ParticleSystem>();
            instantiatedReDraftPawn.Stop();
            magicdeathvfx.Add(new PoolParticleEffects(PoolState.inPool, instantiatedReDraftPawn));
        }

        parentScienceDeath = new GameObject("ScienceDeathVFX").transform;
        parentScienceDeath.parent = transform;
        for (int i = 0; i < maxScienceDeathVFX; i++)
        {
            ParticleSystem instantiatedReDraftPawn = Instantiate(ScienceDeathVFXPrefab, poolPosition, ScienceDeathVFXPrefab.transform.rotation, parentScienceDeath).GetComponent<ParticleSystem>();
            instantiatedReDraftPawn.Stop();
            sciencedeathvfx.Add(new PoolParticleEffects(PoolState.inPool, instantiatedReDraftPawn));
        }

        parentSelected = new GameObject("SelectedPawn").transform;
        parentSelected.parent = transform;
        ParticleSystem instantiatedSelected = Instantiate(selectPawnParticlePrefab, poolPosition, selectPawnParticlePrefab.transform.rotation, parentSelected).GetComponent<ParticleSystem>();
        instantiatedSelected.Stop();
        select = new PoolParticleEffects(PoolState.inPool, instantiatedSelected);

        parentDraftSelected = new GameObject("SelectedDraftPawn").transform;
        parentDraftSelected.parent = transform;
        ParticleSystem instantiatedDraftSelected = Instantiate(selectDraftPawnParticlePrefab, poolPosition, selectDraftPawnParticlePrefab.transform.rotation, parentDraftSelected).GetComponent<ParticleSystem>();
        instantiatedDraftSelected.Stop();
        draftselect = new PoolParticleEffects(PoolState.inPool, instantiatedDraftSelected);
    }

    public void MarkPawn(Vector3 pawnPosition)
    {
        if (CheckOtherMarker(pawnPosition))
        {
            foreach (PoolParticleEffects p in mark)
            {
                if (p.state == PoolState.inPool)
                {
                    p.particle.transform.position = pawnPosition;
                    p.particle.Play();
                    p.state = PoolState.inUse;
                    return;
                }
            }
        }
    }

    bool CheckOtherMarker(Vector3 _position)
    {
        foreach (PoolParticleEffects p in mark)
        {
            if (p.particle.transform.position.x == _position.x && p.particle.transform.position.y == _position.y && p.particle.transform.position.z == _position.z)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetMark()
    {
        foreach (PoolParticleEffects p in mark)
        {
            if (p.state == PoolState.inUse)
            {
                p.particle.Stop();
                p.particle.transform.position = poolPosition;
                p.state = PoolState.inPool;
            }
        }
    }

    public void SelectPawn(Pawn pawnSelected)
    {
        if (select.state == PoolState.inUse)
            DeselectPawn();
        select.particle.transform.position = new Vector3(pawnSelected.transform.position.x, pawnSelected.transform.position.y + 0.5f, pawnSelected.transform.position.z);
        select.particle.transform.parent = pawnSelected.transform;
        select.particle.Play();
        select.state = PoolState.inUse;
    }

    public void DeselectPawn()
    {
        select.particle.Stop();
        select.particle.transform.parent = parentSelected;
        select.particle.transform.position = poolPosition;
        select.state = PoolState.inPool;
    }

    public void TrapTile(GameObject tile, bool active)
    {
        if (active)
        {
            foreach (PoolParticleEffects t in trap)
            {
                if (t.state == PoolState.inPool)
                {
                    t.owner = tile;
                    t.particle.transform.position = tile.transform.position + new Vector3(0, 0.5f, 0);
                    t.particle.Play();
                    t.state = PoolState.inUse;
                    return;
                }
            }
        }
        else
        {
            foreach (PoolParticleEffects t in trap)
            {
                if (t.state == PoolState.inUse && t.owner == tile)
                {
                    t.owner = null;
                    t.particle.transform.position = poolPosition;
                    t.particle.Stop();
                    t.state = PoolState.inPool;
                    return;
                }
            }
        }
    }

    public void SelectDraftPawn(Vector3 pawndraftposition)
    {
        if (draftselect.state == PoolState.inUse)
            DeselectPawn();
        draftselect.particle.transform.position = pawndraftposition;
        draftselect.particle.Play();
        draftselect.state = PoolState.inUse;
    }

    public void DeselectDraftPawn()
    {
        draftselect.particle.Stop();
        draftselect.particle.transform.position = poolPosition;
        draftselect.state = PoolState.inPool;
    }

    public void ReDraftPawn(Vector3 _PawnPosition)
    {
        foreach (PoolParticleEffects p in redraftpawn)
        {
            if (p.state == PoolState.inPool)
            {
                p.state = PoolState.inUse;
                p.particle.transform.position = new Vector3(_PawnPosition.x, 4.5f, _PawnPosition.z);
                StartCoroutine(ReDraftPawnCoroutine(p));
                return;
            }
        }
    }

    private IEnumerator ReDraftPawnCoroutine(PoolParticleEffects _p)
    {
        _p.particle.Play();
        yield return new WaitForSeconds(_p.particle.main.duration);
        _p.particle.Stop();
        _p.state = PoolState.inPool;
    }

    public void DeathVFX (Vector3 pawnPosition, Factions faction)
    {
        switch (faction)
        {
            case Factions.Magic:
                foreach (PoolParticleEffects p in magicdeathvfx)
                {
                    if (p.state == PoolState.inPool)
                    {
                        p.state = PoolState.inUse;
                        p.particle.transform.position = pawnPosition;
                        StartCoroutine(DeathVFXCoroutine(p));
                        return;
                    }
                }
                break;
            case Factions.Science:
                foreach (PoolParticleEffects p in sciencedeathvfx)
                {
                    if (p.state == PoolState.inPool)
                    {
                        p.state = PoolState.inUse;
                        p.particle.transform.position = pawnPosition;
                        StartCoroutine(DeathVFXCoroutine(p));
                        return;
                    }
                }
                break;
        }
    }

    private IEnumerator DeathVFXCoroutine(PoolParticleEffects _p)
    {
        _p.particle.Play();
        yield return new WaitForSeconds(_p.particle.main.duration);
        _p.particle.Stop();
        _p.state = PoolState.inPool;
    }

}
