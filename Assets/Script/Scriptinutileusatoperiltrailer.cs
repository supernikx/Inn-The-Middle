using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scriptinutileusatoperiltrailer : MonoBehaviour {
    int i = 0;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.C))
        {
            ScreenCapture.CaptureScreenshot("Screen"+i+".png");
            Debug.Log("Screenshot");
            i++;
        }
	}
}
