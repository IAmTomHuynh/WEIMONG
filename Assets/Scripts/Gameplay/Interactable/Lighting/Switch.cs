using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    GameObject onImage;
    public bool on=false;

    // Update is called once per frame
    void Update()
    {
        Visualize();
    }
    public void OnTouch()
    {
        on = !on;
    }
    void Visualize()
    {
        onImage.SetActive(on);
    }
}
