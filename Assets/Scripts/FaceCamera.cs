using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase encargada de alinear los Sprites para que siempre miren hacia la camara
/// </summary>
public class FaceCamera : MonoBehaviour
{
    //Referencia de los sprites para asignar
    public Sprite angry;
    public Sprite happy;
    public Sprite scared;
    public Sprite sad;

    /// <summary>
    /// Referencia del padre para reasignar con mayor facilidad
    /// </summary>
    public Transform thisParent;
    /// <summary>
    /// Referencia de la pieza donde esta contenido el emoticon
    /// </summary>
    public MemoryPiece parentPiece;
    /// <summary>
    /// Referencia de la clase nativa de Unity encargada de dibujar los sprites en escena
    /// </summary>
    public SpriteRenderer spriteRenderer;


    // Funcion nativa de Unity que se llama constantemente cada frame que la aplicación esta corriendo
    void Update()
    {
        //Cambiar el sprite en base a la emocion asignada, se podria mejorar cambiandolo a una funcion estatica para evitar llamarlo constantemente
        switch (parentPiece.emotionName)
        {
            case "angry":
                spriteRenderer.sprite = angry;
                break;
            case "happy":
                spriteRenderer.sprite = happy;
                break;
            case "scared":
                spriteRenderer.sprite = scared;
                break;
            case "sad":
                spriteRenderer.sprite = sad;
                break;
            default:
                break;
        }

        //Unity maneja una jerarquia donde el padre es el objeto que encapsula este objeto, para que el emoji no se distorcione se separa del padre
        transform.parent = null;
        //La funcion nativa de Unity LookAt nos permite rotar al emoji de tal forma que mire hacia la posicion de la camara
        transform.LookAt(Camera.main.transform);
        //Se reasigna el padre del emoji para mantener un orden en la jerarquia
        transform.parent = thisParent;
    }

    /// <summary>
    /// Cambio del estado de la clase nativa de unity para dibujar los sprites
    /// </summary>
    /// <param name="state">Estado objetivo de la clase, false = No se dibujara el sprite, true = El sprite sera dibujado y visible</param>
    public void ChangeSpriteRendererState(bool state)
    {
        spriteRenderer.enabled = state;
    }
}
