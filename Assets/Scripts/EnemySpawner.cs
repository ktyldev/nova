using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnDelay = 2;

    private LightSource _source;

    // Start is called before the first frame update
    void Start()
    {
        _source = Game.Instance.ship.GetComponentInChildren<LightSource>();

        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemies()
    {
        // magic number idgaf lul
        yield return new WaitForSeconds(1);
        while (true)
        {
            var pos = GetSpawnLocation();
            if (pos.HasValue)
            {
                Instantiate(enemy, pos.Value, Quaternion.identity, transform);
                //yield return new WaitUntil(() => false);
                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private Vector2? GetSpawnLocation()
    {
        if (_source == null)
            return null;

        var bounds = new Bounds(_source.Position, Vector3.one * _source.range);
        var lightMesh = LightEngine.Instance[_source];
        if (lightMesh == null)
            return null;

        Vector2 point;
        bool done;
        do
        {
            point = new Vector2
            {
                x = Random.Range(bounds.center.x - bounds.max.x, bounds.center.x + bounds.max.x),
                y = Random.Range(bounds.center.y - bounds.max.y, bounds.center.y + bounds.max.y)
            };

            done = lightMesh.ContainsPoint(point);
        } while (!done);

        return point;
    }
}
