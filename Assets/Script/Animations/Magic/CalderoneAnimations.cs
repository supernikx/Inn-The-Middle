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

    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        PlayAttackAnimation();
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
}
