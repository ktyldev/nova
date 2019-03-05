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
    public Transform bulletParent;

    private Dictionary<string, Ship> _players 
        = new Dictionary<string, Ship>();

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception();

        Instance = this;
    }

    public void RegisterPlayer(string netId, Ship player)
    {
        string id = "player_" + netId;
        _players.Add(netId, player);
        player.transform.name = id;

        Debug.Log("Registered player " + id);
    }
}
