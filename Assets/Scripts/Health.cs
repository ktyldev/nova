using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float start;
    public UnityEvent hit = new UnityEvent();

    private float _current;

    public float Normalised => _current / start;

    public void TakeDamage(float damage)
    {
        _current -= damage;
        hit.Invoke();
    }
}
