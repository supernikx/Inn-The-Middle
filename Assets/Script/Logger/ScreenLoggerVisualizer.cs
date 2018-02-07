using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenLoggerVisualizer : MonoBehaviour {

    public TextMeshProUGUI textArea;
	
	// Update is called once per frame
	void Update () {
        textArea.text = CustomLogger.currentLogString;
	}
}
