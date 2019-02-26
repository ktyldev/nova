using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MapGeneration : NetworkBehaviour
{
    public Transform asteroidsParent;
    public Transform asteroidsParent1;
    public Transform asteroidsParent2;
    public Transform asteroidsParent3;
    public float maxToGenerate;
    public Bounds mapBounds;
    public Bounds sizeBounds;
    public float minSeperation;

    private GameObject _asteroid;
    private GameObject _asteroid1;
    private GameObject _asteroid2;
    private GameObject _asteroid3;

    private void Start()
    {
        _asteroid  = Game.Instance.asteroid;
        _asteroid1 = Game.Instance.asteroid1;
        _asteroid2 = Game.Instance.asteroid2;
        _asteroid3 = Game.Instance.asteroid3;
        // only spawn asteroids on the host
        if (!isServer)
            return;

        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        yield return new WaitUntil(() => NetworkServer.connections.Count == Game.Instance.Players);
        GenerateAsteroids();
    }

    private void GenerateAsteroids()
    {
        var asteroids  = new List<GameObject>();
        var asteroids1 = new List<GameObject>();
        var asteroids2 = new List<GameObject>();
        var asteroids3 = new List<GameObject>();

        for (int i = 0; i < maxToGenerate; i++)
        {
            float x  = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float y  = Random.Range(mapBounds.min.y, mapBounds.max.y);
            float x1 = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float y1 = Random.Range(mapBounds.min.y, mapBounds.max.y);
            float x2 = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float y2 = Random.Range(mapBounds.min.y, mapBounds.max.y);
            float x3 = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float y3 = Random.Range(mapBounds.min.y, mapBounds.max.y);
            float xs = Random.Range(sizeBounds.min.x, sizeBounds.max.x);
            float ys = Random.Range(sizeBounds.min.y, sizeBounds.max.y);

            var pos  = new Vector3(x, y, 0);
            var pos1 = new Vector3(x1, y1, 0);
            var pos2 = new Vector3(x2, y2, 0);
            var pos3 = new Vector3(x3, y3, 0);

            if (asteroids.Any() &&
                asteroids.Any(a => Vector3.Distance(a.transform.position, pos) < minSeperation) &&
                asteroids1.Any() &&
                asteroids1.Any(b => Vector3.Distance(b.transform.position, pos1) < minSeperation) &&
                asteroids2.Any() &&
                asteroids2.Any(c => Vector3.Distance(c.transform.position, pos2) < minSeperation) &&
                asteroids3.Any() &&
                asteroids3.Any(d => Vector3.Distance(d.transform.position, pos3) < minSeperation))
                continue;

            var newAsteroid = Instantiate(_asteroid, pos, Quaternion.identity, asteroidsParent);
            NetworkServer.Spawn(newAsteroid);
            asteroids.Add(newAsteroid);
            newAsteroid.transform.localScale += new Vector3(xs, xs, 0);

            var newAsteroid1 = Instantiate(_asteroid1, pos1, Quaternion.identity, asteroidsParent1);
            NetworkServer.Spawn(newAsteroid1);
            asteroids1.Add(newAsteroid1);
            newAsteroid1.transform.localScale += new Vector3(xs, xs, 0);

            var newAsteroid2 = Instantiate(_asteroid2, pos2, Quaternion.identity, asteroidsParent2);
            NetworkServer.Spawn(newAsteroid2);
            asteroids2.Add(newAsteroid2);
            newAsteroid2.transform.localScale += new Vector3(xs, xs, 0);

            var newAsteroid3 = Instantiate(_asteroid3, pos3, Quaternion.identity, asteroidsParent3);
            NetworkServer.Spawn(newAsteroid3);
            asteroids3.Add(newAsteroid3);
            newAsteroid3.transform.localScale += new Vector3(xs, xs, 0);
        }
    }

}
