using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GolemAnimations : PawnAnimationManager
{

    public GameObject rock;
    Vector3 rockStartPosition;
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
        foreach (Box box in patternBox)
        {
            bouncePositions.Add(box.transform.position);
        }
    }

    public IEnumerator LaunchRock()
    {
        rock.SetActive(true);
        Tween launch1 = rock.transform.DOJump(bouncePositions[0], 1.5f, 1, 0.25f);
        yield return launch1.WaitForCompletion();
        Tween launch2 = rock.transform.DOJump(bouncePositions[1], 1.3f, 1, 0.25f);
        yield return launch2.WaitForCompletion();
        Tween launch3 = rock.transform.DOJump(bouncePositions[2], 1.2f, 1, 0.25f);
        yield return launch3.WaitForCompletion();
        rock.SetActive(false);
        rock.transform.position = rockStartPosition;
        OnAttackEnd();
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 _targetPosition, float _speed)
    {
        MovementAnimation(true);
        StartCoroutine(Movement(_myPosition, _targetPosition, _speed));
    }

    private IEnumerator Movement(Transform _myPosition, Vector3 _targetPosition, float _speed)
    {
        Tween movement = _myPosition.DOMove(_targetPosition, _speed);
        yield return movement.WaitForCompletion();
        MovementAnimation(false);
        OnMovementEnd();
    }
}
