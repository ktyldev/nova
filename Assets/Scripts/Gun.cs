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
    // TODO: cheap hack lol
    private Ship _ship;

    private void Awake()
    {
        _fireInterval = 1f / fireRate;
    }

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
    }

    protected override IEnumerator DoFire(Func<bool> getFiring)
    {
        while (getFiring())
        {
            var b = Instantiate(bullet, emitter.position, emitter.rotation)
                .GetComponent<Bullet>();
            b.GetComponent<SpriteRenderer>().color = _ship.WeaponColour;
            // cheap hack lol
            b.ignore = _ship;
            b.transform.SetParent(Game.Instance.bulletParent);

            yield return new WaitForSeconds(_fireInterval);
        }
    }
}
