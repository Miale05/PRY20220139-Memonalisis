using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AnalisisManager : MonoBehaviour
{
    public static AnalisisManager instance;

    public LoadedFamiliar loadedFamiliar;
    public int modelVariations;

    public Transform familyContainer;
    public List<GameObject> basePieces;

    public Transform activePieceContainer;
    public List<GameObject> activePieces;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        loadedFamiliar = GeneralGameManager.instance.selectedFamiliar;
        modelVariations = familyContainer.Find(loadedFamiliar.ToString()).childCount;
        GeneralGameManager.instance.SetMaxModelsLoaded(modelVariations);

        LoadBasePieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadBasePieces()
    {
        Transform container = familyContainer.Find(loadedFamiliar.ToString());
        for (int i = 0; i < container.childCount; i++)
        {
            basePieces.Add(container.GetChild(i).gameObject);
        }
    }

    public void StartMinigame()
    {
        RefreshActivePieces();
        InitializePieces();

        ClearTrash();
    }

    public void ClearTrash()
    {
        GameObject temp = GameObject.Find("Trash");

        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
    }

    public void RefreshActivePieces()
    {
        activePieces.Clear();
        for (int i = 0; i < activePieceContainer.GetChild(0).childCount; i++)
        {
            activePieces.Add(activePieceContainer.GetChild(0).GetChild(i).gameObject);
        }
    }

    public void InitializePieces()
    {
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(basePieces);

        //Enum.GetValues(typeof(SomeEnum)).Cast<SomeEnum>();
        List<Emotions> temp2 = new List<Emotions>();
        temp2.AddRange(Enum.GetValues(typeof(Emotions)).Cast<Emotions>().ToList());

        if (temp.Count != temp2.Count)
        {
            for (int i = 0; i < activePieces.Count; i++)
            {
                activePieces[i].GetComponent<MemoryPiece>().InitializePiece(temp[i],"none");
                activePieces[i].GetComponent<MemoryPiece>().isActive = false;
            }
        } else
        {
            for (int i = 0; i < activePieces.Count; i++)
            {
                activePieces[i].GetComponent<MemoryPiece>().InitializePiece(temp[i], temp2[i].ToString());
                activePieces[i].GetComponent<MemoryPiece>().isActive = false;
            }
        }
    }
}
