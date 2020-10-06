using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    private const int MAX_ASTEROIDS = 6;
    // Manter constante (!)
    // Máximo de asteroides + máximo de fragmentos
    private int TOTAL_ASTEROIDS = MAX_ASTEROIDS
        * (1 + AsteroidBehaviour.FragmentCount);
    private const int SPAWN_PERIOD = 5;
    private const float ASTEROID_SPEED = 24;

    private Vector2 bottomLeftLim;
    private Vector2 topRightLim;
    private double lastSpawnedTime = 0;
    private int asteroidCount = 0;

    private Queue<GameObject> asteroids = new Queue<GameObject>();

    [SerializeField]
    private GameObject[] asteroidPrefabs = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;

        bottomLeftLim = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRightLim = cam.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        InitPool();
    }

    void FixedUpdate()
    {
        double elapsedTimeFromSpawn = Time.time - lastSpawnedTime;
        if (elapsedTimeFromSpawn >= SPAWN_PERIOD)
        {
            int spawnCount = (int)(elapsedTimeFromSpawn / SPAWN_PERIOD);
            for (int i = 0; i < spawnCount; ++i)
            {
                SpawnAsteroid();
            }
            lastSpawnedTime = Time.time;
        }
    }

    private void SpawnAsteroid()
    {
        if (asteroidCount >= MAX_ASTEROIDS)
        {
            return;
        }

        GameObject asteroid = GetAsteroid();

        if (asteroid != null)
        {
            asteroid.transform.position = new Vector3(
                Random.Range(bottomLeftLim.x, topRightLim.x),
                Random.Range(bottomLeftLim.y, topRightLim.y),
                0
            );

            Vector2 randomDirection = new Vector2(
                Random.Range(-1.0f, 2.0f),
                Random.Range(-1.0f, 2.0f)
            );
            asteroid.GetComponent<Rigidbody2D>().AddForce(randomDirection * ASTEROID_SPEED);

            asteroidCount++;
        }
    }

    public void DespawnAsteroid(GameObject asteroid)
    {
        FreeAsteroid(asteroid);
        if (asteroid.GetComponent<AsteroidBehaviour>().Fragmentable)
        {
            asteroidCount--;
        }
    }

    private void InitPool()
    {
        for (int i = 0; i < TOTAL_ASTEROIDS; ++i)
        {
            GameObject asteroid = Instantiate(
                asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                transform.position,
                transform.rotation
            );
            asteroids.Enqueue(asteroid);
        }
    }

    public GameObject GetAsteroid()
    {
        if (asteroids.Count <= 0)
        {
            return null;
        }

        GameObject nextAsteroid = asteroids.Dequeue();
        nextAsteroid.transform.localScale = new Vector3(
            AsteroidBehaviour.AsteroidScale,
            AsteroidBehaviour.AsteroidScale,
            1
        );
        nextAsteroid.GetComponent<AsteroidBehaviour>().Fragmentable = true;

        return nextAsteroid;
    }

    public void FreeAsteroid(GameObject asteroid)
    {
        asteroid.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        asteroid.transform.position = transform.position;
        asteroids.Enqueue(asteroid);
    }
}
