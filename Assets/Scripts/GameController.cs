using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int lives;
    public int score;
    public int time;

    void Start()
    {

    }

    void Update()
    {
        time = (int)Time.time;
    }
}
