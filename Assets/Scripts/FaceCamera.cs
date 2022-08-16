using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    public Sprite angry;
    public Sprite happy;
    public Sprite scared;
    public Sprite sad;

    public Transform thisParent;
    public MemoryPiece parentPiece;
    public SpriteRenderer spriteRenderer;


    // Update is called once per frame
    void Update()
    {
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

        transform.parent = null;
        transform.LookAt(Camera.main.transform);
        transform.parent = thisParent;
    }

    public void ChangeSpriteRendererState(bool state)
    {
        spriteRenderer.enabled = state;
    }
}
