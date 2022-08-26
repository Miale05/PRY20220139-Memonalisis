using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using TMPro;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager instance;

    public LoadedFamiliar loadedFamiliar;
    public int modelVariations;

    public Transform basePieceContainer;
    public List<GameObject> basePieces;
    public List<AudioClip> audioClips;

    public Transform activePieceContainer;
    public List<GameObject> activePieces;

    GameObject currentPiece;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerText2;
    public float timePassed;
    float timerStart;
    public bool ongoingGame;

    public AudioClip correctAudio;
    public AudioClip wrongAudio;
    public AudioClip startAudio;
    public AudioClip winAudio;

    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        loadedFamiliar = GeneralGameManager.instance.selectedFamiliar;
        modelVariations = basePieceContainer.Find(loadedFamiliar.ToString()).childCount;
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            timerStart -= 60;
        }

        if (ongoingGame)
        {
            //Change time
            TimeSpan time = TimeSpan.FromSeconds(Time.time - timerStart);
            timerText.text = string.Format("{0:00}:{1:00}", time.TotalMinutes, time.Seconds);
            timerText2.text = string.Format("{0:00}:{1:00}", time.TotalMinutes, time.Seconds);

        }
    }

    //Reproduce un audio utilizando el AudioSource del MemoryManager para tener el audio de la emocion y el del minijuego al mismo tiempo
    public void PlayLocalAudio(AudioClip pieceAudio) {
        source.Stop();
        source.clip = pieceAudio;
        source.Play();
    }


    public void StartMinigame()
    {
        timerStart = Time.time;
        ongoingGame = true;
        timerText.color = Color.white;
        timerText2.color = Color.white;


        RefreshActivePieces();

        //Checkea que ya haya spawneado el tablero
        //if (MemorySpawnableManager.instance.CheckMaxModelsCount())
        if (MemorySpawnableManager.instance.currentModelsCount >= 1)
        {
            PlayLocalAudio(startAudio);

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

        List<AudioClip> temp3 = new List<AudioClip>();
        temp3.AddRange(audioClips);
        temp3.AddRange(audioClips);

        for (int i = 0; i < activePieces.Count; i++)
        {
            activePieces[i].GetComponent<MemoryPiece>().InitializePiece(temp[i],temp2[i].ToString(),temp3[i]);
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
        for (int i = 0; i < activePieceContainer.GetChild(0).childCount; i++)
        {
            activePieces.Add(activePieceContainer.GetChild(0).GetChild(i).gameObject);
        }
    }

    public void LoadBasePieces()
    {
        Transform container = basePieceContainer.Find(loadedFamiliar.ToString());
        for (int i = 0; i < container.childCount; i++)
        {
            basePieces.Add(container.GetChild(i).gameObject);
            audioClips.Add(container.GetChild(i).GetComponent<AudioSource>().clip);
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
                PlayLocalAudio(correctAudio);

                activePieces.Remove(piece);
                activePieces.Remove(currentPiece);

                currentPiece.GetComponent<MemoryPiece>().isActive = false;
                piece.GetComponent<MemoryPiece>().isActive = false;
                currentPiece = null;

                CheckAllPieces();
            } else
            {
                Debug.Log("ERROR!!!");
                PlayLocalAudio(wrongAudio);

                currentPiece.GetComponent<MemoryPiece>().Hide(true);
                piece.GetComponent<MemoryPiece>().Hide(true);
                currentPiece = null;
            }
        }
    }

    public void CheckAllPieces()
    {
        foreach (GameObject item in activePieces)
        {
            MemoryPiece temp = item.GetComponent<MemoryPiece>();
            if (temp.isActive)
            {
                return;
            }
        }

        PlayLocalAudio(winAudio);

        timerText.color = Color.green;
        timerText2.color = Color.green;
        ongoingGame = false;
    }
}
