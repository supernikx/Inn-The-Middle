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

    public override void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed)
    {
        MovementAnimation(true);
        myPosition.DOMove(targetPosition, speed).OnComplete(MovementAnimationEnd);
    }

    private void MovementAnimationEnd()
    {
        MovementAnimation(false);
        OnMovementEnd();
    }
}
