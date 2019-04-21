using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration Instance { get; private set; }

    public AsteroidsInfo asteroids;
    public ChunksInfo chunks;
    public float minScale, maxScale;
    public float centreRadius;

    [System.Serializable]
    public struct AsteroidsInfo
    {
        public GameObject template;
        public Sprite[] sprites;
        public Transform parent;
    }

    [System.Serializable]
    public struct ChunksInfo
    {
        public int sideLength;
        public int maxAsteroids;
    }

    private Dictionary<Vector2Int, Chunk> _chunks
        = new Dictionary<Vector2Int, Chunk>();

    private List<GameObject> _spawned
        = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }

    private void Start()
    {

        //GenerateAsteroids();
        StartCoroutine(UpdateChunks());
    }

    private void SpawnNeighbourChunks()
    {
        var playerPos = Game.Instance.Ship.transform.position;
        var playerChunk = GetPlayerChunk();

        print("player in chunk: " + playerChunk.Coords);

        var toSpawn = new List<Vector2Int>();
        foreach (var nPos in Chunk.GetNeighbourPositions(playerChunk.Coords))
        {
            if (!_chunks.ContainsKey(nPos))
            {
                toSpawn.Add(nPos);
            }
        }

        foreach (var pos in toSpawn)
        {
            SpawnChunk(pos);
        }
    }

    private void CullColdChunks()
    {
        var hot = new List<Vector2Int>();
        hot.Add(GetPlayerChunk().Coords);
        hot.AddRange(GetPlayerNeighbourPositions());

        var toClear = new List<Vector2Int>();

        foreach (var pos in _chunks.Keys)
        {
            if (!hot.Contains(pos))
            {
                toClear.Add(pos);
            } 
        }

        foreach (var pos in toClear)
        {
            _chunks[pos].Clear();
            _chunks.Remove(pos);
        }
    }


    private IEnumerator UpdateChunks()
    {
        // spawn first chunk
        SpawnChunk(Vector2Int.zero);

        while (true)
        {
            SpawnNeighbourChunks();
            CullColdChunks();
            
            yield return new WaitForSeconds(2);
        }
    }

    private Vector2Int[] GetPlayerNeighbourPositions() => 
        Chunk.GetNeighbourPositions(GetPlayerChunk().Coords);

    private Chunk GetPlayerChunk()
    {
        var playerPos = Game.Instance.Ship.transform.position;
        foreach (var e in _chunks)
        {
            if (e.Value.Bounds.Contains(playerPos))
                return e.Value;
        }

        throw new System.Exception();
    }

    private void SpawnChunk(Vector2Int pos)
    {
        if (_chunks.ContainsKey(pos))
            return;

        _chunks[pos] = new Chunk(pos, CreateAsteroid);
        _chunks[pos].Generate();
    }

    private void RemoveChunk(Vector2Int pos)
    {
        
    }

    private Asteroid CreateAsteroid()
    {
        float size = Random.Range(minScale, maxScale);
        int s = Random.Range(0, asteroids.sprites.Length);

        var ast = Instantiate(asteroids.template);
        ast.transform.SetParent(asteroids.parent);
        ast.transform.localScale = new Vector3(size, size);
        ast.transform.Rotate(0, 0, Random.Range(0, 360));

        var renderer = ast.GetComponent<SpriteRenderer>();
        renderer.sprite = asteroids.sprites[s];

        return ast.GetComponent<Asteroid>();
    }
}

// static
public partial class Chunk
{
    private static readonly Vector2Int[] Directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.right,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.down,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left,
        Vector2Int.left + Vector2Int.up
    };

    private static int _sideLength => MapGeneration.Instance.chunks.sideLength;
    private static int _maxAsteroids => MapGeneration.Instance.chunks.maxAsteroids;
    public static Vector2Int[] GetNeighbourPositions(Vector2Int coords) =>
        Directions.Select(d => d + coords).ToArray();
    public static Vector2 GetChunkWorldPosition(Vector2Int pos) =>
        pos * _sideLength;
}

// non-static
public partial class Chunk
{
    private Vector2 _origin;
    private System.Func<Asteroid> _createAsteroid;
    private List<Asteroid> _asteroids = new List<Asteroid>();

    public Bounds Bounds { get; private set; }
    public Vector2Int Coords { get; private set; }

    public Chunk(Vector2 origin, System.Func<Asteroid> createAsteroid)
    {
        _origin = origin;
        _createAsteroid = createAsteroid;
        Coords = new Vector2Int(
            Mathf.RoundToInt(origin.x),
            Mathf.RoundToInt(origin.y));

        var worldPosition = Chunk.GetChunkWorldPosition(Coords);
        Bounds = new Bounds(worldPosition, new Vector2(1, 1) * _sideLength);
    }

    public void Generate()
    {
        for (int i = 0; i < _maxAsteroids; i++)
        {
            // spawn an asteroid
            var asteroid = _createAsteroid.Invoke();
            // get random position in bounds and put the asteroid there
            asteroid.transform.position = GetAsteroidSpawnPos();
            _asteroids.Add(asteroid);
        }
    }

    private Vector2 GetAsteroidSpawnPos()
    {
        bool posOk = false;
        Vector2 pos;

        do
        {
            var x = Random.Range(Bounds.min.x, Bounds.max.x);
            var y = Random.Range(Bounds.min.y, Bounds.max.y);
            pos = new Vector2(x, y);

            if (Mathf.Abs(pos.magnitude) > Asteroid.SafeDistance)
            {
                posOk = true;
            }

        } while (!posOk);

        return pos;
    }

    public void Clear()
    {
        foreach (var asteroid in _asteroids)
        {
            Object.Destroy(asteroid.gameObject);
        }
    }
}
