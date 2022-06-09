using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedTask : Interactable
{
    PlayerCharacter player;
    bool playing = false;
    [SerializeField]
    GameObject playingUI;
    bool initialized = false;
    bool declaredWin = false;
    [SerializeField]
    GameObject WinPanel;
    int pillowInPlace = 0;
    [SerializeField]
    RectTransform baseBed;
    
    
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
    [SerializeField]
    RectTransform box, pillowTarget, pillowTarget2;
    [SerializeField]
    float boxThreshold = 10f;
    [SerializeField]
    float pillowThreshold = 10f;
    GameObject[] bedObjectsLibrary;
    List<GameObject> sortedBedObjects = new List<GameObject>();
    private new void Awake()
    {
        base.Awake();
        bedObjectsLibrary = Resources.LoadAll<GameObject>("Tasks/Bed/Prefabs");
        
        foreach (GameObject item in bedObjectsLibrary)
        {
            BedObject currentBedObject = item.GetComponent<BedObject>();
            if (!currentBedObject.pillow)
            {
                sortedBedObjects.Add(item);
            }
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

        ClearBedObjects();
        pillowInPlace = 0;

        SpawnBedObject(true);
        SpawnBedObject(true);
        SpawnBedObject(false);
        SpawnBedObject(false);
        SpawnBedObject(false);
        SpawnBedObject(false);
        SpawnBedObject(false);
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
    public void Place(BedObject bedObj)
    {
        if (Vector2.Distance(bedObj.rectTransform.anchoredPosition,box.anchoredPosition)<=boxThreshold)
        {
            if (bedObj.pillow)
            {
                SpawnBedObject(true);
            }
            GameObject.Destroy(bedObj.gameObject);

        }
        if(Vector2.Distance(bedObj.rectTransform.anchoredPosition, pillowTarget.anchoredPosition) <= pillowThreshold && bedObj.pillow)
        {
            bedObj.transform.position = pillowTarget.position;
            bedObj.transform.rotation = Quaternion.identity;
            bedObj.pickable = false;
            bedObj.transform.SetAsFirstSibling();
            this.pillowInPlace++;
        }
        if (Vector2.Distance(bedObj.rectTransform.anchoredPosition, pillowTarget2.anchoredPosition) <= pillowThreshold && bedObj.pillow)
        {
            bedObj.transform.position = pillowTarget2.position;
            bedObj.transform.rotation = Quaternion.identity;
            bedObj.pickable = false;
            bedObj.transform.SetAsFirstSibling();
            this.pillowInPlace++;
        }
        
    }
    void WinCheck()
    {
        BedObject[] bedObjects = FindObjectsOfType<BedObject>();
        if(!declaredWin && pillowInPlace>=2 && bedObjects.Length == 2)
        {
            StartCoroutine(DeclareWin());
        }
    }
    void SpawnBedObject(bool pillow)
    {
        if (pillow)
        {
            foreach (GameObject item in bedObjectsLibrary)
            {
                BedObject currentBedObject = item.GetComponent<BedObject>();
                if (currentBedObject.pillow)
                {
                    GameObject spawnedBedObject = Instantiate(item, baseBed);
                    RectTransform spawnedRect = spawnedBedObject.GetComponent<RectTransform>();
                    spawnedRect.anchoredPosition = new Vector2(Random.Range(-490,140),Random.Range(-370,370));
                    break;
                }
            }
        }
        else
        {
            GameObject spawnedBedObject = Instantiate(sortedBedObjects[Random.Range(0,sortedBedObjects.Count)], baseBed);
            RectTransform spawnedRect = spawnedBedObject.GetComponent<RectTransform>();
            spawnedRect.anchoredPosition = new Vector2(Random.Range(-490, 140), Random.Range(-370, 125));
            spawnedRect.rotation = Quaternion.Euler(0, 0, Random.Range(0, 365));
            
        }
    }
    void ClearBedObjects()
    {
        BedObject[] bedObjects = FindObjectsOfType<BedObject>();
        foreach (BedObject item in bedObjects)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}
