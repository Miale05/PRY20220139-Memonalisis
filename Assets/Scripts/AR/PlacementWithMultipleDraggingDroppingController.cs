using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithMultipleDraggingDroppingController : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private Camera arCamera;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private bool onTouchHold = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private PlacementObject lastSelectedObject;


    private GameObject PlacedPrefab {
        get {
            return placedPrefab;
        }
        set {
            placedPrefab = value;
        }
    }


    void Awake() {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update() {

        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
            return;
        }

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began) {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject)) {
                    lastSelectedObject = hitObject.transform.GetComponent<PlacementObject>();
                    if (lastSelectedObject != null) {
                        PlacementObject[] allOtherObject = FindObjectsOfType<PlacementObject>();
                        foreach (PlacementObject placementObject in allOtherObject) {
                            placementObject.Selected = placementObject == lastSelectedObject;
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended) {
                lastSelectedObject.Selected = false;
            }
        }

        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
            Pose hitPose = hits[0].pose;

            if (lastSelectedObject == null) {
                lastSelectedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
            } else {
                if (lastSelectedObject.Selected) {
                    lastSelectedObject.transform.position = hitPose.position;
                    lastSelectedObject.transform.rotation = hitPose.rotation;
                }
            }
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
} 