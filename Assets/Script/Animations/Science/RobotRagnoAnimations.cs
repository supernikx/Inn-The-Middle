using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotRagnoAnimations : PawnAnimationManager
{
    Transform myPosition;
    Vector3 startRotation;

    public override void AttackAnimation(Transform _myPosition, List<Box> patternBox, Vector3 _startRotation)
    {
        startRotation = _startRotation;
        myPosition = _myPosition;
        _myPosition.eulerAngles = _startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 targetPosition, float speed, Vector3 _startRotation)
    {
        PlayMovementAnimation(true);
        startRotation = _startRotation;
        myPosition = _myPosition;
        StartCoroutine(Movement(myPosition, targetPosition, speed, _startRotation));
    }

    private IEnumerator Movement(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        Tween movement = _myPosition.DOMove(_targetPosition, _speed);
        yield return movement.WaitForCompletion();
        if (_myPosition.eulerAngles.x == _startRotation.x && _myPosition.eulerAngles.y == _startRotation.y && _myPosition.eulerAngles.z == _startRotation.z)
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

    private IEnumerator JumpRotate()
    {
        Tween rotate = myPosition.DORotate(startRotation, 0.5f);
        yield return rotate.WaitForCompletion();
        PlayJumpAnimation(false);
        OnMovementEnd();
    }
}
