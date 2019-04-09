using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var ship = collision.collider.GetComponent<Ship>();
        if (ship == null)
            return;

        ship.Die();
    }
}
