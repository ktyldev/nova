using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid1 : NetworkBehaviour
{
    public Transform parent1;

    void Start()
    {
        transform.SetParent(parent1);
    }
}