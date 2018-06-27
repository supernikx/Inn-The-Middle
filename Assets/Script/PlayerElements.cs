﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElements : MonoBehaviour
{
    public int redElement, blueElement, greenElement;

    /// <summary>
    /// Funzione che aggiunge 1 all'elemento passato come parametro
    /// </summary>
    /// <param name="_element"></param>
    public void AddElement(Element _element)
    {
        switch (_element)
        {
            case Element.Red:
                redElement++;
                break;
            case Element.Green:
                greenElement++;
                break;
            case Element.Blue:
                blueElement++;
                break;
            default:
                CustomLogger.Log("Impossibile aggiungere questo elemento");
                break;
        }
        BoardManager.Instance.uiManager.UpdateElementsUI();
        BoardManager.Instance.uiManager.UpdateReadyElement();
        if (CheckSuperAttack() && !BoardManager.Instance.tutorial.SuperAttackTutorialDone)
        {
            BoardManager.Instance.tutorial.SuperAttackTutorial();
        }
    }

    /// <summary>
    /// Funzione che controlla se è possibile eseguire un superattacco
    /// </summary>
    /// <returns></returns>
    public bool CheckSuperAttack()
    {
        if ((redElement >= 3 || blueElement >= 3 || greenElement >= 3) || (redElement > 0 && blueElement > 0 && greenElement > 0))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che rimuove gli elementi necessari al superattacco
    /// </summary>
    public void UseSuperAttack()
    {
        if (redElement > 0 && blueElement > 0 && greenElement > 0)
        {
            redElement--;
            blueElement--;
            greenElement--;
        }
        else if (redElement >= 3)
        {
            redElement -= 3;
        }
        else if (greenElement >= 3)
        {
            greenElement -= 3;
        }
        else if (blueElement >= 3)
        {
            blueElement -= 3;
        }
        BoardManager.Instance.uiManager.UpdateElementsUI();
        BoardManager.Instance.uiManager.UpdateReadyElement();
    }
}
