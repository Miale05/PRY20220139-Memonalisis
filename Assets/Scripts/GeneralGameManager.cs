using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum de todos los familiares y una variable extra para cargar los animales
/// </summary>
public enum LoadedFamiliar
{
    father,
    mother,
    brother,
    sister,
    uncle,
    aunt,
    cousin_M,
    cousin_F,
    grandfather,
    grandmother,
    none
}

/// <summary>
/// Enum de las emociones utilizadas en el proyecto
/// </summary>
public enum Emotions
{
    angry,
    happy,
    scared,
    sad
}

public class GeneralGameManager : MonoBehaviour
{
    /// <summary>
    /// Instancia estatica para ser llamada desde cualquier otro script, utiliza el patron de diseño Singleton
    /// </summary>
    public static GeneralGameManager instance;
    /// <summary>
    /// Familiar seleccionado para la carga de modelos adecuada
    /// </summary>
    public LoadedFamiliar selectedFamiliar;
    /// <summary>
    /// Variable Deprecada, remover al limpiar el codigo
    /// </summary>
    public int maxModelsLoaded;
    /// <summary>
    /// Fuente de audio llamada para reproducir los sonidos uno a la vez
    /// </summary>
    public AudioSource source;

    /// <summary>
    /// Funcion de Unity que se llama al inicio de cada escena, utilizada para inicializar el singleton
    /// </summary>
    void Start()
    {
        if (GeneralGameManager.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Funcion para seleccionar el familiar a utilizar
    /// </summary>
    /// <param name="familiar">Familiar seleccionado</param>
    public void SelectFamiliar(string familiar)
    {
        Enum.TryParse(familiar, out selectedFamiliar);
    }

    /// <summary>
    /// Funcion Deprecada, remover al limpiar el codigo
    /// </summary>
    /// <param name="value"></param>
    public void SetMaxModelsLoaded(int value)
    {
        maxModelsLoaded = value;
    }
}
