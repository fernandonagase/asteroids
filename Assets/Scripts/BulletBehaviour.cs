using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IDestructible
{
    private float thrust = 800;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>()
            .AddRelativeForce(Vector3.up * thrust);
        Destroy(gameObject, 5.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(gameObject.tag)) return;

        IDamageable damageableObject = collision
            .GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(1);
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        // Implementar pooling
        Destroy(gameObject);
    }
}
