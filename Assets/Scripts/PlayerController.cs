using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Maximum pitch at which device rotation will be recorded"), Range(0, 1)]
    public float maximumPitch;
    [Tooltip("Speed at which player will be rotated (smoothness)"), Range(0, 1)]
    public float rotateSpeed;

    void Start()
    {
    }

    void Update()
    {
        Vector3 acc = Input.acceleration;

        if (acc.z >= -maximumPitch && acc.z <= maximumPitch) // Check pitch. If the phone is flat, yaw cannot be calculated
        {
            /*
              When facing the screen in portrait mode, x is the horizontal vector
              and y is the vertical vector. However, since this game is played in
              landscape mode, they are switched. Yaw is the rotation of the device
              calculated using these vectors.
            */
            float yaw = Mathf.Atan2(acc.x, -acc.y);
            if (yaw < -Mathf.PI) yaw += Mathf.PI * 2;
            if (yaw > Mathf.PI) yaw -= Mathf.PI * 2;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, yaw * Mathf.Rad2Deg), rotateSpeed);
        }
    }
}
