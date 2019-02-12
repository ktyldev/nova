using UnityEngine;
using UnityEngine.Networking;

// provides common references for other scripts in the ship hierarchy
public class Ship : NetworkBehaviour
{
    public IInputProvider InputProvider { get; private set; }
    public bool IsLocalPlayer => isLocalPlayer;

    private void Awake()
    {
        InputProvider = GetComponent<IInputProvider>();
    }

    // TODO: play explosion sound
    public void Die()
    {
        Instantiate(Game.Instance.explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
