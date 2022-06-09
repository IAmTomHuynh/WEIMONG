using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum StorageObjectType { food, cloth, diamond }
public class DragableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    bool holding = false;
    Vector2 pointerPos;
    [HideInInspector]
    public RectTransform rectTransform;
    StorageTask storageTask;
    public StorageObjectType type = StorageObjectType.food;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        storageTask = FindObjectOfType<StorageTask>();
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
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        storageTask.Place(this);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        this.pointerPos = eventData.position;
        
    }
}
