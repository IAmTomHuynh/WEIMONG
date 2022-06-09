using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolarPanelTask : Interactable
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
    RectTransform basePanel;
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
        WinCheck();
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        ClearDirt();
        SpawnDirt(); SpawnDirt(); SpawnDirt(); SpawnDirt(); SpawnDirt();
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
    void SpawnDirt()
    {
        GameObject dirtGO = Instantiate(Resources.Load<GameObject>("Tasks/Solar Panel/Prefabs/dirt 2"), basePanel);
        RectTransform dirtRect = dirtGO.GetComponent<RectTransform>();
        Vector2 randomPos = new Vector2(Random.Range(-580,580),Random.Range(-300,50));
        while(randomPos.x<500 && randomPos.x > -200)
        {
            randomPos = new Vector2(Random.Range(-580, 580), Random.Range(-300, 50));
        }
        dirtRect.anchoredPosition = randomPos;
        dirtRect.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
    void ClearDirt()
    {
        Dirt[] dirts = FindObjectsOfType<Dirt>();
        foreach (Dirt item in dirts)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
    void WinCheck()
    {
        Dirt[] dirts = FindObjectsOfType<Dirt>();
        if(dirts.Length<=0 && !declaredWin && playing)
        {
            StartCoroutine(DeclareWin());
        }
    }
}
