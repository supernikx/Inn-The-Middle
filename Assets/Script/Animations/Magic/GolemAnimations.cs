using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GolemAnimations : PawnAnimationManager
{

    public GameObject rock;
    Vector3 rockStartPosition;
    int bounceCount;
    List<Vector3> bouncePositions = new List<Vector3>();

    protected override void Start()
    {
        base.Start();
        rock.SetActive(false);
        rockStartPosition = rock.transform.position;
    }

    public override void AttackAnimation(Transform myPosition, List<Box> patternBox, Vector3 startRotation)
    {
        myPosition.eulerAngles = startRotation;
        rockStartPosition = new Vector3(myPosition.position.x + 5.5f, myPosition.position.y + 5.5f, myPosition.position.z);
        PlayAttackAnimation();
        bouncePositions.Clear();
        bounceCount = 0;
        foreach (Box box in patternBox)
        {
            bouncePositions.Add(box.transform.position);
            bounceCount++;
        }
    }

    public IEnumerator LaunchRock()
    {
        rock.SetActive(true);
        Tween launch1 = rock.transform.DOJump(bouncePositions[0], 1.5f, 1, 0.25f);
        yield return launch1.WaitForCompletion();
        if (bounceCount > 1)
        {
            Tween launch2 = rock.transform.DOJump(bouncePositions[1], 1.3f, 1, 0.25f);
            yield return launch2.WaitForCompletion();
            if (bounceCount > 2)
            {
                Tween launch3 = rock.transform.DOJump(bouncePositions[2], 1.2f, 1, 0.25f);
                yield return launch3.WaitForCompletion();
            }
        }
        rock.SetActive(false);
        rock.transform.position = rockStartPosition;
        OnAttackEnd();
    }


    public override void MovementAnimation(Transform myPosition, Vector3 targetPosition, float speed)
    {
        PlayMovementAnimation(true);
        StartCoroutine(Movement(myPosition, targetPosition, speed));
    }

    private IEnumerator Movement(Transform _myPosition, Vector3 _targetPosition, float _speed)
    {

        Tween movement = _myPosition.DOMove(_targetPosition, _speed);
        yield return movement.WaitForCompletion();
        //Tween rotate = _myPosition.DORotate(_startRotation, 1f);
        //yield return rotate.WaitForCompletion();
        PlayMovementAnimation(false);
        OnMovementEnd();
    }
}
