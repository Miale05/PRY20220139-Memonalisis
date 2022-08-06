using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using UnityEngine.XR.ARFoundation;

public class MemorySpawnableManager : MonoBehaviour
{
    public static MemorySpawnableManager instance;
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnablePrefab;

    public Transform spawnParent;
    int maxModels;
    public int currentModelsCount;

    Camera arCam;
    GameObject spawnedObject;

    public TextMeshProUGUI text;

    public bool CheckMaxModelsCount()
    {
        maxModels = GeneralGameManager.instance.maxModelsLoaded;
        return maxModels == currentModelsCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("TEST");
            currentModelsCount++;
            SpawnPrefab(Vector3.zero);
        }


        if (Input.touchCount == 0)
        {
            text.text = "exited";
            return;
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnedObject = hit.collider.gameObject;
                    }
                    else if(currentModelsCount < maxModels)
                    {
                        text.text = "Spawn";
                        currentModelsCount++;
                        SpawnPrefab(m_Hits[0].pose.position);
                    } else
                    {
                        text.text = "NADA";
                    }
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;
            }
        }
    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity,spawnParent);
    }
}
