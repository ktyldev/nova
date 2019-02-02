﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public PlayerInput PlayerInput { get; private set; }

    public GameObject player;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;

        PlayerInput = GetComponent<PlayerInput>();
    }
}
