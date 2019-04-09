using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject targetingLaser;
    public GameObject[] weapons;

    private IInputProvider _input;
    private Laser _targeting;
    private Ship _ship;
    private Weapon[] _weapons;
    private int _activeWeapon = 0;

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _input = _ship.InputProvider;

        _targeting = targetingLaser.GetComponent<Laser>();
        _weapons = weapons.Select(w => w.GetComponent<Weapon>()).ToArray();

        var input = _input as PlayerInput;
        input.ScrollUp.AddListener(() =>
        {
            _activeWeapon++;
            if (_activeWeapon >= _weapons.Length)
            {
                _activeWeapon = 0;
            }
        });
        input.ScrollDown.AddListener(() =>
        {
            _activeWeapon--;
            if (_activeWeapon < 0)
            {
                _activeWeapon = _weapons.Length - 1;
            }
        });
    }

    void Update()
    {
        if (_input.IsFiring)
        {
            _weapons[_activeWeapon].Fire(_input);
        }
    }

    private void SwitchWeapons()
    {

    }
}
