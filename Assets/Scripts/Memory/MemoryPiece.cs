using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPiece : MonoBehaviour
{

    public bool isActive;

    public string emotionName = "";
    bool hidePiece = false;
    public float hideFadeTime = 1.5f;
    float fadeStart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hidePiece)
        {
            if (Time.time - fadeStart >= hideFadeTime)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                hidePiece = false;
                ChangeBaseColor(Color.white);
            }
        }
    }

    //TEST
    private void OnMouseUp()
    {
        Select();
    }

    public void Select()
    {
        if (isActive)
        {
            Show();
            MemoryManager.instance.CheckPiece(this.gameObject);
        }
    }

    public void Hide(bool keepActive, Color color)
    {
        isActive = keepActive;
        hidePiece = true;
        fadeStart = Time.time;
        ChangeBaseColor(color);
    }

    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void InitializePiece(GameObject obj,string name)
    {
        GameObject temp = Instantiate(obj, transform.position, Quaternion.identity);
        temp.transform.SetParent(transform);
        emotionName = name;

        transform.GetChild(0).position += new Vector3(0, 0.35f, 0);
        transform.GetChild(0).localScale = new Vector3(0.5f,5,0.5f);

        Show();
        Hide(true,Color.white);
    }

    public void ChangeBaseColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }
}
