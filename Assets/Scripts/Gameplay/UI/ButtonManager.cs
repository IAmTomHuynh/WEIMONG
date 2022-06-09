using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public bool lockInteractions = false;
    [SerializeField]
    public Button reportButton;
    [SerializeField]
    public Button useButton;
    [SerializeField]
    public Button killButton;
    [SerializeField]
    public Button ventButton;
    Interactable[] interactables;
    PlayerCharacter player;
    Interactable interacting;

    PlayerCharacter currentPlayer;

    BannerManager banner;

    TaskManager taskManager;
    MatchManager matchManager;
    private void Start()
    {
        Initialize();
        
    }
    private void Update()
    {
        GameObject currentPlayerGO = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayerGO)
        {
            currentPlayer = currentPlayerGO.GetComponent<PlayerCharacter>();
        }
        InitiateButtons();
        KillButtonVisualize();
        ReportButtonVisualize();
        VentButtonVisualize();

        LockInteractionsOverride();
    }
    void Initialize()
    {
        taskManager = FindObjectOfType<TaskManager>();
        taskManager.taskDone += this.DisableInteractionAfterDone;
        banner = FindObjectOfType<BannerManager>();
        matchManager = FindObjectOfType<MatchManager>();
        interactables = new Interactable[0];
        interactables = GameObject.FindObjectsOfType<Interactable>();
        foreach (Interactable item in interactables)
        {
            item.OnPlayerEnterRange += this.OnPlayerEnter;
            item.OnPlayerExitRange += this.OnPlayerExit;
        }

        reportButton.interactable = false;
        useButton.interactable = false;
        killButton.interactable = false;

        InitiateButtons();
        KillButtonVisualize();
        VentButtonVisualize();
    }
    void OnPlayerEnter(PlayerCharacter player, Interactable interactable)
    {
       
        if (player.alive)
        {
            if (!player.imposter)
            {
                if (!taskManager.finishedTasks.Contains(interactable))
                {
                    useButton.interactable = true;
                    interacting = interactable;
                    this.player = player;
                }
                
            }
        }
    }
    void OnPlayerExit(PlayerCharacter player, Interactable interactable)
    {
        if (!player.imposter && player.alive)
        {
            useButton.interactable = false;
            interacting = null;
            this.player = null;
        }
    }
    public void OnUseButtonPressed()
    {
        if(interacting!= null && this.player!=null)
        {
            if (this.player.alive)
            {
                interacting.OnInteract(this.player);
            }
            
        }
    }
    public void OnKillButtonPressed()
    {
        currentPlayer.Kill();
    }

    void InitiateButtons()
    {
        GameObject currentPlayerGO = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayerGO)
        {
            currentPlayer = currentPlayerGO.GetComponent<PlayerCharacter>();
        }
        if (currentPlayer != null)
        {
            useButton.gameObject.SetActive(!currentPlayer.imposter);
            killButton.gameObject.SetActive(currentPlayer.imposter);
            ventButton.gameObject.SetActive(currentPlayer.imposter);
        }
        
    }
    void KillButtonVisualize()
    {
        GameObject currentPlayerGO = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayerGO)
        {
            currentPlayer = currentPlayerGO.GetComponent<PlayerCharacter>();
        }
        if (currentPlayer != null)
        {
            if (currentPlayer.victim != null)
            {
                killButton.interactable = true;
            }
            else
            {
                killButton.interactable = false;
            }
        }
    }
    public void OnReportButtonPressed()
    {
        StartCoroutine(banner.Report(2));
        StartCoroutine(matchManager.VotingPhaseStart(2));
    }
    void ReportButtonVisualize()
    {
        reportButton.interactable = currentPlayer.reportable;
    }
    void VentButtonVisualize()
    {
        if (currentPlayer.jumpableVent != null)
        {
            ventButton.interactable = true;
        }
        else
        {
            ventButton.interactable = false;
        }
    }
    public void OnVentButtonPressed()
    {
        if(currentPlayer.jumpableVent != null)
        {
            if (!currentPlayer.inVent)
            {
                currentPlayer.jumpableVent.JumpIn(currentPlayer);
            }
            else
            {
                currentPlayer.jumpableVent.JumpOut(currentPlayer);
            }
        }
        
    }
    public void DisableInteractionAfterDone(Interactable disabling)
    {
        if(this.interacting == disabling)
        {
            useButton.interactable = false;
        }
    }
    void LockInteractionsOverride()
    {
        if (lockInteractions)
        {
            killButton.interactable = false;
            useButton.interactable = false;
            reportButton.interactable = false;
            ventButton.interactable = false;
        }
    }
}
