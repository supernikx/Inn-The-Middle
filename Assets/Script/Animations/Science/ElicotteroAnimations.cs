using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElicotteroAnimations : PawnAnimationManager
{
    public GameObject bombToMove;
    public Transform bombReturnPosition;
    public float YHighOffset;
    public float YBombHighExplosion;
    Transform myPosition;
    Vector3 bombTargetPosition;
    Vector3 targetPosition;
    Vector3 startPosition;
    Vector3 startRotation;
    float speed = 0.7f;
    List<Box> patternBox = new List<Box>();

    [Header("VFX References")]
    public ParticleSystem ExplosionVFX;
    public ParticleSystem TileExplosionVFXPrefab;
    public ParticleSystem RubbleVFXPrefab;
    List<ParticleSystem> RubbleVFX = new List<ParticleSystem>();
    List<ParticleSystem> TileExplosionVFX = new List<ParticleSystem>();

    [Header("Sound References")]
    public AudioClip MovementClip;

    protected override void Start()
    {
        base.Start();
        bombToMove.SetActive(false);
        ExplosionVFX.Stop();
    }

    public override void AttackAnimation(Transform _myPosition, List<Box> _patternBox, Vector3 _startRotation)
    {
        myPosition = _myPosition;
        startPosition = myPosition.position;
        startRotation = _startRotation;
        myPosition.eulerAngles = startRotation;
        patternBox = _patternBox;
        Vector3 _targetPosition = new Vector3();
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
        bombTargetPosition = new Vector3(targetx, patternBox[0].transform.position.y + YBombHighExplosion, targetz);
        _targetPosition = new Vector3(bombTargetPosition.x, patternBox[0].transform.position.y + YHighOffset, bombTargetPosition.z);
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
        Tween fallBomb = bombToMove.transform.DOMove(bombTargetPosition, 0.3f);
        yield return fallBomb.WaitForCompletion();
        bombToMove.SetActive(false);
        bombToMove.transform.position = bombReturnPosition.position;
        ExplosionVFX.transform.position = bombTargetPosition;
        ExplosionVFX.Play();
        for (int i = 0; i < patternBox.Count; i++)
        {
            ParticleSystem vfxinstantiated = Instantiate(RubbleVFXPrefab, bombTargetPosition, RubbleVFXPrefab.transform.rotation);
            vfxinstantiated.Play();
            vfxinstantiated.transform.DOJump(patternBox[i].transform.position, 4.5f, 1, 0.8f);
            RubbleVFX.Add(vfxinstantiated);
        }
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < patternBox.Count; i++)
        {
            RubbleVFX[i].Stop();
            Destroy(RubbleVFX[i].gameObject);
            ParticleSystem vfxinstantiated = Instantiate(TileExplosionVFXPrefab, patternBox[i].transform.position + new Vector3(0, 1f, 0), TileExplosionVFXPrefab.transform.rotation);
            vfxinstantiated.Play();
            TileExplosionVFX.Add(vfxinstantiated);
        }        
        yield return new WaitForSeconds(1f);
        ExplosionVFX.Stop();
        for (int i = 0; i < patternBox.Count; i++)
        {
            TileExplosionVFX[i].Stop();
            Destroy(TileExplosionVFX[i].gameObject);
        }
        TileExplosionVFX.Clear();
        RubbleVFX.Clear();
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
