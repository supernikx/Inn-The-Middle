using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElicotteroAnimations : PawnAnimationManager
{
    public GameObject bombToMove;
    public Transform bombReturnPosition;
    public GameObject bombAttachedToModel;
    public float YHighOffset;
    Transform myPosition;
    Vector3 bombTargetPosition;
    Vector3 targetPosition;
    Vector3 startPosition;
    //Vector3 startRotation;
    float speed = 0.7f;
    bool isAttacking;

    protected override void Start()
    {
        base.Start();
        bombToMove.SetActive(false);
    }

    public override void AttackAnimation(Transform _myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        Vector3 _targetPosition = new Vector3();
        startPosition = _myPosition.position;
        float targetx = -1;
        float targetz = -1;
        if (patternBox.Count == 2)
        {
            targetz = patternBox[0].transform.position.z;
            targetx = patternBox[1].transform.position.x;
        }
        else
        {
            if (Mathf.Approximately(patternBox[0].transform.position.x, patternBox[1].transform.position.x))
            {
                targetx = patternBox[0].transform.position.x;
                targetz = patternBox[2].transform.position.z;
            }
            else if (Mathf.Approximately(patternBox[0].transform.position.x, patternBox[2].transform.position.x) || Mathf.Approximately(patternBox[1].transform.position.z, patternBox[2].transform.position.z))
            {
                targetx = patternBox[0].transform.position.x;
                targetz = patternBox[1].transform.position.z;
            }
            else if (Mathf.Approximately(patternBox[1].transform.position.x, patternBox[2].transform.position.x) || Mathf.Approximately(patternBox[0].transform.position.z, patternBox[2].transform.position.z))
            {
                targetx = patternBox[1].transform.position.x;
                targetz = patternBox[0].transform.position.z;
            }
            else if (Mathf.Approximately(patternBox[0].transform.position.z, patternBox[1].transform.position.z))
            {
                targetx = patternBox[2].transform.position.x;
                targetz = patternBox[0].transform.position.z;
            }
        }
        bombTargetPosition = new Vector3(targetx, patternBox[0].transform.position.y, targetz);
        _targetPosition = new Vector3 (bombTargetPosition.x, bombTargetPosition.y + YHighOffset, bombTargetPosition.z);
        isAttacking = true;
        MovementAnimation(_myPosition, _targetPosition, speed, startRotation);
    }

    private IEnumerator ReturnToPosition()
    {
        PlayMovementAnimation(false);
        Tween backmovement = myPosition.DOMove(startPosition, speed);
        yield return backmovement.WaitForCompletion();
        isAttacking = false;
        animator.SetBool("Back", false);
    }

    public IEnumerator LaunchBomb()
    {
        bombAttachedToModel.SetActive(false);
        bombToMove.SetActive(true);
        Tween fallBomb = bombToMove.transform.DOMove(bombTargetPosition, 1f);
        yield return fallBomb.WaitForCompletion();
        bombToMove.SetActive(false);
        bombToMove.transform.position = bombReturnPosition.position;
    }

    public void BombRespawn()
    {
        bombAttachedToModel.SetActive(true);
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        myPosition = _myPosition;
        targetPosition = _targetPosition;
        //startRotation = _startRotation;
        speed = _speed;
        animator.SetTrigger("TakeBomb");
        PlayMovementAnimation(true);
    }

    public IEnumerator OnStartMovementAnimationEnd()
    {
        Tween movement = myPosition.DOMove(targetPosition, speed);
        yield return movement.WaitForCompletion();
        //Tween rotate = myPosition.DORotate(startRotation, 1f);
        //yield return rotate.WaitForCompletion();
        if (!isAttacking)
        {
            PlayMovementAnimation(false);
            OnMovementEnd();
        }
        else
        {
            PlayAttackAnimation();
            animator.SetBool("Back", true);
        }
    }

    public override void PlayDamagedAnimation()
    {
        OnDamagedEnd();
    }
}
