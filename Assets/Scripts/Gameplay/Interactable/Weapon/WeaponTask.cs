using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class WeaponTask : Interactable
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
    PlayerInput input;
    [SerializeField]
    Rigidbody2D aim;
    [SerializeField]
    float aimSpeed=100f;
    [SerializeField]
    GraphicRaycaster rayCaster;
    List<RaycastResult> aimRays = new List<RaycastResult>();
    [SerializeField]
    Transform meteoriteHolder;
    [SerializeField]
    GameObject meteorite;
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
        CheckWin();
    }
    private void FixedUpdate()
    {
        MoveAim();
    }
    void Initialize()
    {
        declaredWin = false;
        WinPanel.SetActive(false);
        SpawnMeteorites();
        SpawnMeteorites();
        SpawnMeteorites();
        SpawnMeteorites();
        SpawnMeteorites();

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
    void MoveAim()
    {
        Vector3 currentMove = input.actions["Move"].ReadValue<Vector2>();
        aim.velocity = currentMove * Time.deltaTime * aimSpeed;
    }
    public void Shoot()
    {
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = aim.position;
        EventSystem.current.RaycastAll(pointerEventData, aimRays);
        Debug.Log(aimRays.Count );
        if (aimRays.Count>0)
        {
            Meteorite meteorite = aimRays[0].gameObject.GetComponent<Meteorite>();
            if (meteorite != null)
            {
                Destroy(aimRays[0].gameObject);
            }
        }
    }
    void CheckWin()
    {
        if (meteoriteHolder.childCount<=0 && !declaredWin && playing)
        {
            StartCoroutine(DeclareWin());
        }
    }
    void SpawnMeteorites()
    {
        GameObject currentMeteorite = Instantiate(this.meteorite, meteoriteHolder);
        RectTransform meteoriteRect = currentMeteorite.GetComponent<RectTransform>();
        meteoriteRect.anchoredPosition = new Vector2(Random.Range(-110, 110), Random.Range(-110, 110));
        meteoriteRect.rotation = Quaternion.Euler(0, 0, Random.Range(0, 365));
    }
}
