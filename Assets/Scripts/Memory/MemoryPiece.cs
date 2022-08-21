using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPiece : MonoBehaviour
{

    public bool isActive;

    public string emotionName = "";
    bool hidePiece = false;
    public float hideFadeTime = 1.5f;
    float extraFadeTime = 0;
    float fadeStart;

    float holdTimeStart;
    public bool hasEmote;
    public FaceCamera emote;

    public Transform boardReference;

    // Update is called once per frame
    void Update()
    {
        if (hidePiece)
        {
            if (Time.time - fadeStart >= hideFadeTime + extraFadeTime)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                hidePiece = false;
                extraFadeTime = 0;

                if (hasEmote)
                {
                    Debug.Log("Change to FALSE");
                    emote.ChangeSpriteRendererState(false);
                }
            }
        }
    }

    public void Select()
    {
        holdTimeStart = Time.time;
        if (isActive)
        {
            Show();
            MemoryManager.instance.CheckPiece(this.gameObject);
        }
    }

    public void DeSelect()
    {
        /*
        if (Time.time - holdTimeStart >= holdTimeToDestroy)
        {
            MemorySpawnableManager.instance.currentModelsCount -= 1;
            Destroy(gameObject);
        }
        */
    }

    public void Hide(bool keepActive,float extraHideTime = 0)
    {
        fadeStart = Time.time;
        isActive = keepActive;
        hidePiece = true;

        extraFadeTime = extraHideTime;
    }

    public void Show()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        if (hasEmote)
        {
            Debug.Log("Change to TRUE");
            emote.ChangeSpriteRendererState(true);
        }
    }

    public void InitializePiece(GameObject obj,string name)
    {
        if (transform.childCount > 1)
        {
            transform.GetChild(1).SetParent(GameObject.Find("Trash").transform);
        }

        GameObject temp = Instantiate(obj, transform.position, boardReference.rotation);
        temp.transform.SetParent(transform);
        emotionName = name;

        transform.GetChild(1).localPosition = new Vector3(0,1,0);
        transform.GetChild(1).localScale = new Vector3(0.5f,5,0.5f);

        Show();
        Hide(true,3.5f);
    }
}
