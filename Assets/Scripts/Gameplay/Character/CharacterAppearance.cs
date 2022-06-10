using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    
    private void Start()
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
