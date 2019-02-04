using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// provides common references for other scripts in the ship hierarchy
public class Ship : MonoBehaviour
{
    public GameObject inputProvider;

    public IInputProvider InputProvider { get; private set; }

    private void Awake()
    {
        InputProvider = inputProvider.GetComponent<IInputProvider>();
    }
}
