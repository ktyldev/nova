using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMesh : MonoBehaviour
{
    public Material material;

    private LightSource _source;
    private Mesh _mesh;
    private IComparer<LightRay> _comparer = new LightRayAngleComparer();


    public void CreateMesh(LightSource source)
    {
        _source = source;

        var go = gameObject;
        var renderer = go.AddComponent<MeshRenderer>();
        renderer.materials = new[] { material };

        var filter = go.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();

        _mesh = filter.mesh;
    }

    private void FixedUpdate()
    {
        if (_source == null)
            return;

        if (_source.drawIlluminations)
        {
            DrawIllumination();
        }

        if (_source.drawShadows)
        {
            DrawShadow();
        }
    }

    public bool ContainsPoint(Vector2 point)
    {
        for (int i = 0; i < _mesh.triangles.Length; i += 3)
        {
            Vector2[] poly = new Vector2[3];
            for (int j = 0; j < 3; j++)
            {
                poly[j] = _mesh.vertices[_mesh.triangles[j]];
            }

            if (IsPointInPolygon(point, poly))
                return true;
        }

        return false;
    }

    // https://codereview.stackexchange.com/a/108903
    private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int polyLength = polygon.Length;
        int i = 0;
        bool inside = false;

        // x, y for tested point
        float pointX = point.x;
        float pointY = point.y;
        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polyLength - 1];
        endX = endPoint.x;
        endY = endPoint.y;

        while (i < polyLength)
        {
            startX = endX;
            startY = endY;

            endPoint = polygon[i++];

            endX = endPoint.x;
            endY = endPoint.y;

            inside ^= (endY > pointY ^ startY > pointY)
                &&
                ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }

        return inside;
    }

    private void DrawIllumination()
    {
        RaycastHit2D[] hits = null;
        _source.GetHits(ref hits);
        if (hits.Length == 0)
            return;

        // pre-calc some values
        int rayCount = 4 * hits.Length;         // 4 rays per hit
        int vertCount = rayCount + 1;           // 1 vert per ray, +1 for centre
        var rays = new LightRay[rayCount];
        var verts = new Vector3[vertCount];
        var uv = new Vector2[verts.Length];
        var indices = new int[3 * rayCount];

        verts[0] = _source.Position;

        int ri = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            var collider = hits[i].collider as CircleCollider2D;
            _source.GetRays(ref rays, ri, collider);
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

        _mesh.Clear();
        _mesh.vertices = verts;
        _mesh.triangles = indices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        var size = _source.range * 2;
        for (int i = 0; i < verts.Length; i++)
        {
            var tv = (Vector2)verts[i] - _source.Position;

            uv[i].x = tv.x / size;
            uv[i].y = tv.y / size;
        }
        _mesh.uv = uv;
    }

    private void DrawShadow()
    {
        RaycastHit2D[] hits = null;
        _source.GetHits(ref hits);
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
            var collider = hits[i].collider as CircleCollider2D;
            _source.GetShadowPositions(ref verts, vi, collider);

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

        _mesh.Clear();
        _mesh.vertices = verts;
        _mesh.triangles = indices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        var size = _source.range * 2;
        for (int i = 0; i < verts.Length; i++)
        {
            var tv = (Vector2)verts[i] - _source.Position;

            uv[i].x = tv.x / size;
            uv[i].y = tv.y / size;
        }
        _mesh.uv = uv;
    }

    private void OnDestroy()
    {
        _mesh.Clear();
    }
}
