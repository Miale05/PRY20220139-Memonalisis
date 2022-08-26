using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using TMPro;

/// <summary>
/// Clase encargada de organizar el tablero y verificar las piezas seleccionadas para el juego de memoria
/// </summary>
public class MemoryManager : MonoBehaviour
{
    /// <summary>
    /// Referencia estatica al MemoryManager, utilizando el patron de diseño Singleton
    /// </summary>
    public static MemoryManager instance;

    /// <summary>
    /// Familiar cargado actualmente, obtenido del GeneralGameManager
    /// </summary>
    public LoadedFamiliar loadedFamiliar;
    /// <summary>
    /// Variable Deprecada, remover al limpiar el codigo
    /// </summary>
    public int modelVariations;

    /// <summary>
    /// Referencia al contenedor en escena donde se guardan los modelos que se cargaran para cada pieza
    /// </summary>
    public Transform basePieceContainer;
    /// <summary>
    /// Modelos que se utilizaran para las piezas, 4 para las personas y 10 para los animales
    /// </summary>
    public List<GameObject> basePieces;
    /// <summary>
    /// Sonidos encontrados dentro de los modelos que se utilizaran en las piezas
    /// </summary>
    public List<AudioClip> audioClips;

    /// <summary>
    /// Referencia al contenedor en escena donde se inicializara el tablero y sus piezas
    /// </summary>
    public Transform activePieceContainer;
    /// <summary>
    /// Piezas inicializadas dentro del tablero
    /// </summary>
    public List<GameObject> activePieces;

    /// <summary>
    /// Pieza actual seleccionada
    /// </summary>
    GameObject currentPiece;

    //Variables para el manejo del contador del tiempo transcurrido
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerText2;
    public float timePassed;
    float timerStart;
    //Variable que determina si el juego de memoria no ha sido iniciado o ya se finalizo
    public bool ongoingGame;
    
    //Variables de audio para acciones dentro del minijuego de memoria
    public AudioClip correctAudio;
    public AudioClip wrongAudio;
    public AudioClip startAudio;
    public AudioClip winAudio;

    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //Inicializacion de modelos en base al familiar cargado
        loadedFamiliar = GeneralGameManager.instance.selectedFamiliar;
        LoadBasePieces();

        //Zona deprecada, remover al limpiar el codigo
        modelVariations = basePieceContainer.Find(loadedFamiliar.ToString()).childCount;
        GeneralGameManager.instance.SetMaxModelsLoaded(modelVariations*2);
    }

    // Update is called once per frame
    void Update()
    {
        //En caso un juego este en curso iniciar el contador de tiempo
        if (ongoingGame)
        {
            //Determinacion de tiempo transcurrido
            TimeSpan time = TimeSpan.FromSeconds(Time.time - timerStart);
            //Configuracion de formato para el tiempo
            timerText.text = string.Format("{0:00}:{1:00}", time.TotalMinutes, time.Seconds);
            timerText2.text = string.Format("{0:00}:{1:00}", time.TotalMinutes, time.Seconds);

        }
    }

    /// <summary>
    /// Reproduce un audio utilizando el AudioSource del MemoryManager para tener el audio de la emocion y el del minijuego al mismo tiempo
    /// </summary>
    /// <param name="pieceAudio">Audio a reproducir</param>
    public void PlayLocalAudio(AudioClip pieceAudio) {
        source.Stop();
        source.clip = pieceAudio;
        source.Play();
    }

    
    /// <summary>
    /// Eliminar todos los modelos dentro del objeto Basura
    /// </summary>
    public void ClearTrash()
    {
        GameObject temp = GameObject.Find("Trash");

        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Inicializa las piezas en base a los datos almacenados en los contenedores
    /// </summary>
    public void InitializePieces()
    {
        //Genera listas temporales, debido a que en el juego de memoria se duplican el numero de piezas se agregan dos veces los valores dentro de los contenedores
        //Agregar Modelos
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(basePieces);
        temp.AddRange(basePieces);

        //Agregar Emocion
        //Enum.GetValues(typeof(SomeEnum)).Cast<SomeEnum>(); <- Referencia
        List<Emotions> temp2 = new List<Emotions>();
        temp2.AddRange(Enum.GetValues(typeof(Emotions)).Cast<Emotions>().ToList());
        temp2.AddRange(Enum.GetValues(typeof(Emotions)).Cast<Emotions>().ToList());

        //Agregar Sonido
        List<AudioClip> temp3 = new List<AudioClip>();
        temp3.AddRange(audioClips);
        temp3.AddRange(audioClips);

        //Finalmente se recorre todas las piezas activas y se inicializan llamando su funcion InitializePiece
        for (int i = 0; i < activePieces.Count; i++)
        {
            activePieces[i].GetComponent<MemoryPiece>().InitializePiece(temp[i],temp2[i].ToString(),temp3[i]);
        }
    }

    /// <summary>
    /// Aleatoriza las piezas usando un algoritmo aleatorio, adicionalmente se invierte la lista debido a que la pieza inicial no se aleatoriza
    /// </summary>
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

    /// <summary>
    /// Elimina las piezas actuales y busca nuevas piezas almacenadas en el contenedor
    /// </summary>
    public void RefreshActivePieces()
    {
        activePieces.Clear();
        for (int i = 0; i < activePieceContainer.GetChild(0).childCount; i++)
        {
            activePieces.Add(activePieceContainer.GetChild(0).GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Carga los modelos del familiar seleccionado en el GeneralGameManager, utiliza el contenedor de las piezas base el cual almacena todos los modelos y sonidos
    /// </summary>
    public void LoadBasePieces()
    {
        //Busca el contenedor del familiar adecuado
        Transform container = basePieceContainer.Find(loadedFamiliar.ToString());
        for (int i = 0; i < container.childCount; i++)
        {
            //Asigna modelo y audio
            basePieces.Add(container.GetChild(i).gameObject);
            audioClips.Add(container.GetChild(i).GetComponent<AudioSource>().clip);
        }
    }

    /// <summary>
    /// Funcion que se encarga de revisar la actividad de todas las piezas, en caso no se encuentren piezas activas se considera completado el minijuego, se reproduce un audio y se cambia de color el contador a verde
    /// </summary>
    public void CheckAllPieces() {
        foreach (GameObject item in activePieces) {
            MemoryPiece temp = item.GetComponent<MemoryPiece>();
            if (temp.isActive) {
                return;
            }
        }

        PlayLocalAudio(winAudio);

        timerText.color = Color.green;
        timerText2.color = Color.green;
        ongoingGame = false;
    }

    /// <summary>
    /// Compara la pieza actual con la pieza seleccionada, en caso no exista una pieza actual se asignara la pieza seleccionada como pieza actual
    /// </summary>
    /// <param name="piece">Referencia de la pieza seleccionada</param>
    public void CheckPiece(GameObject piece)
    {
        if (currentPiece == null)
        {
            currentPiece = piece;
        } else
        {
            //Compara si las emociones de ambas piezas son iguales y la pieza no esta siendo comparada con sigo misma
            if (currentPiece.GetComponent<MemoryPiece>().emotionName == piece.GetComponent<MemoryPiece>().emotionName && piece != currentPiece)
            {
                Debug.Log("CORRECTO");
                //Reproducir audio
                PlayLocalAudio(correctAudio);

                //Remover ambas piezas de la lista de piezas activas
                activePieces.Remove(piece);
                activePieces.Remove(currentPiece);

                //Desactivar las piezas
                currentPiece.GetComponent<MemoryPiece>().isActive = false;
                piece.GetComponent<MemoryPiece>().isActive = false;
                currentPiece = null;

                //Checkeo de todas las piezas para verificar la victoria
                CheckAllPieces();
            } else
            {
                Debug.Log("ERROR!!!");
                //Reproducir audio
                PlayLocalAudio(wrongAudio);

                //Volver a ocultar las piezas y limpia la variable de pieza actual
                currentPiece.GetComponent<MemoryPiece>().Hide(true);
                piece.GetComponent<MemoryPiece>().Hide(true);
                currentPiece = null;
            }
        }
    }

    /// <summary>
    /// Funcion para iniciar el minijuego de la memoria
    /// </summary>
    public void StartMinigame() {
        //Reinicio de variables del contador de tiempo transcurrido
        timerStart = Time.time;
        ongoingGame = true;
        timerText.color = Color.white;
        timerText2.color = Color.white;


        RefreshActivePieces();

        //Checkea que ya haya spawneado el tablero
        //if (MemorySpawnableManager.instance.CheckMaxModelsCount())
        if (MemorySpawnableManager.instance.currentModelsCount >= 1) {
            //Reproducir audio de inicio
            PlayLocalAudio(startAudio);

            InitializePieces();
            ShufflePieces();

            //Generado por InitializePieces
            ClearTrash();
        }
    }
}
