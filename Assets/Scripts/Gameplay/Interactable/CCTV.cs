using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCTV : Interactable
{
    PlayerCharacter player;
    bool playing = false;
    [SerializeField]
    GameObject playingUI;
    bool initialized = false;
    bool declaredWin = false;
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
    }
    void Initialize()
    {
        declaredWin = false;
        initialized = true;
    }
    void DeclareWin()
    {
        declaredWin = true;
        playingUI.SetActive(false);
        playing = false;
        initialized = false;
    }
    public void OnTouch()
    {
        if (!declaredWin)
        {
            DeclareWin();
        }
    }
}
