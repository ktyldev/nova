using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        if (health == null)
            return;

        health.Die();
    }
}
