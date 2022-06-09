using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class OxygenTask : Interactable
{
    PlayerCharacter player;
    bool playing = false;
    [SerializeField]
    GameObject playingUI;
    bool initialized = false;
    bool declaredWin = false;
    [SerializeField]
    GameObject WinPanel;
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
    void WinCheck()
    {
        if (pipeManager.WinCheck() && !declaredWin)
        {
            StartCoroutine(DeclareWin());
        }
    }
}
