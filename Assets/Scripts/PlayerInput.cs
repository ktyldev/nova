using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
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
    public Vector2 GetMovement()
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
                var right = Game.Instance.player.transform.right * h;
                var forward = Game.Instance.player.transform.up * v;

                movement = (right + forward);
                break;

            default:
                throw new System.Exception();
        }

        return movement.normalized;
    }

    /// <summary>
    /// Get the mouse position with the z component zeroed out.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMousePosition()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // unity's 2D is dumb
        mousePos.z = 0;

        return mousePos;
    }

    /// <summary>
    /// Get the direction from the player to the mouse position.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetAimDirection()
    {
        var playerPos = (Vector2)Game.Instance.player.transform.position;
        return (GetMousePosition() - playerPos).normalized;
    }

    /// <summary>
    /// Get the vector from a custom position to the mouse position.
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public Vector2 GetAimDirection(Vector2 origin)
    {
        return (GetMousePosition() - origin).normalized;
    }

    /// <summary>
    /// Get whether the player is firing.
    /// </summary>
    /// <returns></returns>
    public bool GetFiring()
    {
        return Input.GetButton(GameConstants.Axis_Fire1);
    }

    /// <summary>
    /// Get whether the player started firing this frame.
    /// </summary>
    /// <returns></returns>
    public bool GetStartFiring()
    {
        return Input.GetButtonDown(GameConstants.Axis_Fire1);
    }

    /// <summary>
    /// Get whether the player stopped firing this frame.
    /// </summary>
    /// <returns></returns>
    public bool GetStopFiring()
    {
        return Input.GetButtonUp(GameConstants.Axis_Fire1);
    }
}