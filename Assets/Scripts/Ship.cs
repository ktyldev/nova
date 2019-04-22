using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

// provides common references for other scripts in the ship hierarchy
public class Ship : MonoBehaviour
{
    public GameObject[] weaponIndicators;
    private LightPanel[] _weaponIndicatorPanels;
    public GameObject[] heatIndicators;
    private LightPanel[] _heatIndicatorPanels;

    [Range(0, 1)]
    public float weaponRotationSpeed = 0.55f;

    public Heat Heat { get; private set; }
    public IInputProvider InputProvider { get; private set; }
    public LightSource LightSource { get; private set; }

    public Color WeaponColour => _guideWeapon.Colour;

    // cheap hack
    private WeaponController _guideWeapon;

    private void Awake()
    {
        InputProvider = GetComponent<IInputProvider>();

        _weaponIndicatorPanels = weaponIndicators
            .Select(go => go.GetComponent<LightPanel>())
            .ToArray();
        _heatIndicatorPanels = heatIndicators
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
    }

    // TODO: play explosion sound
    public void Die()
    {
        //Explosion.New(Game.Instance.shipExplosion, transform.position);
        Game.Instance.GameOver();
    }
}
