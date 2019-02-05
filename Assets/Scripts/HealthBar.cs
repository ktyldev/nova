using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health Health { get; set; }

    public Image image;

    private void Start()
    {
        Health.hit.AddListener(() => image.fillAmount = Health.Normalised);
    }
}
