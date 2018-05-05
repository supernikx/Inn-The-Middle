using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElicotteroAnimations : PawnAnimationManager
{
    Transform myPosition;
    Vector3 targetPosition;
    Vector3 startPosition;
    //Vector3 startRotation;
    float speed = 0.7f;
    bool isAttacking;

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
        _targetPosition = new Vector3(targetx, patternBox[0].transform.position.y, targetz);
        isAttacking = true;
        MovementAnimation(_myPosition, _targetPosition, speed);
    }

    private IEnumerator ReturnToPosition()
    {
        PlayMovementAnimation(false);
        animator.SetBool("Back", true);
        Tween backmovement = myPosition.DOMove(startPosition, speed);
        yield return backmovement.WaitForCompletion();
        isAttacking = false;
        animator.SetBool("Back", false);
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed)
    {
        myPosition = _myPosition;
        targetPosition = _targetPosition;
        //startRotation = _startRotation;
        speed = _speed;
        animator.SetTrigger("TakeBomb");
        animator.SetTrigger("SMovement");
    }

    public IEnumerator OnStartMovementAnimationEnd()
    {
        PlayMovementAnimation(true);
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
        }
    }

    public override void PlayDamagedAnimation()
    {
        OnDamagedEnd();
    }
}
