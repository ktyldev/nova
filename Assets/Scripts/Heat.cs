using System.Collections;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class Heat : MonoBehaviour
{
    public float max;
    public float ventSpeed; // units to vent per second
    [Range(0, 1)]
    public float dangerZone; // when heat build up is above this percentage it will pulse
    public float pulseRate;
    public GameObject[] particleSystems;
    
    private Ship _ship;
    private PlayerInput _input;
    private float _current = 0;
    private ParticleSystem[] _particleSystems;

    public float Normalised => _current / max;
    public Color IndicatorColour => Normalised > dangerZone 
        ? PulseColourIndicator() 
        : FlatIndicatorColour();
    public bool IsVenting { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _input = (PlayerInput)_ship.InputProvider;
        _particleSystems = particleSystems
            .Select(go => go.GetComponent<ParticleSystem>())
            .ToArray();
    }

    private void Update()
    {
        if (!IsVenting && _input.IsVenting)
        {
            IsVenting = true;
            StartCoroutine(Vent());
        }
    }

    private IEnumerator Vent()
    {
        // turn on particle systems
        foreach (var ps in _particleSystems)
        {
            ps.Play();
        }

        // wait for input to stop
        do
        {
            Remove(ventSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        } while (_input.IsVenting && _current > 0);

        // turn off particle systems
        foreach (var ps in _particleSystems)
        {
            ps.Stop();
        }

        IsVenting = false;
    }

    private void Remove(float amount)
    {
        _current -= amount;
        if (_current <= 0)
        {
            _current = 0;
            print("min heat");
            return;
        }
    }

    public void Add(float amount)
    {
        _current += amount;
        if (_current >= max)
        {
            _current = max;
            _ship.Die();
            return;
        }
    }

    private Color PulseColourIndicator()
    {
        var norm = Mathf.Abs(Mathf.Sin(Time.time * pulseRate));
        return new Color(1, 1 - norm, 1 - norm);
    }

    private Color FlatIndicatorColour()
    {
        return new Color(1, 1 - Normalised, 1 - Normalised); 
    }
}
