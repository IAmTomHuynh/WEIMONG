using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITask : Interactable
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
    RectTransform capClose, capOpen, wheel, basePanel;
    [SerializeField]
    float wheelRadius = 150f;
    public bool hamsterIn = false;
    public bool capOpened = false;

    public float wheelSpeed = 10f;
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
        CapVisualize();
        WheelVisualize();
        WinCheck();
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        hamsterIn = false;
        capOpened = false;
        capOpen.gameObject.SetActive(false);
        capClose.gameObject.SetActive(true);
        HamsterSpawn();
        initialized = true;
    }
    IEnumerator DeclareWin()
    {
        yield return new WaitForSeconds(1f);
        declaredWin = true;
        WinPanel.SetActive(true);
        taskManager.TaskFinished(this);
        yield return new WaitForSeconds(1f);
        WinPanel.SetActive(false);
        playingUI.SetActive(false);
        playing = false;
        initialized = false;
    }
    public void CapClosePressed()
    {
        capOpened = true;
    }
    void CapVisualize()
    {
        capOpen.gameObject.SetActive(capOpened);
        capClose.gameObject.SetActive(!capOpened);
    }
    public void HamsterPlace(Hamster hamster)
    {
        RectTransform hamsterRect = hamster.GetComponent<RectTransform>();
        if (Vector2.Distance(hamsterRect.anchoredPosition,this.wheel.anchoredPosition)<=wheelRadius && capOpened)
        {
            hamsterRect.anchoredPosition = new Vector2(wheel.anchoredPosition.x,22) ;
            hamster.pickable = false;
            hamsterIn = true;
        }
        else
        {
            HamsterSpawn();
        }
    }
    void HamsterSpawn()
    {
        Hamster existingHamster = FindObjectOfType<Hamster>();
        if (existingHamster!=null)
        {
            GameObject.Destroy(existingHamster.gameObject);
        }
        
        GameObject hamsterGO = Instantiate(Resources.Load<GameObject>("Tasks/A.I/Prefabs/Hamster"), basePanel);
        RectTransform hamsterRect = hamsterGO.GetComponent<RectTransform>();
        hamsterRect.anchoredPosition = new Vector2(Random.Range(-550, 550), Random.Range(-310, -380));
    }
    void WheelVisualize()
    {
        if (hamsterIn)
        {
            wheel.Rotate(0,0, -Time.deltaTime * wheelSpeed);
        }
    }
    void WinCheck()
    {
        if (hamsterIn && capOpened && !declaredWin)
        {
            StartCoroutine(DeclareWin());
        }
    }
}
