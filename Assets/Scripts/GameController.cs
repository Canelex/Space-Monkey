using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private float wrapX;
    private float wrapY;

    void Start()
    {
        wrapY = Camera.main.orthographicSize;
        wrapX = Camera.main.aspect * wrapY;
        wrapX++;
        wrapY++;
    }

    void Update()
    {
        AsteroidController[] asteroids = FindObjectsOfType<AsteroidController>();

        foreach (AsteroidController obj in asteroids)
        {
            RepositionAsteroid(obj.gameObject);
        }

    }

    void RepositionAsteroid(GameObject go)
    {
        Vector2 pos = go.transform.position;

        if (Mathf.Abs(pos.x) > wrapX || Mathf.Abs(pos.y) > wrapY)
        {
            pos.x = Random.Range(-wrapX, wrapX);
            pos.y = wrapY;
        }

        go.transform.position = pos;
    }
}
