using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Pipe : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public PipeManager pipeManager;
    public PipeType type = PipeType.green;
    public Pipe connectingPipe = null;
    private void Awake()
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pipeManager.OnPipePressed(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pipeManager.OnPipeRelease(this, eventData);
    }
}
public enum PipeType
{
    green,
    yellow,
    red
}
