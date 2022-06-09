using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Battery : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public bool alive = true;
    bool holding = false;
    Vector2 pointerPos;
    [HideInInspector]
    public RectTransform rectTransform;
    ElectricTask electricTask;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        electricTask = FindObjectOfType<ElectricTask>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (holding && pointerPos != null)
        {
            this.transform.position = pointerPos;
            transform.SetAsLastSibling();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        holding = true;
        if (electricTask.holdingBattery != null)
        {
            if (electricTask.holdingBattery == this)
            {
                electricTask.holdingBattery = null;
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        electricTask.Place(this);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        this.pointerPos = eventData.position;
        Debug.Log("position: " +this.transform.position + " anchor position: "+ this.rectTransform.anchoredPosition +" eventdata position: "+ eventData.position);
    }
}
