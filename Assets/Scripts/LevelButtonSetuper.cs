using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonSetuper : MonoBehaviour
{
    public GameData gameData;

    [Header("Objects")]
    public TextMeshProUGUI numberText;
    public Image background;
    public GameObject starsBox;
    public GameObject[] activeStars;
    public GameObject lockedBox;

    private LevelsMenu levelsMenu;
    private Level level;

    public void Setup(LevelsMenu levelsMenu, Level level, int number, bool locked)
    {
        this.levelsMenu = levelsMenu;
        this.level = level;

        numberText.text = number.ToString();
        background.color = gameData.difficultyColors[(int)level.difficulty];

        lockedBox.SetActive(false);
        this.starsBox.SetActive(false);

        if (locked)
        {
            lockedBox.SetActive(true);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            this.starsBox.SetActive(true);
            for (int i = 0; i < level.GetStars(); i++)
            {
                activeStars[i].SetActive(true);
            }

            GetComponent<Button>().onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        levelsMenu.gameData.SelectedLevel = level;
        levelsMenu.levelLoader.LoadLevel(1);
    }
}
