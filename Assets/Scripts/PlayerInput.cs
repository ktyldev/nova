using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// Get a vector representing the current values of the movement input.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMovement()
    {
        return new Vector2(
            Input.GetAxis(GameConstants.Horizontal), 
            Input.GetAxis(GameConstants.Vertical));
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
}