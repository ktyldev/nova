using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

// provides common references for other scripts in the ship hierarchy
public class Ship : MonoBehaviour
{
    public GameObject[] weaponIndicators;
    private LightPanel[] _weaponIndicatorPanels;

    [Range(0, 1)]
    public float weaponRotationSpeed = 0.55f;

    public IInputProvider InputProvider { get; private set; }

    public Color WeaponColour => _guideWeapon.Colour;

    // cheap hack
    private WeaponController _guideWeapon;

    private void Awake()
    {
        InputProvider = GetComponent<IInputProvider>();

        _weaponIndicatorPanels = weaponIndicators
            .Select(go => go.GetComponent<LightPanel>())
            .ToArray();
    }

    private void Start()
    {
        var cam = Camera.main.GetComponent<FollowCamera>();
        cam.target = transform;

        _guideWeapon = GetComponentsInChildren<WeaponController>()[0];
    }
    
    private void LateUpdate()
    {
        foreach (var p in _weaponIndicatorPanels)
        {
            p.Colour = WeaponColour;
        }
    }

    // TODO: play explosion sound
    public void Die()
    {
        Instantiate(Game.Instance.explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);

        Game.Instance.Restart(3);
    }
}
