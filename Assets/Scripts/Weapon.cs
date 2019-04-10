using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // this means something different for each weapon type - leave it to the
    // implementor
    public float damage;
    public Color colour;

    protected bool _isFiring;

    public void Fire(IInputProvider input)
    {
        // can't fire a weapon that's already firing
        if (_isFiring)
            return;

        StartCoroutine(DoFire(input));
    }

    private IEnumerator DoFire(IInputProvider input)
    {
        _isFiring = true;
        yield return DoFire(() => input.IsFiring);
        _isFiring = false;
    }

    protected abstract IEnumerator DoFire(Func<bool> getFiring);
}
