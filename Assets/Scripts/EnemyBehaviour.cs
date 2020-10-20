using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamageable, IDestructible
{
    private const int SHOOT_PERIOD = 10;

    private Rigidbody2D rb2d;
    private int verticalSpeed = 5;
    private float lastShootTime = 0;

    public bool Spawned { get; set; } = false;

    [SerializeField]
    private GameObject bulletPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2d.AddForce(
            Vector3.up * (
                Mathf.PingPong(Time.time - Random.Range(-1.0f, 2.0f), 2) - 1
            ) * verticalSpeed
        );

        float elapsedTimeSinceShoot = Time.time - lastShootTime;
        if (elapsedTimeSinceShoot >= SHOOT_PERIOD)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void Shoot()
    {
        if (!Spawned)
        {
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");

        var firespot = gameObject.transform.Find("EnemyFirespot");
        Vector3 direction = player.transform.position - firespot.position;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firespot.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        GameObject bullet = Instantiate(bulletPrefab, firespot.position, firespot.rotation);
        bullet.tag = gameObject.tag;
    }

    public void TakeDamage(int damage)
    {
        DestroyObject();
    }

    public void DestroyObject()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        gameManager
            .GetComponent<GameManager>()
            .Score(150);
        gameManager
            .GetComponent<EnemySpawner>()
            .FreeEnemyShip(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        collision.GetComponent<IDamageable>().TakeDamage(1);
    }
}
