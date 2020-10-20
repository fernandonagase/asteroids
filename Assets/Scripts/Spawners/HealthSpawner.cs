using UnityEngine;

public class HealthSpawner : Spawner
{
    private const int MAX_HEALTH = 2;

    private int healthCount = 0;
    private Vector3 lowerLeftLimit;
    private Vector3 upperRightLimit;

    [SerializeField]
    private GameObject healthPrefab = null;

    void Start()
    {
        Camera mainCamera = Camera.main;
        lowerLeftLimit = mainCamera.ScreenToWorldPoint(Vector3.zero);
        upperRightLimit = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        spawnerPeriod = 16;
    }

    public override void Spawn()
    {
        if (healthCount >= MAX_HEALTH)
        {
            return;
        }

        GameObject nextHealth = GetHealth();

        if (nextHealth != null)
        {
            nextHealth.transform.position = new Vector3(
                Random.Range(lowerLeftLimit.x, upperRightLimit.x + 1),
                Random.Range(lowerLeftLimit.y, upperRightLimit.y + 1),
                0
            );
            healthCount++;
        }
    }
    public GameObject GetHealth()
    {
        return Instantiate(healthPrefab, transform.position, transform.rotation);
    }

    public void FreeHealth(GameObject ammo)
    {
        healthCount--;
        Destroy(ammo);
    }
}
