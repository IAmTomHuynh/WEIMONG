using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DocumentTask : Interactable
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
    Transform deniedTarget;
    [SerializeField]
    Transform approvedTarget;
    [SerializeField]
    int placeThreshold = 100;

    [SerializeField]
    GameObject deniedStamp;
    [SerializeField]
    GameObject approvedStamp;
    [SerializeField]
    GameObject papersAndStampsParent;
    [SerializeField]
    GameObject[] papers;
    [SerializeField]
    int point = 0;
    [SerializeField]
    TextMeshProUGUI text;
    bool stamping = false;

    [SerializeField]
    Animator approvedAnim;
    [SerializeField]
    Animator deniedAnim;
    private new void Awake()
    {
        base.Awake();
        papers = Resources.LoadAll<GameObject>("Tasks/Document/Prefabs/Papers");

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
        WinChecker();
        ScoreVisualize();
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        ClearPapers();
        SpawnPaper(); 
        SpawnPaper();
        SpawnPaper();
        SpawnPaper(); 
        SpawnPaper(); 
        SpawnPaper();
        point = 0;
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
    public void Place(Paper paper)
    {
        if (!stamping)
        {
            if (Vector2.Distance(paper.transform.position, deniedTarget.transform.position) <= placeThreshold)
            {
                StartCoroutine(Stamp(paper, false));
                SpawnPaper();
            }

            if (Vector2.Distance(paper.transform.position, approvedTarget.transform.position) <= placeThreshold)
            {
                StartCoroutine(Stamp(paper, true));
                SpawnPaper();
            }
        }
    }
    void SpawnPaper()
    {
        GameObject spawnedPaperGO = Instantiate(papers[Random.Range(0, papers.Length)], papersAndStampsParent.transform);
        Paper spawnPaper = spawnedPaperGO.GetComponent<Paper>();
        Vector2 messyPaperPosition = spawnPaper.rectTransform.anchoredPosition;
        messyPaperPosition.x += Random.Range(-80, 80);
        messyPaperPosition.y += Random.Range(-80, 80);
        spawnPaper.rectTransform.anchoredPosition = messyPaperPosition;
        spawnPaper.transform.SetAsFirstSibling();
    }
    void ClearPapers()
    {
        Paper[] existingPaper = GameObject.FindObjectsOfType<Paper>();
        foreach (Paper paper in existingPaper)
        {
            GameObject.Destroy(paper.gameObject);
        }
    }
    void WinChecker()
    {
        if (point>= 5 && !declaredWin)
        {
            StartCoroutine(DeclareWin());
        }
    }
    void ScoreVisualize()
    {
        text.text = point+"/5";
    }
    IEnumerator Stamp(Paper paper,bool approved)
    {
        if (approved)
        {
            approvedAnim.SetTrigger("Stamp");
        }
        else
        {
            deniedAnim.SetTrigger("Stamp");
        }
        yield return new WaitForSeconds(0.2f);
        if (approved)
        {
            GameObject stamp = Instantiate(this.approvedStamp, playingUI.transform);
            stamp.transform.parent = paper.transform;
        }
        else
        {
            GameObject stamp = Instantiate(this.deniedStamp, playingUI.transform);
            stamp.transform.parent = paper.transform;
        }
        
        paper.stamped = true;
        if (paper.good==approved)
        {
            point++;
        }
        else
        {
            point--;
            point = Mathf.Clamp(point, 0, 5);
        }
        yield return new WaitForSeconds(0.5f);
        stamping = false;
    }
}
