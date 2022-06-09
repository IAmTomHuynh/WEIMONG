using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BedObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public bool pillow = false;
    bool holding=false;
    Vector2 pointerPos;
    BedTask bedTask;
    public RectTransform rectTransform;
    public bool pickable = true;
    private void Awake()
    {
        bedTask = FindObjectOfType<BedTask>();
        rectTransform = GetComponent<RectTransform>();
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
        if (pickable)
        {
            holding = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        bedTask.Place(this);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        this.pointerPos = eventData.position;
    }

    
}
