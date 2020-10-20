using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IDamageable, IDestructible
{
    private const int INITIAL_HEALTH = 3;

    private float thrust = 12f;
    private float torque = 4f;
    private int health = INITIAL_HEALTH;
    private int ammo = 10;

    private Rigidbody2D rb2d;

    [SerializeField]
    private GameObject bulletPrefab = null;
    private GameObject firespot;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        firespot = GameObject.FindWithTag("Firespot");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
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
        if (ammo <= 5)
        {
            ammo += count;
            return true;
        }

        return false;
    }

    public void TakeDamage(int damage)
    {
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
    }
}
