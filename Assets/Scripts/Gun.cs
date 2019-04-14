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
    private Ship _ship;

    protected override void Awake()
    {
        base.Awake();
        _fireInterval = 1f / fireRate;
    }

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _heat = _ship.GetComponent<Heat>();
    }

    protected override IEnumerator DoFire(Func<bool> getFiring)
    {
        while (getFiring())
        {
            var b = Instantiate(bullet, emitter.position, emitter.rotation)
                .GetComponent<Bullet>();
            b.Colour = _ship.WeaponColour;
            // cheap hack lol
            b.ignore = _ship;
            b.transform.SetParent(Game.Instance.bulletParent);
            _heat.Add(heat);

            yield return new WaitForSeconds(_fireInterval);
        }
    }
}
