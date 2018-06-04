using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotRagnoAnimations : PawnAnimationManager
{
    public float YOffset;
    public float ZOffset;
    Transform myPosition;
    Vector3 startRotation;
    Vector3 targetPosition;

    public override void AttackAnimation(Transform _myPosition, List<Box> patternBox, Vector3 _startRotation)
    {
        startRotation = _startRotation;
        myPosition = _myPosition;
        _myPosition.eulerAngles = _startRotation;
        int index = 0;
        for (int i = 0; i < patternBox.Count; i++)
        {            
            if (Mathf.Approximately(_myPosition.position.x, patternBox[i].transform.position.x))
            {
                index = i;
                break;
            }
        }
        targetPosition = new Vector3(patternBox[0].transform.position.x, patternBox[index].transform.position.y + YOffset, patternBox[index].transform.position.z);
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


    #region VFX

    [Header("VFX Reference")]
    public ParticleSystem AttackCharge;
    public GameObject ProjectileVFX;
    public ParticleSystem ExplosionVFX;
    public ParticleSystem ShootVFX;

    protected override void Start()
    {
        base.Start();
        AttackCharge.Stop();
        ProjectileVFX.SetActive(false);
        ShootVFX.Stop();
        ExplosionVFX.Stop();
    }

    public IEnumerator PlayChargeVFX()
    {
        AttackCharge.Play();
        yield return new WaitForSeconds(AttackCharge.main.duration);
        ShootVFX.Play();
        ProjectileVFX.SetActive(true);
        Tween shoot = ProjectileVFX.transform.DOMove(new Vector3(targetPosition.x+ZOffset, targetPosition.y,targetPosition.z), 0.6f);
        yield return shoot.WaitForCompletion();
        ProjectileVFX.SetActive(false);
        ExplosionVFX.transform.position = targetPosition;
        ExplosionVFX.Play();
        yield return new WaitForSeconds(1f);
        Debug.Log("laser");
        ShootVFX.Stop();
        ProjectileVFX.transform.position = AttackCharge.transform.position;
        ExplosionVFX.Stop();
        ExplosionVFX.transform.position = AttackCharge.transform.position;
        OnAttackEnd();
    }

    #endregion

}
