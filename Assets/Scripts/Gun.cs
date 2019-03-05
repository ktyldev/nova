using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform emitter;
    public GameObject bulletPrefab;

    // TODO: cheap hack lol
    private Ship _ship;

    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        if (_ship == null)
            throw new System.Exception();
    }

    //spawning the bullets
    public void Fire()
    {
        var bullet = Instantiate(
            bulletPrefab, 
            emitter.position, 
            emitter.rotation)
            .GetComponent<Bullet>();

        // cheap hack lol
        bullet.ignore = _ship;
    }
}
