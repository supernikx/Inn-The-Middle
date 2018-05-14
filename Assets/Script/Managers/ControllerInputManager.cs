using System.Collections;
using System.Collections.Generic;
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

public class ControllerInputManager : MonoBehaviour
{
    BoardManager bm;

    float XAxisJoy1, XAxisJoy2, YAxisJoy1, YAxisJoy2;
    bool XDpadJoy1, YDpadJoy1, XDpadJoy2, YDpadJoy2;


    [Header("MovementSettings")]
    public KeyCode joy1MovementConfirm;
    public KeyCode joy2MovementConfirm;
    public KeyCode joy1SelectNextPawn;
    public KeyCode joy2SelectNextPawn;

    [Header("Attack Settings")]
    public KeyCode joy1Attack;
    public KeyCode joy2Attack;
    public KeyCode joy1ActiveSuperAttack;
    public KeyCode joy2ActiveSuperAttack;
    public KeyCode joy1SelectNextPawnSuperAttack;
    public KeyCode joy2SelectNextPawnSuperAttack;

    [Header("ChoosePattern Settings")]
    public KeyCode joy1BouncePattern;
    public KeyCode joy1SniperPattern;
    public KeyCode joy1MeleePattern;
    public KeyCode joy1CrossPattern;
    public KeyCode joy2BouncePattern;
    public KeyCode joy2SniperPattern;
    public KeyCode joy2MeleePattern;
    public KeyCode joy2CrossPattern;

    [Header("General Settings")]
    public KeyCode joy1PassTurn;
    public KeyCode joy2PassTurn;
    public KeyCode joyPause;

    private void Start()
    {
        bm = BoardManager.Instance;
    }

    void Update()
    {
        if (!bm.pause)
        {
            if ((bm.turnManager.CurrentTurnState == TurnManager.PlayTurnState.check || bm.turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack) && bm.pawnSelected != null)
            {
                if (bm.turnManager.CurrentPlayerTurn == bm.p1Faction)
                {
                    XAxisJoy1 = Input.GetAxisRaw("JoyStick_HorizontalAxis_1");
                    YAxisJoy1 = Input.GetAxisRaw("JoyStick_VerticalAxis_1");

                    if (XAxisJoy1 == 1f && YAxisJoy1 == -1)
                    {
                        Debug.Log("alto+destra 1");
                        bm.pawnSelected.MoveProjection(Directions.upright);
                    }
                    else if (XAxisJoy1 == -1f && YAxisJoy1 == -1)
                    {
                        Debug.Log("alto+sinistra 1");
                        bm.pawnSelected.MoveProjection(Directions.upleft);
                    }
                    else if (XAxisJoy1 == 1f && YAxisJoy1 == 1)
                    {
                        Debug.Log("basso+destra 1");
                        bm.pawnSelected.MoveProjection(Directions.downright);
                    }
                    else if (XAxisJoy1 == -1f && YAxisJoy1 == 1)
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
                    else if (YAxisJoy1 == 1f)
                    {
                        Debug.Log("basso 1");
                        bm.pawnSelected.MoveProjection(Directions.down);
                    }
                    else if (YAxisJoy1 == -1f)
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

                    if (XAxisJoy2 == 1f && YAxisJoy2 == -1)
                    {
                        Debug.Log("alto+destra 2");
                        bm.pawnSelected.MoveProjection(Directions.upright);
                    }
                    else if (XAxisJoy2 == -1f && YAxisJoy2 == -1)
                    {
                        Debug.Log("alto+sinistra 2");
                        bm.pawnSelected.MoveProjection(Directions.upleft);
                    }
                    else if (XAxisJoy2 == 1f && YAxisJoy2 == 1)
                    {
                        Debug.Log("basso+destra 2");
                        bm.pawnSelected.MoveProjection(Directions.downright);
                    }
                    else if (XAxisJoy2 == -1f && YAxisJoy2 == 1)
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
                    else if (YAxisJoy2 == 1f)
                    {
                        Debug.Log("basso 2");
                        bm.pawnSelected.MoveProjection(Directions.down);
                    }
                    else if (YAxisJoy2 == -1f)
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

            if (bm.turnManager.CurrentPlayerTurn == bm.p1Faction)
            {
                if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing)
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
                            break;
                        case TurnManager.PlayTurnState.check:
                            if (Input.GetKeyDown(joy1PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy1MovementConfirm))
                            {
                                bm.Movement(true);
                            }
                            break;
                        case TurnManager.PlayTurnState.movementattack:
                            if (Input.GetKeyDown(joy1PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy1MovementConfirm))
                            {
                                bm.Movement(false);
                            }

                            if (Input.GetKeyDown(joy1SelectNextPawn))
                            {
                                bm.SelectNextPawn();
                            }

                            if (Input.GetKeyDown(joy1Attack))
                            {
                                bm.Attack();
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnSuperAttack))
                            {
                                bm.SelectNextPawnToAttack();
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

                            if (bm.superAttack && Input.GetKeyDown(joy1SelectNextPawnSuperAttack))
                            {
                                bm.SelectNextPawnToAttack();
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
            }
            else if (bm.turnManager.CurrentPlayerTurn == bm.p2Faction)
            {
                if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing)
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
                            break;
                        case TurnManager.PlayTurnState.check:
                            if (Input.GetKeyDown(joy1PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy2MovementConfirm))
                            {
                                bm.Movement(true);
                            }
                            break;
                        case TurnManager.PlayTurnState.movementattack:
                            if (Input.GetKeyDown(joy2PassTurn))
                            {
                                bm.turnManager.ChangeTurn();
                            }

                            if (Input.GetKeyDown(joy2MovementConfirm))
                            {
                                bm.Movement(false);
                            }

                            if (Input.GetKeyDown(joy2SelectNextPawn))
                            {
                                bm.SelectNextPawn();
                            }

                            if (Input.GetKeyDown(joy2Attack))
                            {
                                bm.Attack();
                            }

                            if (bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnSuperAttack))
                            {
                                bm.SelectNextPawnToAttack();
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

                            if (bm.superAttack && Input.GetKeyDown(joy2SelectNextPawnSuperAttack))
                            {
                                bm.SelectNextPawnToAttack();
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
        }

        if ((bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing) && Input.GetKeyDown(joyPause))
        {
            if (bm.pause)
                EventManager.OnUnPause();
            else
                EventManager.OnPause();
        }
    }
}
