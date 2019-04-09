using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MapGeneration : MonoBehaviour
{
    [System.Serializable]
    public struct AsteroidsInfo
    {
        public GameObject template;
        public Sprite[] sprites;
        public Transform parent;
    }

    public AsteroidsInfo asteroids;
    public float maxToGenerate;
    public Bounds mapBounds;
    public float minScale, maxScale;
    public float minSeperation;

    private List<GameObject> _spawned;

    private void Start()
    {
        GenerateAsteroids();
    }

    private void GenerateAsteroids()
    {
        _spawned = new List<GameObject>();

        for (int i = 0; i < maxToGenerate; i++)
        {
            float x  = Random.Range(mapBounds.min.x, mapBounds.max.x);
            float y  = Random.Range(mapBounds.min.y, mapBounds.max.y);
            var pos  = new Vector3(x, y, 0);
            float size = Random.Range(minScale, maxScale);
            int s = Random.Range(0, asteroids.sprites.Length);

            if (_spawned.Any() &&
                _spawned.Any(a => Vector3.Distance(a.transform.position, pos) < minSeperation))
                continue;

            var ast = Instantiate(
                asteroids.template, 
                pos, 
                Quaternion.identity, 
                asteroids.parent);
            ast.transform.localScale = new Vector3(size, size);

            var renderer = ast.GetComponent<SpriteRenderer>();
            renderer.sprite = asteroids.sprites[s];

            // update collider since the sprite has changed
            ast.AddComponent<PolygonCollider2D>();

            _spawned.Add(ast);
        }
    }
}
