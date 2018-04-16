using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAnimationEvents
{
    public delegate void Events();
}

public interface IPawnAnimations
{
    event PawnAnimationEvents.Events OnAttackAnimationEnd;
    event PawnAnimationEvents.Events OnDeathAnimationEnd;
    event PawnAnimationEvents.Events OnDamagedAnimationEnd;
    void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation);
    void PlayDeathAnimation();
    void PlayDamagedAnimation();
    void MovementAnimation(bool _movementSet);
}

public abstract class PawnAnimationManager : MonoBehaviour, IPawnAnimations
{

    Animator animator;

    #region Events

    public event PawnAnimationEvents.Events OnAttackAnimationEnd;
    public event PawnAnimationEvents.Events OnDeathAnimationEnd;
    public event PawnAnimationEvents.Events OnDamagedAnimationEnd;

    public virtual void OnAttackEnd()
    {
        if (OnAttackAnimationEnd != null)
            OnAttackAnimationEnd();
    }
    public virtual void OnDeathEnd()
    {
        if (OnDeathAnimationEnd != null)
            OnDeathAnimationEnd();
    }
    public virtual void OnDamagedEnd()
    {
        if (OnDamagedAnimationEnd != null)
            OnDamagedAnimationEnd();
    }

    #endregion

    #region PlayAnimations

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    public void PlayDamagedAnimation()
    {
        animator.SetTrigger("Damage");
    }

    public void MovementAnimation(bool _movementSet)
    {
        animator.SetBool("Movement", _movementSet);
    }

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation);
}
