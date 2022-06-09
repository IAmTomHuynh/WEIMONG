using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hamster : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerMoveHandler
{
    public bool pickable = true;
    bool holding = false;
    Vector2 pointerPos;
    AITask aiTask;
    Animator anim;
    private void Awake()
    {
        aiTask = FindObjectOfType<AITask>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holding && pointerPos != null)
        {
            this.transform.position = pointerPos;
            transform.SetAsLastSibling();
        }
        AnimationUpdate();
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
        aiTask.HamsterPlace(this);
    }
    void AnimationUpdate()
    {
        if (aiTask.hamsterIn)
        {
            anim.SetBool("run",true);
        }
        else
        {
            anim.SetBool("run",false);
        }
    }
    
}
