using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAppearance : MonoBehaviour
{
    [Header("Part Sprites")]
    public Sprite _head;
    public Sprite _eyeLeft, _eyeRight, _nose, _earLeft, _earRight, _body, _armLeft, _armRight, _legLeft, _legRight;
    [Space(8)]
    [Header("Part Address (for dev)")]
    [SerializeField]
    SpriteRenderer head;
    [SerializeField]
    SpriteRenderer eyeLeft, eyeRight, nose, earLeft, earRight, body, armLeft, armRight, legLeft, legRight;

    List<Sprite> sprites = new List<Sprite>();
    List<SpriteRenderer> parts = new List<SpriteRenderer>();

    Sprite[] heads, bodies, noses, eyesL, eyesR, earsL , earsR;
    Button but;
    private void Awake()
    {
        Initiate();
    }
    private void Start()
    {
        Randomize();
    }
    void Initiate()
    {
        heads = Resources.LoadAll<Sprite>("Characters/Parts/Head");
        bodies = Resources.LoadAll<Sprite>("Characters/Parts/Body");
        noses = Resources.LoadAll<Sprite>("Characters/Parts/Nose");
        eyesL = Resources.LoadAll<Sprite>("Characters/Parts/EyeL");
        eyesR = Resources.LoadAll<Sprite>("Characters/Parts/EyeR");
        earsL = Resources.LoadAll<Sprite>("Characters/Parts/EarL");
        earsR = Resources.LoadAll<Sprite>("Characters/Parts/EarR");

        UpdateAppearance();
    }
    public void Randomize()
    {
        _head = heads[Random.Range(0, heads.Length)];
        _body = bodies[Random.Range(0, bodies.Length)];
        _nose = noses[Random.Range(0, noses.Length)];

        _eyeLeft = eyesL[Random.Range(0, eyesL.Length)];
        _eyeRight = eyesR[Random.Range(0, eyesL.Length)];
        _earLeft = earsL[Random.Range(0, earsL.Length)];
        _earRight = earsR[Random.Range(0, earsR.Length)];

        UpdateAppearance();
    }
    void UpdateAppearance()
    {
        sprites.Clear();
        sprites.Add(_head); sprites.Add(_eyeLeft); sprites.Add(_eyeRight); sprites.Add(_nose); sprites.Add(_earLeft); sprites.Add(_earRight);
        sprites.Add(_body); sprites.Add(_armLeft); sprites.Add(_armRight); sprites.Add(_legLeft); sprites.Add(_legRight);
        parts.Clear();
        parts.Add(head); parts.Add(eyeLeft); parts.Add(eyeRight); parts.Add(nose); parts.Add(earLeft); parts.Add(earRight);
        parts.Add(body); parts.Add(armLeft); parts.Add(armRight); parts.Add(legLeft); parts.Add(legRight);
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].sprite = sprites[i];
        }
    }

}
