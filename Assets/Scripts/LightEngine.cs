using System.Collections.Generic;
using UnityEngine;

public class LightEngine : MonoBehaviour
{
    public Transform illumination;
    public Transform shadows;

    public Material lightMat;
    public Material shadowMat;

    // angle by which to miss edges of objects while raycasting past them
    public float edgeMiss = 0.02f;
    // TODO
    // how many vertices make up the circle around the edge of light sources
    //public int circlePoints = 50;

    public static LightEngine Instance { get; private set; }

    private IComparer<LightRay> _comparer;

    // areas illuminated by particular light sources
    private Dictionary<LightSource, Mesh> _illuminations
        = new Dictionary<LightSource, Mesh>();
    private Dictionary<LightSource, Mesh> _shadows
        = new Dictionary<LightSource, Mesh>();

    public static void Register(LightSource source)
    {
        Instance.RegisterSource(source);
    }

    protected void RegisterSource(LightSource source)
    {
        GameObject go;
        MeshFilter filter;
        MeshRenderer renderer;

        go = new GameObject();
        go.transform.SetParent(illumination);
        go.transform.localPosition = Vector3.zero;

        filter = go.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();

        renderer = go.AddComponent<MeshRenderer>();
        renderer.materials = new[] { lightMat };

        _illuminations.Add(source, filter.mesh);

        // reassign the things
        go = new GameObject();
        go.transform.SetParent(shadows);
        go.transform.localPosition = Vector3.zero;

        filter = go.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();

        renderer = go.AddComponent<MeshRenderer>();
        renderer.materials = new[] { shadowMat };

        _shadows.Add(source, filter.mesh);
    }

    void Start()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
        _comparer = new LightRayAngleComparer();
    }

    private void FixedUpdate()
    {
        foreach (var s in _illuminations)
        {
            if (s.Key.drawIlluminations)
            {
                DrawIlluminationMesh(s.Key);
            }

            if (s.Key.drawShadows)
            {
                DrawShadowMesh(s.Key);
            }
        }
    }

    private void DrawIlluminationMesh(LightSource source)
    {
        RaycastHit2D[] hits = null;
        source.GetHits(ref hits);
        if (hits.Length == 0)
            return;

        // pre-calc some values
        int rayCount = 4 * hits.Length;         // 4 rays per hit
        int vertCount = rayCount + 1;           // 1 vert per ray, +1 for centre
        var rays = new LightRay[rayCount];
        var verts = new Vector3[vertCount];
        var uv = new Vector2[verts.Length];
        var indices = new int[3 * rayCount];

        verts[0] = source.Position;

        int ri = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            var collider = hits[i].collider as PolygonCollider2D;
            source.GetRays(ref rays, ri, collider);
            ri += 4;
        }

        System.Array.Sort(rays, _comparer);

        for (int i = 0; i < rays.Length; i++)
        {
            verts[i + 1] = rays[i].end;
        }

        // verts[0] is mesh centre
        for (int i = 0; i < rayCount - 1; i++)
        {
            int ti = 3 * i;

            indices[ti] = 0;
            indices[ti + 1] = i + 1;
            indices[ti + 2] = i + 2;
        }
        var li = indices.Length;
        indices[li - 3] = 0;
        indices[li - 2] = vertCount - 1;
        indices[li - 1] = 1;

        var mesh = _illuminations[source];
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        var size = source.range * 2;
        for (int i = 0; i < verts.Length; i++)
        {
            var tv = (Vector2)verts[i] - source.Position;

            uv[i].x = tv.x / size;
            uv[i].y = tv.y / size;
        }
        mesh.uv = uv;
    }

    private void DrawShadowMesh(LightSource source)
    {
        RaycastHit2D[] hits = null;
        source.GetHits(ref hits);
        if (hits.Length == 0)
            return;

        int vertCount = hits.Length * 4;
        int indexCount = hits.Length * 6;
        Vector3[] verts = new Vector3[vertCount];
        int[] indices = new int[indexCount];
        var uv = new Vector2[verts.Length];

        int vi = 0;
        int ii = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            // find edges
            var collider = hits[i].collider as PolygonCollider2D;
            source.GetShadowPositions(ref verts, vi, collider);

            // make two tris
            indices[ii] = vi;
            indices[ii + 1] = vi + 1;
            indices[ii + 2] = vi + 2;
            indices[ii + 3] = vi + 1;
            indices[ii + 4] = vi + 2;
            indices[ii + 5] = vi + 3;

            vi += 4;
            ii += 6;
        }

        var mesh = _shadows[source];
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        var size = source.range * 2;
        for (int i = 0; i < verts.Length; i++)
        {
            var tv = (Vector2)verts[i] - source.Position;

            uv[i].x = tv.x / size;
            uv[i].y = tv.y / size;
        }
        mesh.uv = uv;
    }
}

public class LightRayAngleComparer : IComparer<LightRay>
{
    public int Compare(LightRay x, LightRay y) => x.angle.CompareTo(y.angle);
}

