using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ScreenManager : MonoBehaviour
{

    public void ChangeMenuState(GameObject target) {
        target.SetActive(!target.activeSelf);
    }

    public void ChangeScenes(string target) {
        SceneManager.LoadScene(target);
    }
}
