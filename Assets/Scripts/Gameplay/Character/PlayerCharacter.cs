using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rgbd2D;
    [SerializeField] private float moveSpeed = 300f;

    private Vector2 currentMove;

    private readonly string MoveAction = "Move";
    [SerializeField]
    bool _imposter = false;
    public bool imposter 
    { get
        {
            return _imposter;
        } 
    }
    public bool alive = true;
    public bool lockMoving = false;
    [HideInInspector]
    public PlayerCharacter victim;
    public bool reportable = false;
    [SerializeField]
    float interactionRange = 3f;
    public Vent jumpableVent;
    public bool inVent = false;
    [SerializeField]
    float ventingRange = 3f;
    [SerializeField]
    GameObject Appearance;
    public bool visible = true;
    void Start()
    {
        ResetNew();
    }

    void ResetNew()
    {
        currentMove = Vector2.zero;
    }

    void Update()
    {
        KillAvailability();
        ReportAvailability();
        VentingAvailability();
        Visibility();
    }
    private void FixedUpdate()
    {
        if (this.tag == "Player" & !lockMoving)
        {
            currentMove = playerInput.actions[MoveAction].ReadValue<Vector2>();
            rgbd2D.velocity = currentMove * moveSpeed * Time.deltaTime;
        }
    }
    public void Die()
    {
        Debug.Log("Die blehhh");
        this.alive = false;
        Instantiate(Resources.Load("Interactables/Prefabs/Corpse"), this.transform.position, Quaternion.identity);
        if (!CompareTag("Player"))
        {
            this.visible = false;
        }
        else
        {
            this.visible = false;
            Ghost();
        }
        

    }
    void Ghost()
    {
        Debug.Log("boooo, im a ghost");
    }
    public void Kill()
    {
        if (victim != null)
        {
            victim.Die();
        }
    }
    void KillAvailability()
    {
        PlayerCharacter[] players = GameObject.FindObjectsOfType<PlayerCharacter>();
        victim = null;
        foreach (PlayerCharacter item in players)
        {
            if (Vector2.Distance(this.transform.position,item.transform.position)<=interactionRange && item!=this && !item.imposter && item.alive)
            {
                victim = item;
                break;
            }
        }
    }
    void ReportAvailability()
    {
        reportable = false;
        if (alive)
        {
            Corpse[] corpses = GameObject.FindObjectsOfType<Corpse>();
            foreach (Corpse corpse in corpses)
            {
                if (Vector2.Distance(this.transform.position,corpse.transform.position)<=interactionRange)
                {
                    reportable = true;
                    break;
                }
            }
        }
    }
    void VentingAvailability()
    {
        jumpableVent = null;
        Vent[] vents = FindObjectsOfType<Vent>();
        foreach (Vent item in vents)
        {
            if(Vector2.Distance(item.transform.position,this.transform.position)<= ventingRange)
            {
                jumpableVent = item;
                break;
            }
        }
    }
    void Visibility()
    {
        Appearance.gameObject.SetActive(visible);
    }
}
