using UnityEngine;

public class PlayerInput : MonoBehaviour, IInputProvider
{
    public enum MovementType
    {
        Absolute,
        Relative
    }
    public MovementType movementType;

    /// <summary>
    /// Get a vector representing the current values of the movement input.
    /// </summary>
    /// <returns></returns>
    public Vector2 AccelerationDir
    {
        get
        {
            float h = Input.GetAxis(GameConstants.Axis_Horizontal);
            float v = Input.GetAxis(GameConstants.Axis_Vertical);

            Vector2 movement;

            switch (movementType)
            {
                case MovementType.Absolute:
                    movement = new Vector2(h, v);
                    break;

                case MovementType.Relative:
                    var right = transform.right * h;
                    var forward = transform.up * v;

                    movement = (right + forward);
                    break;

                default:
                    throw new System.Exception();
            }

            return movement.normalized;
        }
    }

    /// <summary>
    /// Get the mouse position with the z component zeroed out.
    /// </summary>
    /// <returns></returns>
    public Vector2 TargetPosition
    {
        get
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // unity's 2D is dumb
            mousePos.z = 0;

            return mousePos;
        }
    }

    /// <summary>
    /// Get whether the player is firing.
    /// </summary>
    /// <returns></returns>
    public bool IsFiring
    {
        get
        {
            return Input.GetButton(GameConstants.Axis_Fire1);
        }
    }
}