using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Poop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public bool pickable = true;
    bool holding = false;
    Vector2 pointerPos;
    ToiletTask toiletTask;
    Animator anim;
    RectTransform rect;

    public bool scared = false;
    private void Awake()
    {
        toiletTask = FindObjectOfType<ToiletTask>();
        anim = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holding && pointerPos != null)
        {
            this.transform.position = pointerPos;
            transform.SetAsLastSibling();
        }
        ScareManager();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (pickable)
        {
            holding = true;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        pointerPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
    }
    void ScareManager()
    {
        if (Vector2.Distance(toiletTask.target.anchoredPosition,this.rect.anchoredPosition) <= toiletTask.targetRadius)
        {
            scared = true;
        }
        else
        {
            scared = false;
        }
        anim.SetBool("scared", scared);
    }
}
