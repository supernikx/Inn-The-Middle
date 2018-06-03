using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FantasmaAnimations : PawnAnimationManager
{
    public float fireballYOffset;
    Transform myposition;
    Vector3 targetPosition;
    List<Box> patternBox = new List<Box>();
    Vector3 startRotation;
    float speed;

    public override void AttackAnimation(Transform _myPosition, List<Box> _patternBox, Vector3 _startRotation)
    {
        myposition = _myPosition;
        startRotation = _startRotation;
        _myPosition.eulerAngles = _startRotation;
        float targetx = -1;
        float targetz = -1;
        if (_patternBox.Count == 2)
        {
            targetz = _patternBox[0].transform.position.z;
            targetx = _patternBox[1].transform.position.x;
        }
        else
        {
            if (Mathf.Approximately(_patternBox[0].transform.position.x, _patternBox[1].transform.position.x))
            {
                targetx = _patternBox[0].transform.position.x;
                targetz = _patternBox[2].transform.position.z;
            }
            else if (Mathf.Approximately(_patternBox[0].transform.position.x, _patternBox[2].transform.position.x) || Mathf.Approximately(_patternBox[1].transform.position.z, _patternBox[2].transform.position.z))
            {
                targetx = _patternBox[0].transform.position.x;
                targetz = _patternBox[1].transform.position.z;
            }
            else if (Mathf.Approximately(_patternBox[1].transform.position.x, _patternBox[2].transform.position.x) || Mathf.Approximately(_patternBox[0].transform.position.z, _patternBox[2].transform.position.z))
            {
                targetx = _patternBox[1].transform.position.x;
                targetz = _patternBox[0].transform.position.z;
            }
            else if (Mathf.Approximately(_patternBox[0].transform.position.z, _patternBox[1].transform.position.z))
            {
                targetx = _patternBox[2].transform.position.x;
                targetz = _patternBox[0].transform.position.z;
            }
        }
        patternBox = _patternBox;
        targetPosition = new Vector3(targetx, _patternBox[0].transform.position.y + fireballYOffset, targetz);
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        myposition = _myPosition;
        targetPosition = _targetPosition;
        startRotation = _startRotation;
        speed = _speed;
        PlayMovementAnimation(true);
    }

    public IEnumerator MovementAnimationStart()
    {
        Tween movement = myposition.DOMove(targetPosition, speed);
        yield return movement.WaitForCompletion();
        if (myposition.eulerAngles.x == startRotation.x && myposition.eulerAngles.y == startRotation.y && myposition.eulerAngles.z == startRotation.z)
        {
            PlayMovementAnimation(false);
            OnMovementEnd();
        }
        else
        {
            PlayMovementAnimation(false);
            PlayJumpAnimation(true);
        }
    }

    private IEnumerator JumpRotation()
    {
        Tween rotate = myposition.DORotate(startRotation, 0.5f);
        yield return rotate.WaitForCompletion();
        PlayJumpAnimation(false);
        OnMovementEnd();
    }

    #region VFX

    [Header("VFX References")]
    public GameObject RIdleFireBall;
    public GameObject LIdleFireBall;
    public GameObject RLaunchFireBall;
    public GameObject LLaunchFireBall;

    protected override void Start()
    {
        base.Start();
        RIdleFireBall.SetActive(true);
        LIdleFireBall.SetActive(true);
        RLaunchFireBall.SetActive(false);
        LLaunchFireBall.SetActive(false);
    }

    public IEnumerator LauncheFireBalls()
    {
        RIdleFireBall.SetActive(false);
        RLaunchFireBall.SetActive(true);
        RLaunchFireBall.transform.DOMove(targetPosition, 1.2f);
        yield return new WaitForSeconds(0.3f);
        LIdleFireBall.SetActive(false);
        LLaunchFireBall.SetActive(true);
        Tween secondlaunch = LLaunchFireBall.transform.DOMove(targetPosition, 0.9f);
        yield return secondlaunch.WaitForCompletion();
        RLaunchFireBall.SetActive(false);
        LLaunchFireBall.SetActive(false);
        LIdleFireBall.SetActive(true);
        RIdleFireBall.SetActive(true);
        RLaunchFireBall.transform.position = RIdleFireBall.transform.position;
        LLaunchFireBall.transform.position = LIdleFireBall.transform.position;
        OnAttackEnd();
    }

    #endregion
}
