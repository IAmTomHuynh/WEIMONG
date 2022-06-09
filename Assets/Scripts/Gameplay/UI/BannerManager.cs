using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BannerManager : MonoBehaviour
{
    [SerializeField]
    GameObject report;
    [SerializeField]
    RectTransform textGO;
    TextMeshProUGUI text;
    private void Awake()
    {
        report.SetActive(false);
        text = textGO.GetComponentInChildren<TextMeshProUGUI>();
        textGO.gameObject.SetActive(false);
    }
    public IEnumerator Report( float time )
    {
        report.SetActive(true);
        yield return new WaitForSeconds(time);
        report.SetActive(false);
    }
    public IEnumerator TextBanner(string _text,float time)
    {
        textGO.gameObject.SetActive(true);
        text.text = _text;
        yield return new WaitForSeconds(time);
        
        text.text = "";
        textGO.gameObject.SetActive(false);
    }
}
