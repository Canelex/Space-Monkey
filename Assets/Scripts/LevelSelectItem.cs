using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectItem : MonoBehaviour
{
    [Tooltip("Index of the target level")]
    public int levelIndex;
    [Tooltip("Display name of target level")]
    public string levelName;

    public void SetSelected()
    {
        FindObjectOfType<LevelSelectManager>().SelectItem(this);
    }
}
