using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PressurePumpTask : Interactable
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
    float pressure = 100f;
    [SerializeField]
    float speed = 10f;
    float pump = 8f;
    [SerializeField]
    Slider fill;
    [SerializeField]
    RectTransform button;
    float timeFactor = 0f;
    [SerializeField]
    float buttonSpeed = 10f;
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
                PressureDecrease();
            }
            else
            {
                player.lockMoving = false;
                this.player = null;
            }
        }
        VisualizePump();
        VisualizeButton();
    }
    void PressureDecrease()
    {
        pressure -= Time.deltaTime * speed;
        pressure = Mathf.Clamp(pressure,0, 100);
        if(pressure >= 95 && !declaredWin)
        {
            StartCoroutine(DeclareWin());
        }
    }
    public void Pump()
    {
        pressure += pump;
        timeFactor = 0f;
    }
    void Initialize()
    {
        pressure = 0f;
        declaredWin = false;
        WinPanel.SetActive(false);
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
    void VisualizePump()
    {
        fill.value = pressure;
    }
    void VisualizeButton()
    {
        timeFactor += Time.deltaTime*buttonSpeed;
        timeFactor = Mathf.Clamp(timeFactor, 0, 1);
        float t = 0f;
        t = Mathf.Sin(timeFactor * Mathf.PI);
        button.anchoredPosition = Vector3.Lerp(new Vector3(-154,65,0),new Vector3(-154,10,0),t);
    }
}
