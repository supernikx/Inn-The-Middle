using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixControllerEventSystem : MonoBehaviour {

    GameObject storedSelected;

	// Use this for initialization
	void Start () {
        storedSelected = EventSystem.current.firstSelectedGameObject;
    }
	
	// Update is called once per frame
	void Update () {		
        if ((BoardManager.Instance.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.menu && BoardManager.Instance.turnManager.CurrentTurnState != TurnManager.PlayTurnState.animation) || (BoardManager.Instance.turnManager.CurrentMacroPhase == TurnManager.MacroPhase.faction && BoardManager.Instance.turnManager.CurrentTurnState != TurnManager.PlayTurnState.animation) || BoardManager.Instance.pause)
        {
            if (EventSystem.current.currentSelectedGameObject != storedSelected)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(storedSelected);
                else
                    storedSelected = EventSystem.current.currentSelectedGameObject;
            }
        }
	}
}
