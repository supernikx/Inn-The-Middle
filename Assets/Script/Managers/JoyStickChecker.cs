using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ControllerInputManagerTwoJoyStick))]
[RequireComponent(typeof(ControllerInputManagerSingleJoyStick))]
public class JoyStickChecker : MonoBehaviour
{
    public ControllerInputManagerSingleJoyStick singlejoystick;
    public ControllerInputManagerTwoJoyStick multiplejoystick;
    IEnumerator joystickcheck;
    StandaloneInputModule[] modules;
    bool zerojoystick, onejoystick, twojoystick;

    // Use this for initialization
    void Start()
    {
        zerojoystick = true;
        onejoystick = true;
        twojoystick = true;
        modules = EventSystem.current.gameObject.GetComponents<StandaloneInputModule>();
        joystickcheck = CheckConncetedController();
        StartCoroutine(joystickcheck);
    }

    private IEnumerator CheckConncetedController()
    {
        string[] joysticks = Input.GetJoystickNames();
        if (joysticks.Length > 0)
        {
            if (string.IsNullOrEmpty(joysticks[0]) && string.IsNullOrEmpty(joysticks[1]))
            {
                if (zerojoystick)
                {
                    onejoystick = true;
                    twojoystick = true;
                    zerojoystick = false;
                    Debug.Log("0 joystick connessi");
                    modules[0].enabled = false;
                    modules[1].enabled = false;
                    EventManager.OnJoystickDisconnected();
                }
            }
            if (!string.IsNullOrEmpty(joysticks[0]) && string.IsNullOrEmpty(joysticks[1]) || string.IsNullOrEmpty(joysticks[0]) && !string.IsNullOrEmpty(joysticks[1]))
            {
                if (onejoystick)
                {
                    twojoystick = true;
                    zerojoystick = true;
                    onejoystick = false;
                    Debug.Log("1 joystick connessi");
                    singlejoystick.enabled = true;
                    multiplejoystick.enabled = false;
                    modules[0].enabled = false;
                    modules[1].enabled = true;
                    if (EventManager.OnJoystickRiconnected != null)
                        EventManager.OnJoystickRiconnected();
                }
            }
            else if (!string.IsNullOrEmpty(joysticks[0]) && !string.IsNullOrEmpty(joysticks[1]))
            {
                if (twojoystick)
                {
                    twojoystick = false;
                    zerojoystick = true;
                    onejoystick = true;
                    Debug.Log("2 joystick connessi");
                    singlejoystick.enabled = false;
                    multiplejoystick.enabled = true;
                    modules[0].enabled = true;
                    modules[1].enabled = false;
                    if (EventManager.OnJoystickRiconnected != null)
                        EventManager.OnJoystickRiconnected();
                }
            }
        }
        else
        {
            if (zerojoystick)
            {
                onejoystick = true;
                twojoystick = true;
                zerojoystick = false;
                Debug.Log("0 joystick connessi");
                EventManager.OnJoystickDisconnected();
            }
        }
        yield return new WaitForSecondsRealtime(2f);
        StopCoroutine(joystickcheck);
        joystickcheck = CheckConncetedController();
        StartCoroutine(joystickcheck);
    }


}
