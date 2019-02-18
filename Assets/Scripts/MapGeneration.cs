using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MapGeneration : NetworkBehaviour
{
    public GameObject asteroid;
    public Transform asteroidsParent;
    public float maxToGenerate;
    public Bounds mapBounds;
    public float minSeperation;


    private void Start()
    {
        // only spawn asteroids on the host
        if (!isServer)
            return;

        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        yield return new WaitUntil(() => NetworkServer.connections.Count == 2);
        GenerateAsteroids();
    }

    private void GenerateAsteroids()
    {
        var asteroids = new List<GameObject>();

        for (int i = 0; i < maxToGenerate; i++)
        {
            float x = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float y = Random.Range(mapBounds.min.y, mapBounds.max.y);

            var pos = new Vector3(x, y, 0);

            if (asteroids.Any() &&
                asteroids.Any(a => Vector3.Distance(a.transform.position, pos) < minSeperation))
                continue;

            var newAsteroid = Instantiate(asteroid, pos, Quaternion.identity, asteroidsParent);
            NetworkServer.Spawn(newAsteroid);

            asteroids.Add(newAsteroid);
        }
    }

}
