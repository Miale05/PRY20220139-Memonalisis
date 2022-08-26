using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Clase encargada de las funciones AR de la aplicacion, instanciando modelos en escena.
/// Derivada de la clase MemorySpawnableManager
/// </summary>
public class ARSpawner : MonoBehaviour
{

    public static ARSpawner instance;
    /// <summary>
    /// Referencia del manager de raycasts utilizados para AR
    /// </summary>
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    /// <summary>
    /// Referencia al manager de planos encargado de dibujar los planos donde se podran colocar tableros en AR
    /// </summary>
    [SerializeField]
    ARPlaneManager m_PlaneManager;
    /// <summary>
    /// Lista de impactos del raycast
    /// </summary>
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    /// <summary>
    /// GameObject que sera instanciado en AR
    /// Prefab => Tablero guardado, revisar documentacion de Unity para aprender mas sobre prefabs.
    /// </summary>
    [SerializeField]
    GameObject spawnablePrefab;

    /// <summary>
    /// Padre en la jerarquia donde se instanciara el prefab
    /// </summary>
    public Transform spawnParent;

    /// <summary>
    /// Referencia a la camara AR dentro de la escena
    /// </summary>
    Camera arCam;
    /// <summary>
    /// Referencia al prefab instanciado en escena
    /// </summary>
    GameObject spawnedObject;

    //La Inicializacion de variables se hace en el start comunmente
    void Start()
    {
        instance = this;
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    /// <summary>
    /// Funcion utilizada en la escena para alternar la visualizacion de los planos en la escena
    /// </summary>
    /// <param name="state">Estado objetivo, apagado = false, prendido = true</param>
    public void TogglePlanes(bool state)
    {
        m_PlaneManager.SetTrackablesActive(state);
        m_PlaneManager.enabled = state;
    }

    // Update is called once per frame
    void Update()
    {
        //Funcion de testeo para poder inicializar un tablero sin necesidad de compilar el proyecto
        //La funcionalidad es limitada al probarlo en el unity debido a que es necesario usar las librerias del celular para que el AR funcione
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnPrefab(Vector3.zero);
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            //En caso se haya presionado la pantalla y el objeto todavia no haya sido spawneado
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                //Confirma la posicion de impacto del raycast
                if (Physics.Raycast(ray, out hit))
                {
                    //Llama al metodo de instanciamiento del prefab, pero no elimina la referencia del objeto instanciado por lo que no vovlera a instanciar mas objetos
                    SpawnPrefab(m_Hits[0].pose.position);
                }
            }
        }
    }

    /// <summary>
    /// Funcion para instanciar el modelo asignado a esta clase, usualmente el tablero
    /// </summary>
    /// <param name="spawnPosition">Posicion(Vector3) donde se instanciara</param>
    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity, spawnParent);
        //A diferencia del spawner de memoria esta clase inicia el minijuego ni bien se instancia el tablero
        AnalisisManager.instance.StartMinigame();
    }
}
