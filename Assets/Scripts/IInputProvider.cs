using UnityEngine;
using System.Collections;

public interface IInputProvider 
{
    /// <summary>
    /// Acceleration vector
    /// </summary>
    Vector2 AccelerationDir { get; }
    /// <summary>
    /// World position to aim at
    /// </summary>
    Vector2 TargetPosition { get; }
    /// <summary>
    /// Weapons hot
    /// </summary>
    bool IsFiring { get; }

    bool LaserActive { get; }

    bool GunActive { get; }
}
