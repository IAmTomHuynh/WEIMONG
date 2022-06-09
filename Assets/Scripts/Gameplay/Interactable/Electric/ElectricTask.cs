using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricTask : Interactable
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
    RectTransform batteryHolder;
    [SerializeField]
    float batteryDistance=250f;

    public Battery holdingBattery;
    [SerializeField]
    RectTransform basePanel;
    PipeManager pipeManager;
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
    private new void Awake()
    {
        base.Awake();
        pipeManager = GetComponent<PipeManager>();
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
        WinCheck();
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        SpawnBatteries();
        pipeManager.Initialize();
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
    void SpawnBatteries()
    {
        Battery[] batteries = FindObjectsOfType<Battery>();
        foreach (Battery battery in batteries)
        {
            GameObject.Destroy(battery.gameObject);
        }
        holdingBattery = null;
        Instantiate(Resources.Load("Tasks/ELECTRIC/Prefabs/battery alive"),basePanel);
        GameObject deadBatteryGO = Instantiate(Resources.Load<GameObject>("Tasks/ELECTRIC/Prefabs/battery dead"), basePanel);
        holdingBattery = deadBatteryGO.GetComponent<Battery>();
    }
    public void Place(Battery battery)
    {
        if (!battery.alive)
        {
            if (Vector2.Distance(battery.rectTransform.anchoredPosition, batteryHolder.anchoredPosition) >= batteryDistance)
            {
                GameObject.Destroy(battery.gameObject);
                this.holdingBattery = null;
            }
            else
            {
                battery.rectTransform.anchoredPosition = batteryHolder.anchoredPosition;
                holdingBattery = battery;
            }
        }
        else
        {
            if (Vector2.Distance(battery.rectTransform.anchoredPosition, batteryHolder.anchoredPosition) <= batteryDistance && holdingBattery==null)
            {
                battery.rectTransform.anchoredPosition = batteryHolder.anchoredPosition;
                holdingBattery = battery;
            }
        }
    }
    void WinCheck()
    {
        if(holdingBattery!= null)
        {
            if (holdingBattery.alive && pipeManager.WinCheck() && !declaredWin)
            {
                StartCoroutine(DeclareWin());
            }
        }
        
    }
}
