using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class CommunicationTask : Interactable
{
    PlayerCharacter player;
    bool playing = false;
    [SerializeField]
    GameObject playingUI;
    bool initialized = false;
    bool declaredWin = false;
    [SerializeField]
    GameObject WinPanel;
    [SerializeField]
    RectTransform button;
    [SerializeField]
    RectTransform greenLight;

    public RectTransform minSlide, maxSlide;
    float target = -600f;
    [SerializeField]
    float winTime = 2f;
    float timer = 0f;
    [SerializeField]
    float winRange = 100f;
    [SerializeField]
    RectTransform signalRect;

    [SerializeField]
    Animator signalLine;
    [SerializeField]
    List<RectTransform> dots = new List<RectTransform>();
    List<GameObject> lines = new List<GameObject>();
    [SerializeField]
    Color lineColor;
    [SerializeField]
    float lineWeight = 10f;
    public override void OnInteract(PlayerCharacter player)
    {
        base.OnInteract(player);
        this.player = player;
        playingUI.SetActive(true);
        playing = true;
        if (!initialized)
        {
            Initialize();
        }
    }
    private void Update()
    {
        if (player != null)
        {
            if (playing)
            {
                player.lockMoving = true;
            }
            else
            {
                player.lockMoving = false;
                this.player = null;
            }
        }
        Timer();
        CheckWin();
        SignalLineAnimate();
        VisualGreenLight();
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        timer = 0f;
        RandomTarget();
        ClearLines();
        initialized = true;
    }
    IEnumerator DeclareWin()
    {
        declaredWin = true;
        WinPanel.SetActive(true);
        taskManager.TaskFinished(this);
        yield return new WaitForSeconds(1f);
        WinPanel.SetActive(false);
        playingUI.SetActive(false);
        playing = false;
        initialized = false;
    }
    void RandomTarget()
    {
        target = Random.Range(minSlide.anchoredPosition.x, maxSlide.anchoredPosition.x);
        while (Mathf.Abs(target - button.anchoredPosition.x) <= winRange)
        {
            target = Random.Range(minSlide.anchoredPosition.x, maxSlide.anchoredPosition.x);
        }
    }
    void CheckWin()
    {
        if (Mathf.Abs(target - button.anchoredPosition.x) <= winRange)
        {
        }
        if (Mathf.Abs(target-button.anchoredPosition.x)<= winRange && timer>=winTime && !declaredWin)
        {
            StartCoroutine(DeclareWin());
        }
    }
    void Timer()
    {
        if (Mathf.Abs(target - button.anchoredPosition.x) <= winRange)
        {
            timer+= Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }
    }
    void SignalLineAnimate()
    {
        float noise ;
        float delta = Mathf.Abs(target - button.anchoredPosition.x);
        noise = delta / (Mathf.Abs(minSlide.anchoredPosition.x - maxSlide.anchoredPosition.x));

        noise = Mathf.Clamp(noise, 0f, 1f);
        if (playing)
        {
            signalLine.SetFloat("noise", noise);
            UpdateLines();
        }
        
    }
    void ClearLines()
    {
        if (lines.Count > 0)
        {
            foreach (GameObject item in lines)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
        lines.Clear();
    }
    GameObject CreateLine(Vector2 posA, Vector2 posB)
    {
        GameObject line = new GameObject("Line", typeof(Image));
        line.transform.SetParent(signalRect, false);
        lines.Add(line);
        line.GetComponent<Image>().color = lineColor;
        line.GetComponent<Image>().raycastTarget = false;
        RectTransform lineRect = line.GetComponent<RectTransform>();
        Vector2 dir = (posB - posA).normalized;
        float distance = Vector2.Distance(posA, posB);
        lineRect.anchorMin = Vector2.zero;
        lineRect.anchorMax = Vector2.zero;
        lineRect.sizeDelta = new Vector2(distance, lineWeight);
        lineRect.anchoredPosition = posA + dir * distance * 0.5f;
        lineRect.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        lineRect.transform.SetAsFirstSibling();
        return line;
    }
    void UpdateLines()
    {
        ClearLines();
        for (int i = 0; i < dots.Count-1; i++)
        {
            CreateLine(dots[i].anchoredPosition, dots[i + 1].anchoredPosition);
        }
    }
    void VisualGreenLight()
    {
        if (Mathf.Abs(target - button.anchoredPosition.x) <= winRange)
        {
            greenLight.gameObject.SetActive(true);
        }
        else
        {
            greenLight.gameObject.SetActive(false);
        }
    }
}
