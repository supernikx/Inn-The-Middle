using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    private static bool created = false;
    public static DataManager instance;

    public bool _SkipTitleScreen;
    public bool SkipTitleScreen {
        get
        {
            return _SkipTitleScreen;
        }
        set
        {
            _SkipTitleScreen = value;
        }
    }

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            created = true;
        }
    }

    private void Start()
    {
        SkipTitleScreen = false;
    }
}
