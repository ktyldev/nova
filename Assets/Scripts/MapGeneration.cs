using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public float centreRadius;

    private List<GameObject> _spawned = new List<GameObject>();

    private void Start()
    {
        GenerateAsteroids();
    }

    private Vector3 SpawnPosition
    {
        get
        {
            Vector3 pos = Vector3.zero;
            do
            {
                float x = Random.Range(mapBounds.min.x, mapBounds.max.x);
                float y = Random.Range(mapBounds.min.y, mapBounds.max.y);

                // bad position, skip it
                pos = new Vector3(x, y, 0);
                if (pos.magnitude < centreRadius)
                    continue;

                if (_spawned.Any(a => Vector3.Distance(a.transform.position, pos) < minSeperation))
                    continue;

            } while (pos == Vector3.zero);

            return pos;
        }
    }

    private void GenerateAsteroids()
    {
        for (int i = 0; i < maxToGenerate; i++)
        {

            float size = Random.Range(minScale, maxScale);
            int s = Random.Range(0, asteroids.sprites.Length);

            var ast = Instantiate(asteroids.template);
            ast.transform.SetParent(asteroids.parent);
            ast.transform.localPosition = SpawnPosition;
            ast.transform.localScale = new Vector3(size, size);
            ast.transform.Rotate(0, 0, Random.Range(0, 360));

            var renderer = ast.GetComponent<SpriteRenderer>();
            renderer.sprite = asteroids.sprites[s];

            _spawned.Add(ast);
        }
    }
}
