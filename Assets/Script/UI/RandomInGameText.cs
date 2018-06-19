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

    public void GenerateRandomText(PhraseType type, Factions faction)
    {
        if ((bm.tutorial.TutorialActive && bm.tutorial.DraftTutorialDone && bm.tutorial.ChoosingTutorialDone && bm.tutorial.PlacingTutorialDone && bm.tutorial.SuperAttackTutorialDone && bm.tutorial.GameTutorialDone) || !bm.tutorial.TutorialActive)
        {
            MagicAButton.SetActive(false);
            ScienceAButton.SetActive(false);
            switch (Random.Range(0,2))
            {
                case 0:
                    switch (type)
                    {
                        case PhraseType.Positive:
                            switch (faction)
                            {
                                case Factions.None:
                                    stringindex = Random.Range(0, GeneralPositive.Count);
                                    StartCoroutine(ShowPhrase(GeneralPositive[stringindex]));
                                    break;
                                case Factions.Magic:
                                    break;
                                case Factions.Science:
                                    break;
                            }
                            break;
                        case PhraseType.Negative:
                            switch (faction)
                            {
                                case Factions.None:
                                    stringindex = Random.Range(0, GeneralNegative.Count);
                                    StartCoroutine(ShowPhrase(GeneralNegative[stringindex]));
                                    break;
                                case Factions.Magic:
                                    break;
                                case Factions.Science:
                                    break;
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
        yield return new WaitForSeconds(1.5f);
        MagicComic.SetActive(false);
        ScienceComic.SetActive(false);
    }

}
