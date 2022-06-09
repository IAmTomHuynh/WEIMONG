using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Paper : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerMoveHandler
{
    public bool good = true;
    bool holding = false;
    Vector2 pointerPos;
    DocumentTask document;
    public RectTransform rectTransform;
    public bool stamped = false;
    [SerializeField]
    int hier;
    Image image;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
        document = GameObject.FindObjectOfType<DocumentTask>();
    }
    private void Update()
    {
        if (holding && pointerPos!= null)
        {
            this.transform.position = pointerPos;
            transform.SetAsLastSibling();
        }
        image.raycastTarget = !stamped;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        holding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holding=false;
        document.Place(this);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        this.pointerPos = eventData.position;
    }
}
