using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class AR_GameManager : MonoBehaviour
{
    private ARPlaneManager planeManager;
    [SerializeField]

    void Awake() {
        planeManager = GetComponent<ARPlaneManager>();
    }

    public void TogglePlaneDetection() {
        planeManager.enabled = !planeManager.enabled;

        if (planeManager.enabled) {
            SetAllPlanesActive(true);
        } else {
            SetAllPlanesActive(false);
        }
    }

    private void SetAllPlanesActive(bool value) {
        foreach (var plane in planeManager.trackables) {
            plane.gameObject.SetActive(value);
        }
    }
}
