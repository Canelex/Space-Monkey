using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LevelSelectLine : MonoBehaviour
{
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        LevelSelectItem[] items = FindObjectsOfType<LevelSelectItem>();
        Array.Sort(items, CompareItems);

        line.positionCount = items.Length;
        for (int i = 0; i < items.Length; i++)
        {
            line.SetPosition(i, (Vector2)items[i].transform.parent.position);
        }
    }

    private int CompareItems(LevelSelectItem a, LevelSelectItem b)
    {
        if (a.levelIndex > b.levelIndex) return 1;
        if (a.levelIndex < b.levelIndex) return -1;
        return 0;
    }
}
