using System.Collections.Generic;
using UnityEngine;

public class LightEngine : MonoBehaviour
{
    public Transform meshParent;
    public Material lightMat;

    public static LightEngine Instance { get; private set; }

    private int _mask;

    private Dictionary<LightSource, Mesh> _sources
        = new Dictionary<LightSource, Mesh>();

    public static void Register(LightSource source)
    {
        Instance.RegisterSource(source);
    }

    protected void RegisterSource(LightSource source)
    {
        var go = new GameObject();

        var f = go.AddComponent<MeshFilter>();
        f.mesh = new Mesh();

        var r = go.AddComponent<MeshRenderer>();
        r.materials = new[] { lightMat };

        _sources.Add(source, f.mesh);
    }

    void Start()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
        _mask = LayerMask.GetMask("Asteroid");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        foreach (var s in _sources)
        {
            UpdateMesh(s.Key);
        }
    }

    private void UpdateMesh(LightSource source)
    {
        var lsPos = (Vector2)source.transform.position;

        var hits = Physics2D.CircleCastAll(
            lsPos,
            source.range,
            Vector2.zero,
            source.range,
            _mask);

        var verts = new List<Vector3> { lsPos };
        for (int i = 0; i < hits.Length; i++)
        {
            var collider = hits[i].collider as PolygonCollider2D;
            var edges = source.FindEdges(collider);

            var m = 0.02f;
            var rotLeft = Quaternion.Euler(0, 0, m);
            var rotRight = Quaternion.Euler(0, 0, -m);

            var dLeft = edges.left.worldPosition - lsPos;
            var dLeftInner = rotRight * dLeft;
            var dLeftOuter = rotLeft * dLeft;

            var dRight = edges.right.worldPosition - lsPos;
            var dRightInner = rotLeft * dRight;
            var dRightOuter = rotRight * dRight;

            var innerL = source.Raycast(dLeftInner, Color.cyan);
            var innerR = source.Raycast(dRightInner, Color.magenta);

            verts.Add(innerL);
            verts.Add(innerR);

            // TODO: handle outer rays
        }

        var indices = new List<int>();

        for (int i = 1; i <= verts.Count - 1; i+=2)
        {
            indices.Add(i);
            indices.Add(i + 1);
            indices.Add(0);
        }

        var mesh = _sources[source];
        mesh.Clear();

        mesh.vertices = verts.ToArray();
        mesh.triangles = indices.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var bounds = new Bounds(Vector3.zero, new Vector3(source.range, source.range));
        var uv = new List<Vector2>();
        foreach (var v in verts)
        {
            var tv = (Vector2)v - lsPos;

            uv.Add(new Vector2(tv.x / bounds.size.x, tv.y / bounds.size.y));
        }
        mesh.uv = uv.ToArray();
    }
}
