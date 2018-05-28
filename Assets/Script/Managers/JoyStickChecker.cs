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
    List<int> connectedjoysticks;

    // Use this for initialization
    void Start()
    {
        zerojoystick = true;
        onejoystick = true;
        twojoystick = true;
        connectedjoysticks = new List<int>();
        modules = EventSystem.current.gameObject.GetComponents<StandaloneInputModule>();
        joystickcheck = CheckConncetedController();
        StartCoroutine(joystickcheck);
    }

    private IEnumerator CheckConncetedController()
    {
        connectedjoysticks.Clear();
        string[] joysticks = Input.GetJoystickNames();
        for (int i = 0; i < joysticks.Length; i++)
        {
            if (!string.IsNullOrEmpty(joysticks[i]))
                connectedjoysticks.Add(i);
        }

        if (connectedjoysticks.Count > 0)
        {
            if (connectedjoysticks.Count == 1)
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
            else if (connectedjoysticks.Count >= 2)
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
                modules[0].enabled = false;
                modules[1].enabled = false;
                EventManager.OnJoystickDisconnected();
            }
        }
        yield return new WaitForSecondsRealtime(2f);
        StopCoroutine(joystickcheck);
        joystickcheck = CheckConncetedController();
        StartCoroutine(joystickcheck);
    }
}
