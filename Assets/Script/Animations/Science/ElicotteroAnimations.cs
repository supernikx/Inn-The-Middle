using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElicotteroAnimations : PawnAnimationManager
{
    public GameObject bombToMove;
    public Transform bombReturnPosition;
    public float YHighOffset;
    Transform myPosition;
    Vector3 bombTargetPosition;
    Vector3 targetPosition;
    Vector3 startPosition;
    Vector3 startRotation;
    float speed = 0.7f;

    [Header("Sound References")]
    public AudioClip MovementClip;

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
        _targetPosition = new Vector3(bombTargetPosition.x, bombTargetPosition.y + YHighOffset, bombTargetPosition.z);
        MovementAnimationAttack(_myPosition, _targetPosition, speed, startRotation);
    }

    private IEnumerator ReturnToPosition()
    {
        animator.SetBool("MovementAttack", false);
        animator.SetBool("Back", true);
        SoundManager.instance.PawnSFX(MovementClip);
        Tween backmovement = myPosition.DOMove(startPosition, speed);
        yield return backmovement.WaitForCompletion();
        animator.SetBool("Back", false);
    }

    public IEnumerator LaunchBomb()
    {
        bombToMove.SetActive(true);
        Tween fallBomb = bombToMove.transform.DOMove(bombTargetPosition, 1f);
        yield return fallBomb.WaitForCompletion();
        bombToMove.SetActive(false);
        bombToMove.transform.position = bombReturnPosition.position;
    }

    public void MovementAnimationAttack(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        myPosition = _myPosition;
        targetPosition = _targetPosition;
        startRotation = _startRotation;
        speed = _speed;
        animator.SetBool("MovementAttack", true);
        StartCoroutine(MoveAttack());
    }

    public IEnumerator MoveAttack()
    {
        SoundManager.instance.PawnSFX(MovementClip);
        Tween movementattack = myPosition.DOMove(targetPosition, speed);
        yield return movementattack.WaitForCompletion();
        animator.SetBool("MovementAttack", true);
        PlayAttackAnimation();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        myPosition = _myPosition;
        targetPosition = _targetPosition;
        startRotation = _startRotation;
        speed = _speed;
        PlayMovementAnimation(true);
        StartCoroutine(Movement());
    }

    public IEnumerator Movement()
    {
        SoundManager.instance.PawnSFX(MovementClip);
        Tween movement = myPosition.DOMove(targetPosition, speed);
        yield return movement.WaitForCompletion();
        PlayMovementAnimation(false);
        if (myPosition.eulerAngles.x == startRotation.x && myPosition.eulerAngles.y == startRotation.y && myPosition.eulerAngles.z == startRotation.z)
        {
            OnMovementEnd();
        }
        else
        {
            StartCoroutine(JumpRotate());
        }
    }

    private IEnumerator JumpRotate()
    {
        Tween rotate = myPosition.DORotate(startRotation, 1f);
        yield return rotate.WaitForCompletion();
        OnMovementEnd();
    }
}
