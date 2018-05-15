using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolState
{
    inPool,
    inUse,
}

public class PawnPoolParticleEffect
{
    public PoolState state;
    public ParticleSystem particle;

    public PawnPoolParticleEffect(PoolState _state, ParticleSystem _particle)
    {
        state = _state;
        particle = _particle;
    }
}

public class PawnHighlightManager : MonoBehaviour
{
    public Vector3 poolPosition;
    public GameObject selectPawnParticlePrefab;
    public GameObject attackMarkerParticlePrefab;
    public int maxMarker;

    Transform parentSelected;
    Transform parentMarker;

    List<PawnPoolParticleEffect> mark = new List<PawnPoolParticleEffect>();
    PawnPoolParticleEffect select;

    private void Start()
    {
        parentMarker = new GameObject("AttackMarker").transform;
        parentMarker.parent = transform;
        for (int i = 0; i < maxMarker; i++)
        {
            ParticleSystem instantiedMarker = Instantiate(attackMarkerParticlePrefab, poolPosition, attackMarkerParticlePrefab.transform.rotation, parentMarker).GetComponent<ParticleSystem>();
            instantiedMarker.Stop();
            mark.Add(new PawnPoolParticleEffect(PoolState.inPool, instantiedMarker));
        }

        parentSelected = new GameObject("SelectedPawn").transform;
        parentSelected.parent = transform;
        ParticleSystem instantiatedSelected = Instantiate(selectPawnParticlePrefab, poolPosition, selectPawnParticlePrefab.transform.rotation, parentSelected).GetComponent<ParticleSystem>();
        instantiatedSelected.Stop();
        select = new PawnPoolParticleEffect(PoolState.inPool, instantiatedSelected);
    }

    public void MarkPawn(Vector3 pawnPosition)
    {
        foreach (PawnPoolParticleEffect p in mark)
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

    public void ResetMark()
    {
        foreach (PawnPoolParticleEffect p in mark)
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
        select.particle.transform.position = pawnSelected.transform.position;
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

}
