using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid3 : NetworkBehaviour
{
    public Transform parent3;

    void Start()
    {
        transform.SetParent(parent3);
    }
}