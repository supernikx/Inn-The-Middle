using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxOutlineNameSpace;
using UnityEngine.EventSystems;

public enum Element { Red, Green, Blue, NeutralWhite, NeutralBlack };

public class Box : MonoBehaviour
{

    //variabili pubbliche
    public int index1, index2, board;
    public bool walkable, free, neutralKill;
    public Material neutral_white, neutral_black;
    public Element element;

    //variabili private
    private BoardManager bm;
    private MeshRenderer mr;
    private BoxOutline outline;

    //parte di codice con funzioni private

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        outline = GetComponent<BoxOutline>();
        outline.enabled = false;
        free = true;
    }

    // Use this for initialization
    void Start()
    {
        walkable = true;
        bm = FindObjectOfType<BoardManager>();
    }

    /// <summary>
    /// Funzione che viene chiamata ogni volta che il mouse è sopra una casella e richiama la funzione BoxOver del boardmanager
    /// </summary>
    private void OnMouseEnter()
    {
        bm.BoxOver(this,true);
    }

    /// <summary>
    /// Funzione che viene chiamata ogni volta che il mouse esce dalla casella e richiama la funzione BoxOver del boardmanager
    /// </summary>
    private void OnMouseExit()
    {
        bm.BoxOver(this,false);
    }

    /// <summary>
    /// Funzione che viene chiamata ogni volta che viene premuta la casella, richiama la funzione BoxClicked all'interno del BoardManager
    /// </summary>
    private void OnMouseDown()
    {
        bm.BoxClicked(this);
    }

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che sostituisce il materiale attuale con il materiale attackedBox e la setta la variabile walkable false
    /// </summary>
    public void AttackBox()
    {
        free = false;
        walkable = false;
        gameObject.SetActive(false);
        //mr.material = boxToDestroy;
    }

    /// <summary>
    /// Funzione che cambia il tipo se si è una casella neutrale: da bianco a nero e da nero a bianco ed esegue le funzioni necessarie
    /// </summary>
    public void ChangeNeutralType()
    {
        if (element == Element.NeutralWhite)
        {
            element = Element.NeutralBlack;
            mr.material = neutral_black;
            walkable = false;
        }
        else if (element == Element.NeutralBlack)
        {
            element = Element.NeutralWhite;
            mr.material = neutral_white;
        }
    }

    /// <summary>
    /// Funzione che sostituisce il material attuale con il material della variabile showedMovement
    /// </summary>
    public void ShowBoxMovement()
    {
        if (walkable && free)
        {
            outline.enabled = true;
            outline.color = 0;
        }
    }

    /// <summary>
    /// Funzione che sostituisce il materiale attuale con il materiale showBox, solo se la casella ha la variabile walkable true
    /// </summary>
    public void ShowBoxActivePattern()
    {
        if (walkable)
        {
            outline.enabled = true;
            outline.color = 1;
        }
    }

    /// <summary>
    /// Funzione che sostituisce il materiale attuale con il materiale showBox, solo se la casella ha la variabile walkable true
    /// </summary>
    public void ShowBoxOtherPattern()
    {
        if (walkable)
        {
            outline.enabled = true;
            outline.color = 2;
        }
    }

    /// <summary>
    /// Funzione che sostituisce il materiale attuale con il materiale di default, solo se la casella ha la variabile walkable true
    /// </summary>
    public void SetAsDefault()
    {
        outline.enabled = false;
    }

    #endregion
}
