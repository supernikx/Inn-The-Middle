using UnityEngine;

public enum Directions
{
    up,
    down,
    right,
    left,
    upright,
    upleft,
    downright,
    downleft,
    idle,
}

public class ControllerInputManagerTwoJoyStick : MonoBehaviour
{
    BoardManager bm;

    float XAxisJoy1, XAxisJoy2, YAxisJoy1, YAxisJoy2;
    bool XDpadJoy1, YDpadJoy1, XDpadJoy2, YDpadJoy2, XStickJoy1, XStickJoy2, YStickJoy1, YStickJoy2;


    [Header("MovementSettings")]
    public KeyCode joy1Confirm;
    public KeyCode joy2Confirm;
    public KeyCode joy1SelectNextPawnRight;
    public KeyCode joy1SelectNextPawnLeft;
    public KeyCode joy2SelectNextPawnRight;
    public KeyCode joy2SelectNextPawnLeft;

    [Header("Attack Settings")]
    public KeyCode joy1Attack;
    public KeyCode joy2Attack;
    public KeyCode joy1ActiveSuperAttack;
    public KeyCode joy2ActiveSuperAttack;

    [Header("General Settings")]
    public KeyCode joy1PassTurn;
    public KeyCode joy2PassTurn;
    public KeyCode joy1StartDraft;
    public KeyCode joyPause;
    public KeyCode joy1Start;
    public KeyCode joy2Start;

    private void Start()
    {
        bm = BoardManager.Instance;
    }

    void Update()
    {
        if (!bm.pause && !bm.TutorialInProgress)
        {
            if ((bm.turnManager.CurrentTurnState == TurnManager.PlayTurnState.check || bm.turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack) && bm.pawnSelected != null)
            {
                if (bm.turnManager.CurrentPlayerTurn == bm.p1Faction)
                {
                    XAxisJoy1 = Input.GetAxisRaw("JoyStick_HorizontalAxis_1");
                    YAxisJoy1 = Input.GetAxisRaw("JoyStick_VerticalAxis_1");

                    if (XAxisJoy1 == 1f && YAxisJoy1 == 1)
                    {
                        Debug.Log("alto+destra 1");
                        bm.pawnSelected.MoveProjection(Directions.upright);
                    }
                    else if (XAxisJoy1 == -1f && YAxisJoy1 == 1)
                    {
                        Debug.Log("alto+sinistra 1");
                        bm.pawnSelected.MoveProjection(Directions.upleft);
                    }
                    else if (XAxisJoy1 == 1f && YAxisJoy1 == -1)
                    {
                        Debug.Log("basso+destra 1");
                        bm.pawnSelected.MoveProjection(Directions.downright);
                    }
                    else if (XAxisJoy1 == -1f && YAxisJoy1 == -1)
                    {
                        Debug.Log("basso+sinistra 1");
                        bm.pawnSelected.MoveProjection(Directions.downleft);
                    }
                    else if (XAxisJoy1 == 1f)
                    {
                        Debug.Log("destra 1");
                        bm.pawnSelected.MoveProjection(Directions.right);
                    }
                    else if (XAxisJoy1 == -1f)
                    {
                        Debug.Log("sinistra 1");
                        bm.pawnSelected.MoveProjection(Directions.left);
                    }
                    else if (YAxisJoy1 == -1f)
                    {
                        Debug.Log("basso 1");
                        bm.pawnSelected.MoveProjection(Directions.down);
                    }
                    else if (YAxisJoy1 == 1f)
                    {
                        Debug.Log("alto 1");
                        bm.pawnSelected.MoveProjection(Directions.up);
                    }
                    else
                    {
                        bm.pawnSelected.MoveProjection(Directions.idle);
                    }
                }
                else if (bm.turnManager.CurrentPlayerTurn == bm.p2Faction)
                {
                    XAxisJoy2 = Input.GetAxisRaw("JoyStick_HorizontalAxis_2");
                    YAxisJoy2 = Input.GetAxisRaw("JoyStick_VerticalAxis_2");

                    if (XAxisJoy2 == 1f && YAxisJoy2 == 1)
                    {
                        Debug.Log("alto+destra 2");
                        bm.pawnSelected.MoveProjection(Directions.upright);
                    }
                    else if (XAxisJoy2 == -1f && YAxisJoy2 == 1)
                    {
                        Debug.Log("alto+sinistra 2");
                        bm.pawnSelected.MoveProjection(Directions.upleft);
                    }
                    else if (XAxisJoy2 == 1f && YAxisJoy2 == -1)
                    {
                        Debug.Log("basso+destra 2");
                        bm.pawnSelected.MoveProjection(Directions.downright);
                    }
                    else if (XAxisJoy2 == -1f && YAxisJoy2 == -1)
                    {
                        Debug.Log("basso+sinistra 2");
                        bm.pawnSelected.MoveProjection(Directions.downleft);
                    }
                    else if (XAxisJoy2 == 1f)
                    {
                        Debug.Log("destra 2");
                        bm.pawnSelected.MoveProjection(Directions.right);
                    }
                    else if (XAxisJoy2 == -1f)
                    {
                        Debug.Log("sinistra 2");
                        bm.pawnSelected.MoveProjection(Directions.left);
                    }
                    else if (YAxisJoy2 == -1f)
                    {
                        Debug.Log("basso 2");
                        bm.pawnSelected.MoveProjection(Directions.down);
                    }
                    else if (YAxisJoy2 == 1f)
                    {
                        Debug.Log("alto 2");
                        bm.pawnSelected.MoveProjection(Directions.up);
                    }
                    else
                    {
                        bm.pawnSelected.MoveProjection(Directions.idle);
                    }
                }
            }
            if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing)
            {
                if (bm.turnManager.CurrentPlayerTurn == bm.p1Faction)
                {
                    switch (bm.turnManager.CurrentTurnState)
                    {
                        case TurnManager.PlayTurnState.choosing:
                            if (bm.pawnSelected.activePattern == 4)
                            {
                                if (Input.GetAxisRaw("JoyStick_DPad_X_1") != 0)
                                {
                                    if (XDpadJoy1 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_X_1") == +1)
                                        {
                                            bm.ChoosePawnPattern(1);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_X_1") == -1)
                                        {
                                            bm.ChoosePawnPattern(3);
                                        }
                                        XDpadJoy1 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_X_1") == 0)
                                {
                                    XDpadJoy1 = false;
                                }

                                if (Input.GetAxisRaw("JoyStick_DPad_Y_1") != 0)
                                {
                                    if (YDpadJoy1 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_Y_1") == +1)
                                        {
                                            bm.ChoosePawnPattern(0);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_Y_1") == -1)
                                        {
                                            bm.ChoosePawnPattern(2);
                                        }
                                        YDpadJoy1 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_Y_1") == 0)
                                {
                                    YDpadJoy1 = false;
                                }
                            }
                            else
                            {
                                if (Input.GetAxisRaw("JoyStick_DPad_X_2") != 0)
                                {
                                    if (XDpadJoy2 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_X_2") == +1)
                                        {
                                            bm.ChoosePawnPattern(1);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_X_2") == -1)
                                        {
                                            bm.ChoosePawnPattern(3);
                                        }
                                        XDpadJoy2 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_X_2") == 0)
                                {
                                    XDpadJoy2 = false;
                                }

                                if (Input.GetAxisRaw("JoyStick_DPad_Y_2") != 0)
                                {
                                    if (YDpadJoy2 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_Y_2") == +1)
                                        {
                                            bm.ChoosePawnPattern(0);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_Y_2") == -1)
                                        {
                                            bm.ChoosePawnPattern(2);
                                        }
                                        YDpadJoy2 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_Y_2") == 0)
                                {
                                    YDpadJoy2 = false;
                                }
                            }
                            break;
                        case TurnManager.PlayTurnState.placing:
                            if (Input.GetKeyDown(joy1SelectNextPawnRight))
                            {
                                bm.SelectNextPawnToPlace(Directions.right);
                            }

                            if (Input.GetKeyDown(joy1SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnToPlace(Directions.left);
                            }

                            if (bm.pawnSelected != null)
                            {
                                if (Input.GetAxisRaw("JoyStick_VerticalAxis_1") != 0)
                                {
                                    if (YStickJoy1 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_VerticalAxis_1") == +1)
                                        {
                                            bm.pawnSelected.MoveProjectionPlacing(Directions.up);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_VerticalAxis_1") == -1)
                                        {
                                            bm.pawnSelected.MoveProjectionPlacing(Directions.down);
                                        }
                                        YStickJoy1 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_VerticalAxis_1") == 0)
                                {
                                    YStickJoy1 = false;
                                }

                                if (Input.GetKeyDown(joy1Confirm))
                                {
                                    bm.PlacingTeleport();
                                }
                            }
                            break;
                        case TurnManager.PlayTurnState.check:
                            if (Input.GetKeyDown(joy1Confirm))
                            {
                                bm.Movement(true);
                            }

                            if (Input.GetKeyDown(joy1SelectNextPawnRight))
                            {
                                bm.SelectNextPawnCheckPhase(Directions.right);
                            }

                            if (Input.GetKeyDown(joy1SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnCheckPhase(Directions.left);
                            }
                            break;
                        case TurnManager.PlayTurnState.movementattack:
                            if (Input.GetKeyDown(joy1PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy1Confirm))
                            {
                                bm.Movement(false);
                            }

                            if (!bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnRight))
                            {
                                bm.SelectNextPawn(Directions.right);
                            }

                            if (!bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnLeft))
                            {
                                bm.SelectNextPawn(Directions.left);
                            }

                            if (Input.GetKeyDown(joy1Attack))
                            {
                                bm.Attack();
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnRight))
                            {
                                bm.SelectNextPawnToAttack(Directions.right);
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnToAttack(Directions.left);
                            }

                            if (bm.CanSuperAttack && Input.GetKeyDown(joy1ActiveSuperAttack))
                            {
                                bm.ActiveSuperAttack();
                            }
                            break;
                        case TurnManager.PlayTurnState.attack:
                            if (Input.GetKeyDown(joy1PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy1Attack))
                            {
                                bm.Attack();
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnRight))
                            {
                                bm.SelectNextPawnToAttack(Directions.right);
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnToAttack(Directions.left);
                            }

                            if (bm.CanSuperAttack && Input.GetKeyDown(joy1ActiveSuperAttack))
                            {
                                bm.ActiveSuperAttack();
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (bm.turnManager.CurrentPlayerTurn == bm.p2Faction)
                {

                    switch (bm.turnManager.CurrentTurnState)
                    {
                        case TurnManager.PlayTurnState.choosing:
                            if (bm.pawnSelected.activePattern == 4)
                            {
                                if (Input.GetAxisRaw("JoyStick_DPad_X_2") != 0)
                                {
                                    if (XDpadJoy2 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_X_2") == +1)
                                        {
                                            bm.ChoosePawnPattern(1);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_X_2") == -1)
                                        {
                                            bm.ChoosePawnPattern(3);
                                        }
                                        XDpadJoy2 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_X_2") == 0)
                                {
                                    XDpadJoy2 = false;
                                }

                                if (Input.GetAxisRaw("JoyStick_DPad_Y_2") != 0)
                                {
                                    if (YDpadJoy2 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_Y_2") == +1)
                                        {
                                            bm.ChoosePawnPattern(0);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_Y_2") == -1)
                                        {
                                            bm.ChoosePawnPattern(2);
                                        }
                                        YDpadJoy2 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_Y_2") == 0)
                                {
                                    YDpadJoy2 = false;
                                }
                            }
                            else
                            {
                                if (Input.GetAxisRaw("JoyStick_DPad_X_1") != 0)
                                {
                                    if (XDpadJoy1 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_X_1") == +1)
                                        {
                                            bm.ChoosePawnPattern(1);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_X_1") == -1)
                                        {
                                            bm.ChoosePawnPattern(3);
                                        }
                                        XDpadJoy1 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_X_1") == 0)
                                {
                                    XDpadJoy1 = false;
                                }

                                if (Input.GetAxisRaw("JoyStick_DPad_Y_1") != 0)
                                {
                                    if (YDpadJoy1 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_DPad_Y_1") == +1)
                                        {
                                            bm.ChoosePawnPattern(0);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_DPad_Y_1") == -1)
                                        {
                                            bm.ChoosePawnPattern(2);
                                        }
                                        YDpadJoy1 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_DPad_Y_1") == 0)
                                {
                                    YDpadJoy1 = false;
                                }
                            }
                            break;
                        case TurnManager.PlayTurnState.placing:
                            if (Input.GetKeyDown(joy2SelectNextPawnRight))
                            {
                                bm.SelectNextPawnToPlace(Directions.right);
                            }

                            if (Input.GetKeyDown(joy2SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnToPlace(Directions.left);
                            }

                            if (bm.pawnSelected != null)
                            {
                                if (Input.GetAxisRaw("JoyStick_VerticalAxis_2") != 0)
                                {
                                    if (YStickJoy2 == false)
                                    {
                                        if (Input.GetAxisRaw("JoyStick_VerticalAxis_2") == +1)
                                        {
                                            bm.pawnSelected.MoveProjectionPlacing(Directions.up);
                                        }
                                        else if (Input.GetAxisRaw("JoyStick_VerticalAxis_2") == -1)
                                        {
                                            bm.pawnSelected.MoveProjectionPlacing(Directions.down);
                                        }
                                        YStickJoy2 = true;
                                    }
                                }
                                if (Input.GetAxisRaw("JoyStick_VerticalAxis_2") == 0)
                                {
                                    YStickJoy2 = false;
                                }

                                if (Input.GetKeyDown(joy2Confirm))
                                {
                                    bm.PlacingTeleport();
                                }
                            }
                            break;
                        case TurnManager.PlayTurnState.check:
                            if (Input.GetKeyDown(joy2Confirm))
                            {
                                bm.Movement(true);
                            }

                            if (!bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnRight))
                            {
                                bm.SelectNextPawnCheckPhase(Directions.right);
                            }

                            if (!bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnCheckPhase(Directions.left);
                            }
                            break;
                        case TurnManager.PlayTurnState.movementattack:
                            if (Input.GetKeyDown(joy2PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy2Confirm))
                            {
                                bm.Movement(false);
                            }

                            if (!bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnRight))
                            {
                                bm.SelectNextPawn(Directions.right);
                            }

                            if (!bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnLeft))
                            {
                                bm.SelectNextPawn(Directions.left);
                            }

                            if (Input.GetKeyDown(joy2Attack))
                            {
                                bm.Attack();
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnRight))
                            {
                                bm.SelectNextPawnToAttack(Directions.right);
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnToAttack(Directions.left);
                            }

                            if (bm.CanSuperAttack && Input.GetKeyDown(joy2ActiveSuperAttack))
                            {
                                bm.ActiveSuperAttack();
                            }
                            break;
                        case TurnManager.PlayTurnState.attack:
                            if (Input.GetKeyDown(joy2PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy2Attack))
                            {
                                bm.Attack();
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnRight))
                            {
                                bm.SelectNextPawnToAttack(Directions.right);
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnLeft))
                            {
                                bm.SelectNextPawnToAttack(Directions.left);
                            }

                            if (bm.CanSuperAttack && Input.GetKeyDown(joy2ActiveSuperAttack))
                            {
                                bm.ActiveSuperAttack();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.draft)
            {
                if (!bm.draftManager.hasDrafted && Input.GetKeyDown(joy1StartDraft))
                {
                    bm.draftManager.DraftRandomPattern();
                }

                if (bm.draftManager.hasDrafted)
                {
                    if (!bm.draftManager.draftEnd)
                    {
                        if (bm.turnManager.CurrentPlayerTurn == bm.p1Faction)
                        {
                            if (Input.GetKeyDown(joy1Confirm))
                            {
                                bm.draftManager.ChooseSelectedDraftPawn();
                            }

                            if (Input.GetAxisRaw("JoyStick_HorizontalAxis_1") != 0)
                            {
                                if (XStickJoy1 == false)
                                {
                                    if (Input.GetAxisRaw("JoyStick_HorizontalAxis_1") == +1)
                                    {
                                        bm.draftManager.SelectNextDraftPawn(Directions.right);
                                    }
                                    else if (Input.GetAxisRaw("JoyStick_HorizontalAxis_1") == -1)
                                    {
                                        bm.draftManager.SelectNextDraftPawn(Directions.left);
                                    }
                                    XStickJoy1 = true;
                                }
                            }
                            if (Input.GetAxisRaw("JoyStick_HorizontalAxis_1") == 0)
                            {
                                XStickJoy1 = false;
                            }
                        }
                        else if (bm.turnManager.CurrentPlayerTurn == bm.p2Faction)
                        {
                            if (Input.GetKeyDown(joy2Confirm))
                            {
                                bm.draftManager.ChooseSelectedDraftPawn();
                            }

                            if (Input.GetAxisRaw("JoyStick_HorizontalAxis_2") != 0)
                            {
                                if (XStickJoy2 == false)
                                {
                                    if (Input.GetAxisRaw("JoyStick_HorizontalAxis_2") == +1)
                                    {
                                        bm.draftManager.SelectNextDraftPawn(Directions.right);
                                    }
                                    else if (Input.GetAxisRaw("JoyStick_HorizontalAxis_2") == -1)
                                    {
                                        bm.draftManager.SelectNextDraftPawn(Directions.left);
                                    }
                                    XStickJoy2 = true;
                                }
                            }
                            if (Input.GetAxisRaw("JoyStick_HorizontalAxis_2") == 0)
                            {
                                XStickJoy2 = false;
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(joy1Start) && !bm.draftManager.p1StartPressed)
                        {
                            bm.draftManager.p1StartPressed = true;
                            bm.turnManager.ChangeTurn();
                        }

                        if (Input.GetKeyDown(joy2Start) && !bm.draftManager.p2StartPressed)
                        {
                            bm.draftManager.p2StartPressed = true;
                            bm.turnManager.ChangeTurn();
                        }
                    }
                }
            }
            else if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.menu)
            {
                if (!DataManager.instance.SkipTitleScreen && Input.anyKeyDown)
                {
                    DataManager.instance.SkipTitleScreen = true;
                    StartCoroutine(BoardManager.Instance.uiManager.SkipTitleScreen());
                }
            }
        }
        else if ((bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.draft) &&
        ((Input.GetKeyDown(joy1Confirm) && bm.turnManager.CurrentPlayerTurn == bm.p1Faction) || ((Input.GetKeyDown(joy2Confirm) && bm.turnManager.CurrentPlayerTurn == bm.p2Faction))) && bm.TutorialInProgress)
        {
            bm.tutorial.AButtonPressed();
        }

        if ((bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.end) && Input.GetKeyDown(joyPause))
        {
            if (bm.pause)
                EventManager.OnUnPause();
            else
                EventManager.OnPause();
        }
    }
}
