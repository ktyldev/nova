using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : MonoBehaviour, IInputProvider
{
    public Vector2 AccelerationDir => Vector2.zero;

    public Vector2 TargetPosition => Vector2.zero;

    public bool IsFiring => false;

    public bool LaserActive => false;

    public bool GunActive => false;
}
