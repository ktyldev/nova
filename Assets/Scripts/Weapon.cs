using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // TODO: maybe we need a proper Laser class
    [Serializable]
    public struct Laser
    {
        public GameObject laser;
        public float length;
        [HideInInspector]
        public LineRenderer line => laser.GetComponent<LineRenderer>();
    }

    public Transform muzzle;

    [Header("Lasers")]
    public Laser targetingLaser;
    public Laser weaponLaser;

    private PlayerInput _input;
    private bool _isFiring;
    // gotta ignore this when firin lazors
    private GameObject _ship;

    private void Start()
    {
        _input = Game.Instance.PlayerInput;
        _ship = GetComponentInParent<ShipMovement>().gameObject;
    }

    void Update()
    {
        if (_input.GetStartFiring())
        {
            StartCoroutine(FireLaser());
        }
    }

    private void FixedUpdate()
    {
        if (!_isFiring)
        {
            DrawLaser(targetingLaser);
        }
        else
        {
            targetingLaser.line.positionCount = 0;
        }
    }

    private void DrawLaser(Laser laser, float distance = -1)
    {
        laser.line.positionCount = 2;

        var start = muzzle.transform.position;
        if (distance == -1)
        {
            distance = laser.length;
        }
        var end = start + muzzle.up * distance;
        var positions = new[] { start, end };

        laser.line.SetPositions(positions);
    }

    private IEnumerator FireLaser()
    {
        _isFiring = true;

        do
        {
            var hits = Physics2D.RaycastAll(muzzle.transform.position, muzzle.up, weaponLaser.length);

            // filter out the ship firing the weapon
            hits = hits
                .Where(h => h.collider.gameObject != _ship)
                .ToArray();

            hits = hits
                .OrderBy(h => h.distance)
                .ToArray();

            if (!hits.Any())
            {
                DrawLaser(weaponLaser);
            }
            else
            {
                DrawLaser(weaponLaser, hits[0].distance);
            }

            yield return new WaitForFixedUpdate();

        } while (_input.GetFiring());

        weaponLaser.line.positionCount = 0;
        _isFiring = false;
    }
}
