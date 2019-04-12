using UnityEngine;

[RequireComponent(typeof(Ship))]
public class ShipMovement : MonoBehaviour
{
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
    private IInputProvider _input;
    private Ship _ship;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _ship = GetComponent<Ship>();
        _input = _ship.InputProvider;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void OnGUI()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 5); 
    }

    private void Move()
    {
        _rb.AddForce(_input.AccelerationDir * acceleration);
    }

    private void Rotate()
    {
        var aimDir = (_input.TargetPosition - (Vector2)transform.position).normalized;
        var lookDir = Vector3.Lerp(transform.up, aimDir, rotationSpeed);

        transform.rotation *= Quaternion.FromToRotation(transform.up, lookDir);
    }
}
