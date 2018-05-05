using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RagnoMagicoAnimations : PawnAnimationManager
{
    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed)
    {
        PlayMovementAnimation(true);
        StartCoroutine(Movement(myPosition, targetPosition, speed));
    }

    private IEnumerator Movement(Transform _myPosition, Vector3 _targetPosition, float _speed)
    {
        Tween move = _myPosition.DOMove(_targetPosition, _speed);
        yield return move.WaitForCompletion();
        //Tween rotate = _myPosition.DORotate(_startRotation, 1f);
        //yield return rotate.WaitForCompletion();
        PlayMovementAnimation(false);
        OnMovementEnd();
    }
}
