using UnityEngine;
using System.Collections;

public class HeatLightPanel : LightPanel
{
    public GameObject fill;

    private SpriteRenderer _fill;
    private Heat _heat;

    protected void Awake()
    {
        base.Awake();
        _fill = fill.GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void SetColour(Color colour)
    {
        base.SetColour(colour);

        var heat = Game.Instance.Ship.Heat;

        if (heat == null)
            return;

        if (heat.DangerZone)
        {
            _fill.color = new Color(colour.r, 0, 0, colour.b);        
        }
        else
        {
            _fill.color = Color.clear;
        }
    }
}
