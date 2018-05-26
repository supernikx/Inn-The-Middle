using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FantasmaAnimations : PawnAnimationManager
{
    Transform myposition;
    Vector3 targetPosition;
    Vector3 startRotation;
    float speed;

    public override void AttackAnimation(Transform _myPosition, List<Box> patternBox, Vector3 _startRotation)
    {
        myposition = _myPosition;
        startRotation = _startRotation;
        _myPosition.eulerAngles = _startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        myposition = _myPosition;
        targetPosition = _targetPosition;
        startRotation = _startRotation;
        speed = _speed;
        PlayMovementAnimation(true);
    }

    public IEnumerator MovementAnimationStart()
    {
        Tween movement=myposition.DOMove(targetPosition, speed);
        yield return movement.WaitForCompletion();
        if (myposition.eulerAngles.x == startRotation.x && myposition.eulerAngles.y == startRotation.y && myposition.eulerAngles.z == startRotation.z)
        {
            PlayMovementAnimation(false);
            OnMovementEnd();
        }
        else
        {
            PlayMovementAnimation(false);
            PlayJumpAnimation(true);
        }
    }

    private IEnumerator JumpRotation()
    {
        Tween rotate = myposition.DORotate(startRotation, 0.5f);
        yield return rotate.WaitForCompletion();
        PlayJumpAnimation(false);
        OnMovementEnd();
    }
}
