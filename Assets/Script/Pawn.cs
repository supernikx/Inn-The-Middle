using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Pawn : MonoBehaviour
{
    #region Events

    public delegate void PawnEvent();
    public PawnEvent OnAttackEnd;
    public PawnEvent OnMovementEnd;
    public delegate void PawnDamageEvent(Pawn pawnHit);
    public PawnDamageEvent OnDeathEnd;
    public PawnDamageEvent OnDamageEnd;

    #endregion

    //variabili pubbliche
    public bool selected, randomized;
    public Player player;
    public Box currentBox;
    public float activeSpeed;
    public List<float> speeds = new List<float>();
    [HideInInspector]
    public List<GameObject> projections;
    [Space]
    [Header("Attack Settings")]
    public int activePattern;
    /// <summary>
    /// Lista che contiene i pattern d'attacco e il colore del pattern, i valori inseriti sono 2 interi che identificano quanto la casella interessata si discosta dalla nostra posizione (index 1 riga, index 2 colonna)
    /// </summary>
    public List<Attack> patterns;
    public bool killMarker;

    //variabili private
    private BoardManager bm;
    private Box projectionTempBox;
    private Transform[][] myboard, enemyboard;
    private PlayerElements myelements;
    private List<GameObject> graphics;
    private Vector3 startRotation;
    private List<IPawnAnimations> animators;

    // Use this for initialization
    void Start()
    {
        selected = false;
        killMarker = false;
        randomized = false;
        bm = BoardManager.Instance;
        projectionTempBox = currentBox;
        startRotation = transform.eulerAngles;
        graphics = new List<GameObject>();
        projections = new List<GameObject>();
        animators = new List<IPawnAnimations>();
        SetGraphics();
        SetBoards();
    }

    /// <summary>
    /// Funzione che imposta qual'è la nostra board e qual'è quella dell'avversario
    /// </summary>
    private void SetBoards()
    {
        if (player == Player.player1)
        {
            myboard = bm.board1;
            enemyboard = bm.board2;
            myelements = bm.player1Elements;
        }
        else if (player == Player.player2)
        {
            myboard = bm.board2;
            enemyboard = bm.board1;
            myelements = bm.player2Elements;
        }
    }

    private void SetGraphics()
    {
        foreach (Transform child in transform)
        {
            GameObject childToAdd = child.gameObject;
            childToAdd.SetActive(false);
            graphics.Add(childToAdd);
            animators.Add(childToAdd.transform.GetChild(0).GetComponent<IPawnAnimations>());
            GameObject projectionToAdd = childToAdd.transform.GetChild(1).gameObject;
            projectionToAdd.SetActive(false);
            projections.Add(projectionToAdd);
        }
    }

    /// <summary>
    /// Funzione chiamata quando quest'oggetto viene distrutto e che lo rimuove dalla lista delle pedina
    /// </summary>
    private void OnDestroy()
    {
        bm.pawns.Remove(this);
    }

    //identifica la zona di codice con le funzioni pubbliche
    #region API

    /// <summary>
    /// Funzione che viene chiamata ogni volta che la pedina viene premuta, e che richiama la funzione del BoardManager PawnSelected
    /// </summary>
    public void OnMouseDown()
    {
        if (killMarker)
        {
            bm.pawnSelected.myelements.UseSuperAttack();
            bm.KillPawnMarked(this);
        }
        else
        {
            bm.PawnSelected(this);
        }
    }

    #region GraphicsFunctions

    /// <summary>
    /// Funzione che mostra le caselle in cui questa pedina può muoversi
    /// </summary>
    public void ShowMovementBoxes()
    {
        for (int index1 = 0; index1 < myboard.Length; index1++)
        {
            for (int index2 = 0; index2 < myboard[0].Length; index2++)
            {
                if ((index1 == currentBox.index1 + 1 || index1 == currentBox.index1 - 1 || index1 == currentBox.index1) && (index2 == currentBox.index2 || index2 == currentBox.index2 + 1 || index2 == currentBox.index2 - 1)
                    && myboard[index1][index2].GetComponent<Box>() != currentBox)
                {
                    myboard[index1][index2].GetComponent<Box>().ShowBoxMovement();
                }
            }
        }
    }

    /// <summary>
    /// Funzione che disabilita la visione delle caselle in cui può muoversi questa pedina
    /// </summary>
    public void DisableMovementBoxes()
    {
        for (int index1 = 0; index1 < myboard.Length; index1++)
        {
            for (int index2 = 0; index2 < myboard[0].Length; index2++)
            {
                if ((index1 == currentBox.index1 + 1 || index1 == currentBox.index1 - 1 || index1 == currentBox.index1) && (index2 == currentBox.index2 || index2 == currentBox.index2 + 1 || index2 == currentBox.index2 - 1)
                    && myboard[index1][index2].GetComponent<Box>() != currentBox)
                {
                    myboard[index1][index2].GetComponent<Box>().SetAsDefault();
                }
            }
        }
    }

    /// <summary>
    /// Mostra sulla board avversaria le caselle attaccabili da questa pedina
    /// </summary>
    public void ShowAttackPattern()
    {
        int currentColumn, currentRow;
        if (projectionTempBox != null)
        {
            currentColumn = projectionTempBox.index2;
            currentRow = projectionTempBox.index1;
        }
        else
        {
            currentColumn = currentBox.index2;
            currentRow = currentBox.index1;
        }

        if (activePattern == 2)
        {
            int patternindex1 = patterns[activePattern].pattern[0].index1;
            int patternindex2 = patterns[activePattern].pattern[0].index2;
            if ((currentColumn + patternindex2 < enemyboard[0].Length && currentColumn + patternindex2 >= 0) && (patternindex1 - currentRow < enemyboard.Length && patternindex1 - currentRow >= 0))
            {
                enemyboard[patternindex1 - currentRow][currentColumn + patternindex2].GetComponent<Box>().ShowBoxActivePattern();
            }

            if (((currentColumn + patternindex2 < myboard[0].Length && currentColumn + patternindex2 >= 0) && (currentRow - patternindex1 < myboard.Length && currentRow - patternindex1 - 1 >= 0)))
            {
                myboard[currentRow - patternindex1 - 1][currentColumn + patternindex2].GetComponent<Box>().ShowBoxActivePattern();
            }
        }
        else
        {
            foreach (Pattern p in patterns[activePattern].pattern)
            {
                if (((currentColumn + p.index2 < enemyboard[0].Length && currentColumn + p.index2 >= 0) && (p.index1 - currentRow < enemyboard.Length && p.index1 - currentRow >= 0)) && enemyboard[p.index1 - currentRow][currentColumn + p.index2].GetComponent<Box>() != currentBox)
                {
                    enemyboard[p.index1 - currentRow][currentColumn + p.index2].GetComponent<Box>().ShowBoxActivePattern();
                }

                if (((currentColumn + p.index2 < myboard[0].Length && currentColumn + p.index2 >= 0) && (currentRow - p.index1 < myboard.Length && currentRow - p.index1 - 1 >= 0)) && myboard[currentRow - p.index1 - 1][currentColumn + p.index2].GetComponent<Box>() != currentBox)
                {
                    myboard[currentRow - p.index1 - 1][currentColumn + p.index2].GetComponent<Box>().ShowBoxActivePattern();
                }
            }
        }
    }

    /// <summary>
    /// Disabilita la visione sulla board avversaria le caselle attaccabili da questa pedina
    /// </summary>
    public void DisableAttackPattern()
    {
        int currentColumn, currentRow;
        if (projectionTempBox != null)
        {
            currentColumn = projectionTempBox.index2;
            currentRow = projectionTempBox.index1;
        }
        else
        {
            currentColumn = currentBox.index2;
            currentRow = currentBox.index1;
        }
        if (activePattern == 2)
        {
            int patternindex1 = patterns[activePattern].pattern[0].index1;
            int patternindex2 = patterns[activePattern].pattern[0].index2;
            if ((currentColumn + patternindex2 < enemyboard[0].Length && currentColumn + patternindex2 >= 0) && (patternindex1 - currentRow < enemyboard.Length && patternindex1 - currentRow >= 0))
            {
                enemyboard[patternindex1 - currentRow][currentColumn + patternindex2].GetComponent<Box>().SetAsDefault();
            }

            if ((currentColumn + patternindex2 < myboard[0].Length && currentColumn + patternindex2 >= 0) && (currentRow - patternindex1 < myboard.Length && currentRow - patternindex1 - 1 >= 0))
            {
                myboard[currentRow - patternindex1 - 1][currentColumn + patternindex2].GetComponent<Box>().SetAsDefault();
            }
        }
        else
        {
            foreach (Pattern a in patterns[activePattern].pattern)
            {
                if ((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentRow < enemyboard.Length && a.index1 - currentRow >= 0))
                {
                    enemyboard[a.index1 - currentRow][currentColumn + a.index2].GetComponent<Box>().SetAsDefault();
                }

                if ((currentColumn + a.index2 < myboard[0].Length && currentColumn + a.index2 >= 0) && (currentRow - a.index1 < myboard.Length && currentRow - a.index1 - 1 >= 0))
                {
                    myboard[currentRow - a.index1 - 1][currentColumn + a.index2].GetComponent<Box>().SetAsDefault();
                }
            }
        }
    }

    #endregion

    #region Attack

    List<Box> patternBox = new List<Box>();
    bool superAttack;
    int pawnHitted;
    int _pawnAnimationEnded;
    public int PawnAnimationEnded
    {
        get
        {
            return _pawnAnimationEnded;
        }
        set
        {
            _pawnAnimationEnded = value;
            if (_pawnAnimationEnded == pawnHitted)
            {
                pawnHitted = 0;
                _pawnAnimationEnded = 0;
                patternBox.Clear();
                if (OnAttackEnd != null)
                    OnAttackEnd();
            }
        }
    }

    /// <summary>
    /// Funzione che controlla se nel pattern è presente una pedina aversaria, allora ritorna true, altrimenti ritorna false
    /// </summary>
    /// <returns></returns>
    public bool CheckAttackPattern()
    {
        int currentColumn = currentBox.index2;
        if (activePattern == 2)
        {
            int patternindex1 = patterns[activePattern].pattern[0].index1;
            int patternindex2 = patterns[activePattern].pattern[0].index2;
            foreach (Pawn p in bm.pawns)
            {
                if (p.player != player)
                {
                    if (((currentColumn + patternindex2 < enemyboard[0].Length && currentColumn + patternindex2 >= 0) && (patternindex1 - currentBox.index1 < enemyboard.Length && patternindex1 - currentBox.index1 >= 0)) && ((p.currentBox.index1 == patternindex1 - currentBox.index1) && (p.currentBox.index2 == currentColumn + patternindex2)))
                    {
                        CustomLogger.Log("c'è una pedina avversaria nel pattern");
                        return true;
                    }
                }
            }
        }
        else
        {
            foreach (Pattern a in patterns[activePattern].pattern)
            {
                foreach (Pawn p in bm.pawns)
                {
                    if (p.player != player)
                    {
                        if (((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentBox.index1 < enemyboard.Length && a.index1 - currentBox.index1 >= 0)) && ((p.currentBox.index1 == a.index1 - currentBox.index1) && (p.currentBox.index2 == currentColumn + a.index2)))
                        {
                            CustomLogger.Log("c'è una pedina avversaria nel pattern");
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void AttackBehaviour(bool _superAttack)
    {
        DisableAttackPattern();
        superAttack = _superAttack;
        BoardManager.Instance.turnManager.turnsWithoutAttack = 0;
        int currentColumn = currentBox.index2;
        foreach (Pattern p in patterns[activePattern].pattern)
        {
            if ((currentColumn + p.index2 < enemyboard[0].Length && currentColumn + p.index2 >= 0) && (p.index1 - currentBox.index1 < enemyboard.Length && p.index1 - currentBox.index1 >= 0))
            {
                patternBox.Add(enemyboard[p.index1 - currentBox.index1][currentColumn + p.index2].GetComponent<Box>());
            }

            if ((currentColumn + p.index2 < myboard[0].Length && currentColumn + p.index2 >= 0) && (currentBox.index1 - p.index1 < myboard.Length && currentBox.index1 - p.index1 - 1 >= 0))
            {
                patternBox.Add(myboard[currentBox.index1 - p.index1 - 1][currentColumn + p.index2].GetComponent<Box>());
            }
        }
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.animation;
        animators[activePattern].AttackAnimation(transform, patternBox, startRotation);
        projections[activePattern].SetActive(false);

    }

    public void OnAttackAnimationEnd()
    {
        projections[activePattern].SetActive(true);
        List<Pawn> pawnsHitted = new List<Pawn>();
        foreach (Box b in patternBox)
        {
            for (int i = 0; i < bm.pawns.Count; i++)
            {
                if (bm.pawns[i].player != player && bm.pawns[i].currentBox == b)
                {
                    pawnsHitted.Add(bm.pawns[i]);
                    pawnHitted++;
                }
            }
        }
        if (superAttack)
        {
            switch (pawnsHitted.Count)
            {
                case 0:
                    CustomLogger.Log("Nessuna pedina nel Pattern");
                    break;
                case 1:
                    pawnsHitted[0].OnDeathEnd += OnPawnKilled;
                    pawnsHitted[0].KillPawn();
                    CustomLogger.Log("Pedina Uccisa");
                    myelements.UseSuperAttack();
                    return;
                default:
                    bm.superAttackPressed = true;
                    foreach (Pawn p in pawnsHitted)
                    {
                        p.killMarker = true;
                        p.projections[p.activePattern].SetActive(true);
                    }
                    CustomLogger.Log("Scegli la pedina da uccidere");
                    break;
            }
        }
        else
        {
            foreach (Pawn p in pawnsHitted)
            {
                switch (p.currentBox.element)
                {
                    case Element.Purple:
                    case Element.Orange:
                    case Element.Azure:
                        p.OnDamageEnd += OnPawnDamaged;
                        p.PlayDamagedAnimation();
                        myelements.AddElement(p.currentBox.element);
                        p.currentBox.AttackBox();
                        break;
                    case Element.NeutralWhite:
                        p.OnDamageEnd += OnPawnDamaged;
                        p.PlayDamagedAnimation();
                        p.currentBox.ChangeNeutralType();
                        break;
                    case Element.NeutralBlack:
                        p.OnDeathEnd += OnPawnDamaged;
                        p.currentBox.ChangeNeutralType();
                        p.KillPawn();                        
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #region Damaged

    public void PlayDamagedAnimation()
    {
        animators[activePattern].PlayDamagedAnimation();
    }

    private void OnPawnDamaged(Pawn pawnDamaged)
    {
        pawnDamaged.OnDamageEnd -= OnPawnDamaged;
        PawnAnimationEnded++;
    }

    private void OnDamagedAnimationEnd()
    {
        OnDamageEnd(this);
    }

    #endregion

    #region Kill

    public void OnPawnKilled(Pawn pawnKilled)
    {
        pawnKilled.OnDeathEnd -= OnPawnKilled;
        OnAttackEnd();
    }

    /// <summary>
    /// Funzione che prende in input una pedina e la distrugge disattivando tutte le funzioni necessarie
    /// </summary>
    /// <param name="pawnToKill"></param>
    public void KillPawn()
    {
        DisableAttackPattern();
        currentBox.free = true;
        currentBox = null;
        animators[activePattern].PlayDeathAnimation();
    }

    private void DeathAnimationEnd()
    {
        if (player == Player.player1)
        {
            BoardManager.Instance.p1pawns--;
            if (BoardManager.Instance.p1pawns <= 0)
            {
                BoardManager.Instance.uiManager.winScreen.SetActive(true);
                BoardManager.Instance.uiManager.gameResult.text = "Science wins! \n " + "The game ended in " + BoardManager.Instance.turnManager.numberOfTurns + " turns.";

            }
        }
        else if (player == Player.player2)
        {
            BoardManager.Instance.p2pawns--;
            if (BoardManager.Instance.p2pawns <= 0)
            {
                BoardManager.Instance.uiManager.winScreen.SetActive(true);
                BoardManager.Instance.uiManager.gameResult.text = "Magic wins! \n" + "The game ended in " + BoardManager.Instance.turnManager.numberOfTurns + " turns.";
            }
        }
        bm.pawns.Remove(this);
        gameObject.SetActive(false);
        OnDeathEnd(this);
    }

    #endregion

    #endregion

    #region Movement

    public bool CheckMovementPattern(Box boxToMove)

    {
        if ((boxToMove.index1 == currentBox.index1 + 1 || boxToMove.index1 == currentBox.index1 - 1 || boxToMove.index1 == currentBox.index1) && (boxToMove.index2 == currentBox.index2 || boxToMove.index2 == currentBox.index2 + 1 || boxToMove.index2 == currentBox.index2 - 1))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che esegue tutti i controlli sulla casella e se rispetta i requisiti muove la pedina
    /// ritorna true se la pedina si muove, ritorna false se non è avvenuto
    /// </summary>
    /// <param name="boxindex1"></param>
    /// <param name="boxindex2"></param>
    /// <param name="boxToMove"></param>
    /// <returns></returns>
    public void Move(Box boxToMove)
    {
        transform.LookAt(new Vector3(boxToMove.transform.position.x, transform.position.y, boxToMove.transform.position.z));
        transform.Rotate(new Vector3(0, 90 - startRotation.y, 0));
        projections[activePattern].SetActive(false);
        DisableMovementBoxes();
        DisableAttackPattern();
        currentBox.free = true;
        if (currentBox.element == Element.NeutralBlack)
            currentBox.walkable = true;
        currentBox = boxToMove.GetComponent<Box>();
        currentBox.free = false;
        ForceMoveProjection(true);
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.animation;
        animators[activePattern].MovementAnimation(transform, boxToMove.transform.position, activeSpeed);
    }

    private void OnMovementCompleted()
    {
        projections[activePattern].SetActive(true);
        OnMovementEnd();
    }

    /// <summary>
    /// Funzione che sposta la proiezione nella box che gli viene passata come parametro
    /// </summary>
    /// <param name="boxToMove"></param>
    /// <returns></returns>
    public bool MoveProjection(Box boxToMove)
    {
        if ((boxToMove.index1 == currentBox.index1 + 1 || boxToMove.index1 == currentBox.index1 - 1 || boxToMove.index1 == currentBox.index1) && (boxToMove.index2 == currentBox.index2 || boxToMove.index2 == currentBox.index2 + 1 || boxToMove.index2 == currentBox.index2 - 1) && (boxToMove.free || boxToMove == currentBox))
        {
            DisableAttackPattern();
            if (boxToMove == currentBox)
            {
                transform.eulerAngles = startRotation;
            }
            else
            {
                transform.LookAt(new Vector3(boxToMove.transform.position.x, transform.position.y, boxToMove.transform.position.z));
                transform.Rotate(new Vector3(0, 90 - startRotation.y, 0));
            }
            projections[activePattern].transform.position = new Vector3(boxToMove.transform.position.x, boxToMove.transform.position.y + graphics[activePattern].transform.position.y, boxToMove.transform.position.z);
            projectionTempBox = boxToMove;
            ShowAttackPattern();
            ShowMovementBoxes();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che forza lo spostamento della proiezione nella casella della pedina corrispondente
    /// </summary>
    public void ForceMoveProjection(bool keepRotation)
    {
        projections[activePattern].transform.position = new Vector3(transform.position.x, transform.position.y + graphics[activePattern].transform.position.y, transform.position.z);
        projectionTempBox = currentBox;
        if (!keepRotation)
        {
            transform.eulerAngles = startRotation;
        }
    }

    #endregion

    /// <summary>
    /// Funzione che randomizza il pattern della pedina e gli assegna il colore corrispondente
    /// </summary>
    public void RandomizePattern()
    {
        graphics[activePattern].SetActive(false);
        projections[activePattern].SetActive(false);
        UnsubscribeAnimationEvent();
        activePattern = UnityEngine.Random.Range(0, patterns.Count);
        if (activePattern == 4 || activePattern == 5)
        {
            bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.choosing;
        }
        activeSpeed = speeds[activePattern];
        graphics[activePattern].SetActive(true);
        projections[activePattern].SetActive(true);
        SubscribeAnimationEvent();
        randomized = true;
    }

    /// <summary>
    /// Funzione che imposta il pattern attivo con il valore che gli viene passato
    /// </summary>
    /// <param name="index"></param>
    public void ChangePattern(int index)
    {
        graphics[activePattern].SetActive(false);
        UnsubscribeAnimationEvent();
        activePattern = index;
        activeSpeed = speeds[activePattern];
        graphics[activePattern].SetActive(true);
        SubscribeAnimationEvent();
    }

    private void SubscribeAnimationEvent()
    {
        if (animators[activePattern] != null)
        {
            animators[activePattern].OnAttackAnimationEnd += OnAttackAnimationEnd;
            animators[activePattern].OnMovementAnimationEnd += OnMovementCompleted;
            animators[activePattern].OnDamagedAnimationEnd += OnDamagedAnimationEnd;
            animators[activePattern].OnDeathAnimationEnd += DeathAnimationEnd;
        }
    }

    private void UnsubscribeAnimationEvent()
    {
        if (animators[activePattern] != null)
        {
            animators[activePattern].OnAttackAnimationEnd -= OnAttackAnimationEnd;
            animators[activePattern].OnMovementAnimationEnd -= OnMovementCompleted;
            animators[activePattern].OnDamagedAnimationEnd -= OnDamagedAnimationEnd;
            animators[activePattern].OnDeathAnimationEnd -= DeathAnimationEnd;
        }
    }

    #endregion
}
