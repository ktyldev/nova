using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    // TODO:
    // aim weapons
    // (weapons will probably follow the mouse more closely)
    // is shooting

    // TODO: 
    // make movement more sophisticated. it should be faster for the ship
    // to accelerate forwards, not equal in every direction
    public float acceleration;
    [Range(0, 1)]
    public float rotationSpeed;
    public GameObject[] weapons;
    [Range(0, 1)]
    public float weaponRotationSpeed;

    private Rigidbody2D _rb;
    private PlayerInput _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>(); 
    }

    private void Start()
    {
        _input = Game.Instance.PlayerInput;
    }

    private void FixedUpdate()
    {
        Movement();
        RotateShip();
        // aim weapons
        RotateWeapons();
    }

    private void Movement()
    {
        _rb.AddForce(_input.GetMovement() * acceleration);
    }

    private void RotateShip()
    {
        var aimDir = _input.GetAimDirection();
        var lookDir = Vector3.Lerp(transform.up, aimDir, rotationSpeed);

        transform.rotation *= Quaternion.FromToRotation(transform.up, lookDir);
    }

    private void RotateWeapons()
    {
        var targetPos = _input.GetMousePosition();

        foreach (var w in weapons)
        {
            var aimDir = _input.GetAimDirection(w.transform.position);

            var target = Quaternion.LookRotation(transform.forward, aimDir);

            w.transform.rotation = Quaternion.Lerp(w.transform.rotation, target, weaponRotationSpeed);
        }
    }
}
