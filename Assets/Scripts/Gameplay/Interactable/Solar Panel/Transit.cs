using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transit : Interactable
{
    bool moving = false;
    bool currentRight = true;
    public bool dockedRight = true;
    [SerializeField]
    float movingTime = 4f;
    [SerializeField]
    GameObject colliderRight, colliderLeft, colliderInnerLeft,colliderInnerRight;
    [SerializeField]
    Vector2 rightPosition, leftPosition;
    float counter = 0f;
    public override void OnInteract(PlayerCharacter player)
    {
        base.OnInteract(player);
        Click();
    }
    private void Update()
    {
        if (moving)
        {
            Move();

        }
        else
        {
            colliderRight.gameObject.SetActive(!currentRight);
            colliderInnerRight.gameObject.SetActive(!currentRight);

            colliderLeft.gameObject.SetActive(currentRight);
            colliderInnerLeft.gameObject.SetActive(currentRight);
            
        }

        
    }
    public void Call(bool right)
    {
        if(!moving && currentRight != right)
        {
            moving = true;
            counter = 0f;
        }
    }
    void Click()
    {
        if (!moving)
        {
            moving = true;
            counter = 0f;
        }
    }
    void Move()
    {
        colliderRight.gameObject.SetActive(true);
        colliderLeft.gameObject.SetActive(true);
        colliderInnerLeft.gameObject.SetActive(true);
        colliderInnerRight.gameObject.SetActive(true);

        counter += Time.deltaTime;
        if (currentRight)
        {

            this.transform.position = Vector2.Lerp(rightPosition, leftPosition, counter / movingTime);
        }
        else
        {
            this.transform.position = Vector2.Lerp(leftPosition,rightPosition, counter / movingTime);
        }

        if (counter >= movingTime)
        {
            moving = false;
            currentRight = !currentRight;

            counter = 0f;
        }
    }
}
