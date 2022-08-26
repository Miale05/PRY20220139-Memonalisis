using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ScreenManager : MonoBehaviour
{

    public void SetScreenToLandscape(bool landscape) {
        if (landscape) {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        } else {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    public void ChangeMenuState(GameObject target) {
        target.SetActive(!target.activeSelf);
    }

    public void ChangeScenes(string target) {
        GeneralGameManager.instance.source.Stop();
        SceneManager.LoadScene(target);
    }

    public void ChangePlaneManagerState(GameObject target)
    {
        target.GetComponent<ARPlaneManager>().enabled = !target.GetComponent<ARPlaneManager>().isActiveAndEnabled;
    }

    public void CallSelectFamiliar(string value)
    {
        GeneralGameManager.instance.SelectFamiliar(value);
    }
}
