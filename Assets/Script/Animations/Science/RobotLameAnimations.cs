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
    Vector3 startrotation;
    Transform myposition;

    protected override void Start()
    {
        base.Start();
        Chainsaw.SetActive(false);
        ChainsawkStartPosition = Chainsaw.transform.position;
    }

    public override void AttackAnimation(Transform _myPosition, List<Box> patternBox, Vector3 _startRotation)
    {
        startrotation = _startRotation;
        myposition = _myPosition;
        _myPosition.eulerAngles = _startRotation;
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

    public override void MovementAnimation(Transform _myPosition, Vector3 targetPosition, float speed, Vector3 _startRotation)
    {
        myposition = _myPosition;
        startrotation = _startRotation;
        PlayMovementAnimation(true);
        StartCoroutine(Movement(targetPosition, speed, _startRotation));
    }

    private IEnumerator Movement( Vector3 _targetPosition, float _speed, Vector3 _startRotation)
    {
        Tween movement = myposition.DOMove(_targetPosition, _speed);
        yield return movement.WaitForCompletion();
        if (myposition.eulerAngles.x == startrotation.x && myposition.eulerAngles.y == startrotation.y && myposition.eulerAngles.z == startrotation.z)
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
        Tween rotate = myposition.DORotate(startrotation, 1f);
        yield return rotate.WaitForCompletion();
        PlayJumpAnimation(false);
        OnMovementEnd();
    }
}
