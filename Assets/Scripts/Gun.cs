using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Gun : Weapon
{
    public Transform emitter;
    public GameObject bullet;

    [Tooltip("rounds per second")]
    public float fireRate = 2.5f;

    private float _fireInterval;

    protected override string SFXName => GameConstants.SFX_PewPewNoise;

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

            PlaySFX(false, false);

            yield return new WaitForSeconds(_fireInterval);
        }
    }
}
