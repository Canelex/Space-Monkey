using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStars : MonoBehaviour
{
    [Tooltip("Degrees starfield rotates in one second")]
    public float rotateSpeed;

    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }
}
