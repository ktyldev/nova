using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float restoreAmount = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship == null)
            return;

        var health = ship.GetComponent<Health>();
        if (health == null)
            throw new System.Exception();

        health.TakeDamage(-restoreAmount);

        Destroy(gameObject);
    }
}
