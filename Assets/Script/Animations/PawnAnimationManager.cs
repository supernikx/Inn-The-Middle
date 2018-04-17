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
    event PawnAnimationEvents.Events OnMovementAnimationEnd;
    void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation);
    void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed);
    void PlayDeathAnimation();
    void PlayDamagedAnimation();
}

public abstract class PawnAnimationManager : MonoBehaviour, IPawnAnimations
{

    protected Animator animator;

    #region Events

    public event PawnAnimationEvents.Events OnAttackAnimationEnd;
    public event PawnAnimationEvents.Events OnDeathAnimationEnd;
    public event PawnAnimationEvents.Events OnDamagedAnimationEnd;
    public event PawnAnimationEvents.Events OnMovementAnimationEnd;

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
    public virtual void OnMovementEnd()
    {
        if (OnMovementAnimationEnd != null)
            OnMovementAnimationEnd();
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
    public abstract void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed);
}
