using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElements : MonoBehaviour
{
    public int purpleElement, azureElement, orangeElement;

    public void AddElement(Element _element)
    {
        switch (_element)
        {
            case Element.Purple:
                purpleElement++;
                break;
            case Element.Orange:
                orangeElement++;
                break;
            case Element.Azure:
                azureElement++;
                break;
            default:
                CustomLogger.Log("Impossibile aggiungere questo elemento");
                break;
        }
    }

    public bool CheckSuperAttack()
    {
        if ((purpleElement >= 3 || azureElement >= 3 || orangeElement >= 3) || (purpleElement > 0 && azureElement > 0 && orangeElement > 0))
        {
            return true;
        }
        return false;
    }

    public void UseSuperAttack()
    {
        if (purpleElement >= 3)
        {
            purpleElement -= 3;
        }
        else if (azureElement >= 3)
        {
            azureElement -= 3;
        }
        else if (orangeElement >= 3)
        {
            orangeElement -= 3;
        }
        else if (purpleElement > 0 && azureElement > 0 && orangeElement > 0)
        {
            purpleElement--;
            azureElement--;
            orangeElement--;
        }
    }
}
