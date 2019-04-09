using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float range;
    public bool drawIlluminations;
    public bool drawShadows;

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
            hit = raycastHit.collider?.gameObject
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
        if (collider == null)
        {
            throw new System.Exception();
        }

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

    /// <summary>
    /// get four rays for the collider - one hitting each edge and one going either side
    /// </summary>
    /// <param name="collider"></param>
    public void GetRays(ref LightRay[] dest, int startIndex, PolygonCollider2D collider)
    {
        if (startIndex + 3 >= dest.Length || startIndex < 0)
            throw new System.Exception();

        var edges = FindEdges(collider);
        var m = LightEngine.Instance.edgeMiss;

        // rotations
        var rotL = Quaternion.Euler(0, 0, m);
        var rotR = Quaternion.Euler(0, 0, -m);

        // directions
        var dirL = edges.left.worldPosition - Position;
        var dirR = edges.right.worldPosition - Position;

        dest[startIndex] = Raycast(rotL * dirL);
        dest[startIndex + 1] = Raycast(rotR * dirR);
        dest[startIndex + 2] = Raycast(rotR * dirL);
        dest[startIndex + 3] = Raycast(rotL * dirR);
    }

    public void GetShadowPositions(ref Vector3[] dest, int startIndex, PolygonCollider2D collider)
    {
        if (startIndex + 3 >= dest.Length || startIndex < 0)
            throw new System.Exception();

        var edges = FindEdges(collider);
        var m = LightEngine.Instance.edgeMiss;

        // rotations
        var rotL = Quaternion.Euler(0, 0, m);
        var rotR = Quaternion.Euler(0, 0, -m);

        // directions
        var dirL = edges.left.worldPosition - Position;
        var dirR = edges.right.worldPosition - Position;

        // outer
        dest[startIndex] = Position + (Vector2)(rotL * dirL).normalized * range;
        dest[startIndex + 1] = Position + (Vector2)(rotR * dirR).normalized * range;

        // inner
        dest[startIndex + 2] = Position + (Vector2)(rotR * dirL);
        dest[startIndex + 3] = Position + (Vector2)(rotL * dirR);
    }

    public void GetHits(ref RaycastHit2D[] dest) =>
        dest = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, _mask);

    private void OnDestroy()
    {
        LightEngine.Unregister(this);
    }
}

public struct LightRay
{
    public float angle;
    public Vector2 end;
    public GameObject hit;
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
