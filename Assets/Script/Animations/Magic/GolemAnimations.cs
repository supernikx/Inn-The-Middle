using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GolemAnimations : PawnAnimationManager
{
    public ParticleSystem bounceVFXeffect;
    List<ParticleSystem> bounceeffects;
    public GameObject rock;
    Transform myposition;
    Vector3 startrotation;
    Vector3 rockStartPosition;
    int bounceCount;
    List<Vector3> bouncePositions = new List<Vector3>();

    protected override void Start()
    {
        base.Start();
        rock.SetActive(false);
        rockStartPosition = rock.transform.position;
        bounceeffects = new List<ParticleSystem>();
        for (int i = 0; i < 3; i++)
        {
            bounceeffects.Add(Instantiate(bounceVFXeffect, transform));
            bounceeffects[i].Stop();
        }
    }

    public override void AttackAnimation(Transform _myPosition, List<Box> patternBox, Vector3 _startRotation)
    {
        startrotation = _startRotation;
        myposition = _myPosition;
        _myPosition.eulerAngles = _startRotation;
        rockStartPosition = rock.transform.position;
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
        bounceeffects[0].transform.position = bouncePositions[0];
        bounceeffects[0].Play();
        if (bounceCount > 1)
        {
            Tween launch2 = rock.transform.DOJump(bouncePositions[1], 1.3f, 1, 0.25f);
            yield return launch2.WaitForCompletion();
            bounceeffects[1].transform.position = bouncePositions[1];
            bounceeffects[1].Play();
            if (bounceCount > 2)
            {
                Tween launch3 = rock.transform.DOJump(bouncePositions[2], 1.2f, 1, 0.25f);
                yield return launch3.WaitForCompletion();
                bounceeffects[2].transform.position = bouncePositions[2];
                bounceeffects[2].Play();
            }
        }
        rock.SetActive(false);
        rock.transform.position = rockStartPosition;
        StartCoroutine(ResetVFX());
        OnAttackEnd();
    }

    IEnumerator ResetVFX()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 3; i++)
        {
            bounceeffects[i].Stop();
        }
    }

    public override void MovementAnimation(Transform _myPosition, Vector3 targetPosition, float speed, Vector3 _startRotation)
    {
        startrotation = _startRotation;
        myposition = _myPosition;
        PlayMovementAnimation(true);
        StartCoroutine(Movement(targetPosition, speed));
    }

    private IEnumerator Movement(Vector3 _targetPosition, float _speed)
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
