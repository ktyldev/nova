using UnityEngine;

public class LightSource : MonoBehaviour
{
    public float range;
    public int rays = 50;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // cast rays in a circle
        //float theta = 0;
        //for (int i = 0; i < rays; i++)
        //{
        //    theta = i * 360f / rays * Mathf.Deg2Rad;
        //    var dir = new Vector2(Mathf.Sin(theta), Mathf.Cos(theta));

        //    var hit = Physics2D.Raycast(transform.position, dir, range, LayerMask.GetMask("Asteroid"));
        //    if (hit.collider != null)
        //    {
        //        Debug.DrawLine(transform.position, hit.transform.position, Color.red);
        //    }
        //    else
        //    {
        //        Debug.DrawLine(transform.position, (Vector2)transform.position + dir * range, Color.green);
        //    }
        //}

        var mask = LayerMask.GetMask("Asteroid");

        var hits = Physics2D.CircleCastAll(
            transform.position,
            range,
            Vector2.zero,
            range,
            mask);

        foreach (var h in hits)
        {
            var collider = h.collider as PolygonCollider2D;
            //Debug.DrawLine(transform.position, h.transform.position, Color.green);

            // find the limits of each collider
            var ho = FindEdges(collider);

            var m = 0.01f;
            // raycast towards limits, might be stopped on the way
            var dLeft = (Vector3)ho.left - transform.position;
            var rotLeft = Quaternion.Euler(0, 0, -m);
            var dLeft2 = rotLeft * dLeft.normalized * range;

            var dRight = (Vector3)ho.right - transform.position;
            var rotRight = Quaternion.Euler(0, 0, m);
            var dRight2 = rotRight * dRight.normalized * range;

            // raycast just to the left (clockwise) of the object
            DrawRay(dLeft2, Color.red);

            //var hitLeft2 = Physics2D.Raycast(transform.position, dLeft2.normalized, range, mask);
            //if (hitLeft2.collider != null)
            //{
            //    Debug.DrawLine(transform.position, hitLeft2.transform.position, Color.red);
            //}
            //else
            //{
            //    Debug.DrawLine(transform.position, transform.position + dLeft2, Color.red);
            //}

            // raycast just anticlockwise of the object
            DrawRay(dRight2, Color.magenta);

            //var hitRight2 = Physics2D.Raycast(transform.position, dRight2.normalized, range, mask);
            //if (hitRight2.collider != null)
            //{
            //    Debug.DrawLine(transform.position, hitRight2.transform.position, Color.magenta);
            //}
            //else
            //{
            //    Debug.DrawLine(transform.position, transform.position + dRight2, Color.magenta);
            //}

            // raycast towards the clockwise edge of the object
            DrawRay(dLeft, Color.green);

            //Debug.DrawLine(transform.position, transform.position + dLeft, Color.green);

            // raycast towards the anticlickwise edge of the object
            DrawRay(dRight, Color.blue);

            //Debug.DrawLine(transform.position, transform.position + dRight, Color.blue);
        }
    }

    private void DrawRay(Vector2 dir, Color colour)
    {
        var mask = LayerMask.GetMask("Asteroid");
        var hit = Physics2D.Raycast(transform.position, dir.normalized, range, mask);
        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.transform.position, colour);
        }
        else
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + dir, colour);
        }
    }

    private struct HitObject
    {
        public Vector2 left;
        public Vector2 right;
    }

    private HitObject FindEdges(PolygonCollider2D collider)
    {
        var v = collider.transform.position - transform.position;

        var ho = new HitObject();

        float lAngle = 0f;
        float rAngle = 0f;

        foreach (var point in collider.points)
        {
            var p = (Vector2)collider.transform.position + point * collider.transform.localScale;

            var v2 = p - (Vector2)transform.position;
            float a = Vector2.SignedAngle(v, v2);

            if (a < 0)
            {
                if (a < lAngle)
                {
                    lAngle = a;
                    ho.left = p;
                }
            }
            else
            {
                if (a > rAngle)
                {
                    rAngle = a;
                    ho.right = p;
                }
            }
        }

        return ho;

    }
}
