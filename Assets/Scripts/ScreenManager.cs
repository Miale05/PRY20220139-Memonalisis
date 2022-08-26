using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Script encargado de las funciones de los menus, las funciones son llamadas desde el inspector en los botones de la escena
/// </summary>
public class ScreenManager : MonoBehaviour
{

    /// <summary>
    /// Cambio de tipo de orientacion
    /// </summary>
    /// <param name="landscape">La nueva orientacion es Landscape</param>
    public void SetScreenToLandscape(bool landscape) {
        if (landscape) {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        } else {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    /// <summary>
    /// Apagar/Prender Menus, en los botones que se utiliza se llama dos veces, un para apagar el menu actual y otro para prender el siguiente menu
    /// </summary>
    /// <param name="target">Estado del menu a cambiar</param>
    public void ChangeMenuState(GameObject target) {
        target.SetActive(!target.activeSelf);
    }

    /// <summary>
    /// Funcion para cambiar a una nueva escena
    /// </summary>
    /// <param name="target">Escena objetivo a cambiar</param>
    public void ChangeScenes(string target) {
        GeneralGameManager.instance.source.Stop();

        //SceneManager es una clase nativa de Unity para la carga de escenas
        SceneManager.LoadScene(target);
    }

    /// <summary>
    /// Alternar el estado del PlaneManager, se utiliza para el boton del ojo en las escenas AR para apagar los planos
    /// </summary>
    /// <param name="target">GameObject que contiene el ARPlaneManager</param>
    public void ChangePlaneManagerState(GameObject target)
    {
        target.GetComponent<ARPlaneManager>().enabled = !target.GetComponent<ARPlaneManager>().isActiveAndEnabled;
    }

    /// <summary>
    /// Cambiar el valor del familiar seleccionado durante el menu de seleccion, en caso se cargue el menu de animales se selecciona el valor none
    /// </summary>
    /// <param name="value">Familiar seleccionado</param>
    public void CallSelectFamiliar(string value)
    {
        GeneralGameManager.instance.SelectFamiliar(value);
    }
}
