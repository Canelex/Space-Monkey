using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private bool gameOver;
    private float percentFade;
    public int lives;
    public int score;
    public int time;
    public GameObject asteroidCrumbs;
    public GameObject canvasGame;
    public GameObject canvasVictory;
    public GameObject canvasDefeat;
    public Text textLives;
    public Text textScore;
    public Text textTime;
    private Vector2 cameraBounds;
    private PlayerController player;

    public GameController()
    {
        instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        float height = Camera.main.orthographicSize;
        float width = Camera.main.aspect * height;
        cameraBounds = new Vector2(width, height);
    }

    void Update()
    {
        if (!gameOver)
        {
            int thisTime = (int)Time.timeSinceLevelLoad;
            if (thisTime != time) // Time has changed.
            {
                time = thisTime;
                string minutes = (time / 60).ToString();
                string seconds = (time % 60).ToString("00");
                textTime.text = string.Format("{0}:{1}", minutes, seconds); // Update timer count.
            }

            if (!player.isDead) // Asteroids stay out of bounds when player respawning.
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
                            // Reset position and target random pos again.
                            asteroid.transform.position = GetRandomPointOnY(-bounds.x, bounds.x, bounds.y);
                            asteroid.TargetPos(GetRandomPointOnY(-bounds.x, bounds.x, -bounds.y));
                        }
                    }
                }
                else
                {
                    // Victory
                    gameOver = true;
                    canvasGame.SetActive(false);
                    canvasVictory.SetActive(true);

                    // Save Scores
                    int index = SceneManager.GetActiveScene().buildIndex;
                    int bestTime = PlayerPrefs.GetInt("best-time-level" + index, int.MaxValue);
                    if (time < bestTime)
                    {
                        PlayerPrefs.SetInt("best-time-level" + index, time);
                    }

                    int highScore = PlayerPrefs.GetInt("high-score-level" + index, int.MinValue);
                    if (score > highScore)
                    {
                        PlayerPrefs.SetInt("high-score-level" + index, score);
                    }
                }
            }
        }
        else // Game Over
        {

        }
    }

    public void PlayerDestroyed(GameObject destroyer)
    {
        player.isDead = true;

        if (lives > 1)
        {
            player.sprite.color = new Color(1F, 1F, 1F, 0.25F); // temp
            lives--;
            textLives.text = "x" + lives;
            Invoke("RespawnPlayer", 3F);
        }
        else
        {
            gameOver = true;
            Destroy(player.gameObject); // Bye bye player.
            canvasGame.SetActive(false);
            canvasDefeat.SetActive(true);
        }

        // TODO: Some kind of effect
        GameObject go = Instantiate(asteroidCrumbs, destroyer.transform.position, destroyer.transform.rotation);
        Destroy(go, 1F);
        Destroy(destroyer); // Asteroid(?) gets deleted.
    }

    public void AsteroidDestroyed(AsteroidController asteroid, GameObject destroyer)
    {
        // TODO: some kind of explosion effect.
        GameObject go = Instantiate(asteroidCrumbs, asteroid.transform.position, asteroid.transform.rotation);
        Destroy(go, 1F);

        score += 10;
        textScore.text = score.ToString("000");
        Destroy(asteroid.gameObject); // Asteroid gets deleted.
        Destroy(destroyer); // Bullet(?) gets deleted.
    }

    void RespawnPlayer()
    {
        player.isDead = false;
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
