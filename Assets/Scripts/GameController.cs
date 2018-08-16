using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int lives;
    public int score;
    public int time;

    private Vector2 cameraBounds;
    private PlayerController player;

    void Start()
    {
        float height = Camera.main.orthographicSize;
        float width = Camera.main.aspect * height;
        cameraBounds = new Vector2(width, height);
        player = FindObjectOfType<PlayerController>();

    }

    void Update()
    {
        // Update time.
        time = (int)Time.time;

        AsteroidController[] asteroids = FindObjectsOfType<AsteroidController>();
        if (asteroids.Length > 0)
        {
            foreach (AsteroidController asteroid in asteroids)
            {
                Vector3 position = asteroid.transform.position;
                Vector3 bounds = GetBoundsForObject(asteroid.gameObject);

                if (Mathf.Abs(position.x) > bounds.x || Mathf.Abs(position.y) > bounds.y)
                {
                    // Reset position and target player again.
                    asteroid.transform.position = GetRandomPointOnY(-bounds.x, bounds.x, bounds.y);
                    asteroid.TargetPos(GetRandomPointOnY(-bounds.x, bounds.x, -bounds.y));
                }
            }
        }
        else
        {
            // Victory
        }
    }

    Vector2 GetBoundsForObject(GameObject go)
    {
        // X and Y value at which sprite will no longer be rendered.
        SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
        if (renderer == null) return cameraBounds;
        return cameraBounds + renderer.size / 2;
    }

    Vector2 GetRandomPointOnY(float x1, float x2, float y)
    {
        return new Vector2(Random.Range(x1, x2), y);
    }
}
