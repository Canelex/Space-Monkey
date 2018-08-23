using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Length (seconds) of evade animation"), Range(0.05F, 1F)]
    public float animationSpeed;
    [Tooltip("Prefab of bullet object.")]
    public Rigidbody2D bulletObj;
    [Tooltip("Velocity of bullets fired.")]
    public float bulletSpeed;
    [Tooltip("Rate at which bullets are fired.")]
    public float bulletRate;
    [Tooltip("Despawn time of bullets.")]
    public float bulletLife;
    public SpriteRenderer sprite;
    private Accelerometer accelerometer;

    public bool isDead;
    public bool isEvading;
    private bool isAnimating;
    private float animationTime;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        accelerometer = new Accelerometer(0.05F);
        InvokeRepeating("FireBullet", 1F, bulletRate);
    }

    void Update()
    {
        if (CanUseControls())
        {
            accelerometer.ReadInput(); // Wrapper takes accelerometer values and calculates tilts.
            float yaw = accelerometer.Yaw();
            float pitch = accelerometer.Pitch();
            float roll = accelerometer.Roll();

            float minPitch = isEvading ? 55 : 66; // If already evading, more intense tilt needed to stop.
            if (pitch >= minPitch && Mathf.Abs(roll) <= 45F)
            {
                SetEvading(true);
                yaw = 0F;
            }
            else
            {
                SetEvading(false);
            }

            if (isAnimating)
            {
                animationTime += Time.deltaTime / animationSpeed;

                if (animationTime > 1)
                {
                    animationTime = 1;
                    isAnimating = false;
                }

                float scaleXY = isEvading ? 1F - 0.2F * animationTime : 0.8F + 0.2F * animationTime;
                transform.localScale = new Vector3(scaleXY, scaleXY, 1F);
            }

            transform.rotation = Quaternion.Euler(0, 0, yaw);
        }
    }

    void FireBullet()
    {
        if (CanFireBullets())
        {
             //TODO: Play bullet sound.
            Rigidbody2D bullet = Instantiate(bulletObj, transform.position, transform.rotation);
            bullet.velocity = transform.up * bulletSpeed;
            bullet.transform.position += transform.up * 1.5F;
            Destroy(bullet.gameObject, bulletLife);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (CanBeDestroyed())
        {
            if (coll.tag == "Asteroid")
            {
                GameController.instance.PlayerDestroyed(coll.gameObject);
            }
        }
    }

    private void SetEvading(bool evading)
    {
        if (evading != isEvading) // Started now.
        {
            isAnimating = true;
            animationTime = 0F;
        }

        sprite.sortingLayerName = evading ? "Player Down" : "Player Up"; 
        isEvading = evading;
    }

    public bool CanUseControls()
    {
        return !isDead;
    }

    public bool CanBeDestroyed()
    {
        return !isDead && (!isEvading || isAnimating);
    }

    public bool CanFireBullets()
    {
        return !isDead && !isEvading;
    }

}