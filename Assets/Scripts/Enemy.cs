using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float rotationSpeed;

    private Health _health;
    private Transform _target;

    void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _target = Game.Instance.ship.transform;        
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        var toTarget = _target.position - transform.position;
        var cross = Vector3.Cross(toTarget, transform.up);

        if (cross.z > 0)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
