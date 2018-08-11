using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer
{
    public Vector3 acceleration;
    public float tolerance;

    public Accelerometer(float tolerance)
    {
        this.tolerance = tolerance;
        this.acceleration = Vector3.zero;
    }

    public void ReadInput()
    {
        acceleration = Input.acceleration;
    }

    public float Yaw()
    {
        if (Mathf.Abs(acceleration.z) <= tolerance)
        {
            return Normalize(Mathf.Atan2(acceleration.y, acceleration.x) * Mathf.Rad2Deg + 90F);
        }

        return 0;
    }

    public float Pitch()
    {
        if (Mathf.Abs(acceleration.x) <= tolerance)
        {
            return Normalize(Mathf.Atan2(acceleration.z, acceleration.y) * Mathf.Rad2Deg + 180F);
        }

        return 0;
    }

    public float Roll()
    {
        if (Mathf.Abs(acceleration.y) <= tolerance)
        {
            return Normalize(Mathf.Atan2(acceleration.z, acceleration.x) * Mathf.Rad2Deg + 90F);
        }

        return 0;
    }

    float Normalize(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
