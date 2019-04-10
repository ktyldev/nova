using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LightPanel : MonoBehaviour
{
    private SpriteRenderer _sprite;

    public Color Colour { set { _sprite.color = value; } }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
}
