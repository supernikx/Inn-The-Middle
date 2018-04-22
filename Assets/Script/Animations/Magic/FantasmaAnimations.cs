using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FantasmaAnimations : PawnAnimationManager
{
    Transform myPosition;
    Vector3 targetPosition;
    float speed;

    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed)
    {
        myPosition = _myPosition;
        targetPosition = _targetPosition;
        speed = _speed;
        MovementAnimation(true);
    }

    public void MovementAnimationStart()
    {
        myPosition.DOMove(targetPosition, speed).OnComplete(MovementAnimationEnd);
    }

    private void MovementAnimationEnd()
    {
        MovementAnimation(false);
        OnMovementEnd();
    }
}
