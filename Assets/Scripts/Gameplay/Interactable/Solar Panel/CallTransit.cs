using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallTransit : Interactable
{
    public bool right = true;
    Transit transit;
    private void Awake()
    {
        transit = FindObjectOfType<Transit>();
    }
    public override void OnInteract(PlayerCharacter player)
    {
        base.OnInteract(player);
        transit.Call(this.right);
    }
}
