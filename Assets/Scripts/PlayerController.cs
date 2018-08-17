using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Prefab of bullet object")]
    public Rigidbody2D bulletPrefab;
    [Tooltip("Speed at which bullets are fired")]
    public float bulletSpeed;
    [Tooltip("Rate at which bullets are fired")]
    public float bulletRate;
    [Tooltip("How many seconds after spawning are bullets deleted")]
    public float bulletLife;
    private float minPitchEvade;
    private float rotationSpeed;
    public bool respawning;
    public bool invulnerable;
    public SpriteRenderer sprite;
    private Accelerometer accelerometer;

    void Awake()
    {
        minPitchEvade = PlayerPrefs.GetFloat("min-pitch-to-evade", Defaults.MIN_PITCH_TO_EVADE);
        rotationSpeed = PlayerPrefs.GetFloat("rotation-speed", Defaults.ROTATION_SPEED);
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        accelerometer = new Accelerometer(0.05F);
        InvokeRepeating("FireBullet", 1F, bulletRate);
    }

    void Update()
    {
        if (!respawning)
        {
            // Wrapper takes accelerometer values and calculates tilts.
            accelerometer.ReadInput();
            float yaw = accelerometer.Yaw();
            float pitch = accelerometer.Pitch();
            float roll = accelerometer.Roll();

            if (pitch >= minPitchEvade && Mathf.Abs(roll) <= 45)
            {
                invulnerable = true;
                sprite.color = new Color(1F, 1F, 1F, 0.5F); // Translucent.
                yaw = 0;
            }
            else
            {
                invulnerable = false;
                sprite.color = Color.white; // Not translucent.
            }

            // Rotate towards specified yaw (non-lineear).
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, yaw), rotationSpeed);
            transform.rotation = rotation;
        }
    }

    void FireBullet()
    {
        if (invulnerable) return;

        //TODO: Play bullet sound.
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.velocity = transform.up * bulletSpeed;
        bullet.transform.position += transform.up * 1.5F;
        Destroy(bullet.gameObject, bulletLife);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (invulnerable) return;

        if (coll.tag == "Asteroid")
        {
            GameController.instance.PlayerDestroyed(this, coll.gameObject);
        }
    }
}