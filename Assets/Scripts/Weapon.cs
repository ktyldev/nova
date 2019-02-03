using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;

    [Header("Lasers")]
    public GameObject weaponLaser;
    public GameObject targeting;

    private PlayerInput _input;
    private bool _isFiring;
    // gotta ignore this when firin lazors
    private GameObject _ship;

    private Laser _laser;
    private Laser _targeting;

    private void Start()
    {
        _input = Game.Instance.PlayerInput;
        _ship = GetComponentInParent<ShipMovement>().gameObject;

        _laser = weaponLaser.GetComponent<Laser>();
        _targeting = targeting.GetComponent<Laser>();
    }

    void Update()
    {
        if (_input.GetStartFiring())
        {
            StartCoroutine(FireLaser());
        }
    }

    private IEnumerator FireLaser()
    {
        _targeting.SetActive(false);
        _laser.SetActive(true);

        yield return new WaitWhile(() => _input.GetFiring());

        _targeting.SetActive(true);
        _laser.SetActive(false);
    }
}
