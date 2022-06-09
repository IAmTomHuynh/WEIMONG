using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VotingManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    RectTransform profileHolder;
    List<ProfileButton> profileButtons = new List<ProfileButton>();
    PlayerCharacter[] players;
    public float voteTime = 60f;
    float counter = 0f;

    MatchManager match;
    PlayerCharacter currentPlayer;

    ProfileButton selectingProfile;

    public bool currentPlayerVoted = false;
    BannerManager banner;

    bool doneVoting = false;
    private void Awake()
    {
        players = FindObjectsOfType<PlayerCharacter>();
        match = FindObjectOfType<MatchManager>();
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        banner = FindObjectOfType<BannerManager>();
    }
    private void Start()
    {
        Initiate();
    }
    private void OnEnable()
    {
        Initiate();
    }
    private void Update()
    {
        if (counter>0)
        {
            counter -= Time.deltaTime;
        }
        
        if (counter<=0 && !doneVoting)
        {
            
            StartCoroutine(CheckVoteAndEliminate(3));
            StartCoroutine(match.PlayingPhaseStart(3));
        }
        timerText.text = Mathf.FloorToInt(counter).ToString();
    }
    void UpdateAllProfile()
    {
        foreach (ProfileButton profile in profileButtons)
        {
            profile.UpdateProfile();
        }
    }
    void Initiate()
    {
        doneVoting = false;
        foreach (ProfileButton profile in profileButtons)
           {
                GameObject.Destroy(profile.gameObject);
            }
        profileButtons.Clear();

        foreach (PlayerCharacter player in players)
        {
            GameObject profileGO = Instantiate(Resources.Load<GameObject>("UI/Voting/ProfileButton"),this.profileHolder);
            ProfileButton currentButton = profileGO.GetComponent<ProfileButton>();

            currentButton.player = player;

            currentButton.UpdateProfile();
            this.profileButtons.Add(currentButton);
            
        }
        counter = voteTime;
        selectingProfile = null;
        currentPlayerVoted = false;
        UpdateAllProfile();
    }
    public void ButtonSkip()
    {
        if (currentPlayer.alive)
        {
            ClearConfirmButton();
            foreach (ProfileButton profile in profileButtons)
            {

                if (profile.player == currentPlayer)
                {
                    profile.voted = true;
                    currentPlayerVoted = true;
                    ClearConfirmButton();
                    UpdateAllProfile();
                }
            }
        }
        
    }
    public void ButtonChat()
    {
        if (currentPlayer.alive)
        {
            ClearConfirmButton();
        }
    }
    public void ProfileButtonClicked(ProfileButton profile)
    {

        ClearConfirmButton();

        if (currentPlayer.alive)
        {
            UpdateAllProfile();
            selectingProfile = profile;
            Instantiate(Resources.Load("UI/Voting/ConfirmButton"), selectingProfile.transform);
        }

    }
    public void ConfirmVote()
    {
        if (selectingProfile!=null)
        {
            ClearConfirmButton();
            selectingProfile.votes++;
            foreach (ProfileButton item in profileButtons)
            {
                if (item.player == currentPlayer)
                {
                    item.voted = true;
                    currentPlayerVoted = true;
                    UpdateAllProfile();
                    break;
                }

            }
        }

    }
    IEnumerator CheckVoteAndEliminate(float time)
    {
        doneVoting = true;

        int maxVote = 1;
        List<PlayerCharacter> suspects = new List<PlayerCharacter>();
        foreach (ProfileButton item in profileButtons)
        {
            if (item.player.alive)
            {
                if (item.votes==maxVote)
                {
                    suspects.Add(item.player);
                    maxVote = item.votes;
                }
                else if (item.votes > maxVote)
                {
                    suspects.Clear();
                    suspects.Add(item.player);
                    maxVote = item.votes;
                }
            }
        }
        if (suspects.Count==1)
        {
            string declare = "";
            if (suspects[0].imposter)
            {
                declare = "";

            }
            else
            {
                declare = " not";
            }
            StartCoroutine(banner.TextBanner(suspects[0].name + " is"+ declare +" an impostor", time));
        }else
        {
            StartCoroutine(banner.TextBanner("No one died today", time));
        }
        
        
        yield return new WaitForSeconds(time);
    }
    void ClearConfirmButton()
    {
        ConfirmButton[] confirms = FindObjectsOfType<ConfirmButton>();
        foreach (ConfirmButton item in confirms)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}
