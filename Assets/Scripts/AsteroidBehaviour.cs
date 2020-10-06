using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour, IDamageable, IDestructible
{
    public static int FragmentCount { get; private set; } = 3;
    public static float AsteroidScale { get; private set; } = 1.6f;
    public static float FragmentScale { get; private set; } = 0.8f;

    private float fragmentSpeed = 32;
    public bool Fragmentable { get; set; } = true;

    private Vector3[] directions = new Vector3[3];

    private void Start()
    {
        directions[0] = -transform.right;
        directions[1] = -transform.up;
        directions[2] = transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        collision.GetComponent<IDamageable>().TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        DestroyObject();
    }

    public void DestroyObject()
    {
        if (Fragmentable)
        {
            SpawnFragments();
        }

        GameObject gameManager = GameObject.FindWithTag("GameManager");
        gameManager
            .GetComponent<GameManager>()
            .Score(100);
        gameManager
            .GetComponent<AsteroidSpawner>()
            .DespawnAsteroid(gameObject);
    }

    private void SpawnFragments()
    {
        AsteroidSpawner spawner = GameObject.FindWithTag("GameManager")
            .GetComponent<AsteroidSpawner>();
        for (int i = 0; i < FragmentCount; ++i)
        {
            GameObject nextFragment = spawner.GetAsteroid();
            if (nextFragment == null)
            {
                return;
            }
            nextFragment.transform.localScale = new Vector3(
                FragmentScale,
                FragmentScale,
                1
            );
            nextFragment.GetComponent<AsteroidBehaviour>().Fragmentable = false;
            nextFragment.transform.position = transform.position + directions[i];
            nextFragment.GetComponent<Rigidbody2D>().AddForce(
                new Vector3(
                    Random.Range(-1.0f, 2.0f),
                    Random.Range(-1.0f, 2.0f),
                    0
                ) * fragmentSpeed
            );
        }
    }
}
