using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject targetingLaser;
    public GameObject[] weapons;

    private IInputProvider _input;
    private Laser _targeting;
    private Weapon[] _weapons;
    private int _activeWeapon = 0;

    private void Start()
    {
        _input = GetComponentInParent<Ship>().InputProvider;
        _targeting = targetingLaser.GetComponent<Laser>();
        _weapons = weapons.Select(w => w.GetComponent<Weapon>()).ToArray();
    }

    void Update()
    {
        if (_input.IsFiring)
        {
            _weapons[_activeWeapon].Fire(_input);
        }
    }
}
