using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Corpse: MonoBehaviour
{
    MatchManager match;
    private void OnEnable()
    {
        match = FindObjectOfType<MatchManager>();
        match.OnVotingPhaseStart += this.CleanCorpse;
        match.OnPlayingPhaseStart += this.CleanCorpse;
    }
    private void OnDestroy()
    {
        match.OnVotingPhaseStart -= this.CleanCorpse;
        match.OnPlayingPhaseStart -= this.CleanCorpse;
    }
    private void OnDisable()
    {
        match.OnVotingPhaseStart -= this.CleanCorpse;
        match.OnPlayingPhaseStart -= this.CleanCorpse;
    }
    void CleanCorpse()
    {
        GameObject.Destroy(this.gameObject);
    }
}
