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

    private PlayerInput _input;
    private Laser _targeting;
    private Ship _ship;
    private WeaponMeta[] _weaponData;
    private int _index = 0;

    public Color Colour => _activeWeapon.colour;
    private WeaponMeta _activeWeapon => weaponData[_index];

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _input = (PlayerInput)_ship.InputProvider;

        _targeting = targetingLaser.GetComponent<Laser>();

        var input = _input as PlayerInput;
        input.ScrollUp.AddListener(CycleWeaponUp);
        input.ScrollDown.AddListener(CycleWeaponDown);
    }

    private void CycleWeaponUp()
    {
        StopWeapons();
        _index++;
        if (_index >= weaponData.Length)
        {
            _index = 0;
        }
    }

    private void CycleWeaponDown()
    {
        StopWeapons();
        _index--;
        if (_index < 0)
        {
            _index = weaponData.Length - 1;
        }
    }

    private void StopWeapons()
    {
        foreach (var w in weaponData)
        {
            w.weapon.Stop();
        }
    }

    private void FixedUpdate()
    {
        Aim();
    }

    private void Aim()
    {
        var up = -_ship.transform.forward;
        var targetPos = _input.TargetPosition;

        var aimDir = (targetPos - (Vector2)transform.position).normalized;
        var target = Quaternion.LookRotation(aimDir, up);

        var limit = 15f;
        var leftLim = Quaternion.Euler(0, 0, -limit) * _ship.transform.up;
        var rightLim = Quaternion.Euler(0, 0, limit) * _ship.transform.up;

        var angle = Vector3.SignedAngle(aimDir, _ship.transform.up, up);
        if (angle < -limit)
        {
            target = Quaternion.LookRotation(leftLim, up);
        }
        else if (angle > limit)
        {
            target = Quaternion.LookRotation(rightLim, up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, target, _ship.weaponRotationSpeed);
    }

    void LateUpdate()
    {
        if (_input.IsVenting) return;

        bool f = !_input.IsFiring;
        _targeting.SetActive(f);

        if (f) return;

        _activeWeapon.weapon.Fire(_input);
    }

}
