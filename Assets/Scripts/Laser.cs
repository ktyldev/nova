using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public bool startActive;
    public float length;
    public bool occlude;
    public Transform emitter;
    [Tooltip("Objects in this collection won't be hit by the laser")]
    public GameObject[] ignore;

    /// <summary>
    /// Object currently occluding the laser. Null if laser isn't occluded.
    /// </summary>
    public GameObject Occluder { get; private set; }
    public bool IsActive
    {
        get { return _line.positionCount == 2; }
        set { _line.positionCount = value ? 2 : 0; }
    }

    private LineRenderer _line;
    private IInputProvider _input;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();

        var ship = GetComponentInParent<Ship>();
        _input = ship.GetComponent<IInputProvider>();
    }

    private void Start()
    {
        SetActive(startActive);
    }

    private void LateUpdate()
    {
        if (_line.positionCount == 0)
        {
            Occluder = null;
            return;
        }

        Draw();
    }

    private void Draw()
    {
        var start = emitter.transform.position;
        float l = length;

        if (occlude)
        {
            var hits = Physics2D.RaycastAll(emitter.transform.position, emitter.up, length);

            // filter out ignored objects
            hits = hits
                .Where(h => !ignore.Contains(h.collider.gameObject))
                .OrderBy(h => h.distance)
                .ToArray();

            if (hits.Any())
            {
                var hit = hits[0];

                Occluder = hit.collider.gameObject;
                l = hit.distance;
            }
            else
            {
                Occluder = null;
            }
        }

        //var dir = (Vector3)_input.TargetPosition - start;
        var dir = transform.forward;
        dir.Normalize();

        var end = start + dir * l;
        _line.SetPositions(new[] { start, end });
    }

    public void SetActive(bool active)
    {
        _line.positionCount = active ? 2 : 0;
    }

    public void SetColour(Color colour)
    {
        _line.startColor = colour;
        _line.endColor = colour;
    }
}
