using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float startScale;
    public float duration;

    private Vector2 _endScale;

    private void Awake()
    {
        _endScale = transform.localScale;
        transform.localScale = Vector2.one * startScale;
    }

    private void Start()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        float elapsed = 0;
        float start = Time.time;

        while (elapsed < duration)
        {
            yield return new WaitForEndOfFrame();

            elapsed = Time.time - start;
            transform.localScale = Vector3.Lerp(transform.localScale, _endScale, elapsed / duration);
        }

        Destroy(gameObject);
    }
}
