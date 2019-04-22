using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid : MonoBehaviour
{
    public static float SafeDistance = 5.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        if (health == null)
            return;

        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship == null)
        {
            health.Die();
            return;
        }

        var maxSpeed = ship.Movement.acceleration;
        var dmg = Mathf.Sqrt(health.max / maxSpeed);
        health.TakeDamage(dmg);
    }
}
