using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    [Range(0, 1)]
    public float smoothing;

    private float _z;

    private void Awake()
    {
        _z = transform.position.z;
    }

    private void FixedUpdate()
    {
        var targetPosition = target.transform.position;
        targetPosition.z = _z;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothing);
    }
}
