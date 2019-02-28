using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float range;

    public Vector2 Position => transform.position;

    private int _mask;

    private void Awake()
    {
        _mask = LayerMask.GetMask(GameConstants.Asteroid);
    }

    void Start()
    {
        LightEngine.Register(this);
    }

    private float GetAngle(Vector2 p1, Vector2 p2)
    {
        return Vector2.SignedAngle(p1, p2);
    }

    public LightRay Raycast(Vector2 dir)
    {
        dir.Normalize();

        var raycastHit = Physics2D.Raycast(transform.position, dir, range, _mask);
        bool hit = raycastHit.collider != null;

        var v = hit ? dir * raycastHit.distance : dir * range;
        var end = (Vector2)transform.position + v;

        return new LightRay
        {
            angle = Vector2.SignedAngle(Vector2.up, dir) + 180,
            end = end,
            hit = hit
        };
    }

    public LightRay Raycast(Vector2 dir, Color colour)
    {
        var ray = Raycast(dir);
        Debug.DrawLine(transform.position, ray.end, colour);
        return ray;
    }

    // TODO: possibly reduces the number of points to sample from the collider?
    public HitObject FindEdges(PolygonCollider2D collider)
    {
        var toCentre = collider.transform.position - transform.position;
        var colliderPos = (Vector2)collider.transform.position;

        HitVertex v;
        var o = new HitObject();

        for (int i = 0; i < collider.points.Length; i++)
        {
            var worldPos = colliderPos + collider.points[i] * collider.transform.localScale;
            var toVert = worldPos - (Vector2)transform.position;

            var angleFromCentre = Vector2.SignedAngle(toCentre.normalized, toVert.normalized);

            v = new HitVertex
            {
                worldPosition = worldPos,
                angleFromCentre = angleFromCentre
            };

            if (v.angleFromCentre < o.right.angleFromCentre)
            {
                o.right = v;
            }
            else if (v.angleFromCentre >= o.left.angleFromCentre)
            {
                o.left = v;
            }
        }

        return o;
    }
}

public struct LightRay
{
    public float angle;
    public Vector2 end;
    public bool hit;
}

public struct HitObject
{
    public HitVertex left;
    public HitVertex right;
}

public struct HitVertex
{
    public Vector2 worldPosition;
    public float angleFromCentre;
}
