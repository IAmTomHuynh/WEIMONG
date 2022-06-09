using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Cleaner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    bool holding = false;
    Vector2 pointerPos;
    SolarPanelTask solarPanel;
    public bool pickable = true;
    Dirt[] dirts;
    [SerializeField]
    float cleanRadius =100f;
    RectTransform rect;
    private void Awake()
    {
        solarPanel = FindObjectOfType<SolarPanelTask>();
        rect = GetComponent<RectTransform>();
    }
    void Start()
    {
    }
    private void OnEnable()
    {
        this.rect.anchoredPosition = new Vector2(348, 303);
    }

    // Update is called once per frame
    void Update()
    {
        if (holding && pointerPos != null)
        {
            this.transform.position = pointerPos;
            transform.SetAsLastSibling();
        }
        Clean();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (pickable)
        {
            holding = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        this.pointerPos = eventData.position;
    }
    void GetDirts()
    {
        dirts = FindObjectsOfType<Dirt>();
    }

    void Clean()
    {
        GetDirts();
        foreach (Dirt item in dirts)
        {
            RectTransform dirtRect = item.GetComponent<RectTransform>();
            Vector2 brushPos = this.rect.anchoredPosition;
            brushPos.y -= 135;
            if (Vector2.Distance(dirtRect.anchoredPosition, brushPos) <= cleanRadius)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
        //GetDirts();
    }
}
