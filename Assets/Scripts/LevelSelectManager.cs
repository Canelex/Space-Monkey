using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public Sprite outlineSelectedSprite;
    public Sprite outlineUnselectedSprite;
    public GameObject overview;
    public Text overviewName;
    public Text overviewTime;
    public Text overviewScore;
    private LevelSelectItem selected;
    private AudioSource asource;

    void Start()
    {
        asource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
            {
                DeselectCurrentItem();
                overview.SetActive(false);
            }
        }
    }

    private void DeselectCurrentItem()
    {
        if (selected != null) // Is there a selected item currently?
        {
            selected.GetComponent<Image>().sprite = outlineUnselectedSprite;
            selected = null;
        }
    }

    public void SelectItem(LevelSelectItem item)
    {
        if (item != selected)
        {
            DeselectCurrentItem();

            // Update selected item
            selected = item;
            selected.GetComponent<Image>().sprite = outlineSelectedSprite;

            // Update overview board
            overviewName.text = item.levelName;
            
            // Update best time
            int time = PlayerPrefs.GetInt("best-time-level" + item.levelIndex, -1);
            if (time != -1)
            {
                string minutes = (time / 60).ToString();
                string seconds = (time % 60).ToString("00");
                overviewTime.text = string.Format("{0}:{1}", minutes, seconds); // Update timer count.
            }
            else
            {
                overviewTime.text = "--";
            }
            
            // Update high score
            int score = PlayerPrefs.GetInt("high-score-level" + item.levelIndex, -1);
            if (score != -1)
            {
                overviewScore.text = score.ToString();
            }
            else
            {
                overviewScore.text = "--";
            }
            
            overview.SetActive(true);

            // Play selection sound
            asource.Play();
        }
    }

    public void StartSelected()
    {
        if (selected != null) // Is there a selected item already?
        {
            //TODO: Async with splash scren
            SceneManager.LoadScene(selected.levelIndex);
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        return results.Count > 0;
    }
}
