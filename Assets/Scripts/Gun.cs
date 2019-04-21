using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Gun : Weapon
{
    public Transform emitter;
    public GameObject bullet;

    [Tooltip("rounds per second")]
    public float fireRate = 2.5f;

    private float _fireInterval;

    private void Awake()
    {
        _fireInterval = 1f / fireRate;
    }

    protected override IEnumerator DoFire(Func<bool> getFiring)
    {
        while (getFiring())
        {
            var b = Instantiate(bullet, emitter.position, emitter.rotation)
                .GetComponent<Bullet>();
            b.Colour = Ship.WeaponColour;
            // cheap hack lol
            b.ignore = Ship;
            b.transform.SetParent(Game.Instance.bulletParent);
            Ship.Heat.Add(heat);

            yield return new WaitForSeconds(_fireInterval);
        }
    }
}
