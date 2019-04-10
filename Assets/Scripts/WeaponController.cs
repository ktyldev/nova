using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject targetingLaser;
    [Serializable]
    public struct WeaponMeta
    {
        [SerializeField]
        private GameObject gameObject;
        public Color colour;
        public GameObject[] panels;

        private Weapon _weapon;

        public Weapon weapon => GetWeapon();
        private Weapon GetWeapon()
        {
            if (_weapon == null)
            {
                _weapon = gameObject.GetComponent<Weapon>();
            }

            return _weapon;
        }
    }
    public WeaponMeta[] weaponData;

    private IInputProvider _input;
    private Laser _targeting;
    private Ship _ship;
    private WeaponMeta[] _weaponData;
    private int _index = 0;

    public Color Colour => _activeWeapon.colour;
    private WeaponMeta _activeWeapon => weaponData[_index];

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _input = _ship.InputProvider;

        _targeting = targetingLaser.GetComponent<Laser>();

        var input = _input as PlayerInput;
        input.ScrollUp.AddListener(() =>
        {
            _index++;
            if (_index >= weaponData.Length)
            {
                _index = 0;
            }
        });
        input.ScrollDown.AddListener(() =>
        {
            _index--;
            if (_index < 0)
            {
                _index = weaponData.Length - 1;
            }
        });
    }

    void LateUpdate()
    {
        bool f = !_input.IsFiring;
        _targeting.SetActive(f);

        if (f) return;

        _activeWeapon.weapon.Fire(_input);
    }

}
