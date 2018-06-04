using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RagnoMagicoAnimations : PawnAnimationManager
{
    public Transform ShootPosition;
    public float YOffset;
    Vector3 startRotation;
    Transform myPosition;
    Vector3 targetPosition;
    Vector3 lasertargetposition;

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
        float minz = 0;
        float maxz = 0;
        for (int i = 0; i < patternBox.Count; i++)
        {
            if (patternBox[i].transform.position.z <= 0)
                minz = patternBox[i].transform.position.z;
            else if (patternBox[i].transform.position.z > maxz)
            {
                maxz = patternBox[i].transform.position.z;
            }
        }
        lasertargetposition = new Vector3(targetPosition.x, targetPosition.y, (minz + maxz) / 2);
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

    #region VFX

    [Header("VFX References")]
    public GameObject bulletVFX;

    protected override void Start()
    {
        base.Start();
        bulletVFX.SetActive(false);
    }

    public IEnumerator Shoot()
    {
        bulletVFX.SetActive(true);
        Tween shoot = bulletVFX.transform.DOMove(targetPosition, 1f);
        yield return shoot.WaitForCompletion();
        bulletVFX.SetActive(false);
        bulletVFX.transform.position = ShootPosition.position;
        OnAttackEnd();
    }

    #endregion
}
