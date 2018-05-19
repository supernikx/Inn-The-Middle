using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotLameAnimations : PawnAnimationManager
{
    public GameObject rightHand;
    public GameObject Chainsaw;
    public Transform ChainsawLunchPosition;
    Vector3 ChainsawkStartPosition;
    int bounceCount;
    List<Vector3> bouncePositions = new List<Vector3>();

    protected override void Start()
    {
        base.Start();
        Chainsaw.SetActive(false);
        ChainsawkStartPosition = Chainsaw.transform.position;
    }

    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        ChainsawkStartPosition = Chainsaw.transform.position;
        bouncePositions.Clear();
        bounceCount = 0;
        foreach (Box box in patternBox)
        {
            bouncePositions.Add(box.transform.position + new Vector3(0f, 1f, 0f));
            bounceCount++;
        }
        PlayAttackAnimation();
    }

    public IEnumerator LaunchChainsaw()
    {
        Chainsaw.transform.position = ChainsawLunchPosition.position;
        rightHand.SetActive(false);
        Chainsaw.SetActive(true);
        Tween launch1 = Chainsaw.transform.DOJump(bouncePositions[0], 1.8f, 1, 0.3f);
        yield return launch1.WaitForCompletion();
        if (bounceCount > 1)
        {
            Tween launch2 = Chainsaw.transform.DOJump(bouncePositions[1], 1.5f, 1, 0.3f);
            yield return launch2.WaitForCompletion();
            if (bounceCount > 2)
            {
                Tween launch3 = Chainsaw.transform.DOJump(bouncePositions[2], 1.3f, 1, 0.3f);
                yield return launch3.WaitForCompletion();
            }
        }

        Tween back = Chainsaw.transform.DOJump(ChainsawkStartPosition, 7, 1, 0.7f);
        yield return back.WaitForCompletion();
        Chainsaw.SetActive(false);
        rightHand.SetActive(true);
        animator.SetTrigger("AttackEnd");
    }

    public override void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed, Vector3 _startRotation)
    {
        PlayMovementAnimation(true);
        StartCoroutine(Movement(myPosition, targetPosition, speed, _startRotation));
    }

    private IEnumerator Movement(Transform _myPosition, Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        Tween movement = _myPosition.DOMove(_targetPosition, _speed);
        yield return movement.WaitForCompletion();
        PlayMovementAnimation(false);
        //Tween rotate = _myPosition.DORotate(_startRotation, 1f);
        //yield return rotate.WaitForCompletion();
        OnMovementEnd();
    }
}
