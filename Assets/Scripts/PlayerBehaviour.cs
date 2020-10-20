using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IDamageable, IDestructible
{
    private const int INITIAL_HEALTH = 3;
    private const int INITIAL_AMMO = 10;

    private float thrust = 12f;
    private float torque = 4f;
    private int health = INITIAL_HEALTH;
    private int ammo = INITIAL_AMMO;

    private Rigidbody2D rb2d;
    private GameManager gameManager;

    [SerializeField]
    private GameObject bulletPrefab = null;
    private GameObject firespot;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        firespot = GameObject.FindWithTag("Firespot");

        gameManager = GameObject.FindWithTag("GameManager")
            .GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameManager.IsGameOver)
            {
                FactoryReset();
            }
            else
            {
                Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        if (gameManager.IsGameOver) {
            return;
        }

        float force = Input.GetAxis("Vertical");
        float rotation = Input.GetAxisRaw("Horizontal");

        rb2d.AddRelativeForce(new Vector3(0, force, 0) * thrust);
        transform.Rotate(new Vector3(0, 0, -rotation) * torque);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageableObject = collision.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(1);
        }
    }

    private void Shoot()
    {
        if (ammo <= 0)
        {
            return;
        }

        Recoil();
        Instantiate(
            bulletPrefab,
            firespot.transform.position,
            transform.rotation
        );
        GameObject.FindWithTag("HUD")
            .GetComponent<HUDManager>()
            .UpdateAmmo(--ammo);
    }

    private void Recoil()
    {
        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.5f, 0.02f));
        rb2d.AddRelativeForce(-Vector2.up * 100);
    }

    public bool ReloadAmmo(int count)
    {
        if (ammo == 10) return false;

        ammo += (ammo <= 10 - count ? count : (10 - ammo));
        GameObject.FindWithTag("HUD")
            .GetComponent<HUDManager>()
            .UpdateAmmo(ammo);
        return true;
    }

    public void TakeDamage(int damage)
    {
        if (GameObject.FindWithTag("GameManager")
            .GetComponent<GameManager>()
            .IsGameOver
        ) {
            return;
        }

        health -= damage;
        GameObject.FindWithTag("HUD")
            .GetComponent<HUDManager>()
            .UpdateLifeBar(health, INITIAL_HEALTH);
        if (health <= 0)
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        transform.position = Vector3.zero;
        rb2d.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;

        ToggleGameOver();
    }

    public void FactoryReset()
    {
        health = INITIAL_HEALTH;
        ammo = INITIAL_AMMO;

        HUDManager hudManager = GameObject.FindWithTag("HUD")
            .GetComponent<HUDManager>();
        hudManager.UpdateLifeBar(health, INITIAL_HEALTH);
        hudManager.UpdateAmmo(ammo);
        gameManager.ResetGame();
        gameManager.ToggleGameOver();
    }

    private void ToggleGameOver()
    {
        GameManager gameManager = GameObject.FindWithTag("GameManager")
            .GetComponent<GameManager>();
        gameManager.ToggleGameOver();
    }
}
