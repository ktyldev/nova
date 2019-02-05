using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public float damagePerSecond;

    [Header("Lasers")]
    public GameObject weaponLaser;
    public GameObject targeting;

    private IInputProvider _input;
    private bool _isFiring;

    private Laser _laser;
    private Laser _targeting;

    private void Start()
    {
        var ship = GetComponentInParent<Ship>();
        _input = ship.InputProvider;

        _laser = weaponLaser.GetComponent<Laser>();
        _targeting = targeting.GetComponent<Laser>();
    }

    void Update()
    {
        if (!_isFiring && _input.IsFiring)
        {
            _isFiring = true;
            StartCoroutine(FireLaser());
        }
    }

    private IEnumerator FireLaser()
    {
        _targeting.SetActive(false);
        _laser.SetActive(true);

        yield return new WaitWhile(() => _input.IsFiring);

        _isFiring = false;
        _laser.SetActive(false);

        _targeting.SetActive(true);
    }

    private IEnumerator DoLaserDamage()
    {
        while (_isFiring)
        {
            yield return new WaitForSeconds(1);
            if (!_isFiring)
                break;

            var health = _laser.Occluder.GetComponent<Health>();
            if (health == null)
                continue;

            health.TakeDamage(damagePerSecond);
        }
    }
}
