using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid : MonoBehaviour
{
    public Transform parent;

    void Start()
    {
        transform.SetParent(parent);
    }
}
