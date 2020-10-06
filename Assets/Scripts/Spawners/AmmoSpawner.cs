using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : Spawner
{
    private const int MAX_AMMO = 4;

    private int ammoCount = 0;
    private Vector3 lowerLeftLimit;
    private Vector3 upperRightLimit;
    private Queue<GameObject> ammoPool = new Queue<GameObject>();

    [SerializeField]
    private GameObject ammoPrefab = null;

    void Start()
    {
        Camera mainCamera = Camera.main;
        lowerLeftLimit = mainCamera.ScreenToWorldPoint(Vector3.zero);
        upperRightLimit = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        spawnerPeriod = 8;
        InitPool();
    }

    public override void Spawn()
    {
        if (ammoCount >= MAX_AMMO)
        {
            return;
        }

        GameObject nextAmmo = GetAmmo();

        if (nextAmmo != null)
        {
            nextAmmo.transform.position = new Vector3(
                Random.Range(lowerLeftLimit.x, upperRightLimit.x + 1),
                Random.Range(lowerLeftLimit.y, upperRightLimit.y + 1),
                0
            );
        }
    }

    private void InitPool()
    {
        for (int i = 0; i < MAX_AMMO; ++i)
        {
            ammoPool.Enqueue(Instantiate(
                ammoPrefab,
                transform.position,
                transform.rotation
            ));
        }
    }

    public GameObject GetAmmo()
    {
        if (ammoPool.Count <= 0)
        {
            return null;
        }

        return ammoPool.Dequeue();
    }

    public void FreeAmmo(GameObject ammo)
    {
        ammo.transform.position = transform.position;
        ammoPool.Enqueue(ammo);
    }
}
