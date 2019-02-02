using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerInput _input;

    private void Start()
    {
        _input = Game.Instance.PlayerInput;
    }

    void Update()
    {
        if (_input.GetStartFiring())
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        while (_input.GetFiring())
        {
            print("pew!");
            yield return new WaitForEndOfFrame();
        }
    }
}
