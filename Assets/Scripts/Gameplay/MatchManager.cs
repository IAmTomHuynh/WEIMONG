using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum MatchPhase
{
    playing,
    voting
}
public class MatchManager : MonoBehaviour
{
    TaskManager taskManager;
    public MatchPhase gamePhase { get { return this._phase; } }
    MatchPhase _phase = MatchPhase.playing;

    public event Action OnMatchStart;
    public event Action OnPlayingPhaseStart;
    public event Action OnVotingPhaseStart;
    public event Action OnMatchEnd;

    PlayerCharacter[] players;
    List<PlayerCharacter> survivors = new List<PlayerCharacter>();

    bool declaredWin = false;
    [SerializeField]
    List<Transform> gatherPositions = new List<Transform>();

    ButtonManager buttonManager;
    [SerializeField]
    RectTransform VotingUI;
    BannerManager banner;
    private void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
        players = FindObjectsOfType<PlayerCharacter>();
        buttonManager = FindObjectOfType<ButtonManager>();
        banner = FindObjectOfType<BannerManager>();
        declaredWin = false;
    }
    private void Start()
    {
        BeginMatch();
    }
    private void Update()
    {
        CheckWin();
    }
    void BeginMatch()
    {
        OnMatchStart?.Invoke();
        StartCoroutine(PlayingPhaseStart(0));
        taskManager.FinishedRandomizeTasks(7);
        VotingUI.gameObject.SetActive(false);
    }
    public IEnumerator PlayingPhaseStart(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _phase = MatchPhase.playing;
        OnPlayingPhaseStart?.Invoke();
        GatherSurvivors(false);
        VotingUI.gameObject.SetActive(false);
    }
    public IEnumerator VotingPhaseStart( float delayTime)
    {
        GatherSurvivors(true);
        yield return new WaitForSeconds(delayTime);
        _phase = MatchPhase.voting;
        OnVotingPhaseStart?.Invoke();
        
        VotingUI.gameObject.SetActive(true);
    }
    void CheckWin()
    {
        UpdateSurvivor();
        int aliveImposterCount = 0;
        foreach (PlayerCharacter player in survivors)
        {
            if (player.imposter)
            {
                aliveImposterCount++;
            }
        }
        if(aliveImposterCount >= (survivors.Count - aliveImposterCount)) // if there are equal or more imposters than crewmates.
        {
            DeclareWin(true);
        }else 
        if (taskManager.allTaskDone)
        {
            DeclareWin(false);
        }
    }
    void DeclareWin(bool imposter)
    {
        if (!declaredWin)
        {
            _phase = MatchPhase.voting;
            OnMatchEnd?.Invoke();
            if (imposter)
            {
                StartCoroutine(banner.TextBanner("imposter win", 10));
            }
            else
            {
                StartCoroutine(banner.TextBanner("crewmates win", 10));

            }
            foreach (PlayerCharacter player in survivors)
            {
                if (player.imposter)
                {
                    Debug.Log(player.name);
                }
            }
        }
        
    }
    void UpdateSurvivor()
    {
        survivors.Clear();
        foreach (PlayerCharacter player in players)
        {
            if (player.alive)
            {
                survivors.Add(player);
            }
        }
    }
    void GatherSurvivors(bool lockMoveAndLockInterations)
    {
        UpdateSurvivor();
        for (int i = 0; i < survivors.Count; i++)
        {
            survivors[i].transform.position = gatherPositions[i].position;
            survivors[i].lockMoving = lockMoveAndLockInterations;
            buttonManager.lockInteractions = lockMoveAndLockInterations;

        }
    }
}
