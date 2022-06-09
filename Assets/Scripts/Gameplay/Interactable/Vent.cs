using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vent : MonoBehaviour
{
    [SerializeField]
    GameObject ventOpen;
    [SerializeField]
    List<Vent> connectedVents;
    [SerializeField]
    List<Button> buttons;
    PlayerCharacter currentPlayer;
    private void Start()
    {
        ventOpen.gameObject.SetActive(false);
    }
    public void JumpIn(PlayerCharacter player)
    {
        currentPlayer = player;
        player.lockMoving = true;
        player.visible = false;
        player.inVent = true;
        player.transform.position = this.transform.position;
        StartCoroutine(Animation());
    }
    public void JumpOut(PlayerCharacter player)
    {
        player.lockMoving = false;
        player.visible = true;
        player.inVent = false;
        player.transform.position = this.transform.position;
        currentPlayer = null;
        StartCoroutine(Animation());
    }
    public void OnVentJump(Button button)
    {
        Debug.Log("jumped");
        foreach (Button item in buttons)
        {
            if (button == item)
            {
                currentPlayer.transform.position = connectedVents[buttons.IndexOf(item)].transform.position;
                connectedVents[buttons.IndexOf(item)].currentPlayer = this.currentPlayer;
                this.currentPlayer = null;
                break;
            }
        }
    }
    private void Update()
    {
        VisualizeJumpButtons();
    }
    void VisualizeJumpButtons()
    {
        if (currentPlayer == null)
        {
            HideButtons();
        }
        else
        {
            ShowButtons();
        }
    }
    void ShowButtons()
    {
        foreach (Button item in buttons)
        {
            item.gameObject.SetActive(true);
        }
    }
    void HideButtons()
    {
        foreach (Button item in buttons)
        {
            item.gameObject.SetActive(false);
        }
    }
    IEnumerator Animation()
    {
        ventOpen.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        ventOpen.gameObject.SetActive(false);
    }
}
