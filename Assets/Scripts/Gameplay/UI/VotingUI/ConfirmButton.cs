using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    VotingManager votingManager;
    private void Awake()
    {
        votingManager = FindObjectOfType<VotingManager>();
    }
    public void OnClick()
    {
        votingManager.ConfirmVote();
    }
}
