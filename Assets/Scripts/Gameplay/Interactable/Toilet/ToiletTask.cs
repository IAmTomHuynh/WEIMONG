using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToiletTask : Interactable
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
    public RectTransform target;
    public float targetRadius = 200f;
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
        ClearPoop();
        PoopSpawn(); PoopSpawn(); PoopSpawn(); PoopSpawn(); PoopSpawn();
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
    public void FlushButton()
    {
        Poop[] poops = FindObjectsOfType<Poop>();
        foreach (Poop poop in poops)
        {
            if (poop.scared)
            {
                GameObject.Destroy(poop.gameObject);
            }
        }
    }
    void WinCheck()
    {
        Poop[] poops = FindObjectsOfType<Poop>();
        if (poops.Length<=0 && !declaredWin && playing)
        {
            StartCoroutine(DeclareWin());
        }
    }
    void PoopSpawn()
    {
        
        GameObject poopGO = Instantiate(Resources.Load<GameObject>("Tasks/Toilet/Prefabs/poop"), basePanel);
        RectTransform poopRect = poopGO.GetComponent<RectTransform>();
        poopRect.anchoredPosition = new Vector2(Random.Range(220,600),Random.Range(80,-350));
    }
    void ClearPoop()
    {
        Poop[] poops = FindObjectsOfType<Poop>();
        foreach (Poop poop in poops)
        {
            GameObject.Destroy(poop.gameObject);
        }
    }
}
