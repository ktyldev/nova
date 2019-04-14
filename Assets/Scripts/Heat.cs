using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    public float max;

    private Ship _ship;
    private float _current = 0;

    public float Normalised => _current / max;
    public Color IndicatorColour => new Color(1, 1 - Normalised, 1 - Normalised);

    // Start is called before the first frame update
    void Start()
    {
        _ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(float amount)
    {
        _current += amount;
        if (_current >= max)
        {
            _current = max;
            print("max heat");
            return;
        }

        print(_current);
    }
}
