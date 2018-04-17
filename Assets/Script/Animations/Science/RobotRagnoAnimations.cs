using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotRagnoAnimations : PawnAnimationManager
{
    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        OnAttackEnd();
    }

    public override void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed)
    {
        myPosition.DOMove(targetPosition, speed);
        OnMovementEnd();
    }
}
