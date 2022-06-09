using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    [SerializeField]
    float speed = 5f;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
        {
            Vector3 desiredPosition = target.transform.position;
            desiredPosition.z = this.transform.position.z;
            this.transform.position = Vector3.Lerp(this.transform.position, desiredPosition, Time.deltaTime * speed); // this line in framerate dependent, NEED FIX IN THE FUTURE!!!
        }
    }
}
