using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CalderoneAnimations : PawnAnimationManager
{
    Transform myPosition;
    Vector3 targetPosition;
    Vector3 startRotation;
    float speed;

    [Header("Sound References")]
    public AudioClip MovementClip;

    [Header("VFX References")]
    public ParticleSystem SpoonAttackVFX;
    public ParticleSystem SplashVFX;
    public ParticleSystem ShieldAttackVFX;
    public ParticleSystem PoolVFX;

    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        PlayAttackAnimation();
        StartCoroutine(StartSpoonAttack());
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        myPosition = _myPosition;
        targetPosition = _targetPosition;
        startRotation = _startRotation;
        speed = _speed;
        PlayMovementAnimation(true);        
    }

    private IEnumerator MovementJumpEnd()
    {
        SoundManager.instance.PawnSFX(MovementClip);
        Tween movement = myPosition.DOMove(targetPosition, speed);
        yield return movement.WaitForCompletion();
        if (myPosition.eulerAngles.x == startRotation.x && myPosition.eulerAngles.y == startRotation.y && myPosition.eulerAngles.z == startRotation.z)
        {
            PlayMovementAnimation(false);
            OnMovementEnd();
        }
        else
        {
            PlayJumpAnimation(true);
            PlayMovementAnimation(false);
        }
    }

    private IEnumerator JumpRotation()
    {
        Tween rotate = myPosition.DORotate(startRotation, 1f);
        yield return rotate.WaitForCompletion();
        PlayJumpAnimation(false);
        OnMovementEnd();
    }

    #region VFX

    public IEnumerator StartSpoonAttack()
    {
        yield return new WaitForSeconds(0.3f);
        SpoonAttackVFX.Play();
        yield return new WaitForSeconds(0.1f);        
        PoolVFX.Play();
        yield return new WaitForSeconds(0.1f);
        SplashVFX.Play();
        yield return new WaitForSeconds(0.45f);
        SpoonAttackVFX.Stop();
        SplashVFX.Stop();
        PoolVFX.Stop();
        yield return new WaitForSeconds(0.9f);
        ShieldAttackVFX.Play();
        yield return new WaitForSeconds(1f);
        ShieldAttackVFX.Stop();
    }

    #endregion
}
