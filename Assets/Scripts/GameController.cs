using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int lives;
    public int score;
    public int time;
    private Vector2 cameraBounds;
    private PlayerController player;

    void Awake()
    {
        instance = this;
    }

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

        if (!player.respawning) // If player is respawning, do not launch asteroids.
        {
            // Reset out of bounds asteroids.
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
                SceneManager.LoadScene(0); // Temp
                // Victory
            }
        }
    }

    public void PlayerDestroyed(PlayerController player, GameObject destroyer)
    {
        if (lives > 0)
        {
            player.respawning = true;
            player.invulnerable = true;
            player.sprite.color = new Color(1F, 1F, 1F, 0.25F);
            lives--;
            Invoke("RespawnPlayer", 3F);
        }
        else
        {
            SceneManager.LoadScene(0); // Temp
            // Defeat
        }

        // TODO: Some kind of effect

        Destroy(destroyer); // Asteroid(?) gets deleted.
    }

    public void AsteroidDestroyed(AsteroidController asteroid, GameObject destroyer)
    {
        // TODO: some kind of explosion effect.

        score++;
        Destroy(asteroid.gameObject); // Asteroid gets deleted.
        Destroy(destroyer); // Bullet(?) gets deleted.
    }

    void RespawnPlayer()
    {
        player.respawning = false;
        player.invulnerable = false;
        player.sprite.color = Color.white;
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
