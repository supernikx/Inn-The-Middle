using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PhraseType
{
    Positive,
    Negative,
}



public class RandomInGameText : MonoBehaviour
{
    BoardManager bm;
    int stringindex;

    [Header("Comics Lists")]
    public List<string> GeneralPositive = new List<string>();
    public List<string> GeneralNegative = new List<string>();
    public List<string> SciencePositive = new List<string>();
    public List<string> ScienceNegative = new List<string>();
    public List<string> MagicPositive = new List<string>();
    public List<string> MagicNegative = new List<string>();

    [Header("Comics Holders")]
    public GameObject MagicComic;
    public GameObject ScienceComic;
    public TextMeshProUGUI MagicComicText;
    public TextMeshProUGUI ScienceComicText;
    public GameObject MagicAButton;
    public GameObject ScienceAButton;

    private void Start()
    {
        bm = BoardManager.Instance;
    }

    /// <summary>
    /// Funzione che genera una frase random in base al tipo passato come parametro
    /// </summary>
    /// <param name="type"></param>
    public void GenerateRandomText(PhraseType type)
    {
        if ((bm.tutorial.TutorialActive && bm.tutorial.DraftTutorialDone && bm.tutorial.ChoosingTutorialDone && bm.tutorial.PlacingTutorialDone && bm.tutorial.SuperAttackTutorialDone && bm.tutorial.GameTutorialDone) || !bm.tutorial.TutorialActive)
        {
            MagicAButton.SetActive(false);
            ScienceAButton.SetActive(false);
            switch (Random.Range(0, 2))
            {
                case 0:
                    switch (type)
                    {
                        case PhraseType.Positive:
                            if (Random.Range(0, 11) % 2 == 0)
                            {
                                stringindex = Random.Range(0, GeneralPositive.Count);
                                StartCoroutine(ShowPhrase(GeneralPositive[stringindex]));
                            }
                            else
                            {
                                switch (bm.turnManager.CurrentPlayerTurn)
                                {
                                    case Factions.Magic:
                                        stringindex = Random.Range(0, MagicPositive.Count);
                                        StartCoroutine(ShowPhrase(MagicPositive[stringindex]));
                                        break;
                                    case Factions.Science:
                                        stringindex = Random.Range(0, SciencePositive.Count);
                                        StartCoroutine(ShowPhrase(SciencePositive[stringindex]));
                                        break;
                                }
                            }
                            break;
                        case PhraseType.Negative:
                            if (Random.Range(0, 11) % 2 == 0)
                            {
                                stringindex = Random.Range(0, GeneralNegative.Count);
                                StartCoroutine(ShowPhrase(GeneralNegative[stringindex]));
                            }
                            else
                            {
                                switch (bm.turnManager.CurrentPlayerTurn)
                                {
                                    case Factions.Magic:
                                        stringindex = Random.Range(0, MagicNegative.Count);
                                        StartCoroutine(ShowPhrase(MagicNegative[stringindex]));
                                        break;
                                    case Factions.Science:
                                        stringindex = Random.Range(0, ScienceNegative.Count);
                                        StartCoroutine(ShowPhrase(ScienceNegative[stringindex]));
                                        break;
                                }
                            }
                            break;
                    }
                    break;
                case 1:
                    Debug.Log("Nessuna Frase");
                    break;
            }
        }
    }

    /// <summary>
    /// Coroutine che mostra la frase randomizzata e la fa sparire dopo 2.5s
    /// </summary>
    /// <param name="_phrase"></param>
    /// <returns></returns>
    private IEnumerator ShowPhrase(string _phrase)
    {
        switch (bm.turnManager.CurrentPlayerTurn)
        {
            case Factions.Magic:
                MagicComic.SetActive(true);
                MagicComicText.text = _phrase;
                break;
            case Factions.Science:
                ScienceComic.SetActive(true);
                ScienceComicText.text = _phrase;
                break;
        }
        yield return new WaitForSeconds(2.5f);
        MagicComic.SetActive(false);
        ScienceComic.SetActive(false);
    }

}
