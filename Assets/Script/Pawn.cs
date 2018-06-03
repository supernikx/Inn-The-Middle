using System.Collections.Generic;
using UnityEngine;

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
    public Factions faction;
    public Box currentBox;
    public float activeSpeed;
    public List<float> speeds = new List<float>();
    [HideInInspector]
    public List<GameObject> projections;
    [Space]
    [Header("Attack Settings")]
    public bool _attackMarker;
    public bool attackMarker
    {
        get
        {
            return _attackMarker;
        }
        set
        {
            _attackMarker = value;
        }
    }
    public int activePattern;
    public List<Attack> patterns;

    [Header("Placing Settings")]
    public int projectionPlacingStartPositionIndex1;
    public int projectionPlacingStartPositionIndex2;
    [HideInInspector]
    public int projectionPlacingPositionIndex1;
    [HideInInspector]
    public int projectionPlacingPositionIndex2;

    //variabili private
    private BoardManager bm;
    [HideInInspector]
    public Box projectionTempBox;
    private Transform[][] myboard, enemyboard;
    private PlayerElements myelements;
    private List<GameObject> graphics;
    private Vector3 startRotation;
    private List<IPawnAnimations> animators;

    // Use this for initialization
    void Start()
    {
        selected = false;
        randomized = false;
        bm = BoardManager.Instance;
        projectionTempBox = currentBox;
        startRotation = transform.eulerAngles;
        graphics = new List<GameObject>();
        projections = new List<GameObject>();
        animators = new List<IPawnAnimations>();
        SetGraphics();
        SetBoards();
        attackMarker = false;
    }

    /// <summary>
    /// Funzione che imposta qual'è la nostra board e qual'è quella dell'avversario
    /// </summary>
    private void SetBoards()
    {
        if (faction == Factions.Magic)
        {
            myboard = bm.magicBoard;
            enemyboard = bm.scienceBoard;
            myelements = bm.MagicElements;
        }
        else if (faction == Factions.Science)
        {
            myboard = bm.scienceBoard;
            enemyboard = bm.magicBoard;
            myelements = bm.ScienceElements;
        }
    }

    /// <summary>
    /// Funzione che imposta la proiezione e la grafica delle pedine nelle rispettive liste
    /// </summary>
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

    #region API

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

            else if (((currentColumn + patternindex2 < myboard[0].Length && currentColumn + patternindex2 >= 0) && (currentRow - patternindex1 < myboard.Length && currentRow - patternindex1 - 1 >= 0)))
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

                else if (((currentColumn + p.index2 < myboard[0].Length && currentColumn + p.index2 >= 0) && (currentRow - p.index1 < myboard.Length && currentRow - p.index1 - 1 >= 0)) && myboard[currentRow - p.index1 - 1][currentColumn + p.index2].GetComponent<Box>() != currentBox)
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

            else if ((currentColumn + patternindex2 < myboard[0].Length && currentColumn + patternindex2 >= 0) && (currentRow - patternindex1 < myboard.Length && currentRow - patternindex1 - 1 >= 0))
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

                else if ((currentColumn + a.index2 < myboard[0].Length && currentColumn + a.index2 >= 0) && (currentRow - a.index1 < myboard.Length && currentRow - a.index1 - 1 >= 0))
                {
                    myboard[currentRow - a.index1 - 1][currentColumn + a.index2].GetComponent<Box>().SetAsDefault();
                }
            }
        }
    }

    #endregion

    #region Attack

    List<Box> patternBox = new List<Box>();
    Pawn pawnAttackClicked;
    bool superAttack;
    int pawnHitted;
    int _pawnAnimationEnded;

    /// <summary>
    /// Property che controlla se le animazioni di danneggiamento concluse coincidono con le pedine colpite, nel caso fosse vero avvisa il board manager della fine dell'attacco
    /// </summary>
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
                if (OnAttackEnd != null)
                    OnAttackEnd();
            }
        }
    }

    /// <summary>
    /// Funzione che controlla se nel pattern è presente una pedina aversaria, allora ritorna true e marchia queste pedine con l'attackMarker, altrimenti ritorna false
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
                if (p.faction != faction)
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
                    if (p.faction != faction)
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

    /// <summary>
    /// Funzione che marchia con markattack le pedine presenti nel pattern
    /// </summary>
    public void MarkAttackPawn()
    {
        if (activePattern == 2 && !CheckAttackPattern())
        {
            return;
        }
        else
        {
            int currentColumn = currentBox.index2;
            foreach (Pattern a in patterns[activePattern].pattern)
            {
                foreach (Pawn p in bm.pawns)
                {
                    if (p.faction != faction)
                    {
                        if (((currentColumn + a.index2 < enemyboard[0].Length && currentColumn + a.index2 >= 0) && (a.index1 - currentBox.index1 < enemyboard.Length && a.index1 - currentBox.index1 >= 0)) && ((p.currentBox.index1 == a.index1 - currentBox.index1) && (p.currentBox.index2 == currentColumn + a.index2)))
                        {
                            p.attackMarker = true;
                            CustomLogger.Log("c'è una pedina avversaria nel pattern");
                        }
                    }
                }                
            }
            bm.PawnHighlighted(true);
        }
    }

    /// <summary>
    /// Funzione che prendendo come valori un boolean che identifica un super attacco e la pedina cliccata per eseguire l'attacco, identifica tutte le caselle del pattern e le passa all'animator 
    /// della pedina attiva per far avvenire l'animazione d'attacco e imposta lo stato del turno su animation
    /// </summary>
    /// <param name="_superAttack"></param>
    /// <param name="_pawnClicked"></param>
    public void AttackBehaviour(bool _superAttack, Pawn _pawnClicked = null)
    {
        DisableAttackPattern();
        superAttack = _superAttack;
        pawnAttackClicked = _pawnClicked;
        pawnHitted = 0;
        _pawnAnimationEnded = 0;
        patternBox.Clear();
        BoardManager.Instance.turnManager.turnsWithoutAttack = 0;
        int currentRow = currentBox.index1;
        int currentColumn = currentBox.index2;
        foreach (Pattern p in patterns[activePattern].pattern)
        {
            if ((currentColumn + p.index2 < enemyboard[0].Length && currentColumn + p.index2 >= 0) && (p.index1 - currentRow < enemyboard.Length && p.index1 - currentRow >= 0))
            {
                patternBox.Add(enemyboard[p.index1 - currentRow][currentColumn + p.index2].GetComponent<Box>());
            }

            else if ((currentColumn + p.index2 < myboard[0].Length && currentColumn + p.index2 >= 0) && (currentRow - p.index1 < myboard.Length && currentRow - p.index1 - 1 >= 0))
            {
                patternBox.Add(myboard[currentRow - p.index1 - 1][currentColumn + p.index2].GetComponent<Box>());
            }
        }
        bm.vfx.DeselectPawn();
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.animation;
        animators[activePattern].AttackAnimation(transform, patternBox, startRotation);
    }

    /// <summary>
    /// Funzione chiamata dall'animator quando finisce l'animazione d'attacco della pedina, raccoglie gli elementi o esegue un super attacco (in base al bool della funzione AttackBehaviour)
    /// chiama le animazioni di danneggiamento/morte delle pedine coinvolte nell'attacco
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        List<Pawn> pawnsHitted = new List<Pawn>();
        foreach (Box b in patternBox)
        {
            for (int i = 0; i < bm.pawns.Count; i++)
            {
                if (bm.pawns[i].faction != faction && bm.pawns[i].currentBox == b)
                {
                    pawnsHitted.Add(bm.pawns[i]);
                    pawnHitted++;
                }
            }
        }
        if (superAttack)
        {
            myelements.UseSuperAttack();
            pawnAttackClicked.OnDeathEnd += OnPawnKilled;
            pawnAttackClicked.KillPawn();
            CustomLogger.Log("Pedina Uccisa");
            return;
        }
        else
        {
            foreach (Pawn p in pawnsHitted)
            {
                switch (p.currentBox.element)
                {
                    case Element.Red:
                    case Element.Green:
                    case Element.Blue:
                        myelements.AddElement(p.currentBox.element);
                        p.currentBox.AttackBox();
                        p.OnDamageEnd += OnPawnDamaged;
                        p.PlayDamagedAnimation();
                        break;
                    case Element.NeutralWhite:
                        p.currentBox.ChangeNeutralType();
                        p.OnDamageEnd += OnPawnDamaged;
                        p.PlayDamagedAnimation();
                        break;
                    case Element.NeutralBlack:
                        p.currentBox.ChangeNeutralType();
                        p.OnDeathEnd += OnPawnDamaged;
                        p.KillPawn();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #region Damaged

    /// <summary>
    /// Esegue l'animazione di danneggiamento chiamando l'animator attivo
    /// </summary>
    public void PlayDamagedAnimation()
    {
        animators[activePattern].PlayDamagedAnimation();
    }

    /// <summary>
    /// Funzione che avvisa la pedina attaccante della fine dell'animazione di dannaggeiamento di una delle pedine attaccate
    /// </summary>
    /// <param name="pawnDamaged"></param>
    private void OnPawnDamaged(Pawn pawnDamaged)
    {
        pawnDamaged.OnDamageEnd -= OnPawnDamaged;
        PawnAnimationEnded++;
    }

    /// <summary>
    /// Funzione che viene chiamata dall'animator quando è finita l'animazione di danneggiamento, poi avviserà la pedina attaccante della sua conclusione
    /// </summary>
    private void OnDamagedAnimationEnd()
    {
        OnDamageEnd(this);
    }

    #endregion

    #region Kill

    /// <summary>
    /// Funzione che viene chiamata quando una pedina attaccata ha finito l'animazione di morte, avvisa il boardmanager che l'attacco e finito
    /// </summary>
    /// <param name="pawnKilled"></param>
    public void OnPawnKilled(Pawn pawnKilled)
    {
        pawnKilled.OnDeathEnd -= OnPawnKilled;
        OnAttackEnd();
    }

    /// <summary>
    /// Funzione che avvia l'animazione di morte su questa pedina
    /// </summary>
    /// <param name="pawnToKill"></param>
    public void KillPawn()
    {
        DisableAttackPattern();
        currentBox.free = true;
        currentBox = null;
        animators[activePattern].PlayDeathAnimation();
    }

    /// <summary>
    /// Funzione che viene chiamata dall'animator quando è finita l'animazione di morte di questa pedina
    /// controlla se una delle 2 fazioni è rimasta senza pedine e di conseguenza c'è un vincitore
    /// avvisa la pedina attaccante che è finita la sua animazione
    /// </summary>
    private void DeathAnimationEnd()
    {
        if (faction == Factions.Magic)
        {
            BoardManager.Instance.magicPawns.Remove(this);
        }
        else if (faction == Factions.Science)
        {
            BoardManager.Instance.sciencePawns.Remove(this);
        }
        bm.WinCondition(false);
        bm.pawns.Remove(this);
        gameObject.SetActive(false);
        OnDeathEnd(this);
    }

    #endregion

    #endregion

    #region Movement

    /// <summary>
    /// Funzione che avvia l'animazione di movimento sulla boxToMove impostando la fase del turno su animation
    /// </summary>
    /// <param name="boxToMove"></param>
    public void MoveBehaviour()
    {
        Box boxToMove = projectionTempBox;
        transform.LookAt(new Vector3(boxToMove.transform.position.x, transform.position.y, boxToMove.transform.position.z));
        transform.Rotate(new Vector3(0, 90 - startRotation.y, 0));
        DisableMovementBoxes();
        DisableAttackPattern();
        currentBox.free = true;
        if (currentBox.element == Element.NeutralBlack)
            currentBox.walkable = true;
        currentBox = boxToMove.GetComponent<Box>();
        currentBox.free = false;
        ForceMoveProjection(true);
        bm.turnManager.CurrentTurnState = TurnManager.PlayTurnState.animation;
        animators[activePattern].MovementAnimation(transform, boxToMove.transform.position, activeSpeed, startRotation);
    }

    /// <summary>
    /// Funzione che viene chiamata dall'animator quando è finita l'animazione di movimento
    /// </summary>
    private void OnMovementCompleted()
    {
        if (OnMovementEnd != null)
            OnMovementEnd();
    }

    #region Projection

    /// <summary>
    /// Funzione che sposta la proiezione nella direzione che gli viene passata come parametro
    /// </summary>
    /// <param name="projectionDirection"></param>
    /// <returns></returns>
    public void MoveProjection(Directions projectionDirection)
    {
        Box boxToMove = currentBox;
        bool moved = false;
        switch (faction)
        {
            case Factions.Magic:
                switch (projectionDirection)
                {
                    case Directions.up:
                        if (currentBox.index2 + 1 < myboard[0].Length)
                        {
                            boxToMove = myboard[currentBox.index1][currentBox.index2 + 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.down:
                        if (currentBox.index2 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1][currentBox.index2 - 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.right:
                        if (currentBox.index1 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1 - 1][currentBox.index2].GetComponent<Box>();
                        }
                        break;
                    case Directions.left:
                        if (currentBox.index1 + 1 < myboard.Length)
                        {
                            boxToMove = myboard[currentBox.index1 + 1][currentBox.index2].GetComponent<Box>();
                        }
                        break;
                    case Directions.upright:
                        if (currentBox.index1 - 1 >= 0 && currentBox.index2 + 1 < myboard[0].Length)
                        {
                            boxToMove = myboard[currentBox.index1 - 1][currentBox.index2 + 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.upleft:
                        if (currentBox.index1 + 1 < myboard.Length && currentBox.index2 + 1 < myboard[0].Length)
                        {
                            boxToMove = myboard[currentBox.index1 + 1][currentBox.index2 + 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.downright:
                        if (currentBox.index1 - 1 >= 0 && currentBox.index2 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1 - 1][currentBox.index2 - 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.downleft:
                        if (currentBox.index1 + 1 < myboard.Length && currentBox.index2 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1 + 1][currentBox.index2 - 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.idle:
                        projections[activePattern].SetActive(false);
                        break;
                }
                break;
            case Factions.Science:
                switch (projectionDirection)
                {
                    case Directions.up:
                        if (currentBox.index2 + 1 < myboard[0].Length)
                        {
                            boxToMove = myboard[currentBox.index1][currentBox.index2 + 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.down:
                        if (currentBox.index2 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1][currentBox.index2 - 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.right:
                        if (currentBox.index1 + 1 < myboard.Length)
                        {
                            boxToMove = myboard[currentBox.index1 + 1][currentBox.index2].GetComponent<Box>();
                        }
                        break;
                    case Directions.left:
                        if (currentBox.index1 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1 - 1][currentBox.index2].GetComponent<Box>();
                        }
                        break;
                    case Directions.upright:
                        if (currentBox.index1 + 1 < myboard.Length && currentBox.index2 + 1 < myboard[0].Length)
                        {
                            boxToMove = myboard[currentBox.index1 + 1][currentBox.index2 + 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.upleft:
                        if (currentBox.index1 - 1 >= 0 && currentBox.index2 + 1 < myboard[0].Length)
                        {
                            boxToMove = myboard[currentBox.index1 - 1][currentBox.index2 + 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.downright:
                        if (currentBox.index1 + 1 < myboard.Length && currentBox.index2 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1 + 1][currentBox.index2 - 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.downleft:
                        if (currentBox.index1 - 1 >= 0 && currentBox.index2 - 1 >= 0)
                        {
                            boxToMove = myboard[currentBox.index1 - 1][currentBox.index2 - 1].GetComponent<Box>();
                        }
                        break;
                    case Directions.idle:
                        projections[activePattern].SetActive(false);
                        break;
                }
                break;
        }
        if (bm.CheckFreeBox(boxToMove))
        {
            moved = true;
        }
        DisableAttackPattern();
        if (moved)
        {
            projections[activePattern].SetActive(true);
            transform.LookAt(new Vector3(boxToMove.transform.position.x, transform.position.y, boxToMove.transform.position.z));
            transform.Rotate(new Vector3(0, 90 - startRotation.y, 0));
            projections[activePattern].transform.position = new Vector3(boxToMove.transform.position.x, boxToMove.transform.position.y + graphics[activePattern].transform.position.y, boxToMove.transform.position.z);
            projectionTempBox = boxToMove;
        }
        else
        {
            ForceMoveProjection(false);
        }
        ShowAttackPattern();
        ShowMovementBoxes();
    }

    /// <summary>
    /// Funzione che muove la proiezione nella placing phase nella direzione passata come parametro
    /// </summary>
    /// <param name="projectionDirection"></param>
    public void MoveProjectionPlacing(Directions projectionDirection)
    {
        switch (projectionDirection)
        {
            case Directions.up:
                int i = projectionPlacingPositionIndex2 + 1;
                if (i < myboard[0].Length)
                {
                    while (!myboard[projectionPlacingPositionIndex1][i].GetComponent<Box>().free)
                    {
                        i++;
                        if (i >= myboard[0].Length)
                            break;
                    }
                }
                if (i < myboard[0].Length)
                {
                    projectionPlacingPositionIndex2 = i;
                }
                break;
            case Directions.down:
                int j = projectionPlacingPositionIndex2 - 1;
                if (j >= 0)
                {
                    while (!myboard[projectionPlacingPositionIndex1][j].GetComponent<Box>().free)
                    {
                        j--;
                        if (j < 0)
                            break;
                    }
                }
                if (j >= 0)
                {
                    projectionPlacingPositionIndex2 = j;
                }
                break;
            default:
                Debug.Log("Direzione errata");
                break;
        }
        projections[activePattern].transform.position = new Vector3(myboard[projectionPlacingPositionIndex1][projectionPlacingPositionIndex2].position.x, myboard[projectionPlacingPositionIndex1][projectionPlacingPositionIndex2].position.y + graphics[activePattern].transform.position.y, myboard[projectionPlacingPositionIndex1][projectionPlacingPositionIndex2].position.z);
    }

    /// <summary>
    /// Funzione che imposta la posizione della proiezione della pedine selezionata nella fase di placing
    /// </summary>
    public void SetProjection()
    {
        projections[activePattern].SetActive(true);
        projectionPlacingPositionIndex1 = projectionPlacingStartPositionIndex1;
        projectionPlacingPositionIndex2 = projectionPlacingStartPositionIndex2;
        if (!myboard[projectionPlacingPositionIndex1][projectionPlacingPositionIndex2].GetComponent<Box>().free)
        {
            int i = projectionPlacingPositionIndex2 + 1;
            if (i < myboard[0].Length)
            {
                while (!myboard[projectionPlacingPositionIndex1][i].GetComponent<Box>().free)
                {
                    i++;
                    if (i >= myboard[0].Length)
                        break;
                }
                if (i < myboard[0].Length)
                {
                    projectionPlacingPositionIndex2 = i;
                }
            }
            int j = projectionPlacingPositionIndex2 - 1;
            if (j >= 0)
            {
                while (!myboard[projectionPlacingPositionIndex1][j].GetComponent<Box>().free)
                {
                    j--;
                    if (j < 0)
                        break;
                }
            }
            if (j >= 0)
            {
                projectionPlacingPositionIndex2 = j;
            }
        }
        projections[activePattern].transform.position = new Vector3(myboard[projectionPlacingPositionIndex1][projectionPlacingPositionIndex2].position.x, myboard[projectionPlacingPositionIndex1][projectionPlacingPositionIndex2].position.y + graphics[activePattern].transform.position.y, myboard[projectionPlacingPositionIndex1][projectionPlacingPositionIndex2].position.z);
    }

    /// <summary>
    /// Funzione che forza lo spostamento della proiezione nella casella della pedina corrispondente
    /// </summary>
    public void ForceMoveProjection(bool keepRotation)
    {
        projections[activePattern].SetActive(false);
        projections[activePattern].transform.position = new Vector3(transform.position.x, transform.position.y + graphics[activePattern].transform.position.y, transform.position.z);
        projectionTempBox = currentBox;
        if (!keepRotation)
        {
            transform.eulerAngles = startRotation;
        }
    }

    #endregion
    #endregion

    /// <summary>
    /// Funzione che randomizza il pattern della pedina e attiva il modello corrispondente
    /// </summary>
    public void RandomizePattern()
    {
        graphics[activePattern].SetActive(false);
        bm.vfx.DeselectPawn();
        UnsubscribeAnimationEvent();
        activePattern = UnityEngine.Random.Range(0, patterns.Count);
        activeSpeed = speeds[activePattern];
        graphics[activePattern].SetActive(true);
        bm.vfx.SelectPawn(this);
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

    /// <summary>
    /// Funzione che iscrive la pedina a tutti gli eventi dell'animator (chiamata quando viene randomizzato un nuovo pattern)
    /// </summary>
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

    /// <summary>
    /// Funzione che disiscrive la pedina a tutti gli eventi dell'animator (chiamata quando viene randomizzato un nuovo pattern)
    /// </summary>
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
