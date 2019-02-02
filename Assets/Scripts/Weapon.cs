using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        _input = Game.Instance.PlayerInput;
    }

    void Update()
    {
        if (_input.GetStartFiring())
        {
            StartCoroutine(FireLaser());
        }
    }

    private void LateUpdate()
    {
        DrawLasers();
    }

    private void DrawLasers()
    {
        weaponLaser.line.positionCount = 0;
        targetingLaser.line.positionCount = 0;

        if (_isFiring)
        {
            DrawLaser(weaponLaser);
        }
        else
        {
            DrawLaser(targetingLaser);
        }
    }

    private void DrawLaser(Laser laser)
    {
        laser.line.positionCount = 2;

        var start = muzzle.transform.position;
        var end = start + muzzle.up * laser.length;
        var positions = new[] { start, end };

        laser.line.SetPositions(positions);
    }

    private IEnumerator FireLaser()
    {
        _isFiring = true;
        yield return new WaitWhile(() => _input.GetFiring());
        _isFiring = false;
    }
}
