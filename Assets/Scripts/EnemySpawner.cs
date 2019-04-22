using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public int maxEnemies; 
    public float spawnRate = 2;
    public float waveMultiplier;
    public int waveLength = 10;

    private float _spawnDelay => 1.0f / spawnRate;
    private int _spawned = 0;
    private int _current = 0;
    private LightSource _source;

    void Start()
    {
        _source = Game.Instance.Ship.GetComponentInChildren<LightSource>();

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        // magic number idgaf lul
        //yield return new WaitForSeconds(1);

        while (true)
        {
            yield return new WaitUntil(() => Enemy.All.Count() < maxEnemies);

            var pos = GetSpawnLocation();
            if (pos.HasValue)
            {
                Instantiate(enemy, pos.Value, Quaternion.identity, transform);

                _spawned++;
                if (_spawned % waveLength == 0)
                {
                    spawnRate *= waveMultiplier;
                }
            }

            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private Vector2? GetSpawnLocation()
    {
        if (_source == null)
            return null;

        var asteroid = GetNearbyAsteroid();
        if (asteroid == null)
            return null;

        var astPos = asteroid.transform.position;
        var srcPos = _source.transform.position;

        var dir = (astPos - srcPos).normalized;
        var spawnPos = astPos + dir * Asteroid.SafeDistance;

        return spawnPos;
    }

    private Asteroid GetNearbyAsteroid()
    {
        if (!Chunk.ActiveAsteroids.Any())
            return null;

        var asteroids = Chunk.ActiveAsteroids
            .Where(a => Vector2.Distance(
                Game.Instance.Ship.transform.position, 
                a.transform.position) < _source.range)
            .ToArray();
        if (!asteroids.Any())
            return null;

        return asteroids[Random.Range(0, asteroids.Length)];
    }
}
