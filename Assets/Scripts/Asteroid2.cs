using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid2 : NetworkBehaviour
{
    public Transform parent2;

    void Start()
    {
        transform.SetParent(parent2);
    }
}