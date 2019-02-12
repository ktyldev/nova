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
    private Ship _ship;

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _input = _ship.InputProvider;

        _laser = weaponLaser.GetComponent<Laser>();
        _targeting = targeting.GetComponent<Laser>();
    }

    void Update()
    {
        if (!_ship.isLocalPlayer)
            return;

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

        float elapsed = 0;
        float damageInterval = 0.2f;    // do damage every 0.2 seconds
        float damageChunk = damagePerSecond * damageInterval;

        while (_input.IsFiring)
        {
            yield return new WaitForEndOfFrame();

            elapsed += Time.deltaTime;
            if (elapsed < damageInterval)
                continue;

            // go back so the interval check will fail next frame
            elapsed -= damageInterval;

            // damage interval has passed, look for a health component and smack it
            var health = _laser.Occluder?.GetComponent<Health>();
            if (health == null)
                continue;

            health.TakeDamage(damageChunk);
        }

        _isFiring = false;
        _laser.SetActive(false);

        _targeting.SetActive(true);
    }

    private IEnumerator DoLaserDamage()
    {
        while (_isFiring)
        {
            yield return new WaitForSeconds(1);
            if (!_input.IsFiring)
                yield break;

            var health = _laser.Occluder?.GetComponent<Health>();
            if (health == null)
                continue;

            health.TakeDamage(damagePerSecond);
        }
    }
}
