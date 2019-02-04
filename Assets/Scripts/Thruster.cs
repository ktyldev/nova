using UnityEngine;

public class Thruster : MonoBehaviour
{
    // thruster needs to figure out the component of the player's input vector
    // that's pointing in the same direction as itself

    public GameObject plume;

    // what direction is the thruster pointing in
    private Vector2 ExhaustDir => transform.up;

    private IInputProvider _input;
    private Vector3 _originalScale;

    // Start is called before the first frame update
    void Start()
    {
        var ship = GetComponentInParent<Ship>();
        _input = ship.InputProvider;

        _originalScale = plume.transform.localScale;
        SetPlumeScale(0);
    }

    private void LateUpdate()
    {
        float scale;

        var dir = _input.AccelerationDir;
        scale = Vector3.Dot(dir, ExhaustDir);

        // not accelerating, or accelerating in the opposite direction to
        // thruster exhaust
        if (dir.magnitude == 0 || scale <= 0)
        {
            scale = 0;
        }

        SetPlumeScale(scale);
    }

    private void SetPlumeScale(float scale)
    {
        plume.transform.localScale = new Vector3(
            _originalScale.x, 
            scale * _originalScale.y, 
            _originalScale.z);
    }
}
