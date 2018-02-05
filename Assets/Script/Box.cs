using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    
    //variabili pubbliche
    public int index1,index2,board;
    public bool walkable,pattern;
    public Material attackedBox, showedBox;

    //variabili private
    private BoardManager bm;
    private MeshRenderer mr;
    private Material defaultMaterial;

    //parte di codice con funzioni private
    // Use this for initialization
    void Start () {
        mr = GetComponent<MeshRenderer>();
        defaultMaterial = mr.material;
        walkable = true;
        pattern = false;
        bm = FindObjectOfType<BoardManager>();
    }

    /// <summary>
    /// Funzione che viene chiamata ogni volta che viene premuta la casella, richiama la funzione BoxClicked all'interno del BoardManager
    /// </summary>
    private void OnMouseDown()
    {
       bm.BoxClicked(gameObject.GetComponent<Box>());
    }

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che sostituisce il materiale attuale con il materiale attackedBox e la setta la variabile walkable false
    /// </summary>
    public void AttackBox()
    {
        mr.material = attackedBox;
        walkable = false;
        pattern = false;
    }

    /// <summary>
    /// Funzione che sostituisce il materiale attuale con il materiale showBox, solo se la casella ha la variabile walkable true
    /// </summary>
    public void ShowBox()
    {
        if (walkable)
        {
            mr.material = showedBox;
            pattern = true;
        }
    }

    /// <summary>
    /// Funzione che sostituisce il materiale attuale con il materiale di default, solo se la casella ha la variabile walkable true
    /// </summary>
    public void SetAsDefault()
    {
        if (walkable)
        {
            mr.material = defaultMaterial;
            pattern = false;
        }
    }

    #endregion
}
