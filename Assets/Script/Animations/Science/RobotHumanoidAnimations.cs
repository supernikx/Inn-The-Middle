using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotHumanoidAnimations : PawnAnimationManager
{
    Vector3 startRotation;
    Transform myPosition;

    [Header("VFX Refrences")]
    public ParticleSystem AttackVFX;

    protected override void Start()
    {
        base.Start();
        AttackVFX.Stop();
    }

    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 targetPosition, float speed, Vector3 _startRotation)
    {
        PlayMovementAnimation(true);
        startRotation = _startRotation;
        myPosition = _myPosition;
        StartCoroutine(Movement(targetPosition, speed));
    }

    public IEnumerator AttackVFXWave()
    {
        AttackVFX.Play();
        yield return new WaitForSeconds(AttackVFX.main.duration);
        AttackVFX.Stop();
        OnAttackEnd();
    }

    private IEnumerator Movement(Vector3 _targetPosition, float _speed)
    {
        Tween movement = myPosition.DOMove(_targetPosition, _speed);
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

    private IEnumerator JumpRotate()
    {
        Tween rotate = myPosition.DORotate(startRotation, 1f);
        yield return rotate.WaitForCompletion();
        PlayJumpAnimation(false);
        OnMovementEnd();
    }
}
