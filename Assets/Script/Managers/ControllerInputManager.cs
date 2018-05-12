using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour
{

    float x1, x2, y1, y2;

    // Update is called once per frame
    void Update()
    {

        if (!BoardManager.Instance.pause)
        {
            if ((BoardManager.Instance.turnManager.CurrentTurnState == TurnManager.PlayTurnState.check || BoardManager.Instance.turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack) && BoardManager.Instance.pawnSelected != null)
            {
                if (BoardManager.Instance.turnManager.CurrentPlayerTurn == BoardManager.Instance.p1Faction)
                {
                    x1 = Input.GetAxis("JoyStick_HorizontalAxis_1");
                    y1 = Input.GetAxis("JoyStick_VerticalAxis_1");

                    switch (BoardManager.Instance.p1Faction)
                    {
                        case Factions.Magic:
                            if (x1 == 1f && y1 == -1)
                            {
                                Debug.Log("alto+destra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x1 == -1f && y1 == -1)
                            {
                                Debug.Log("alto+sinistra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x1 == 1f && y1 == 1)
                            {
                                Debug.Log("basso+destra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x1 == -1f && y1 == 1)
                            {
                                Debug.Log("basso+sinistra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x1 == 1f)
                            {
                                Debug.Log("destra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (x1 == -1f)
                            {
                                Debug.Log("sinistra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (y1 == 1f)
                            {
                                Debug.Log("basso 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (y1 == -1f)
                            {
                                Debug.Log("alto 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else
                            {
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.pawnSelected.currentBox);
                            }
                            break;
                        case Factions.Science:
                            if (x1 == 1f && y1 == -1)
                            {
                                Debug.Log("alto+destra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x1 == -1f && y1 == -1)
                            {
                                Debug.Log("alto+sinistra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x1 == 1f && y1 == 1)
                            {
                                Debug.Log("basso+destra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x1 == -1f && y1 == 1)
                            {
                                Debug.Log("basso+sinistra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x1 == 1f)
                            {
                                Debug.Log("destra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (x1 == -1f)
                            {
                                Debug.Log("sinistra 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (y1 == 1f)
                            {
                                Debug.Log("basso 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (y1 == -1f)
                            {
                                Debug.Log("alto 1");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else
                            {
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.pawnSelected.currentBox);
                            }
                            break;
                    }
                }
                else if (BoardManager.Instance.turnManager.CurrentPlayerTurn == BoardManager.Instance.p2Faction)
                {
                    x2 = Input.GetAxis("JoyStick_HorizontalAxis_2");
                    y2 = Input.GetAxis("JoyStick_VerticalAxis_2");

                    switch (BoardManager.Instance.p2Faction)
                    {
                        case Factions.Magic:
                            if (x2 == 1f && y2 == -1)
                            {
                                Debug.Log("alto+destra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x2 == -1f && y2 == -1)
                            {
                                Debug.Log("alto+sinistra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x2 == 1f && y2 == 1)
                            {
                                Debug.Log("basso+destra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x2 == -1f && y2 == 1)
                            {
                                Debug.Log("basso+sinistra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x2 == 1f)
                            {
                                Debug.Log("destra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (x2 == -1f)
                            {
                                Debug.Log("sinistra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (y2 == 1f)
                            {
                                Debug.Log("basso 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (y2 == -1f)
                            {
                                Debug.Log("alto 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board1[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else
                            {
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.pawnSelected.currentBox);
                            }
                            break;
                        case Factions.Science:
                            if (x2 == 1f && y2 == -1)
                            {
                                Debug.Log("alto+destra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x2 == -1f && y2 == -1)
                            {
                                Debug.Log("alto+sinistra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else if (x2 == 1f && y2 == 1)
                            {
                                Debug.Log("basso+destra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x2 == -1f && y2 == 1)
                            {
                                Debug.Log("basso+sinistra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (x2 == 1f)
                            {
                                Debug.Log("destra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 + 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (x2 == -1f)
                            {
                                Debug.Log("sinistra 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1 - 1][BoardManager.Instance.pawnSelected.currentBox.index2].GetComponent<Box>());
                            }
                            else if (y2 == 1f)
                            {
                                Debug.Log("basso 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 - 1].GetComponent<Box>());
                            }
                            else if (y2 == -1f)
                            {
                                Debug.Log("alto 2");
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.board2[BoardManager.Instance.pawnSelected.currentBox.index1][BoardManager.Instance.pawnSelected.currentBox.index2 + 1].GetComponent<Box>());
                            }
                            else
                            {
                                BoardManager.Instance.pawnSelected.MoveProjection(BoardManager.Instance.pawnSelected.currentBox);
                            }
                            break;
                    }
                }
            }
        }



        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Debug.Log("test");
        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            Debug.Log("test2");
        }

    }
}
