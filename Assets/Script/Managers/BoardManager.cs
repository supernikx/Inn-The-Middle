using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class BoardManager : MonoBehaviour
{

    public static BoardManager Instance;

    //variabili pubbliche
    public Transform[][] magicBoard, scienceBoard;
    public PlayerElements MagicElements, ScienceElements;
    public List<Pawn> pawns;
    [HideInInspector]
    public Pawn pawnSelected;
    [HideInInspector]
    public bool _superAttack, CanSuperAttack;
    public bool superAttack
    {
        get
        {
            return _superAttack;
        }
        set
        {
            _superAttack = value;
            uiManager.ActiveSuperAttackText();
            if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack || turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack)
            {
                if (_superAttack)
                {
                    CreateMarkList();
                    SelectNextPawnToAttack(Directions.idle);
                }
                else
                {
                    PawnHighlighted(true);
                }
            }
        }
    }
    [HideInInspector]
    public int pawnsToPlace;
    [HideInInspector]
    public List<Pawn> magicPawns, sciencePawns;
    List<Pawn> magicPlacing, sciencePlacing;
    [HideInInspector]
    public int MagicPawnIndex, SciencePawnIndex;
    public int magictiles, sciencetiles;
    public Box[] boxesArray;
    public int placingsLeft;
    public Factions p1Faction, p2Faction;
    [HideInInspector]
    private bool _TutorialInProgress = false;
    public bool TutorialInProgress
    {
        get
        {
            return _TutorialInProgress;
        }
        set
        {
            _TutorialInProgress = value;
            if (_TutorialInProgress)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    //managers
    [Header("Managers")]
    public TurnManager turnManager;
    public DraftManager draftManager;
    public UIManager uiManager;
    public VFXManager vfx;
    public TutorialManager tutorial;

    /// <summary>
    /// Funzioni che iscrivono/disiscrivono il boardmanager agli eventi appena viene abilitato/disabilitato
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnPause += OnGamePause;
        EventManager.OnUnPause += OnGameUnPause;
        EventManager.OnJoystickDisconnected += JoystickDisconnected;
        EventManager.OnJoystickRiconnected += JoystickRiconnected;
    }
    private void OnDisable()
    {
        EventManager.OnPause -= OnGamePause;
        EventManager.OnUnPause -= OnGameUnPause;
        EventManager.OnJoystickDisconnected -= JoystickDisconnected;
        EventManager.OnJoystickRiconnected -= JoystickRiconnected;
    }

    #region Pause

    public bool pause;

    /// <summary>
    /// Funzione che imposta la variabile pause a true stoppando il gioco
    /// </summary>
    private void OnGamePause()
    {
        Time.timeScale = 0f;
        pause = true;
    }

    /// <summary>
    /// Funzione che imposta la variabile pause a false facendo ripartire il gioco
    /// </summary>
    private void OnGameUnPause()
    {
        Time.timeScale = 1f;
        StartCoroutine(UnPauseCoroutine());
    }

    /// <summary>
    /// Coroutine che viene chiamata quando si toglie la pausa
    /// </summary>
    /// <returns></returns>
    public IEnumerator UnPauseCoroutine()
    {
        yield return null;
        pause = false;
    }

    #endregion
    #region JoystickDisconected

    /// <summary>
    /// Funzione che viene chiamata quando si disconnette un joystick
    /// </summary>
    private void JoystickDisconnected()
    {
        Time.timeScale = 0f;
        pause = true;
    }

    /// <summary>
    /// Funzione che viene chiamata quando si riconnette un joystick
    /// </summary>
    private void JoystickRiconnected()
    {
        if (!uiManager.pausePanel.activeSelf)
        {
            Time.timeScale = 1f;
            pause = false;
        }
    }

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        draftManager = GetComponent<DraftManager>();
        turnManager = GetComponent<TurnManager>();
        uiManager = GetComponent<UIManager>();
        vfx = GetComponent<VFXManager>();
        tutorial = GetComponent<TutorialManager>();
    }

    void Start()
    {
        placingsLeft = 1;
        superAttack = false;
        pause = false;
        pawns = FindObjectsOfType<Pawn>().ToList();
        pawnsToPlace = pawns.Count;
        boxesArray = FindObjectsOfType<Box>();
        magicPawns = new List<Pawn>();
        sciencePlacing = new List<Pawn>();
        MagicPawnIndex = 0;
        SciencePawnIndex = 0;
        SetupPawns();
        sciencePlacing = new List<Pawn>(sciencePawns);
        magicPlacing = new List<Pawn>(magicPawns);
    }

    /// <summary>
    /// Funzione che crea le 2 Liste di pedine Magic e Science nell'ordine corretto
    /// </summary>
    void SetupPawns()
    {
        int magicnumber = 1;
        int sciencenumber = 1;
        while (magicPawns.Count < 4 || sciencePawns.Count < 4)
        {
            foreach (Pawn p in pawns)
            {
                if (p.faction == Factions.Magic && p.name == ("Pawn" + magicnumber))
                {
                    magicPawns.Add(p);
                    magicnumber++;
                }
                else if (p.faction == Factions.Science && p.name == ("Pawn" + sciencenumber))
                {
                    sciencePawns.Add(p);
                    sciencenumber++;
                }
            }
        }
    }

    #region Movement

    /// <summary>
    /// Funzione che controlla se la casella selezionata fa parte della board del player di turno e chiama la funzione di movimento della pawnselected
    /// si siscrive a 2 eventi diversi nel caso il bool checkphase sia true o false
    /// </summary>
    /// <param name="boxclicked"></param>
    public void Movement(bool checkphase)
    {
        if (pawnSelected != null)
        {
            if (pawnSelected.faction == turnManager.CurrentPlayerTurn && pawnSelected.currentBox != pawnSelected.projectionTempBox)
            {
                if (checkphase)
                {
                    pawnSelected.OnMovementEnd += OnMovementCheckEnd;
                    PawnToRandom.Remove(pawnSelected);
                }
                else
                {
                    pawnSelected.OnMovementEnd += OnMovementEnd;
                }
                UnmarkAttackMarker();
                PawnHighlighted(false);
                pawnSelected.MoveBehaviour();
            }
            else
            {
                CustomLogger.Log("Casella non valida");
            }
        }
    }

    /// <summary>
    /// Funzione che viene chiamata quando si conclude il moviemento se il bool della funzione movement era true
    /// </summary>
    private void OnMovementCheckEnd()
    {
        pawnSelected.OnMovementEnd -= OnMovementCheckEnd;
        CustomLogger.Log(pawnSelected.faction + " si è mosso");
        pawnSelected.randomized = false;
        switch (turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                for (int i = 0; i < magicPawns.Count; i++)
                {
                    if (magicPawns[i] == pawnSelected)
                    {
                        MagicPawnIndex = i;
                        break;
                    }
                }
                break;
            case Factions.Science:
                for (int i = 0; i < sciencePawns.Count; i++)
                {
                    if (sciencePawns[i] == pawnSelected)
                    {
                        SciencePawnIndex = i;
                        break;
                    }
                }
                break;
        }
        turnManager.CurrentTurnState = TurnManager.PlayTurnState.check;
        if (PawnToRandom.Count > 0)
            SelectNextPawnCheckPhase(Directions.idle);
        else
            turnManager.CurrentTurnState = TurnManager.PlayTurnState.movementattack;
    }

    /// <summary>
    /// Funzione che viene chiamata quando si conclude il moviemento se il bool della funzione movement era true
    /// </summary>
    private void OnMovementEnd()
    {
        CustomLogger.Log(pawnSelected.faction + " si è mosso");
        pawnSelected.OnMovementEnd -= OnMovementEnd;
        pawnSelected.ShowAttackPattern();
        turnManager.CurrentTurnState = TurnManager.PlayTurnState.attack;
    }

    /// <summary>
    /// Funzione che teletrasporta la pawnselected alla box passata come paramentro se rispetta i requisiti richiesti
    /// </summary>
    /// <param name="boxclicked"></param>
    public void PlacingTeleport()
    {
        if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing && turnManager.CurrentTurnState == TurnManager.PlayTurnState.placing)
        {
            if (pawnSelected != null)
            {
                Box boxSelected = pawnSelected.currentBox;
                switch (pawnSelected.faction)
                {
                    case Factions.Magic:
                        boxSelected = magicBoard[pawnSelected.projectionPlacingPositionIndex1][pawnSelected.projectionPlacingPositionIndex2].GetComponent<Box>();
                        magicPlacing.Remove(pawnSelected);
                        break;
                    case Factions.Science:
                        boxSelected = scienceBoard[pawnSelected.projectionPlacingPositionIndex1][pawnSelected.projectionPlacingPositionIndex2].GetComponent<Box>();
                        sciencePlacing.Remove(pawnSelected);
                        break;
                }
                pawnSelected.transform.position = boxSelected.transform.position;
                pawnSelected.currentBox = boxSelected;
                pawnSelected.currentBox.free = false;
                DeselectPawn();
                pawnsToPlace--;
                placingsLeft--;
                if (placingsLeft == 0 || pawnsToPlace == 0)
                {
                    turnManager.ChangeTurn();
                    placingsLeft = 2;
                }
                else
                {
                    SelectNextPawnToPlace(Directions.idle);
                }
            }
        }
    }

    #endregion

    #region API

    /// <summary>
    /// Funzione che permette di scegliere la fazione
    /// </summary>
    public void FactionChosen(int _factionID)
    {
        if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.faction)
        {
            if (_factionID == 1)
            {
                p1Faction = Factions.Magic;
                p2Faction = Factions.Science;
            }
            else if (_factionID == 2)
            {
                p1Faction = Factions.Science;
                p2Faction = Factions.Magic;
            }
        }
    }

    #region Attack
    List<Pawn> MarkedPawnList = new List<Pawn>();
    int MarkedPawnIndex;

    /// <summary>
    /// Funzione che crea la lista di pedine che possono essere colpite dalla pedina selezionata
    /// </summary>
    public void CreateMarkList()
    {
        foreach (Pawn p in pawns)
        {
            if (p.attackMarker)
            {
                MarkedPawnList.Add(p);
            }
        }
    }

    /// <summary>
    /// Funzione che toglie il marchio di attacco a tutte le pedine
    /// </summary>
    public void UnmarkAttackMarker()
    {
        MarkedPawnList.Clear();
        foreach (Pawn p in pawns)
        {
            if (p.attackMarker)
            {
                p.attackMarker = false;
            }
        }
    }

    /// <summary>
    /// Funzione che chiama la funzione attack della pawnselected e gli passa come parametro un bool che identifica se è attavio o no il super attacco
    /// e la pedina che è stata cliccata per eseguire l'attacco
    /// </summary>
    /// <param name="pawnClicked"></param>
    public void Attack()
    {
        if (pawnSelected != null && !pause)
        {
            if (pawnSelected.CheckAttackPattern())
            {
                if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack || turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack)
                {
                    if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack)
                    {
                        pawnSelected.DisableAttackPattern();
                        pawnSelected.ForceMoveProjection(false);
                        pawnSelected.DisableMovementBoxes();
                    }
                    vfx.ResetMark();
                    pawnSelected.OnAttackEnd += OnAttackEnd;
                    uiManager.UpdateExpressions(Expressions.Happy);
                    pawnSelected.AttackBehaviour(superAttack, (superAttack) ? MarkedPawnList[MarkedPawnIndex] : null);
                }
            }
        }
    }

    /// <summary>
    /// Funzione che viene chiamata quando sono finite tutte le animazioni d'attacco, danneggiamento e morte necessarie
    /// </summary>
    public void OnAttackEnd()
    {
        pawnSelected.OnAttackEnd -= OnAttackEnd;
        CustomLogger.Log(pawnSelected.faction + " ha attaccato");
        turnManager.ChangeTurn();
    }

    /// <summary>
    /// Funzione che attiva/disattiv ail bool superattack
    /// </summary>
    public void ActiveSuperAttack()
    {
        superAttack = !superAttack;
        Debug.Log("superattacco premuto");
    }

    #endregion

    #region Check

    /// <summary>
    /// Funzione che controlla se è possibile eseguire un superattacco
    /// </summary>
    public void CheckSuperAttack()
    {
        switch (turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                CanSuperAttack = MagicElements.CheckSuperAttack();
                break;
            case Factions.Science:
                CanSuperAttack = ScienceElements.CheckSuperAttack();
                break;
        }
    }

    /// <summary>
    /// Funzione che viene chiamata all'inizio di ogni turno della fase di game e crea la lista di pedina colpite
    /// </summary>
    List<Pawn> PawnToRandom = new List<Pawn>();
    int checkpawnindex = 0;
    public void CheckPhaseControll()
    {
        turnManager.CheckAlreadyDone = true;
        PawnToRandom.Clear();
        checkpawnindex = 0;
        foreach (Pawn p in pawns)
        {
            if (!p.currentBox.walkable)
            {
                if (CheckFreeBoxes(p))
                {
                    PawnToRandom.Add(p);
                }
                else
                {
                    p.KillPawn();
                }
            }
        }
        if (PawnToRandom.Count > 0)
        {
            uiManager.UpdateExpressions(Expressions.Surprised);
            RandomizePatterns(PawnToRandom);
        }
        else
            turnManager.CurrentTurnState = TurnManager.PlayTurnState.movementattack;
    }

    /// <summary>
    /// Controlla se sono presenti delle pedine da scegliere (bianche/nere) e ritorna true se ci sono o false se non ci sono
    /// </summary>
    /// <returns></returns>
    public bool CheckPawnToChoose()
    {
        bool foundPawn = false;
        foreach (Pawn p in pawns)
        {
            if (p.activePattern == 4 || p.activePattern == 5)
            {
                foundPawn = true;
            }
        }
        return foundPawn;
    }

    /// <summary>
    /// Funzione che controlla se la casella che gli è stata passata in input è già occupata da un altro player o se non è walkable
    /// se è libera ritorna true, altrimenti se è occupata ritorna false
    /// </summary>
    /// <param name="boxclicked"></param>
    /// <returns></returns>
    public bool CheckFreeBox(Box boxclicked)
    {
        if (boxclicked.walkable && boxclicked.free)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Funzione che controlla tutte le caselle adiacenti di pawnToCheck e ritorna true se c'è almeno una casella disponibile altrimenti ritorna false
    /// </summary>
    /// <param name="pawnToCheck"></param>
    /// <returns></returns>
    private bool CheckFreeBoxes(Pawn pawnToCheck)
    {
        Transform[][] boardToUse;
        Box currentBox = pawnToCheck.currentBox;
        if (pawnToCheck.faction == Factions.Magic)
        {
            boardToUse = magicBoard;
        }
        else
        {
            boardToUse = scienceBoard;
        }

        for (int index1 = 0; index1 < boardToUse.Length; index1++)
        {
            for (int index2 = 0; index2 < boardToUse[0].Length; index2++)
            {
                if ((index1 == currentBox.index1 + 1 || index1 == currentBox.index1 - 1 || index1 == currentBox.index1) && (index2 == currentBox.index2 || index2 == currentBox.index2 + 1 || index2 == currentBox.index2 - 1)
                    && boardToUse[index1][index2].GetComponent<Box>() != currentBox && CheckFreeBox(boardToUse[index1][index2].GetComponent<Box>()))
                {
                    return true;
                }
            }
        }
        CustomLogger.Log("Non c'è una casella libera");
        return false;
    }
    #endregion

    #region Pawn

    /// <summary>
    /// Funzione che seleziona la prossima pedina della lista di quella fazione
    /// </summary>
    public void SelectNextPawn(Directions nextpawn)
    {
        PawnHighlighted(false);
        switch (turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                switch (nextpawn)
                {
                    case Directions.right:
                        MagicPawnIndex--;
                        if (MagicPawnIndex < 0)
                            MagicPawnIndex = magicPawns.Count - 1;
                        break;
                    case Directions.left:
                        MagicPawnIndex++;
                        if (MagicPawnIndex > magicPawns.Count - 1)
                            MagicPawnIndex = 0;
                        break;
                    case Directions.idle:
                        if (MagicPawnIndex > magicPawns.Count - 1)
                            MagicPawnIndex = 0;
                        else if (MagicPawnIndex < 0)
                            MagicPawnIndex = magicPawns.Count - 1;
                        break;
                }
                PawnSelected(magicPawns[MagicPawnIndex]);
                break;
            case Factions.Science:
                switch (nextpawn)
                {
                    case Directions.right:
                        SciencePawnIndex++;
                        if (SciencePawnIndex > sciencePawns.Count - 1)
                            SciencePawnIndex = 0;
                        break;
                    case Directions.left:
                        SciencePawnIndex--;
                        if (SciencePawnIndex < 0)
                            SciencePawnIndex = sciencePawns.Count - 1;
                        break;
                    case Directions.idle:
                        if (SciencePawnIndex < 0)
                            SciencePawnIndex = sciencePawns.Count - 1;
                        else if (SciencePawnIndex > sciencePawns.Count - 1)
                            SciencePawnIndex = 0;
                        break;
                }
                PawnSelected(sciencePawns[SciencePawnIndex]);
                break;
        }
    }

    /// <summary>
    /// Funzione che seleziona la prossima pedina che deve subire un superattacco (se è attivo)
    /// </summary>
    public void SelectNextPawnToAttack(Directions nextpawn)
    {
        PawnHighlighted(false);
        switch (nextpawn)
        {
            case Directions.right:
                MarkedPawnIndex++;
                if (MarkedPawnIndex > MarkedPawnList.Count - 1)
                    MarkedPawnIndex = 0;
                break;
            case Directions.left:
                MarkedPawnIndex--;
                if (MarkedPawnIndex < 0)
                    MarkedPawnIndex = MarkedPawnList.Count - 1;
                break;
            case Directions.idle:
                if (MarkedPawnIndex < 0)
                    MarkedPawnIndex = MarkedPawnList.Count - 1;
                else if (MarkedPawnIndex > MarkedPawnList.Count - 1)
                    MarkedPawnIndex = 0;
                break;
        }
        PawnHighlighted(true, MarkedPawnList[MarkedPawnIndex]);
    }

    /// <summary>
    /// Funzione che seleziona la prossima pedina della lista di quella fazione che deve essere piazzata
    /// </summary>
    public void SelectNextPawnToPlace(Directions nextpawn)
    {
        switch (turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                if (pawnSelected != null)
                    pawnSelected.projections[pawnSelected.activePattern].SetActive(false);
                switch (nextpawn)
                {
                    case Directions.right:
                        MagicPawnIndex--;
                        if (MagicPawnIndex < 0)
                            MagicPawnIndex = magicPlacing.Count - 1;
                        break;
                    case Directions.left:
                        MagicPawnIndex++;
                        if (MagicPawnIndex > magicPlacing.Count - 1)
                            MagicPawnIndex = 0;
                        break;
                    case Directions.idle:
                        if (MagicPawnIndex > magicPlacing.Count - 1)
                            MagicPawnIndex = 0;
                        else if (MagicPawnIndex < 0)
                            MagicPawnIndex = magicPlacing.Count - 1;
                        break;
                }
                PawnSelected(magicPlacing[MagicPawnIndex]);
                pawnSelected.SetProjection();
                break;
            case Factions.Science:
                if (pawnSelected != null)
                    pawnSelected.projections[pawnSelected.activePattern].SetActive(false);
                switch (nextpawn)
                {
                    case Directions.right:
                        SciencePawnIndex++;
                        if (SciencePawnIndex > sciencePlacing.Count - 1)
                            SciencePawnIndex = 0;
                        break;
                    case Directions.left:
                        SciencePawnIndex--;
                        if (SciencePawnIndex < 0)
                            SciencePawnIndex = sciencePlacing.Count - 1;
                        break;
                    case Directions.idle:
                        if (SciencePawnIndex < 0)
                            SciencePawnIndex = sciencePlacing.Count - 1;
                        else if (SciencePawnIndex > sciencePlacing.Count - 1)
                            SciencePawnIndex = 0;
                        break;
                }
                PawnSelected(sciencePlacing[SciencePawnIndex]);
                pawnSelected.SetProjection();
                break;
        }
    }

    /// <summary>
    /// Funzione che seleziona la prossima pedina della lista fra le pedine che sono state colpite e sono obbligate a spostarsi
    /// </summary>
    /// <param name="nextpawn"></param>
    public void SelectNextPawnCheckPhase(Directions nextpawn)
    {
        switch (turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                switch (nextpawn)
                {
                    case Directions.right:
                        checkpawnindex--;
                        if (checkpawnindex < 0)
                            checkpawnindex = PawnToRandom.Count - 1;
                        break;
                    case Directions.left:
                        checkpawnindex++;
                        if (checkpawnindex > PawnToRandom.Count - 1)
                            checkpawnindex = 0;
                        break;
                    case Directions.idle:
                        if (checkpawnindex > PawnToRandom.Count - 1)
                            checkpawnindex = 0;
                        else if (checkpawnindex < 0)
                            checkpawnindex = PawnToRandom.Count - 1;
                        break;
                }
                PawnSelected(PawnToRandom[checkpawnindex]);
                break;
            case Factions.Science:
                switch (nextpawn)
                {
                    case Directions.right:
                        checkpawnindex++;
                        if (checkpawnindex > PawnToRandom.Count - 1)
                            checkpawnindex = 0;
                        break;
                    case Directions.left:
                        checkpawnindex--;
                        if (checkpawnindex < 0)
                            checkpawnindex = PawnToRandom.Count - 1;
                        break;
                    case Directions.idle:
                        if (checkpawnindex < 0)
                            checkpawnindex = PawnToRandom.Count - 1;
                        else if (checkpawnindex > PawnToRandom.Count - 1)
                            checkpawnindex = 0;
                        break;
                }
                PawnSelected(PawnToRandom[checkpawnindex]);
                break;
        }
    }

    /// <summary>
    /// Funzione che imposta la variabile pawnSelected a null, imposta a false il bool selected
    /// </summary>
    public void DeselectPawn()
    {
        if (pawnSelected != null)
        {
            if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing)
            {
                pawnSelected.ForceMoveProjection(false);
            }
            else if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game)
            {
                pawnSelected.DisableMovementBoxes();
                pawnSelected.DisableAttackPattern();
                pawnSelected.ForceMoveProjection(!(turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack || turnManager.CurrentTurnState == TurnManager.PlayTurnState.check));
                if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.attack || turnManager.CurrentTurnState == TurnManager.PlayTurnState.movementattack)
                {
                    UnmarkAttackMarker();
                    superAttack = false;
                }
            }
            vfx.DeselectPawn();
            pawnSelected.selected = false;
            pawnSelected = null;
        }
    }

    /// <summary>
    /// Funzione che imposta nella variabile pawnSelected l'oggetto Pawn passato in input, solo se la pedina selezionata appartiene al giocatore del turno in corso e se la fase del turno e quella di movimento
    /// prima di impostarla chiama la funzione DeselectPawn per resettare l'oggetto pawnSelected precedente
    /// </summary>
    /// <param name="selected"></param>
    public void PawnSelected(Pawn selected)
    {
        if (!pause)
        {
            switch (turnManager.CurrentMacroPhase)
            {
                case TurnManager.MacroPhase.placing:
                    if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.placing)
                    {
                        if ((turnManager.CurrentPlayerTurn == selected.faction) && !selected.currentBox)
                        {
                            if (pawnSelected != null)
                            {
                                DeselectPawn();
                            }
                            selected.selected = true;
                            pawnSelected = selected;
                            vfx.SelectPawn(pawnSelected);
                        }
                    }
                    else if (turnManager.CurrentTurnState == TurnManager.PlayTurnState.choosing)
                    {
                        if (pawnSelected != null)
                        {
                            DeselectPawn();
                        }
                        selected.selected = true;
                        pawnSelected = selected;
                        vfx.SelectPawn(pawnSelected);
                    }
                    break;
                case TurnManager.MacroPhase.game:
                    switch (turnManager.CurrentTurnState)
                    {
                        case TurnManager.PlayTurnState.check:
                            if (pawnSelected != null)
                            {
                                DeselectPawn();
                            }
                            selected.selected = true;
                            pawnSelected = selected;
                            vfx.SelectPawn(pawnSelected);
                            pawnSelected.ShowMovementBoxes();
                            break;
                        case TurnManager.PlayTurnState.choosing:
                            if (pawnSelected != null)
                            {
                                DeselectPawn();
                            }
                            selected.selected = true;
                            pawnSelected = selected;
                            vfx.SelectPawn(pawnSelected);
                            break;
                        case TurnManager.PlayTurnState.movementattack:
                            if (pawnSelected != null)
                            {
                                DeselectPawn();
                            }
                            selected.selected = true;
                            pawnSelected = selected;
                            vfx.SelectPawn(pawnSelected);
                            pawnSelected.MarkAttackPawn();
                            pawnSelected.ShowAttackPattern();
                            pawnSelected.ShowMovementBoxes();
                            break;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Funzione che setta il pattern delle pedine a seconda della scelta fatta nella fase di draft
    /// </summary>
    public void SetPawnsPattern()
    {
        int i = 0;
        foreach (Pawn p in magicPawns)
        {
            p.ChangePattern(draftManager.magic_pawns_picks[i]);
            i++;
        }

        int j = 0;
        foreach (Pawn p in sciencePawns)
        {
            p.ChangePattern(draftManager.science_pawns_picks[j]);
            j++;
        }
    }

    /// <summary>
    /// Imposta la pedina di cui bisogna scegliere il pattern in base al turno del giocatore
    /// </summary>
    public void SetPawnToChoose(bool startchoose)
    {        
        bool foundPawn = false;
        foreach (Pawn p in pawns)
        {
            if ((p.activePattern == 4 || p.activePattern == 5) && p.faction == turnManager.CurrentPlayerTurn)
            {
                foundPawn = true;
                PawnSelected(p);                
                CustomLogger.Log("trovata una pedina");
                break;
            }
        }
        if (foundPawn)
        {
            uiManager.SetChoosingUI();
            return;
        }
        else
        {
            if (startchoose)
            {
                turnManager.ChangeTurn();
                CustomLogger.Log("Cambio turno");
            }
            else
            {
                turnManager.CurrentTurnState = TurnManager.PlayTurnState.check;
                SelectNextPawnCheckPhase(Directions.idle);
            }
        }
    }

    /// <summary>
    /// Funzione che imposta il pattern della selectedPawn con il valore passato in input (usata quando viene premuto il pulsante del rispettivo colore)
    /// </summary>
    /// <param name="patternIndex"></param>
    public void ChoosePawnPattern(int patternIndex)
    {
        if (!pause && pawnSelected != null)
        {
            pawnSelected.ChangePattern(patternIndex);
            if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.placing)
            {
                DeselectPawn();
                turnManager.ChangeTurn();
            }
            else if (turnManager.CurrentMacroPhase == TurnManager.MacroPhase.game)
            {
                SetPawnToChoose(false);
            }
        }
    }

    /// <summary>
    /// Funzione che evidenzia le pedine che sono marchiate quando il mouse gli passa sopra, _enter serve per determinare se evidenziarle o togliere l'evidenziamento
    /// </summary>
    /// <param name="_active"></param>
    public void PawnHighlighted(bool _active, Pawn _pawnHighlighted = null)
    {
        if (_active)
        {
            if (!superAttack)
            {
                foreach (Pawn p in pawns)
                {
                    if (p.attackMarker)
                    {
                        vfx.MarkPawn(p.transform.position);
                    }
                }
            }
            else
            {
                vfx.MarkPawn(_pawnHighlighted.transform.position);
            }
        }
        else
        {
            vfx.ResetMark();
        }
    }

    /// <summary>
    /// Funzione che randomizza la lista di pedine che gli viene passata come parametro
    /// </summary>
    /// <param name="randomize"></param>
    public void RandomizePatterns(List<Pawn> randomize)
    {
        foreach (Pawn p in randomize)
        {
            if (!p.randomized)
                p.RandomizePattern();
        }
        if (CheckPawnToChoose())
        {
            turnManager.CurrentTurnState = TurnManager.PlayTurnState.choosing;
            SetPawnToChoose(false);
        }
        else
        {
            SelectNextPawnCheckPhase(Directions.idle);
        }
    }

    #endregion

    /// <summary>
    /// Funzione che gestisce le condizioni di vittoria
    /// </summary>
    public void WinCondition(bool noAttack)
    {
        if (noAttack)
        {
            if (magicPawns.Count > sciencePawns.Count)
            {
                uiManager.winScreen.SetActive(true);
                uiManager.gameResult.text = "Magic wins by having more pawns! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
                EventManager.OnGameEnd();
            }
            else if (sciencePawns.Count > magicPawns.Count)
            {
                uiManager.winScreen.SetActive(true);
                uiManager.gameResult.text = "Science wins by having more pawns! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
                EventManager.OnGameEnd();
            }
            else if (magicPawns.Count == sciencePawns.Count)
            {
                foreach (Box box in boxesArray)
                {
                    if (box.board == Factions.Magic)
                    {
                        magictiles++;
                    }
                    else if (box.board == Factions.Science)
                    {
                        sciencetiles++;
                    }
                }

                if (magictiles > sciencetiles)
                {
                    uiManager.winScreen.SetActive(true);
                    uiManager.gameResult.text = "Magic wins by destroying more tiles! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
                    EventManager.OnGameEnd();
                }
                else if (sciencetiles > magictiles)
                {
                    uiManager.winScreen.SetActive(true);
                    uiManager.gameResult.text = "Science wins by destroying more tiles! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
                    EventManager.OnGameEnd();
                }
                else if (magictiles == sciencetiles)
                {
                    uiManager.winScreen.SetActive(true);
                    uiManager.gameResult.text = "DRAW! Both players had the same amount of pawns and destroyed the same amount of tiles! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
                    EventManager.OnGameEnd();
                }
            }
        }
        else
        {
            if (magicPawns.Count == 0)
            {
                uiManager.winScreen.SetActive(true);
                uiManager.gameResult.text = "Science wins! \n " + "The game ended in " + turnManager.numberOfTurns + " turns.";
                EventManager.OnGameEnd();

            }
            else if (sciencePawns.Count == 0)
            {
                uiManager.winScreen.SetActive(true);
                uiManager.gameResult.text = "Magic wins! \n" + "The game ended in " + turnManager.numberOfTurns + " turns.";
                EventManager.OnGameEnd();
            }
        }
    }

    #endregion
}
