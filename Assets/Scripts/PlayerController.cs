using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Maximum tilt of opposite axis for rotation to be recorded"), Range(0, 1)]
    public float tolerance;
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
    public bool evading;

    void Start()
    {
        InvokeRepeating("FireBullet", 1F, bulletRate);
    }

    void Update()
    {
        #region accelerometer Controls
        /* Using accelerometer readings, it is possible to calculate yaw,
        pitch, and roll. However, these rotations become innacurate when
        the device is completely flat on a correlating side. */

        Vector3 acceleration = Input.acceleration;
        float yaw = 0; // Rotation of phone on horiztonal/vertical axes
        float pitch = 0; // Rotation of phone on vertical/depth axes

        // Yaw cannot be calculated when phone is flat.
        if (Mathf.Abs(acceleration.z) <= tolerance)
        {
            yaw = NormalizeAngle(Mathf.Atan2(acceleration.x, -acceleration.y) * Mathf.Rad2Deg);
        }

        // Pitch cannot be calculated when the phone is on side.
        if (Mathf.Abs(acceleration.x) <= tolerance)
        {
            pitch = NormalizeAngle(Mathf.Atan2(acceleration.z, -acceleration.y) * Mathf.Rad2Deg);
        }

        // Use rotations to update player. 
        evading = pitch < -90;
        if (!evading)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, yaw), rotateSpeed);
        }
        #endregion
    }

    void FireBullet()
    {
        //TODO: Play bullet sound.
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.velocity = transform.up * bulletSpeed;
        bullet.transform.position += transform.up * 1.5F;
        Destroy(bullet.gameObject, bulletLife);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}