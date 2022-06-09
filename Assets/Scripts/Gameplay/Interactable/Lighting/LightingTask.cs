using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightingTask : Interactable
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
    Switch[] switches;
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
        if (CheckWin()&&!declaredWin)
        {
            StartCoroutine(DeclareWin());
        }
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        foreach (Switch item in switches)
        {
            if (Random.value > 0.6f)
            {
                item.on = true;
            }
            else
            {
                item.on = false;
            }
        }
        while (CheckWin())
        {
            foreach (Switch item in switches)
            {
                if (Random.value > 0.6f)
                {
                    item.on = true;
                }
                else
                {
                    item.on = false;
                }
            }
        }

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
    bool CheckWin()
    {
        bool win = true;
        foreach (Switch item in switches)
        {
            if (!item.on)
            {
                win = false;
            }
        }
        return win;
    }
}