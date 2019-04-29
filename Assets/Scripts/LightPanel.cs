using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LightPanel : MonoBehaviour
{
    private SpriteRenderer _sprite;

    public Color Colour { set { SetColour(value); } }

    protected virtual void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void SetColour(Color colour) =>
        _sprite.color = colour;
}
