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
    public Image overview;
    public Text overviewName;
    public Text overviewTime;
    public Text overviewScore;
    private LevelSelectItem selected;
    private Image image;
    private AudioSource asource;

    void Start()
    {
        asource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Deselect current item when you touch somewhere else in screen
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        DeselectCurrentItem();
                        overview.gameObject.SetActive(false); // Hide level overview
                    }
                }
            }
        }
    }

    private void DeselectCurrentItem()
    {
        if (selected != null) // Is there a selected item already?
        {
            selected.GetComponent<Image>().sprite = outlineUnselectedSprite;
        }

        selected = null;
    }

    public void SelectItem(LevelSelectItem item)
    {
        if (item == selected)
        {
            return;
        }

        DeselectCurrentItem();

        // Update selected item
        selected = item;
        selected.GetComponent<Image>().sprite = outlineSelectedSprite;

        // Update overview board
        overviewName.text = item.levelName;
        overviewTime.text = "--"; // TODO: Fetch
        overviewScore.text = "--"; // TODO: Fetch
        overview.gameObject.SetActive(true);

        // Play selection sound
        asource.Play();
    }

    public void StartSelected()
    {
        if (selected != null) // Is there a selected item already?
        {
            //TODO: Async with splash scren
            SceneManager.LoadScene(selected.levelIndex);
        }
    }
}
