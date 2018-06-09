using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDraftImagePatterns : MonoBehaviour
{

    public List<GameObject> PatternImage;
    public List<GameObject> DissolvedPatternImage;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject g in PatternImage)
        {
            g.SetActive(false);
        }

        foreach (GameObject d in DissolvedPatternImage)
        {
            d.SetActive(false);
        }
    }

    public void SetPatternImage(int patternindex)
    {
        if (oldpattern != -1)
            DissolvedPatternImage[oldpattern].SetActive(false);
        PatternImage[patternindex].SetActive(true);
    }

    int oldpattern = -1;
    public void SetDissolvedPatternImage(int patternindex)
    {
        if (oldpattern != -1)
            DissolvedPatternImage[oldpattern].SetActive(false);
        oldpattern = patternindex;
        DissolvedPatternImage[patternindex].SetActive(true);
    }
}
