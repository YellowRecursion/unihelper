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

    [Header("Additional info")]
    public CanvasGroup infoContainer;
    public GameObject infoButton;
    public TextMeshProUGUI info;
    public float animTime = 0.3f;
    public AnimationCurve animationCurve;

    private LevelsMenu levelsMenu;
    private Level level;
    private bool isInfoEnabled;

    public void Setup(LevelsMenu levelsMenu, Level level, int number, bool locked)
    {
        this.levelsMenu = levelsMenu;
        this.level = level;

        numberText.text = number.ToString();
        background.color = gameData.difficultyColors[(int)level.difficulty];

        // Information
        string mistakes = "\nОшибки в: ";
        for (int i = 0; i < level.MistakeIndexes.Length; i++)
        {
            if (i == level.MistakeIndexes.Length - 1) mistakes += level.MistakeIndexes[i];
            else mistakes += level.MistakeIndexes[i] + ", ";
        }
        if (level.CorrectAnswersCount == level.questions.Count) mistakes = string.Empty;

        if (level.Stars != 0) info.text = $"Ответы: {level.CorrectAnswersCount}/{level.questions.Count}" + mistakes;
        else info.text = "Информация появится после прохождения";

        lockedBox.SetActive(false);
        this.starsBox.SetActive(false);

        if (locked)
        {
            lockedBox.SetActive(true);
            GetComponent<Button>().interactable = false;
            infoButton.gameObject.SetActive(false);
        }
        else
        {
            this.starsBox.SetActive(true);
            for (int i = 0; i < level.Stars; i++)
            {
                activeStars[i].SetActive(true);
            }
        }
    }

    public void OnButtonClicked()
    {
        levelsMenu.gameData.SelectedLevel = level;
        levelsMenu.levelLoader.LoadLevel(1);
    }

    public void ShowHideInfo()
    {
        if (!isInfoEnabled) ShowInfo();
        else HideInfo();
    }

    public void ShowInfo()
    {
        isInfoEnabled = true;

        var rt = GetComponent<RectTransform>();
        LeanTween.size(rt, new Vector2(rt.sizeDelta.x, 300f), animTime).setEase(animationCurve);
        LeanTween.rotate(infoButton, new Vector3(0f, 0f, 180f), animTime).setEase(animationCurve);
        LeanTween.alphaCanvas(infoContainer, 1f, animTime).setEase(animationCurve);
    }

    public void HideInfo()
    {
        isInfoEnabled = false;

        var rt = GetComponent<RectTransform>();
        LeanTween.size(rt, new Vector2(rt.sizeDelta.x, 150f), animTime).setEase(animationCurve);
        LeanTween.rotate(infoButton, new Vector3(0f, 0f, 0f), animTime).setEase(animationCurve);
        LeanTween.alphaCanvas(infoContainer, 0f, animTime).setEase(animationCurve);
    }
}
