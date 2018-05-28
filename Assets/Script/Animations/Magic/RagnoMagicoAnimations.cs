using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RagnoMagicoAnimations : PawnAnimationManager
{
    Vector3 startRotation;
    Transform myPosition;

    public override void AttackAnimation(Transform _myPosition, List<Box> patternBox, Vector3 _startRotation)
    {
        startRotation = _startRotation;
        myPosition = _myPosition;
        _myPosition.eulerAngles = _startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 targetPosition, float speed, Vector3 _startRotation)
    {
        startRotation = _startRotation;
        myPosition = _myPosition;
        PlayMovementAnimation(true);
        StartCoroutine(Movement(targetPosition, speed));
    }

    private IEnumerator Movement(Vector3 _targetPosition, float _speed)
    {
        Tween move = myPosition.DOMove(_targetPosition, _speed);
        yield return move.WaitForCompletion();
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
        Tween rotate = myPosition.DORotate(startRotation, 0.5f);
        yield return rotate.WaitForCompletion();
        PlayJumpAnimation(false);
        OnMovementEnd();
    }
}
