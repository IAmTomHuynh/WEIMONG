using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    bool holding = false;
    Vector2 pointerPos;
    CommunicationTask comm;
    public RectTransform rect;
    public bool pickable = true;
    private void Awake()
    {
        comm = FindObjectOfType<CommunicationTask>();
        rect = GetComponent<RectTransform>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (holding && pointerPos != null)
        {
            Vector2 desiredPos = pointerPos;
            desiredPos.y = this.transform.position.y;



            desiredPos.x = Mathf.Clamp(desiredPos.x, comm.minSlide.position.x, comm.maxSlide.position.x);
            this.transform.position = desiredPos;
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
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        this.pointerPos = eventData.position;
    }

}