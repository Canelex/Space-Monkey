using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Speed at which player will be rotated (smoothness)"), Range(0, 1)]
    public float rotateSpeed;
    [Tooltip("Prefab of bullet object")]
    public Rigidbody2D bulletPrefab;
    [Tooltip("Speed at which bullets are fired")]
    public float bulletSpeed;
    [Tooltip("Rate at which bullets are fired")]
    public float bulletRate;
    [Tooltip("How many seconds after spawning are bullets deleted")]
    public float bulletLife;
    private SpriteRenderer sprite;
    private Accelerometer accelerometer;
    public bool evading;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        accelerometer = new Accelerometer(0.9F);
        InvokeRepeating("FireBullet", 1F, bulletRate);
    }

    void Update()
    {
        // Wrapper takes accelerometer values and calculates tilts.
        accelerometer.ReadInput();
        float tiltRight = accelerometer.Yaw();
        float tiltBack = accelerometer.Pitch();

        if (tiltBack >= 90 || (evading && tiltBack >= 75))
        {
            tiltRight = 0;
            evading = true;
            sprite.color = new Color(1F, 1F, 1F, 0.5F);
        }
        else
        {
            evading = false;
            sprite.color = Color.white;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, tiltRight), rotateSpeed);

    }

    void FireBullet()
    {
        if (evading) return;

        //TODO: Play bullet sound.
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.velocity = transform.up * bulletSpeed;
        bullet.transform.position += transform.up * 1.5F;
        Destroy(bullet.gameObject, bulletLife);
    }
}