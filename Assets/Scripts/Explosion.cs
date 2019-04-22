using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float startScale;
    public float duration;
    public float endScale = 1f;

    private Vector2 _endScale;

    public static void New(GameObject template, Vector3 position) =>
        Instantiate(template, position, Quaternion.identity);

    private void Awake()
    {
        _endScale = endScale * Vector2.one;
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

        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPoint.x > 0 && screenPoint.x < Screen.width)
        {
            if (screenPoint.y > 0 && screenPoint.y < Screen.height)
            {
                PlaySFX();
            }
        }

        while (elapsed < duration)
        {
            yield return new WaitForEndOfFrame();

            elapsed = Time.time - start;
            transform.localScale = Vector3.Lerp(transform.localScale, _endScale, elapsed / duration);
        }

        Destroy(gameObject);
    }

    private string[] _explosionNoises = new[]
    {
        GameConstants.SFX_Explosion1,
        GameConstants.SFX_Explosion2,
        GameConstants.SFX_Explosion3
    };
    private void PlaySFX()
    {
        var noise = _explosionNoises[Random.Range(0, _explosionNoises.Length)];
        Game.Instance.Audio.PlaySFX(noise);
    }
}
