using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManagerSingleJoyStick : MonoBehaviour
{
    BoardManager bm;

    float XAxisJoy, YAxisJoy;
    bool XDpadJoy, YDpadJoy, XStickJoy, YStickJoy;


    [Header("MovementSettings")]
    public KeyCode joyConfirm;
    public KeyCode joySelectNextPawnRight;
    public KeyCode joySelectNextPawnLeft;

    [Header("Attack Settings")]
    public KeyCode joyAttack;
    public KeyCode joyActiveSuperAttack;

    [Header("General Settings")]
    public KeyCode joyPassTurn;
    public KeyCode joyStartDraft;
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
                XAxisJoy = Input.GetAxisRaw("JoyStick_HorizontalAxis");
                YAxisJoy = Input.GetAxisRaw("JoyStick_VerticalAxis");

                if (XAxisJoy == 1f && YAxisJoy == 1)
                {
                    Debug.Log("alto+destra");
                    bm.pawnSelected.MoveProjection(Directions.upright);
                }
                else if (XAxisJoy == -1f && YAxisJoy == 1)
                {
                    Debug.Log("alto+sinistra");
                    bm.pawnSelected.MoveProjection(Directions.upleft);
                }
                else if (XAxisJoy == 1f && YAxisJoy == -1)
                {
                    Debug.Log("basso+destra");
                    bm.pawnSelected.MoveProjection(Directions.downright);
                }
                else if (XAxisJoy == -1f && YAxisJoy == -1)
                {
                    Debug.Log("basso+sinistra");
                    bm.pawnSelected.MoveProjection(Directions.downleft);
                }
                else if (XAxisJoy == 1f)
                {
                    Debug.Log("destra");
                    bm.pawnSelected.MoveProjection(Directions.right);
                }
                else if (XAxisJoy == -1f)
                {
                    Debug.Log("sinistra");
                    bm.pawnSelected.MoveProjection(Directions.left);
                }
                else if (YAxisJoy == -1f)
                {
                    Debug.Log("basso");
                    bm.pawnSelected.MoveProjection(Directions.down);
                }
                else if (YAxisJoy == 1f)
                {
                    Debug.Log("alto");
                    bm.pawnSelected.MoveProjection(Directions.up);
                }
                else
                {
                    bm.pawnSelected.MoveProjection(Directions.idle);
                }

            }
            if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game || bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing)
            {
                switch (bm.turnManager.CurrentTurnState)
                {
                    case TurnManager.PlayTurnState.choosing:
                        if (Input.GetAxisRaw("JoyStick_DPad_X") != 0)
                        {
                            if (XDpadJoy == false)
                            {
                                if (Input.GetAxisRaw("JoyStick_DPad_X") == +1)
                                {
                                    bm.ChoosePawnPattern(1);
                                }
                                else if (Input.GetAxisRaw("JoyStick_DPad_X") == -1)
                                {
                                    bm.ChoosePawnPattern(3);
                                }
                                XDpadJoy = true;
                            }
                        }
                        if (Input.GetAxisRaw("JoyStick_DPad_X") == 0)
                        {
                            XDpadJoy = false;
                        }

                        if (Input.GetAxisRaw("JoyStick_DPad_Y") != 0)
                        {
                            if (YDpadJoy == false)
                            {
                                if (Input.GetAxisRaw("JoyStick_DPad_Y") == +1)
                                {
                                    bm.ChoosePawnPattern(0);
                                }
                                else if (Input.GetAxisRaw("JoyStick_DPad_Y") == -1)
                                {
                                    bm.ChoosePawnPattern(2);
                                }
                                YDpadJoy = true;
                            }
                        }
                        if (Input.GetAxisRaw("JoyStick_DPad_Y") == 0)
                        {
                            YDpadJoy = false;
                        }
                        break;
                    case TurnManager.PlayTurnState.placing:
                        if (Input.GetKeyDown(joySelectNextPawnRight))
                        {
                            bm.SelectNextPawnToPlace(Directions.right);
                        }

                        if (Input.GetKeyDown(joySelectNextPawnLeft))
                        {
                            bm.SelectNextPawnToPlace(Directions.left);
                        }

                        if (bm.pawnSelected != null)
                        {
                            if (Input.GetAxisRaw("JoyStick_VerticalAxis") != 0)
                            {
                                if (YStickJoy == false)
                                {
                                    if (Input.GetAxisRaw("JoyStick_VerticalAxis") == +1)
                                    {
                                        bm.pawnSelected.MoveProjectionPlacing(Directions.up);
                                    }
                                    else if (Input.GetAxisRaw("JoyStick_VerticalAxis") == -1)
                                    {
                                        bm.pawnSelected.MoveProjectionPlacing(Directions.down);
                                    }
                                    YStickJoy = true;
                                }
                            }
                            if (Input.GetAxisRaw("JoyStick_VerticalAxis") == 0)
                            {
                                YStickJoy = false;
                            }

                            if (Input.GetKeyDown(joyConfirm))
                            {
                                bm.PlacingTeleport();
                            }
                        }
                        break;
                    case TurnManager.PlayTurnState.check:
                        if (Input.GetKeyDown(joyConfirm))
                        {
                            bm.Movement(true);
                        }
                        break;
                    case TurnManager.PlayTurnState.movementattack:
                        if (Input.GetKeyDown(joyPassTurn))
                        {
                            bm.turnManager.ChangeTurn();
                        }

                        if (Input.GetKeyDown(joyConfirm))
                        {
                            bm.Movement(false);
                        }

                        if (!bm.superAttack && Input.GetKeyDown(joySelectNextPawnRight))
                        {
                            bm.SelectNextPawn(Directions.right);
                        }

                        if (!bm.superAttack && Input.GetKeyDown(joySelectNextPawnLeft))
                        {
                            bm.SelectNextPawn(Directions.left);
                        }

                        if (Input.GetKeyDown(joyAttack))
                        {
                            bm.Attack();
                        }

                        if (bm.superAttack && Input.GetKeyDown(joySelectNextPawnRight))
                        {
                            bm.SelectNextPawnToAttack(Directions.right);
                        }

                        if (bm.superAttack && Input.GetKeyDown(joySelectNextPawnLeft))
                        {
                            bm.SelectNextPawnToAttack(Directions.left);
                        }

                        if (bm.CanSuperAttack && Input.GetKeyDown(joyActiveSuperAttack))
                        {
                            bm.ActiveSuperAttack();
                        }
                        break;
                    case TurnManager.PlayTurnState.attack:
                        if (Input.GetKeyDown(joyPassTurn))
                        {
                            bm.turnManager.ChangeTurn();
                        }

                        if (Input.GetKeyDown(joyAttack))
                        {
                            bm.Attack();
                        }

                        if (bm.superAttack && Input.GetKeyDown(joySelectNextPawnRight))
                        {
                            bm.SelectNextPawnToAttack(Directions.right);
                        }

                        if (bm.superAttack && Input.GetKeyDown(joySelectNextPawnLeft))
                        {
                            bm.SelectNextPawnToAttack(Directions.left);
                        }

                        if (bm.CanSuperAttack && Input.GetKeyDown(joyActiveSuperAttack))
                        {
                            bm.ActiveSuperAttack();
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.draft)
            {
                if (!bm.draftManager.hasDrafted && Input.GetKeyDown(joyStartDraft))
                {
                    bm.draftManager.DraftRandomPattern();
                }

                if (bm.draftManager.hasDrafted)
                {
                    if (Input.GetKeyDown(joyConfirm))
                    {
                        bm.draftManager.ChooseSelectedDraftPawn();
                    }

                    if (Input.GetAxisRaw("JoyStick_HorizontalAxis") != 0)
                    {
                        if (XStickJoy == false)
                        {
                            if (Input.GetAxisRaw("JoyStick_HorizontalAxis") == +1)
                            {
                                bm.draftManager.SelectNextDraftPawn(Directions.right);
                            }
                            else if (Input.GetAxisRaw("JoyStick_HorizontalAxis") == -1)
                            {
                                bm.draftManager.SelectNextDraftPawn(Directions.left);
                            }
                            XStickJoy = true;
                        }
                    }
                    if (Input.GetAxisRaw("JoyStick_HorizontalAxis") == 0)
                    {
                        XStickJoy = false;
                    }
                }
            }
            else if (bm.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.menu)
            {
                if (!DataManager.instance.SkipTitleScreen && Input.anyKeyDown)
                {
                    DataManager.instance.SkipTitleScreen = true;
                }
            }
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
