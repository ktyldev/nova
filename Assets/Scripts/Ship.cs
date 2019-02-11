using UnityEngine;

// provides common references for other scripts in the ship hierarchy
public class Ship : MonoBehaviour
{
    public IInputProvider InputProvider { get; private set; }

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
