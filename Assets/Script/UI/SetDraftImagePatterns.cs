using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDraftImagePatterns : MonoBehaviour {

    public List<GameObject> PatternImage;

	// Use this for initialization
	void Start () {
        foreach (GameObject g in PatternImage)
        {
            g.SetActive(false);
        }
	}

    public void SetPatternImage(int patternindex)
    {
        PatternImage[patternindex].SetActive(true);
    }

}
