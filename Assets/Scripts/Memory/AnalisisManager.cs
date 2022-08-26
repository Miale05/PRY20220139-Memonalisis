using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Clase encargada de organizar el tablero para las escenas de analisis de personas o animales, es derivada de la clase de MemoryManager por lo que podria heredar de ella en una segunda revision del proyecto
/// </summary>
public class AnalisisManager : MonoBehaviour
{
    /// <summary>
    /// Referencia estatica al AnalisisManager, utilizando el patron de diseño Singleton
    /// </summary>
    public static AnalisisManager instance;

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

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //Inicializacion de modelos en base al familiar cargado
        loadedFamiliar = GeneralGameManager.instance.selectedFamiliar;
        LoadBasePieces();

        //Zona deprecada, remover al limpiar el codigo
        modelVariations = basePieceContainer.Find(loadedFamiliar.ToString()).childCount;
        GeneralGameManager.instance.SetMaxModelsLoaded(modelVariations);
    }

    /// <summary>
    /// Eliminar todos los modelos dentro del objeto Basura
    /// </summary>
    public void ClearTrash() {
        GameObject temp = GameObject.Find("Trash");

        for (int i = 0; i < temp.transform.childCount; i++) {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Inicializa las piezas en base a los datos almacenados en los contenedores
    /// </summary>
    public void InitializePieces() {
        //Agregar Modelos
        List<GameObject> temp = new List<GameObject>();
        temp.AddRange(basePieces);

        //Agregar Emocion
        List<Emotions> temp2 = new List<Emotions>();
        temp2.AddRange(Enum.GetValues(typeof(Emotions)).Cast<Emotions>().ToList());

        //Agregar Sonido
        List<AudioClip> temp3 = new List<AudioClip>();
        temp3.AddRange(audioClips);

        //En caso la escena cargada sea la de los animales debido a que hay mas animales que emociones no se utilizara la lista de emociones y se desactivan la interaccion con las piezas
        if (temp.Count != temp2.Count) {
            for (int i = 0; i < activePieces.Count; i++) {
                //Inicializar la pieza con la emocion "none"
                activePieces[i].GetComponent<MemoryPiece>().InitializePiece(temp[i], "none", temp3[i]);
                activePieces[i].GetComponent<MemoryPiece>().isActive = false;
            }
        } else {
            for (int i = 0; i < activePieces.Count; i++) {
                //Inicializar la pieza de manera normal
                activePieces[i].GetComponent<MemoryPiece>().InitializePiece(temp[i], temp2[i].ToString(), temp3[i]);
                activePieces[i].GetComponent<MemoryPiece>().isActive = false;

            }
        }
    }

    /// <summary>
    /// Elimina las piezas actuales y busca nuevas piezas almacenadas en el contenedor
    /// </summary>
    public void RefreshActivePieces() {
        activePieces.Clear();
        for (int i = 0; i < activePieceContainer.GetChild(0).childCount; i++) {
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
    /// Funcion para iniciar la visualizacion de los modelos
    /// </summary>
    public void StartMinigame() {
        RefreshActivePieces();
        InitializePieces();

        ClearTrash();
    }
}
