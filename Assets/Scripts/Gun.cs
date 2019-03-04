using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float range;
    public Transform emitter;
    public GameObject bulletPrefab;
    public float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    //spawning the bullets
    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, emitter.position, emitter.rotation);
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }
}
