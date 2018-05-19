using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotRagnoAnimations : PawnAnimationManager
{
    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed, Vector3 _startRotation)
    {
        PlayMovementAnimation(true);
        StartCoroutine(Movement(myPosition, targetPosition, speed, _startRotation));
    }

    private IEnumerator Movement(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        Tween movement = _myPosition.DOMove(_targetPosition, _speed);
        yield return movement.WaitForCompletion();
        //Tween rotate = _myPosition.DORotate(_startRotation, 1f);
        //yield return rotate.WaitForCompletion();
        PlayMovementAnimation(false);
        OnMovementEnd();
    }
}
