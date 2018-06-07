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
    public ParticleSystem ExplosionVFX;
    public ParticleSystem TileExplosionVFXPrefab;
    public ParticleSystem RubbleVFXPrefab;
    List<ParticleSystem> RubbleVFX;
    List<ParticleSystem> TileExplosionVFX;

    protected override void Start()
    {
        base.Start();
        RIdleFireBall.SetActive(true);
        LIdleFireBall.SetActive(true);
        RLaunchFireBall.SetActive(false);
        LLaunchFireBall.SetActive(false);
        ExplosionVFX.Stop();
        RubbleVFX = new List<ParticleSystem>();
        TileExplosionVFX = new List<ParticleSystem>();
        for (int i = 0; i < 4; i++)
        {
            ParticleSystem vfxinstantiated = Instantiate(RubbleVFXPrefab, transform);
            vfxinstantiated.Stop();
            RubbleVFX.Add(vfxinstantiated);
            vfxinstantiated = Instantiate(TileExplosionVFXPrefab, transform);
            vfxinstantiated.Stop();
            TileExplosionVFX.Add(vfxinstantiated);
        }
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
        ExplosionVFX.transform.position = targetPosition;
        ExplosionVFX.Play();
        for (int i = 0; i < patternBox.Count; i++)
        {
            RubbleVFX[i].transform.position = targetPosition;
            RubbleVFX[i].Play();
            RubbleVFX[i].transform.DOJump(patternBox[i].transform.position, 4.5f, 1, 0.8f);
        }
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < patternBox.Count; i++)
        {
            RubbleVFX[i].Stop();
            TileExplosionVFX[i].transform.position = RubbleVFX[i].transform.position + new Vector3(0, 1f, 0);
            TileExplosionVFX[i].Play();
        }
        yield return new WaitForSeconds(1f);
        ExplosionVFX.Stop();
        foreach (ParticleSystem vfx in TileExplosionVFX)
        {
            vfx.Stop();
        }
        LIdleFireBall.SetActive(true);
        RIdleFireBall.SetActive(true);
        RLaunchFireBall.transform.position = RIdleFireBall.transform.position;
        LLaunchFireBall.transform.position = LIdleFireBall.transform.position;
        OnAttackEnd();
    }

    #endregion
}
