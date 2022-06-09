using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationTask : Interactable
{
    PlayerCharacter player;
    bool playing = false;
    [SerializeField]
    GameObject playingUI;
    [SerializeField]
    RectTransform whiteDiagram;
    [SerializeField]
    RectTransform head;
    [SerializeField]
    RectTransform target1;
    [SerializeField]
    RectTransform target2;
    public float speed = 20f;
    int inputFactor = 0;
    bool initialized = false;
    float winThreshold = 10f;
    bool declaredWin = false;
    [SerializeField]
    GameObject WinPanel;
    [SerializeField]
    GameObject thrusterClockwise;
    [SerializeField]
    GameObject thrusterCounterClockwise;
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
        if(player!= null)
        {
            if (playing)
            {
                player.lockMoving = true;
                Navigation();
            }
            else
            {
                player.lockMoving = false;
                this.player = null;
            }
        }
        
    }
    public void Navigation() //Will be called by ButtonManager
    {
        this.whiteDiagram.transform.Rotate(0,0,speed*Time.deltaTime*inputFactor);
        if(Vector3.Distance(this.head.position, target1.position)<=winThreshold | Vector3.Distance(this.head.position, target2.position) < winThreshold)
        {
            if (!declaredWin)
            {
                StartCoroutine(DeclareWin());
            }
        }
    }
    public void NavButtonDown(bool clockwise)
    {
        if (clockwise)
        {
            inputFactor =-1;
            thrusterClockwise.SetActive(true);
        }
        else
        {
            inputFactor = 1;
            thrusterCounterClockwise.SetActive(true);
        }
        
    }
    public void NavButtonUp(bool clockwise)
    {
        inputFactor = 0;
        thrusterClockwise.SetActive(false);
        thrusterCounterClockwise.SetActive(false);
    }
    void Initialize()
    {
        this.whiteDiagram.rotation = Quaternion.Euler(0,0,Random.Range(-180, 180));
        declaredWin = false;
        WinPanel.SetActive(false);
        initialized = true;
        thrusterClockwise.SetActive(false);
        thrusterCounterClockwise.SetActive(false);
    }
    IEnumerator DeclareWin()
    {
        declaredWin = true;
        speed = 0f;
        WinPanel.SetActive(true);
        taskManager.TaskFinished(this);
        yield return new WaitForSeconds(1f);
        WinPanel.SetActive(false);
        speed = 20f;
        playingUI.SetActive(false);
        playing = false;
        initialized = false;
    }
}
 