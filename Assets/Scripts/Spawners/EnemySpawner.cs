using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private const int MAX_ENEMIES = 2;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private float[] horizontalPositions = new float[2];
    private Vector3 lowerLeftLimit;
    private Vector3 upperRightLimit;
    private int initialSpeed = 32;

    [SerializeField]
    private GameObject enemyShipPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Camera.main;
        lowerLeftLimit = mainCamera.ScreenToWorldPoint(Vector3.zero);
        upperRightLimit = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        horizontalPositions[0] = lowerLeftLimit.x + 2;
        horizontalPositions[1] = upperRightLimit.x - 2;

        spawnerPeriod = 10;
        InitPool();
    }

    public override void Spawn()
    {
        GameObject enemyShip = GetEnemyShip();

        if (enemyShip != null)
        {
            int positionOption = Random.Range(0, 2);
            enemyShip.transform.position = new Vector3(
                horizontalPositions[positionOption],
                Random.Range(lowerLeftLimit.y, upperRightLimit.y),
                0
            );
            Vector3 enemyDirection = Vector3.right
                * (positionOption == 0 ? 1 : -1);
            enemyShip.GetComponent<Rigidbody2D>()
                .AddForce(enemyDirection * initialSpeed);
        }
    }

    private void InitPool()
    {
        for (int i = 0; i < MAX_ENEMIES; ++i)
        {
            enemyPool.Enqueue(Instantiate(
                enemyShipPrefab,
                transform.position,
                transform.rotation
            ));
        }
    }

    public GameObject GetEnemyShip()
    {
        if (enemyPool.Count <= 0)
        {
            return null;
        }

        GameObject nextEnemy = enemyPool.Dequeue();
        nextEnemy.GetComponent<EnemyBehaviour>().Spawned = true;
        return nextEnemy;
    }

    public void FreeEnemyShip(GameObject enemy)
    {
        enemy.GetComponent<EnemyBehaviour>().Spawned = false;
        enemy.transform.position = transform.position;
        enemyPool.Enqueue(enemy);
    }
}
