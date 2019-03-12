using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform muzzle;
    public float damagePerSecond;
    public float bulletDamage;

    public GameObject weaponGun;
    [Header("Lasers")]
    public GameObject targeting;

    private IInputProvider _input;
    private bool _isFiring;
    private bool _laserActive = true;
    private bool _gunActive;

    private Gun _gun;
    private Laser _targeting;
    private Ship _ship;

    private Weapon _laserWeapon;

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _input = _ship.InputProvider;

        _targeting = targeting.GetComponent<Laser>();

        _gun = weaponGun.GetComponent<Gun>();

        _laserWeapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        //if (!_ship.isLocalPlayer)
        //    return;

        if (_input.IsFiring)
        {
            if(_laserActive == true)
            {
                _laserWeapon.Fire(_input);
            }

            if (_gunActive == true)
            {
                StartCoroutine(FireBullet());
            }
        }
    }

    private IEnumerator FireBullet()
    {
        _targeting.SetActive(false);
        float elapsed = 0;
        float fireInterval = 0.3f;

        while(_input.IsFiring){
            elapsed += Time.deltaTime;
            if (elapsed < fireInterval)
            {
                _gun.Fire();
            }

            elapsed -= fireInterval;

            _isFiring = false;
            _targeting.SetActive(true);
            yield break;
        }
    }
}
