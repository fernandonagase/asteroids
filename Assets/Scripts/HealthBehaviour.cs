using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (!collision.GetComponent<PlayerBehaviour>().Heal(1))
        {
            return;
        }

        // TO-DO: Implementar como Object Pool
        GameObject.FindWithTag("GameManager")
            .GetComponent<HealthSpawner>()
            .FreeHealth(gameObject);
    }
}
