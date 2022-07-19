using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Insantiates this prefab on a plane at the touch location")]
    GameObject prefabObject;

    UnityEvent placementUpdate;

    [SerializeField]
    GameObject visualObject;

    ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject spawnedObject { get; private set; }


    public GameObject placedPrefab {
        get { return prefabObject; }
        set { prefabObject = value; }
    }

    public int maxObjects;
    public List<GameObject> objects;


    private void Awake() {
        raycastManager = GetComponent<ARRaycastManager>();

        if (placementUpdate == null) {
            placementUpdate = new UnityEvent();

            placementUpdate.AddListener(DisableVisual);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
            return;
        }

        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)) {
            var hitPose = hits[0].pose;
            
            if (spawnedObject == null) {
                spawnedObject = Instantiate(prefabObject, hitPose.position, hitPose.rotation);
            } else {
                spawnedObject.transform.position = hitPose.position;
            }

            placementUpdate.Invoke();
        }
    }



    bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void DisableVisual() {
        visualObject.SetActive(false);
    }
}