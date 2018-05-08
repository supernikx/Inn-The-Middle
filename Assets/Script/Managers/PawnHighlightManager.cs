using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolState{
    inPool,
    inUse,
}

public class PawnAttackMarkEffect
{
    public PoolState state;
    public ParticleSystem particle;

    public PawnAttackMarkEffect(PoolState _state, ParticleSystem _particle)
    {
        state = _state;
        particle = _particle;
    }
}

public class PawnHighlightManager : MonoBehaviour {
    public Vector3 poolPosition;
    public GameObject attackMarkerParticlePrefab;
    public int maxMarker;
    
    List<PawnAttackMarkEffect> mark = new List<PawnAttackMarkEffect>();

    private void Start()
    {
        Transform parentMarker = new GameObject("AttackMarker").transform;
        parentMarker.parent = transform;
        for (int i = 0; i < maxMarker; i++)
        {
            ParticleSystem instantiedMarker = Instantiate(attackMarkerParticlePrefab, poolPosition, Quaternion.identity, parentMarker).GetComponent<ParticleSystem>();
            instantiedMarker.Stop();
            mark.Add(new PawnAttackMarkEffect(PoolState.inPool,instantiedMarker));
        }
    }

    public void MarkPawn(Vector3 pawnPosition)
    {
        foreach (PawnAttackMarkEffect p in mark)
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
        foreach (PawnAttackMarkEffect p in mark)
        {
            if (p.state == PoolState.inUse)
            {
                p.particle.Stop();
                p.particle.transform.position = poolPosition;
                p.state = PoolState.inPool;
            }
        }
    }

}
