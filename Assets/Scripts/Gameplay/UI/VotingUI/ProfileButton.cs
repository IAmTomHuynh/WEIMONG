using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ProfileButton : MonoBehaviour
{
    public PlayerCharacter player;
    public bool voted = false;
    TextMeshProUGUI text;
    public int votes = 0;
    public Button button;
    PlayerCharacter currentPlayer ;
    VotingManager votingManager;
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        votingManager = FindObjectOfType<VotingManager>();
        Initiate();
    }
    private void OnEnable()
    {
        Initiate();
    }
    public void Initiate()
    {
        votes = 0;
        UpdateProfile();
    }
    public void UpdateProfile()
    {
        if (player!=null)
        {
            text.text = player.name;
            if (!player.alive)
            {
                text.text +=" IS DEAD";
            }
            else if(voted)
            {
                text.text += " have voted";
            }
        }
        if (player != null)
        {
            
            if (player.alive && this.player != currentPlayer && !votingManager.currentPlayerVoted)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
        //set image, name, voting status, selecting status
    }
    public void OnClick()
    {
        votingManager.ProfileButtonClicked(this);
    }
}
