using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class PipeManager : MonoBehaviour
{
    EventSystem eventSystem;

    Pipe currentPressedPipe;
    [SerializeField]
    List<Pipe> pipes;
    [SerializeField]
    RectTransform basePanel;
    List<GameObject> dotConnections = new List<GameObject>();
    GameObject currentDotConnection;
    Touch touchInput;
    [SerializeField]
    RectTransform pointerFollower;
    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        touchInput = new Touch();
        GetAllPipesUnderThisManager();
    }
    private void OnEnable()
    {
        touchInput.Enable();
    }
    private void OnDisable()
    {
        touchInput.Disable();
    }
    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        VisualizeCurrentConnection();
    }
    void GetAllPipesUnderThisManager()
    {
        pipes.Clear();
        Pipe[] allPipes = FindObjectsOfType<Pipe>(true);
        foreach (Pipe item in allPipes)
        {
            if (item.pipeManager != null)
            {
                if (item.pipeManager==this)
                {
                    pipes.Add(item);
                }
            }
        }
    }
    public void Initialize()
    {
        currentPressedPipe = null;
        GetAllPipesUnderThisManager();
        foreach (Pipe pipe in pipes)
        {
            pipe.connectingPipe = null;
        }
        currentDotConnection = null;
        ClearDotConnections();
    }
    public void OnPipePressed(Pipe pipe, PointerEventData data)
    {
        if (pipe.connectingPipe != null)
        {
            currentPressedPipe = pipe.connectingPipe;
            currentPressedPipe.connectingPipe = null;
            pipe.connectingPipe = null;

        }
        else
        {
            currentPressedPipe = pipe;
        }
        VisualizePipe();
        currentDotConnection = CreateDotConnection(currentPressedPipe.GetComponent<RectTransform>().anchoredPosition, data.position);
    }
    public void OnPipeRelease(Pipe pipe, PointerEventData data)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        eventSystem.RaycastAll(data, raycastResults);
        if (raycastResults.Count > 0)
        {
            Pipe dstPipe = raycastResults[0].gameObject.GetComponent<Pipe>();
            if (dstPipe != null && currentPressedPipe != null)
            {
                if (dstPipe != currentPressedPipe)
                {
                    if (dstPipe.connectingPipe != null)
                    {
                        dstPipe.connectingPipe.connectingPipe = null;
                    }
                    if (currentPressedPipe.connectingPipe != null)
                    {
                        currentPressedPipe.connectingPipe.connectingPipe = null;
                    }
                    currentPressedPipe.connectingPipe = dstPipe;
                    dstPipe.connectingPipe = currentPressedPipe;
                    Debug.Log("connected " + currentPressedPipe.name + " with " + dstPipe.name);
                    currentPressedPipe = null;
                }

            }
        }
        else if (raycastResults.Count == 0)
        {
            if (pipe.connectingPipe != null)
            {
                pipe.connectingPipe.connectingPipe = null;
            }
            pipe.connectingPipe = null;
        }
        currentPressedPipe = null;
        VisualizePipe();
        if (currentDotConnection != null)
        {
            GameObject.Destroy(currentDotConnection.gameObject);
            currentDotConnection = null;
        }
    }
    public bool WinCheck()
    {
        bool win = true;
        foreach (Pipe pipe in pipes)
        {
            if (pipe.connectingPipe!=null)
            {
                if (pipe.connectingPipe.type != pipe.type)
                {
                    win = false;
                }
            }
            else
            {
                win = false;
            }
            
        }
        return win;
    }
    /// <summary>
    /// ////VISUAL FUNCTIONS////////
    /// </summary>
    void VisualizePipe()
    {
        ClearDotConnections();
        List<Pipe> drawedPipes = new List<Pipe>();
        foreach (Pipe pipe in pipes)
        {
            if (!drawedPipes.Contains(pipe))
            {
                if (pipe.connectingPipe != null)
                {
                    CreateDotConnection(pipe.GetComponent<RectTransform>().anchoredPosition, pipe.connectingPipe.GetComponent<RectTransform>().anchoredPosition);
                    drawedPipes.Add(pipe);
                    drawedPipes.Add(pipe.connectingPipe);
                }
            }
        }
    }
    void VisualizeCurrentConnection()
    {
        if (currentDotConnection != null && currentPressedPipe != null)
        {
            Vector2 pointerScreenPos = touchInput.TouchMap.Position.ReadValue<Vector2>();

            pointerFollower.transform.position = pointerScreenPos;
            ModifyDotConnection(currentDotConnection, currentPressedPipe.GetComponent<RectTransform>().anchoredPosition, pointerFollower.anchoredPosition);
        }
    }
    GameObject CreateDotConnection(Vector2 posA, Vector2 posB)
    {
        GameObject pipeLine = new GameObject("Pipe Line", typeof(Image));
        pipeLine.transform.SetParent(basePanel, false);
        dotConnections.Add(pipeLine);
        pipeLine.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.57f);
        pipeLine.GetComponent<Image>().raycastTarget = false;
        RectTransform pipeRect = pipeLine.GetComponent<RectTransform>();
        Vector2 dir = (posB - posA).normalized;
        float distance = Vector2.Distance(posA, posB);
        pipeRect.anchorMin = Vector2.zero;
        pipeRect.anchorMax = Vector2.zero;
        pipeRect.sizeDelta = new Vector2(distance, 50f);
        pipeRect.anchoredPosition = posA + dir * distance * 0.5f;
        pipeRect.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        pipeRect.transform.SetAsFirstSibling();
        return pipeLine;
    }
    void ModifyDotConnection(GameObject dotConnection, Vector2 posA, Vector2 posB)
    {
        RectTransform pipeRect = dotConnection.GetComponent<RectTransform>();

        Vector2 dir = (posB - posA).normalized;
        float distance = Vector2.Distance(posA, posB);

        pipeRect.anchorMin = Vector2.zero;
        pipeRect.anchorMax = Vector2.zero;

        pipeRect.sizeDelta = new Vector2(distance, 50f);
        pipeRect.anchoredPosition = posA + dir * distance * 0.5f;
        pipeRect.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
    void ClearDotConnections()
    {
        foreach (GameObject item in dotConnections)
        {
            GameObject.Destroy(item);
        }
        dotConnections.Clear();
    }
}
