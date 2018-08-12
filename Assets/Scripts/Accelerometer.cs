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
        if (Pythagorean(acceleration.x, acceleration.y) >= tolerance)
        {
            return Normalize(Mathf.Atan2(acceleration.x, -acceleration.y) * Mathf.Rad2Deg);
        }

        return 0;
    }

    public float Pitch()
    {
        if (Pythagorean(acceleration.z, acceleration.y) >= tolerance)
        {
            return Normalize(Mathf.Atan2(-acceleration.z, -acceleration.y) * Mathf.Rad2Deg);
        }

        return 0;
    }

    public float Roll()
    {
        if (Pythagorean(acceleration.z, acceleration.x) >= tolerance)
        {
            return Normalize(Mathf.Atan2(-acceleration.x, -acceleration.z) * Mathf.Rad2Deg);
        }

        return 0;
    }

    float Normalize(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    float Pythagorean(float a, float b)
    {
        return Mathf.Sqrt(a * a + b * b);
    }
}
