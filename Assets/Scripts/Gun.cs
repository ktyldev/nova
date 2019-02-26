using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float range;
    public Transform emitter;
    public GameObject bulletPrefab;
    public float speed = 20f;
    public Rigidbody2D rb;

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
        Instantiate(bulletPrefab, emitter.position, emitter.rotation);
        rb.velocity = transform.right * speed;
        Debug.Log("it fired");
    }

    //when bullet hits, gets destroyed
    void OnTriggerEnter2D()
    {
        Destroy(gameObject);
    }
}
