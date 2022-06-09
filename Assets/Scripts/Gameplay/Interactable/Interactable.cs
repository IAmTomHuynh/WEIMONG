using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    public event Action<PlayerCharacter, Interactable> OnPlayerEnterRange;
    public event Action<PlayerCharacter, Interactable> OnPlayerExitRange;
    public TaskManager taskManager;
    protected void Awake()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }
    private void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            OnPlayerEnterRange?.Invoke(player, this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            OnPlayerExitRange?.Invoke(player, this);
        }
    }
    public virtual void OnInteract(PlayerCharacter player)
    {
        Debug.Log("using " + this.name);
    }
}
