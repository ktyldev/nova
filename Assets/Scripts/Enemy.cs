using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float rotationSpeed;

    private Health _health;
    private Transform _target;
    private LightSource _source;
    private Rigidbody2D _rb;

    private bool _hidden = true;

    void Awake()
    {
        _health = GetComponent<Health>();
        _health.death.AddListener(Explode);

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        var ship = Game.Instance.Ship;

        _target = ship.transform;
        _source = ship.LightSource;

        StartCoroutine(WaitInShadow());
    }

    private IEnumerator WaitInShadow()
    {
        while (_hidden)
        {
            _hidden = LightEngine.Instance[_source].ContainsPoint(transform.position); 
            yield return new WaitForSeconds(0.5f);
        }

        

        print("no longer hidden!");
    }

    private void FixedUpdate()
    {
        if (_hidden)
            return;

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

        var dir = _target.position - transform.position;
        _rb.AddForce(dir);
    }

    private void Explode()
    {
        Explosion.New(Game.Instance.shipExplosion, transform.position);
        Destroy(gameObject);
    }
}
