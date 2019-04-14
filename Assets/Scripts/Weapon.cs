using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // this means something different for each weapon type - leave it to the
    // implementor
    public float damage;
    public float heat;
    public Color colour;

    protected bool _isFiring;
    protected Heat _heat;

    private bool _stop;

    protected virtual void Awake()
    {
        var ship = GetComponentInParent<Ship>();
        _heat = ship.GetComponent<Heat>();
    }

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

        yield return DoFire(() => input.IsFiring && !_stop);

        _isFiring = false;
        _stop = false;
    }

    protected abstract IEnumerator DoFire(Func<bool> getFiring);

    public void Stop()
    {
        _stop = true;
    }
}
