using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using UnityEngine.XR.ARFoundation;

/// <summary>
/// Clase encargada de las funciones AR de la aplicacion, instanciando modelos en escena.
/// Los campos "[SerializeField]" se agregan para poder asignar variables desde el inspector sin hacerlos publicas, esto no es del todo necesario pero la mayoria de los tutoriales hace lo mismo
/// </summary>
public class MemorySpawnableManager : MonoBehaviour
{
    /// <summary>
    /// Referencia estatica al MemorySpawnableManager
    /// </summary>
    public static MemorySpawnableManager instance;

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
    /// Variable Deprecada, remover al limpiar el codigo
    /// </summary>
    int maxModels;
    /// <summary>
    /// Variable utilizada para determinar si se ha instanciado el tablero correctamente, adicionalmente podria usarse para instanciar multiples objetos al mismo tiempo
    /// </summary>
    public int currentModelsCount;

    /// <summary>
    /// Referencia a la camara AR dentro de la escena
    /// </summary>
    Camera arCam;
    /// <summary>
    /// Referencia al prefab instanciado en escena
    /// </summary>
    GameObject spawnedObject;

    /// <summary>
    /// Funcion Deprecada, remover al limpiar el codigo
    /// </summary>
    public bool CheckMaxModelsCount()
    {
        maxModels = GeneralGameManager.instance.maxModelsLoaded;

        //Remove lines about model count since the model spawned already has all the pieces
        //return maxModels == currentModelsCount;
        return true;
    }

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
            currentModelsCount++;
            SpawnPrefab(Vector3.zero);
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            //Si se ha iniciado a presionar la pantalla y no hay un objeto spawneado
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                //Checkea los impactos del raycast
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnedObject = hit.collider.gameObject;
                    }
                    //else if(currentModelsCount < maxModels)    Remove lines about model count since the model spawned already has all the pieces
                    //En caso no haya modelos instanciados, instanciar el prefab y aumentar su contador
                    else if (currentModelsCount < 1)
                    {
                        currentModelsCount++;
                        SpawnPrefab(m_Hits[0].pose.position);
                    }
                }
            }

            //En caso se haya alzado el dedo se elimina la referencia del objeto instanciado
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;
            }
        }
    }

    /// <summary>
    /// Funcion para instanciar el modelo asignado a esta clase, usualmente el tablero
    /// </summary>
    /// <param name="spawnPosition">Posicion(Vector3) donde se instanciara</param>
    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity,spawnParent);
    }
}
