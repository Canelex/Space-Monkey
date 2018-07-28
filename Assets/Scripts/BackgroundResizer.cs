using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResizer : MonoBehaviour
{
    void Start()
    {
        float aspect = Camera.main.aspect;
        transform.localScale = new Vector3(aspect, aspect, 1);
    }
}
