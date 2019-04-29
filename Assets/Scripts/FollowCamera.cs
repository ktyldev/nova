using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    public Transform target;
    [Range(0, 1)]
    public float smoothing;

    private float _z;
    private Camera _camera;

    private void Awake()
    {
        _z = transform.position.z;
        _camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        var targetPosition = target.transform.position;
        targetPosition.z = _z;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothing);
    }
}
