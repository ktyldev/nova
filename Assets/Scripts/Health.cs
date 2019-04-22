using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public class HealthUpdateEvent : UnityEvent<float> { }

    public float max;
    public UnityEvent<float> hit = new HealthUpdateEvent();
    public UnityEvent death = new UnityEvent();

    private float _current;

    public float Normalised => _current / max;

    private void Awake()
    {
        _current = max;
    }

    private void Start()
    {
        //hit.AddListener(_ => print(ToString()));
        //death.AddListener(() => print(ToString() + " - dead"));
    }

    public void TakeDamage(float damage)
    {
        // we're dead, can't take any more damage
        if (_current == 0)
            return;

        _current -= damage;
        if (_current <= 0)
        {
            Die();
            return;
        }

        hit.Invoke(Normalised);
    }

    public void Die()
    {
        _current = 0;
        death.Invoke();
    }

    public override string ToString()
    {
        return string.Format("{0} health: {1}%", gameObject.ToString(), Normalised * 100);
    }
}
