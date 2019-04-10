using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 1f;
    // seconds before the bullet destroys itself
    // TODO: pool bullets, more performant that way
    public float life = 3f;

    private Rigidbody2D _rb;

    // TODO: this is a cheap hack
    public Ship ignore;

    void Start()
    {
        transform.SetParent(Game.Instance.bulletParent);

        Destroy(gameObject, life);

        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var go = collision.gameObject;

        // don't hit player that fired the bullet
        var ship = go.GetComponent<Ship>();
        if (ship == ignore)
            return;

        var health = go.GetComponent<Health>();
        if (health == null)
            return;

        health.TakeDamage(damage);
    }
}
