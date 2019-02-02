using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // TODO:
    // aim dir
    // move dir
    // is shooting

    public float rotationSpeed;

    private Rigidbody2D _rb;
    // the direction the player is trying to move
    private Vector2 _moveInput;
    // the direction the player is trying to aim
    // weapons will probably follow this more closely
    private Vector2 _aimDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // move
        // aim
        Rotate();
    }

    private void MoveInput()
    {
    }

    private void Rotate()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        print(mousePos);

        var aimDir = (mousePos - transform.position).normalized;
        var lookDir = Vector3.Lerp(transform.up, aimDir, rotationSpeed);

        transform.rotation *= Quaternion.FromToRotation(transform.up, lookDir);
    }
}