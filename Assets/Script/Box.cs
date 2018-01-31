using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    private BoardManager bm;
    public int index1,index2,board;
    public bool walkable;
    private MeshRenderer mr;
    public Material attackedBox, showedBox;
    private Material defaultMaterial;
    // Use this for initialization
    void Start () {
        mr = GetComponent<MeshRenderer>();
        defaultMaterial = mr.material;
        walkable = true;
        bm = FindObjectOfType<BoardManager>();
    }

    private void OnMouseDown()
    {
       bm.BoxClicked(gameObject.GetComponent<Box>());
    }

    public void AttackBox()
    {
        mr.material = attackedBox;
        walkable = false;
    }

    public void ShowBox()
    {
        if (walkable)
        {
            mr.material = showedBox;
        }
    }

    public void SetAsDefault()
    {
        if (walkable)
        {
            mr.material = defaultMaterial;
        }
    }
}
