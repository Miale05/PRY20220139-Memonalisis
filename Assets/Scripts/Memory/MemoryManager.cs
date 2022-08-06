using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager instance;

    public LoadedFamiliar loadedFamiliar;
    public int modelVariations;

    public Transform familyContainer;
    public List<GameObject> basePieces;

    public Transform activePieceContainer;
    public List<GameObject> activePieces;

    public GameObject currentPiece;

    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        loadedFamiliar = GeneralGameManager.instance.selectedFamiliar;
        modelVariations = familyContainer.Find(loadedFamiliar.ToString()).childCount;
        GeneralGameManager.instance.SetMaxModelsLoaded(modelVariations*2);

        LoadBasePieces();

        //REMOVE AFTER TESTS
        //StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(MemoryManager.instance.familyContainer.name);
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartMinigame();
        }
    }

    public void StartMinigame()
    {
        RefreshActivePieces();

        Debug.LogError(MemorySpawnableManager.instance.CheckMaxModelsCount());
        if (MemorySpawnableManager.instance.CheckMaxModelsCount())
        {
            InitializePieces();
            ShufflePieces();

            //Generado por InitializePieces
            ClearTrash();
        }
    }

    public void ClearTrash()
    {
        GameObject temp = GameObject.Find("Trash");

        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
    }

    public void InitializePieces()
    {
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(basePieces);
        temp.AddRange(basePieces);

        //Enum.GetValues(typeof(SomeEnum)).Cast<SomeEnum>();
        List<Emotions> temp2 = new List<Emotions>();
        temp2.AddRange(Enum.GetValues(typeof(Emotions)).Cast<Emotions>().ToList());
        temp2.AddRange(Enum.GetValues(typeof(Emotions)).Cast<Emotions>().ToList());

        for (int i = 0; i < activePieces.Count; i++)
        {
            activePieces[i].GetComponent<MemoryPiece>().InitializePiece(temp[i],temp2[i].ToString());
        }
    }

    public void ShufflePieces()
    {
        activePieces.Reverse();

        for (int i = 0; i < activePieces.Count; i++)
        {
            Vector3 pos = activePieces[i].transform.position;
            int randomIndex = UnityEngine.Random.Range(i,activePieces.Count);
            activePieces[i].transform.position = activePieces[randomIndex].transform.position;
            activePieces[randomIndex].transform.position = pos;
        }
    }

    public void RefreshActivePieces()
    {
        activePieces.Clear();
        for (int i = 0; i < activePieceContainer.childCount; i++)
        {
            activePieces.Add(activePieceContainer.GetChild(i).gameObject);
        }
    }

    public void LoadBasePieces()
    {
        Transform container = familyContainer.Find(loadedFamiliar.ToString());
        for (int i = 0; i < container.childCount; i++)
        {
            basePieces.Add(container.GetChild(i).gameObject);
        }
    }

    public void CheckPiece(GameObject piece)
    {
        if (currentPiece == null)
        {
            currentPiece = piece;
        } else
        {
            if (currentPiece.GetComponent<MemoryPiece>().emotionName == piece.GetComponent<MemoryPiece>().emotionName && piece != currentPiece)
            {
                Debug.Log("CORRECTO");
                activePieces.Remove(piece);
                activePieces.Remove(currentPiece);
                currentPiece.GetComponent<MemoryPiece>().Hide(false);
                piece.GetComponent<MemoryPiece>().Hide(false);
                currentPiece = null;
            } else
            {
                Debug.Log("ERROR!!!");
                currentPiece.GetComponent<MemoryPiece>().Hide(true);
                piece.GetComponent<MemoryPiece>().Hide(true);
                currentPiece = null;
            }
        }
    }
}
