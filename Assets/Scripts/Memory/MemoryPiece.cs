using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase asignada a cada pieza de los tableros
/// </summary>
public class MemoryPiece : MonoBehaviour
{
    /// <summary>
    /// Variable que determina si se puede interactuar con la pieza
    /// </summary>
    public bool isActive;

    /// <summary>
    /// Nombre de la emocion utilizado para facilitar el analisis de la emocion en el codigo
    /// </summary>
    public string emotionName = "";

    /// <summary>
    /// Tiempo que se demora la pieza en volverse invisible
    /// </summary>
    public float hideFadeTime = 1.5f;
    //Variables que determinan si la pieza se deberia ocultar
    bool hidePiece = false;
    float extraFadeTime = 0;
    float fadeStart;

    /// <summary>
    /// Variable que determina si la pieza cuenta con emojis, usado no intentar acceder a los emojis de piezas que no cuentan con ellos como los animales
    /// </summary>
    public bool hasEmote;
    /// <summary>
    /// Referencia del script del emoji que se encarga que mirar hacia la camara
    /// </summary>
    public FaceCamera emote;

    /// <summary>
    /// Referencia del tablero al cual esta pieza pertenece
    /// </summary>
    public Transform boardReference;

    /// <summary>
    /// Audio que la pieza deberia reproducir al ser seleccionada
    /// </summary>
    public AudioClip pieceAudio;

    // Update is called once per frame
    void Update()
    {
        //En caso se haya marcado que la pieza debe ocultarse se utilizara un contador para determinar cuando se volvera invisible
        if (hidePiece)
        {
            if (Time.time - fadeStart >= hideFadeTime + extraFadeTime)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                hidePiece = false;
                extraFadeTime = 0;

                //En caso tenga un emoji tambien se volvera invisible dicho emoji
                if (hasEmote)
                {
                    Debug.Log("Change to FALSE");
                    emote.ChangeSpriteRendererState(false);
                }
            }
        }
    }

    /// <summary>
    /// Funcion llamada al seleccionar la pieza
    /// </summary>
    public void Select()
    {
        //llamar el reproductor de audio encontrado en el GeneralGameManager por medio del singleton, se detine el audio, se cambia el clip de audio y se vuelve a reproducir
        GeneralGameManager.instance.source.Stop();
        GeneralGameManager.instance.source.clip = pieceAudio;
        GeneralGameManager.instance.source.Play();

        //En caso este activo el objeto se llamara la función para validar si la pieza seleccionada es la correcta
        //Esta zona solo se debe ejecutar en la escena de la memoria en caso se ejecute en otras resultara en un error debido a que las otras escenas no cuentan con un MemoryManager
        if (isActive)
        {
            Show();
            MemoryManager.instance.CheckPiece(this.gameObject);
        }
    }

    /// <summary>
    /// Funcion deprecada, Remover al limpiar el codigo
    /// </summary>
    public void DeSelect()
    {
        /*
        if (Time.time - holdTimeStart >= holdTimeToDestroy)
        {
            MemorySpawnableManager.instance.currentModelsCount -= 1;
            Destroy(gameObject);
        }
        */
    }

    /// <summary>
    /// Funcion encargada de iniciar el proceso para ocultar los modelos dentro de la pieza
    /// </summary>
    /// <param name="keepActive">Mantener la pieza activa e interactuable mientras espera a ser escondida</param>
    /// <param name="extraHideTime">Tiempo extra para esconderse, en caso se necesite mostrar la pieza mas tiempo de lo especificado, utilizado principalmente en las escenas de analisis</param>
    public void Hide(bool keepActive,float extraHideTime = 0)
    {
        fadeStart = Time.time;
        isActive = keepActive;
        hidePiece = true;

        extraFadeTime = extraHideTime;
    }

    /// <summary>
    /// Funcion para volver visible los modelos dentra de la pieza
    /// </summary>
    public void Show()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        if (hasEmote)
        {
            emote.ChangeSpriteRendererState(true);
        }
    }

    /// <summary>
    /// Funcion para la inicializacion de la pieza llamada por los Managers para cargar el modelo, emocion y sonido adecuado
    /// </summary>
    /// <param name="obj">Modelo de la pieza</param>
    /// <param name="name">Emocion de la pieza</param>
    /// <param name="sound">Sonido que emite la pieza al ser seleccionado</param>
    public void InitializePiece(GameObject obj,string name, AudioClip sound)
    {
        //En caso la pieza ya tenga un modelo se enviara el viejo modelo a un objeto llamado Basura
        if (transform.childCount > 1)
        {
            transform.GetChild(1).SetParent(GameObject.Find("Trash").transform);
        }

        //Instanciar el modelo correcto
        GameObject temp = Instantiate(obj, transform.position, boardReference.rotation);
        temp.transform.SetParent(transform);
        emotionName = name;
        pieceAudio = sound;

        //Alteracion del modelo cargado para que se adecue al tamaño de la pieza
        transform.GetChild(1).localPosition = new Vector3(0,1,0);
        transform.GetChild(1).localScale = new Vector3(0.5f,5,0.5f);

        //Mostrar modelo y ocultar luego de 5 segundos, 1.5 asignado en la pieza y 3.5 asignado en esta inicializacion
        //En las escenas de analisis el valor 1.5 es reemplazado por 999999
        Show();
        Hide(true,3.5f);
    }
}
