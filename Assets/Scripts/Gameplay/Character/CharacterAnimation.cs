using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        FlipManager();
    }
    void FlipManager()
    {
        if (rb.velocity.x > 0.1)
        {
            this.transform.localScale = new Vector3(-1,1,1);
        }
        else if (rb.velocity.x < -0.1)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
