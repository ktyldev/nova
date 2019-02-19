using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid : NetworkBehaviour
{
    public Transform parent;

    void Start()
    {
        transform.SetParent(parent);
    }
}
