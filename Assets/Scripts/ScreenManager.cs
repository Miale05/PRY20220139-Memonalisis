using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    public void ChangeMenuState(GameObject target) {
        target.SetActive(!target.activeSelf);
    }
}
