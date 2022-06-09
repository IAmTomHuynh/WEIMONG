using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageTask : Interactable
{
    PlayerCharacter player;
    bool playing = false;
    [SerializeField]
    GameObject playingUI;
    bool initialized = false;
    bool declaredWin = false;
    [SerializeField]
    GameObject WinPanel;
    GameObject[] storageObjectLibrary;
    [SerializeField]
    RectTransform foodBox, clothBox, diamondBox;
    [SerializeField]
    float boxDistance = 180f;
    [SerializeField]
    Transform objectsParent;
    int score = 0;
    private new void Awake()
    {
        base.Awake();
        storageObjectLibrary = Resources.LoadAll<GameObject>("Tasks/Storage/Prefabs");
    }
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
        Spawn();
        Spawn();
        Spawn();
        Spawn();
        Spawn();
        Spawn();
        score = 0;
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
    public void Place(DragableObject dragObject)
    {
        if (CheckIfInbox(dragObject))
        {
            if (CheckIfCorrectBox(dragObject))
            {
                GameObject.Destroy(dragObject.gameObject);
                score += 1;
            }
            else
            {
                Relocate(dragObject);
            }
        }
    }
    bool CheckIfInbox(DragableObject dragObject)
    {
        bool inbox = false;
        if (Vector2.Distance(dragObject.transform.position,foodBox.position)<=boxDistance)
        {
            inbox = true;
        }
        if (Vector2.Distance(dragObject.transform.position, clothBox.position) <= boxDistance)
        {
            inbox = true;
        }
        if (Vector2.Distance(dragObject.transform.position, diamondBox.position) <= boxDistance)
        {
            inbox = true;
        }
        return inbox;
    }
    bool CheckIfCorrectBox(DragableObject dragObject)
    {
        bool correct = false;
        if (Vector2.Distance(dragObject.transform.position, foodBox.position) <= boxDistance && dragObject.type == StorageObjectType.food)
        {
            correct = true;
        }
        if (Vector2.Distance(dragObject.transform.position, clothBox.position) <= boxDistance && dragObject.type == StorageObjectType.cloth)
        {
            correct = true;
        }
        if (Vector2.Distance(dragObject.transform.position, diamondBox.position) <= boxDistance && dragObject.type == StorageObjectType.diamond)
        {
            correct = true;
        }
        return correct;
    }
    void Spawn()
    {
        GameObject spawnedObject = Instantiate(storageObjectLibrary[Random.Range(0, storageObjectLibrary.Length)],objectsParent);
        RectTransform spawnedRectTransform = spawnedObject.GetComponent<RectTransform>();
        spawnedRectTransform.anchoredPosition = new Vector2(Random.Range(-550, 550), Random.Range(-50, -370));
    }
    void Relocate(DragableObject dragObject)
    {
        dragObject.rectTransform.anchoredPosition = new Vector2(Random.Range(-550,550), Random.Range(-50,-370)) ;
    }
    void WinCheck()
    {
        if (score>=6)
        {
            if (!declaredWin)
            {
                StartCoroutine(DeclareWin());
            }
        }
    }
}
