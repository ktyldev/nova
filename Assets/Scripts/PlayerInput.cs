using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 GetMovement()
    {
        return new Vector2(
            Input.GetAxis(GameConstants.Horizontal), 
            Input.GetAxis(GameConstants.Vertical));
    }

    public Vector2 GetAimDirection()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // unity's 2D is dumb
        mousePos.z = 0;

        return (mousePos - Game.Instance.player.transform.position).normalized;
    }
}