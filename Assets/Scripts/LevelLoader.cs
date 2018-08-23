using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index); // temp
    }

    public void LoadLevelIn(int num)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + num); // temp
    }
}
