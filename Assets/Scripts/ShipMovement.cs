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
        Rotate();
        // aim weapons
    }

    private void Movement()
    {
        _rb.AddForce(_input.GetMovement() * acceleration);
    }

    private void Rotate()
    {
        var aimDir = _input.GetAimDirection();
        var lookDir = Vector3.Lerp(transform.up, aimDir, rotationSpeed);

        transform.rotation *= Quaternion.FromToRotation(transform.up, lookDir);
    }
}
