using UnityEngine;

public class AmmoBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (!collision.GetComponent<PlayerBehaviour>().ReloadAmmo(5))
        {
            return;
        }

        GameObject.FindWithTag("GameManager")
            .GetComponent<AmmoSpawner>()
            .FreeAmmo(gameObject);
    }
}
