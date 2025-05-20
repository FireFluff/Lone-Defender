using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 1.0f;
    public float bulletSpeed = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 0f, shootInterval);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.up * bulletSpeed;
        }
    }
}
