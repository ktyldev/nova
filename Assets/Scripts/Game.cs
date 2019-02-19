using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum Mode
    {
        SinglePlayer = 1,
        MultiPlayer = 2
    }
    public Mode mode;
    public int Players => (int)mode;

    public static Game Instance { get; private set; }

    public GameObject explosion;
    public GameObject asteroid;


    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }
}
