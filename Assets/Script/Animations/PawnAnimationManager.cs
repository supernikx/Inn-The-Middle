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
    void MovementAnimation(Transform myPosition, Vector3 targetPosition, float movementSpeed, Vector3 startRotation);
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

    /// <summary>
    /// Funzione che esegue l'animazione d'attacco
    /// </summary>
    public void PlayAttackAnimation()
    {
        if (animator.runtimeAnimatorController != null)
            animator.SetTrigger("Attack");
        else
            OnAttackEnd();
    }

    /// <summary>
    /// Funzione che esegue l'animazione di morte
    /// </summary>
    public void PlayDeathAnimation()
    {
        if (animator.runtimeAnimatorController != null)
            animator.SetTrigger("Death");
        else
            OnDeathEnd();
    }

    /// <summary>
    /// Funzione che esegue l'animazione di danneggiamento
    /// </summary>
    public virtual void PlayDamagedAnimation()
    {
        if (animator.runtimeAnimatorController != null)
            animator.SetTrigger("Damage");
        else
            OnDamagedEnd();
    }

    /// <summary>
    /// Funzione che attiva/disattiva l'animazione di movimento
    /// </summary>
    /// <param name="_movementSet"></param>
    public void PlayMovementAnimation(bool _movementSet)
    {
        if (animator.runtimeAnimatorController != null)
            animator.SetBool("Movement", _movementSet);
        else
            OnMovementEnd();
    }

    public void PlayJumpAnimation()
    {
        if (animator.runtimeAnimatorController != null)
            animator.SetTrigger("Jump");
        else
            OnDamagedEnd();
    }

    #endregion

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation);
    public abstract void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed, Vector3 startRotation);
}
