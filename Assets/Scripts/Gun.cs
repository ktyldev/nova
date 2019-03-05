using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float range;
    public Transform emitter;
    public GameObject bulletPrefab;
    public float speed = 20f;
    public bool bulletHit = false;

    //spawning the bullets
    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, emitter.position, emitter.rotation);
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D()
    {
        Destroy(gameObject);
        bulletHit = true;
    }
}
