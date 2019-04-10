using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

// provides common references for other scripts in the ship hierarchy
public class Ship : MonoBehaviour
{
    public GameObject[] weaponIndicators;
    private LightPanel[] _weaponIndicatorPanels;

    public IInputProvider InputProvider { get; private set; }

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
            p.Colour = _guideWeapon.Colour;
        }
    }

    // TODO: play explosion sound
    public void Die()
    {
        Instantiate(Game.Instance.explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
