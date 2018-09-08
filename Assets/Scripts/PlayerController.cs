using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Length (seconds) of evade animation"), Range(0.05F, 1F)]
    public float animationLength;
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
            }
            else
            {
                SetEvading(false);
                transform.rotation = Quaternion.Euler(0, 0, yaw); // Rotate player to match device yaw.
            }

            if (isAnimating)
            {
                animationTime += Time.deltaTime;

                if (animationTime > animationLength)
                {
                    isAnimating = false; // Animation is finished.
                }

                float ap = Mathf.Min(animationTime, animationLength) / animationLength; // [0 - 1];\
                float scale = isEvading ? 1F - 0.2F * ap : 0.8F + 0.2F * ap; // If evading, 1.0->0.8. Otherwise, 0.8->1.0
                transform.localScale = new Vector3(scale, scale, 1F);
            }
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

    public void SetDead(bool dead)
    {
        isDead = dead;
        sprite.color = isDead ? new Color(1F, 1F, 1F, 0.25F) : Color.white;
    }

    public void SetEvading(bool evading)
    {
        if (evading != isEvading) // Started now.
        {
            isAnimating = true;
            animationTime = 0F;
        }

        sprite.sortingLayerName = evading ? "Player Down" : "Player Up"; // Render either above/below asteroids. 
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