using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pattern", menuName = "Pattern")]
public class Attack : ScriptableObject{
    public List<Pattern> pattern;
    public Material patternMaterial;
}
