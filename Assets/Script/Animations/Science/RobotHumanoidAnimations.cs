using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotHumanoidAnimations : PawnAnimationManager
{
    Vector3 _startRotation;
    Transform _myPosition;

    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed, Vector3 startRotation)
    {
        PlayMovementAnimation(true);
        _startRotation = startRotation;
        _myPosition = myPosition;
        StartCoroutine(Movement(targetPosition, speed));
    }

    private IEnumerator Movement(Vector3 _targetPosition, float _speed)
    {
        Tween movement = _myPosition.DOMove(_targetPosition, _speed);
        yield return movement.WaitForCompletion();
        if (_myPosition.eulerAngles.x == _startRotation.x && _myPosition.eulerAngles.y == _startRotation.y && _myPosition.eulerAngles.z == _startRotation.z)
        {
            OnMovementEnd();
        }
        else
        {           
            PlayJumpAnimation();
        }
        PlayMovementAnimation(false);
    }

    private IEnumerator JumpRotate()
    {
        Tween rotate = _myPosition.DORotate(_startRotation, 1f);
        yield return rotate.WaitForCompletion();
        OnMovementEnd();
    }
}
