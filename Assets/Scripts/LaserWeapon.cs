using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LaserWeapon : Weapon
{
    [Tooltip("how often damage is dealt, per second")]
    public float damageFrequency = 5f;

    public GameObject laser;

    private float _damageInterval;
    private Laser _laser;

    private void Awake()
    {
        _damageInterval = 1f / damageFrequency;
    }

    private void Start()
    {
        _laser = laser.GetComponent<Laser>();
        _laser.SetActive(false);
    }

    protected override IEnumerator DoFire(Func<bool> getFiring)
    {
        _laser.SetActive(true);

        float elapsed = 0;
        float chunk = damage * _damageInterval;

        while (getFiring())
        {
            yield return new WaitForEndOfFrame();

            elapsed += Time.deltaTime;
            if (elapsed < _damageInterval)
                continue;

            // go back so the interval check will fail next time
            elapsed -= _damageInterval;

            // damage interval has passed, look for a health component and smack it
            var health = _laser.Occluder?.GetComponent<Health>();
            if (health == null)
                continue;

            health.TakeDamage(chunk);
        }

        _laser.SetActive(false);
    }
}
