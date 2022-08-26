using System;
using System.Collections.Generic;
using UnityEngine;

public enum LoadedFamiliar
{
    father,
    mother,
    brother,
    sister,
    uncle,
    aunt,
    cousin_M,
    cousin_F,
    grandfather,
    grandmother,
    none
}

public enum Emotions
{
    angry,
    happy,
    scared,
    sad
}

public class GeneralGameManager : MonoBehaviour
{
    public static GeneralGameManager instance;
    public LoadedFamiliar selectedFamiliar;
    public int maxModelsLoaded;
    public AudioSource source;

    void Start()
    {
        if (GeneralGameManager.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectFamiliar(string familiar)
    {
        Enum.TryParse(familiar, out selectedFamiliar);
    }

    public void SetMaxModelsLoaded(int value)
    {
        maxModelsLoaded = value;
    }
}
