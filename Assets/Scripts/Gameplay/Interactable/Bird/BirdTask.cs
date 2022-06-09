using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class BirdTask : Interactable
{
    PlayerCharacter player;
    bool playing = false;
    [SerializeField]
    GameObject playingUI;
    bool initialized = false;
    bool declaredWin = false;
    [SerializeField]
    GameObject WinPanel;
    Touch touchInput;
    GameObject currentWorm;
    [SerializeField]
    RectTransform basePanel;
    [SerializeField]
    RectTransform pointerFollower;
    [SerializeField]
    RectTransform birdMouth;
    [SerializeField]
    float feedDistance = 120f;
    [SerializeField]
    int score = 0;
    [SerializeField]
    Animator anim;
    [SerializeField]
    TextMeshProUGUI text;
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
    protected new void Awake()
    {
        base.Awake();
        touchInput = new Touch();
    }
    private void Start()
    {/*
        touchInput.TouchMap.Position.started += this.OnTouch;
        touchInput.TouchMap.Position.performed += this.OnRelease;
        //touchInput.TouchMap.Position.performed += this.OnRelease;*/
    }
    private void OnEnable()
    {
        touchInput.Enable();
    }
    private void OnDisable()
    {
        touchInput.Disable();
    }
    private void Update()
    {
        if (player != null)
        {
            if (playing)
            {
                player.lockMoving = true;
                VisualizeBird();
            }
            else
            {
                player.lockMoving = false;
                this.player = null;
            }
        }
        if (currentWorm != null)
        {
            currentWorm.transform.position = touchInput.TouchMap.Position.ReadValue<Vector2>();
        }
        pointerFollower.transform.position = touchInput.TouchMap.Position.ReadValue<Vector2>();
        WinCheck();
        
        text.text = score + "/5";
        //Debug.Log(touchInput.TouchMap.Position.ReadValue<Vector2>());
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        score = 0;
        if (currentWorm != null)
        {
            GameObject.Destroy(currentWorm.gameObject);
            currentWorm = null;
        }
        anim.SetBool("foodNear", false);
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
    public void OnTouch()
    {
        Debug.Log("touched " + touchInput.TouchMap.Position.ReadValue<Vector2>());
        if (currentWorm != null)
        {
            GameObject.Destroy(currentWorm.gameObject);
            currentWorm = null;
        }
        currentWorm = Instantiate<GameObject>(Resources.Load<GameObject>("Tasks/Bird/Prefabs/worm"), basePanel);
        currentWorm.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
    public void OnRelease()
    {
        if (currentWorm != null)
        {
            if (Vector2.Distance(pointerFollower.anchoredPosition,birdMouth.anchoredPosition)<=feedDistance)
            {
                score++;
                anim.SetTrigger("feed");
            }
            GameObject.Destroy(currentWorm.gameObject);
            currentWorm = null;
        }
    }
    void WinCheck()
    {
        if(score>=5 && !declaredWin)
        {
            StartCoroutine(DeclareWin());
        }
    }
    void VisualizeBird()
    {
        if (anim.enabled)
        {
            if (currentWorm != null && Vector2.Distance(pointerFollower.anchoredPosition, birdMouth.anchoredPosition) <= feedDistance)
            {
                anim.SetBool("foodNear", true);
            }
            else
            {
                anim.SetBool("foodNear", false);
            }
        }
        
    }
}
