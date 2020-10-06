using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    private float lastSpawnedTime = 0f;
    protected int spawnerPeriod = 5;

    void FixedUpdate()
    {
        float elapsedTimeSinceSpawn = Time.time - lastSpawnedTime;
        if (elapsedTimeSinceSpawn >= spawnerPeriod)
        {
            Spawn();
            lastSpawnedTime = Time.time;
        }
    }

    public abstract void Spawn();
}
