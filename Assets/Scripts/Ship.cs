using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

// provides common references for other scripts in the ship hierarchy
public class Ship : MonoBehaviour
{
    public GameObject[] weaponIndicators;
    public GameObject[] heatIndicators;
    public GameObject[] hullIndicators;
    public float hullFlickerSpeed = 10f;
    public float hullFlickerThreshold = 0.2f;
    [Range(0, 1)]
    public float weaponRotationSpeed = 0.55f;

    private LightPanel[] _weaponIndicatorPanels;
    private LightPanel[] _heatIndicatorPanels;
    private LightPanel[] _hullIndicatorPanels;

    public Heat Heat { get; private set; }
    public IInputProvider InputProvider { get; private set; }
    public LightSource LightSource { get; private set; }
    public ShipMovement Movement { get; private set; }

    public Color WeaponColour => _guideWeapon.Colour;

    // cheap hack
    private WeaponController _guideWeapon;
    private Health _health;

    private void Awake()
    {

        InputProvider = GetComponent<IInputProvider>();
        Movement = GetComponent<ShipMovement>();
        _health = GetComponent<Health>();

        _weaponIndicatorPanels = weaponIndicators
            .Select(go => go.GetComponent<LightPanel>())
            .ToArray();
        _heatIndicatorPanels = heatIndicators
            .Select(go => go.GetComponent<LightPanel>())
            .ToArray();
        _hullIndicatorPanels = hullIndicators
            .Select(go => go.GetComponent<LightPanel>())
            .ToArray();

        LightSource = GetComponentInChildren<LightSource>();
    }

    private void Start()
    {
        var cam = Camera.main.GetComponent<FollowCamera>();
        cam.target = transform;

        _guideWeapon = GetComponentsInChildren<WeaponController>()[0];
        Heat = GetComponentInChildren<Heat>();
    }
    
    private void LateUpdate()
    {
        foreach (var p in _weaponIndicatorPanels)
        {
            p.Colour = WeaponColour;
        }

        foreach (var p in _heatIndicatorPanels)
        {
            p.Colour = Heat.IndicatorColour;
        }

        var hullColour = GetHullColour();
        foreach (var p in _hullIndicatorPanels)
        {
            p.Colour = hullColour;
        }
    }

    private Color _white = new Color(1, 1, 1, 1);
    private Color GetHullColour()
    {
        float flickerSpeed = 10f;
        float flickerThreshold = 0.2f;

        if (_health.Normalised > flickerSpeed)
            return _health.Normalised * _white;

        return Mathf.Abs(Mathf.PerlinNoise(Time.time * flickerSpeed, 0)) * _white;
    }

    // TODO: play explosion sound
    public void Die()
    {
        Game.Instance.GameOver();
    }
}
