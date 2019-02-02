using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // TODO:
    // move dir
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
    // the direction the player is trying to move
    private Vector2 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotate();
        // aim weapons
    }

    private void Movement()
    {
        var input = new Vector2(
            Input.GetAxis(GameConstants.Horizontal), 
            Input.GetAxis(GameConstants.Vertical));

        _rb.AddForce(input * acceleration);
    }

    private void Rotate()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // unity's 2D is dumb
        mousePos.z = 0;

        var aimDir = (mousePos - transform.position).normalized;
        var lookDir = Vector3.Lerp(transform.up, aimDir, rotationSpeed);

        transform.rotation *= Quaternion.FromToRotation(transform.up, lookDir);
    }
}